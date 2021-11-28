// Copyright (c) Team CharLS.
// SPDX-License-Identifier: BSD-3-Clause

using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using static CharLS.Native.SafeNativeMethods;

namespace CharLS.Native;

/// <summary>
/// JPEG-LS Encoder that uses the native CharLS implementation to encode JPEG-LS images.
/// </summary>
public sealed class JpegLSEncoder : IDisposable
{
    private readonly SafeHandleJpegLSEncoder _encoder = CreateEncoder();
    private FrameInfo? _frameInfo;
    private int _nearLossless;
    private JpegLSInterleaveMode _interleaveMode;
    private JpegLSPresetCodingParameters? _presetCodingParameters;
    private Memory<byte> _destination;
    private MemoryHandle _destinationPin;

    /// <summary>
    /// Initializes a new instance of the <see cref="JpegLSEncoder"/> class.
    /// </summary>
    public JpegLSEncoder()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="JpegLSEncoder"/> class.
    /// </summary>
    /// <param name="width">The width of the image to encode.</param>
    /// <param name="height">The height of the image to encode.</param>
    /// <param name="bitsPerSample">The bits per sample of the image to encode.</param>
    /// <param name="componentCount">The component count of the image to encode.</param>
    /// <param name="allocateDestination">Flag to control if destination buffer should be allocated or not.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when one of the arguments is invalid.</exception>
    /// <exception cref="OutOfMemoryException">Thrown when memory allocation for the destination buffer fails.</exception>
    public JpegLSEncoder(int width, int height, int bitsPerSample, int componentCount, bool allocateDestination = true)
    {
        try
        {
            FrameInfo = new(width, height, bitsPerSample, componentCount);
            if (allocateDestination)
            {
                Destination = new byte[EstimatedDestinationSize];
            }
        }
        catch
        {
            _encoder.Dispose();
            throw;
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="JpegLSEncoder"/> class.
    /// </summary>
    /// <param name="frameInfo">The frameInfo of the image to encode.</param>
    /// <param name="allocateDestination">Flag to control if destination buffer should be allocated or not.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when one of the arguments is invalid.</exception>
    /// <exception cref="OutOfMemoryException">Thrown when memory allocation for the destination buffer fails.</exception>
    public JpegLSEncoder(FrameInfo frameInfo, bool allocateDestination = true) :
        this(frameInfo.Width, frameInfo.Height, frameInfo.BitsPerSample, frameInfo.ComponentCount, allocateDestination)
    {
    }

    /// <summary>
    /// Gets or sets the frame information of the image.
    /// </summary>
    /// <value>
    /// The frame information of the image.
    /// </value>
    /// <exception cref="ArgumentException">Thrown when the passed FrameInfo is invalid.</exception>
    /// <exception cref="ArgumentNullException">Thrown when the passed FrameInfo instance is null.</exception>
    public FrameInfo? FrameInfo
    {
        get => _frameInfo;

        set
        {
            if (value is null)
                throw new ArgumentNullException(nameof(value));

            FrameInfoNative infoNative = new() {
                Height = (uint)value.Height,
                Width = (uint)value.Width,
                BitsPerSample = value.BitsPerSample,
                ComponentCount = value.ComponentCount
            };

            HandleJpegLSError(CharLSSetFrameInfo(_encoder, ref infoNative));
            _frameInfo = value;
        }
    }

    /// <summary>
    /// Gets or sets the near lossless parameter to be used to encode the JPEG-LS stream.
    /// </summary>
    /// <value>
    /// The near lossless parameter value, 0 means lossless.
    /// </value>
    /// <exception cref="ArgumentException">Thrown when the passed value is invalid.</exception>
    public int NearLossless
    {
        get => _nearLossless;

        set
        {
            HandleJpegLSError(CharLSSetNearLossless(_encoder, value));
            _nearLossless = value;
        }
    }

    /// <summary>
    /// Gets or sets the interleave mode.
    /// </summary>
    /// <value>
    /// The interleave mode that should be used to encode the image. Default is None.
    /// </value>
    /// <exception cref="ArgumentException">Thrown when the passed value is invalid for the defined image.</exception>
    public JpegLSInterleaveMode InterleaveMode
    {
        get => _interleaveMode;

        set
        {
            HandleJpegLSError(CharLSSetInterleaveMode(_encoder, value));
            _interleaveMode = value;
        }
    }

    /// <summary>
    /// Gets or sets the JPEG-LS preset coding parameters.
    /// </summary>
    /// <value>
    /// The JPEG-LS preset coding parameters that should be used to encode the image.
    /// </value>
    /// <exception cref="ArgumentNullException">value.</exception>
    public JpegLSPresetCodingParameters? PresetCodingParameters
    {
        get => _presetCodingParameters;

        set
        {
            if (value is null)
                throw new ArgumentNullException(nameof(value));

            JpegLSPresetCodingParametersNative native = new() {
                MaximumSampleValue = value.MaximumSampleValue,
                Threshold1 = value.Threshold1,
                Threshold2 = value.Threshold2,
                Threshold3 = value.Threshold3,
                ResetValue = value.ResetValue
            };

            HandleJpegLSError(CharLSSetPresetCodingParameters(_encoder, ref native));
            _presetCodingParameters = value;
        }
    }

    /// <summary>
    /// Gets the estimated size in bytes of the memory buffer that should used as output destination.
    /// </summary>
    /// <value>
    /// The size in bytes of the memory buffer.
    /// </value>
    /// <exception cref="OverflowException">When the required size doesn't fit in an int.</exception>
    public int EstimatedDestinationSize
    {
        get
        {
            HandleJpegLSError(CharLSGetEstimatedDestinationSize(_encoder, out nuint sizeInBytes));
            return Convert.ToInt32(sizeInBytes);
        }
    }

    /// <summary>
    /// Gets or sets the memory region that will be the destination for the encoded JPEG-LS data.
    /// </summary>
    /// <value>
    /// The memory buffer to be used as the destination.
    /// </value>
    /// <exception cref="ArgumentException">Thrown when the passed value is an empty buffer.</exception>
    public Memory<byte> Destination
    {
        get => _destination;

        set
        {
            _destinationPin.Dispose();
            _destinationPin = value.Pin();

            try
            {
                unsafe
                {
                    HandleJpegLSError(CharLSSetDestinationBuffer(_encoder,
                        (byte*)_destinationPin.Pointer, (nuint)value.Length));
                }

                _destination = value;
            }
            catch
            {
                _destination = default;
                _destinationPin.Dispose();
                throw;
            }
        }
    }

    /// <summary>
    /// Gets the memory region with the encoded JPEG-LS data.
    /// </summary>
    /// <value>
    /// The memory region with the encoded data.
    /// </value>
    public ReadOnlyMemory<byte> EncodedData => _destination[..BytesWritten];

    /// <summary>
    /// Gets the bytes written to the destination buffer.
    /// </summary>
    /// <value>
    /// The bytes written to the destination buffer.
    /// </value>
    /// <exception cref="OverflowException">When the required size doesn't fit in an int.</exception>
    public int BytesWritten
    {
        get
        {
            HandleJpegLSError(CharLSGetBytesWritten(_encoder, out nuint bytesWritten));
            return Convert.ToInt32(bytesWritten);
        }
    }

    /// <summary>
    /// Releases the unmanaged resources used by the <see cref="JpegLSEncoder"/>.
    /// </summary>
    public void Dispose()
    {
        _encoder.Dispose();
        _destinationPin.Dispose();
    }

    /// <summary>
    /// Encodes the passed image data into encoded JPEG-LS data.
    /// </summary>
    /// <param name="source">The memory region that is the source input to the encoding process.</param>
    /// <param name="stride">The stride of the image pixel of the source input.</param>
    public void Encode(ReadOnlySpan<byte> source, int stride = 0)
    {
        HandleJpegLSError(CharLSEncodeFromBuffer(_encoder, ref MemoryMarshal.GetReference(source), (nuint)source.Length, (uint)stride));
    }

    /// <summary>
    /// Writes a standard SPIFF header to the destination. The additional values are computed from the current encoder settings.
    /// A SPIFF header is optional, but recommended for standalone JPEG-LS files.
    /// It should not be used when embedding a JPEG-LS image in a DICOM file.
    /// </summary>
    /// <param name="colorSpace">The color space of the image.</param>
    /// <param name="resolutionUnit">The resolution units of the next 2 parameters.</param>
    /// <param name="verticalResolution">The vertical resolution.</param>
    /// <param name="horizontalResolution">The horizontal resolution.</param>
    public void WriteStandardSpiffHeader(SpiffColorSpace colorSpace, SpiffResolutionUnit resolutionUnit = SpiffResolutionUnit.AspectRatio,
        int verticalResolution = 1, int horizontalResolution = 1)
    {
        HandleJpegLSError(CharLSWriteStandardSpiffHeader(_encoder, colorSpace, resolutionUnit, (uint)verticalResolution, (uint)horizontalResolution));
    }

    /// <summary>
    /// Writes a SPIFF header to the destination memory buffer.
    /// A SPIFF header is optional, but recommended for standalone JPEG-LS files.
    /// It should not be used when embedding a JPEG-LS image in a DICOM file.
    /// </summary>
    /// <param name="spiffHeader">Reference to a SPIFF header that will be written to the destination buffer.</param>
    public void WriteSpiffHeader(SpiffHeader spiffHeader)
    {
        SpiffHeaderNative headerNative = new(spiffHeader);
        HandleJpegLSError(CharLSWriteSpiffHeader(_encoder, ref headerNative));
    }

    [SuppressMessage("Usage", "CA2201:Do not raise reserved exception types", Justification = "CharLSCreateEncoder will only fail in an out of memory condition")]
    private static SafeHandleJpegLSEncoder CreateEncoder()
    {
        SafeHandleJpegLSEncoder encoder = CharLSCreateEncoder();
        return encoder.IsInvalid ? throw new OutOfMemoryException() : encoder;
    }
}
