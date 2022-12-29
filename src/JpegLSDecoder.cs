// Copyright (c) Team CharLS.
// SPDX-License-Identifier: BSD-3-Clause

using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using static CharLS.Native.Interop;

namespace CharLS.Native;

/// <summary>
/// JPEG-LS Decoder that uses the native CharLS implementation to decode JPEG-LS images.
/// </summary>
public sealed class JpegLSDecoder : IDisposable
{
    private readonly SafeHandleJpegLSDecoder _decoder = CreateDecoder();
    private FrameInfo? _frameInfo;
    private int? _nearLossless;
    private JpegLSInterleaveMode? _interleaveMode;
    private ReadOnlyMemory<byte> _source;
    private MemoryHandle _sourcePin;
    private AtCommentHandler? _atCommentHandler;
    private AtApplicationDataHandler? _atApplicationData;
    private EventHandler<CommentEventArgs>? _comment;
    private EventHandler<ApplicationDataEventArgs>? _applicationData;

    /// <summary>
    /// Occurs when a comment (COM segment) is read.
    /// </summary>
    public event EventHandler<CommentEventArgs> Comment
    {
        add
        {
            if (_atCommentHandler == null)
            {
                _atCommentHandler = AtComment; // Ensure that the delegate is not collected.
                HandleJpegLSError(CharLSAtComment(_decoder, _atCommentHandler, IntPtr.Zero));
            }
            _comment += value;
        }

        remove => _comment -= value;// Note: Keep the delegate installed, it will be cleaned when the instance is disposed.
    }

    /// <summary>
    /// Occurs when an application data (APPn segment) is read.
    /// </summary>
    public event EventHandler<ApplicationDataEventArgs> ApplicationData
    {
        add
        {
            if (_atApplicationData == null)
            {
                _atApplicationData = AtApplicationData; // Ensure that the delegate is not collected.
                HandleJpegLSError(CharLSAtApplicationData(_decoder, _atApplicationData, IntPtr.Zero));
            }
            _applicationData += value;
        }

        remove => _applicationData -= value;// Note: Keep the delegate installed, it will be cleaned when the instance is disposed.
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="JpegLSDecoder"/> class.
    /// </summary>
    public JpegLSDecoder()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="JpegLSDecoder"/> class.
    /// </summary>
    /// <param name="source">The buffer containing the encoded data.</param>
    /// <param name="readHeader">When true the header from the JPEG-LS stream is parsed.</param>
    /// <exception cref="InvalidDataException">Thrown when the JPEG-LS stream is not valid.</exception>
    public JpegLSDecoder(ReadOnlyMemory<byte> source, bool readHeader = true)
    {
        try
        {
            Source = source;
            if (readHeader)
            {
                ReadHeader();
            }
        }
        catch
        {
            _decoder.Dispose();
            throw;
        }
    }

    /// <summary>
    /// Gets or sets the the source buffer that contains the encoded JPEG-LS bytes.
    /// </summary>
    /// <value>
    /// A region of memory that contains an encoded JPEG-LS image.
    /// </value>
    /// <exception cref="ObjectDisposedException">Thrown when the instance is used after being disposed.</exception>
    public ReadOnlyMemory<byte> Source
    {
        get => _source;

        set
        {
            _sourcePin.Dispose();
            _sourcePin = value.Pin();

            try
            {
                unsafe
                {
                    HandleJpegLSError(CharLSSetSourceBuffer(_decoder, (byte*)_sourcePin.Pointer,
                        (nuint)value.Length));
                }

                _source = value;
            }
            catch
            {
                _sourcePin.Dispose();
                throw;
            }
        }
    }

    /// <summary>
    /// Gets the SPIFF header that was found during reading the header.
    /// </summary>
    /// <value>
    /// The SPIFF header or null when no valid SPIFF header could be found.
    /// </value>
    public SpiffHeader? SpiffHeader { get; private set; }

    /// <summary>
    /// Gets the frame information of the image contained in the JPEG-LS stream.
    /// </summary>
    /// <remarks>
    /// Property should be obtained after calling <see cref="ReadHeader"/>".
    /// </remarks>
    /// <value>
    /// The frame information of the parsed JPEG-LS image.
    /// </value>
    /// <exception cref="OverflowException">Thrown when the native result doesn't fit in an Int32.</exception>
    /// <exception cref="ObjectDisposedException">Thrown when the instance is used after being disposed.</exception>
    /// <exception cref="InvalidOperationException">Thrown when this property is used before <see cref="ReadHeader(bool)"/>.</exception>
    public FrameInfo FrameInfo
    {
        get
        {
            if (_frameInfo is null)
            {
                HandleJpegLSError(CharLSGetFrameInfo(_decoder, out var frameInfoNative));
                _frameInfo = new(frameInfoNative);
            }

            return _frameInfo;
        }
    }

    /// <summary>
    /// Gets the near lossless parameter used to encode the JPEG-LS stream.
    /// </summary>
    /// <remarks>
    /// Property should be obtained after calling <see cref="ReadHeader"/>".
    /// </remarks>
    /// <value>
    /// The near lossless parameter. A value of 0 means that the image is lossless encoded.
    /// </value>
    /// <exception cref="ObjectDisposedException">Thrown when the instance is used after being disposed.</exception>
    /// <exception cref="InvalidOperationException">Thrown when this property is used before <see cref="ReadHeader(bool)"/>.</exception>
    public int NearLossless
    {
        get
        {
            if (!_nearLossless.HasValue)
            {
                HandleJpegLSError(CharLSGetNearLossless(_decoder, 0, out var nearLossless));
                _nearLossless = nearLossless;
            }

            return _nearLossless.Value;
        }
    }

    /// <summary>
    /// Gets the interleave mode that was used to encode the scan(s).
    /// </summary>
    /// <remarks>
    /// Property should be obtained after calling <see cref="ReadHeader"/>".
    /// </remarks>
    /// <returns>The result of the operation: success or a failure code.</returns>
    /// <exception cref="ObjectDisposedException">Thrown when the instance is used after being disposed.</exception>
    /// <exception cref="InvalidOperationException">Thrown when this property is used before <see cref="ReadHeader(bool)"/>.</exception>
    public JpegLSInterleaveMode InterleaveMode
    {
        get
        {
            if (!_interleaveMode.HasValue)
            {
                HandleJpegLSError(CharLSGetInterleaveMode(_decoder, out var interleaveMode));
                _interleaveMode = interleaveMode;
            }

            return _interleaveMode.Value;
        }
    }

    /// <summary>
    /// Gets the preset coding parameters.
    /// </summary>
    /// <value>
    /// The preset coding parameters.
    /// </value>
    /// <exception cref="ObjectDisposedException">Thrown when the instance is used after being disposed.</exception>
    /// <exception cref="InvalidOperationException">Thrown when this property is used before <see cref="ReadHeader(bool)"/>.</exception>
    public JpegLSPresetCodingParameters PresetCodingParameters
    {
        get
        {
            HandleJpegLSError(CharLSGetPresetCodingParameters(_decoder, 0, out var native));
            return new(native);
        }
    }

    /// <summary>
    /// Releases the unmanaged resources used by the <see cref="JpegLSDecoder"/> instance.
    /// </summary>
    public void Dispose()
    {
        _decoder.Dispose();
        _sourcePin.Dispose();
    }

    /// <summary>
    /// Gets the required size of the destination buffer.
    /// </summary>
    /// <param name="stride">The stride to use; byte count to the next pixel row. Pass 0 for the default.</param>
    /// <returns>The size of the destination buffer in bytes.</returns>
    /// <exception cref="OverflowException">When the required destination size doesn't fit in an int.</exception>
    /// <exception cref="ObjectDisposedException">Thrown when the instance is used after being disposed.</exception>
    /// <exception cref="InvalidOperationException">Thrown when this method is called before <see cref="ReadHeader(bool)"/>.</exception>
    public int GetDestinationSize(int stride = 0)
    {
        HandleJpegLSError(CharLSGetDestinationSize(_decoder, ConvertStrideToUint32(stride), out nuint destinationSize));
        return Convert.ToInt32(destinationSize);
    }

    /// <summary>
    /// Reads the SPIFF (Still Picture Interchange File Format) header.
    /// </summary>
    /// <param name="spiffHeader">The header or null when no valid header was found.</param>
    /// <returns>true if a SPIFF header was present and could be read.</returns>
    /// <exception cref="ObjectDisposedException">Thrown when the instance is used after being disposed.</exception>
    public bool TryReadSpiffHeader(out SpiffHeader? spiffHeader)
    {
        HandleJpegLSError(CharLSReadSpiffHeader(_decoder, out SpiffHeaderNative headerNative, out int headerFound));
        bool found = headerFound != 0;
        if (found)
        {
            found = SpiffHeader.TryCreate(headerNative, out spiffHeader);
        }
        else
        {
            spiffHeader = default;
        }

        return found;
    }

    /// <summary>
    /// Validates a SPIFF header with the FrameInfo.
    /// </summary>
    /// <param name="spiffHeader">Reference to a SPIFF header that will be validated.</param>
    /// <exception cref="InvalidDataException">Thrown when the SPIFF header is not valid.</exception>
    public void ValidateSpiffHeader(SpiffHeader spiffHeader)
    {
        var frameInfo = new FrameInfoNative
        {
            BitsPerSample = FrameInfo.BitsPerSample,
            ComponentCount = FrameInfo.ComponentCount,
            Height = (uint)FrameInfo.Height,
            Width = (uint)FrameInfo.Width
        };
        var spiffHeaderNative = new SpiffHeaderNative(spiffHeader);

        HandleJpegLSError(CharLSValidateSpiffHeader(ref spiffHeaderNative, ref frameInfo));
    }

    /// <summary>
    /// Reads the header of the JPEG-LS stream.
    /// After calling this method, the informational properties can be obtained.
    /// </summary>
    /// <param name="tryReadSpiffHeader">if set to <c>true</c> try to read the SPIFF header first.</param>
    /// <exception cref="InvalidDataException">Thrown when the JPEG-LS stream or SPIFF header is not valid.</exception>
    /// <exception cref="ObjectDisposedException">Thrown when the instance is used after being disposed.</exception>
    public void ReadHeader(bool tryReadSpiffHeader = true)
    {
        SpiffHeader? spiffHeader = null;
        if (tryReadSpiffHeader)
        {
            _ = TryReadSpiffHeader(out spiffHeader);
        }

        HandleJpegLSError(JpegLSDecoderReadHeader(_decoder));
        if (spiffHeader == null)
            return;

        ValidateSpiffHeader(spiffHeader);
        SpiffHeader = spiffHeader;
    }

    /// <summary>
    /// Decodes the encoded JPEG-LS data and returns the created byte buffer.
    /// </summary>
    /// <param name="stride">The stride to use, or 0 for the default.</param>
    /// <returns>A byte array with the decoded JPEG-LS data.</returns>
    /// <exception cref="InvalidDataException">Thrown when the JPEG-LS stream is not valid.</exception>
    /// <exception cref="ObjectDisposedException">Thrown when the instance is used after being disposed.</exception>
    /// <exception cref="InvalidOperationException">Thrown when this method is called before <see cref="ReadHeader(bool)"/>.</exception>
    public byte[] Decode(int stride = 0)
    {
        var destination = new byte[GetDestinationSize()];
        Decode(destination, stride);
        return destination;
    }

    /// <summary>
    /// Decodes the encoded JPEG-LS data to the passed byte buffer.
    /// </summary>
    /// <param name="destination">The memory region that is the destination for the decoded data.</param>
    /// <param name="stride">The stride to use, or 0 for the default.</param>
    /// <exception cref="InvalidDataException">Thrown when the JPEG-LS stream is not valid.</exception>
    /// <exception cref="ObjectDisposedException">Thrown when the instance is used after being disposed.</exception>
    public void Decode(Span<byte> destination, int stride = 0)
    {
        HandleJpegLSError(CharLSDecodeToBuffer(_decoder, ref MemoryMarshal.GetReference(destination),
            (nuint)destination.Length, ConvertStrideToUint32(stride)));
    }

    [SuppressMessage("Usage", "CA2201:Do not raise reserved exception types", Justification = "CharLSCreateDecoder will only fail in an out of memory condition")]
    private static SafeHandleJpegLSDecoder CreateDecoder()
    {
        SafeHandleJpegLSDecoder encoder = CharLSCreateDecoder();
        return encoder.IsInvalid ? throw new OutOfMemoryException() : encoder;
    }

    private static uint ConvertStrideToUint32(int stride)
    {
        return stride < 0 ? throw new ArgumentOutOfRangeException(nameof(stride), "Stride needs to be >= 0") : (uint)stride;
    }

    private unsafe int AtComment(IntPtr data, nuint size)
    {
        // Take a copy to prevent that subscribers to this event need to be unsafe to read the comment.
        var eventArgs = new CommentEventArgs(new ReadOnlySpan<byte>(data.ToPointer(), (int)size).ToArray());
        _comment?.Invoke(this, eventArgs);
        return Convert.ToInt32(eventArgs.Failed);
    }

    private unsafe int AtApplicationData(int applicationDataId, IntPtr data, nuint size)
    {
        // Take a copy to prevent that subscribers to this event need to be unsafe to read the comment.
        var eventArgs = new ApplicationDataEventArgs(applicationDataId, new ReadOnlySpan<byte>(data.ToPointer(), (int)size).ToArray());
        _applicationData?.Invoke(this, eventArgs);
        return Convert.ToInt32(eventArgs.Failed);
    }
}
