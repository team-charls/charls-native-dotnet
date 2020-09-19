// Copyright (c) Team CharLS.
// SPDX-License-Identifier: BSD-3-Clause

using System;
using System.Buffers;
using System.Runtime.InteropServices;
using static CharLS.Native.SafeNativeMethods;

namespace CharLS.Native
{
    /// <summary>
    /// JPEG-LS Encoder.
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
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="bitsPerSample">The bits per sample.</param>
        /// <param name="componentCount">The component count.</param>
        /// <param name="allocateDestination">Flag to control if destination buffer should be allocated or not.</param>
        public JpegLSEncoder(int width, int height, int bitsPerSample, int componentCount, bool allocateDestination = true)
        {
            FrameInfo = new FrameInfo(width, height, bitsPerSample, componentCount);
            if (allocateDestination)
            {
                Destination = new byte[EstimatedDestinationSize];
            }
        }

        /// <summary>
        /// Gets or sets the frame information.
        /// </summary>
        /// <value>
        /// The frame information.
        /// </value>
        public FrameInfo? FrameInfo
        {
            get => _frameInfo;

            set
            {
                if (value is null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                var infoNative = new FrameInfoNative
                {
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
        /// Gets or sets the near lossless parameter used to encode the JPEG-LS stream.
        /// </summary>
        /// <value>
        /// The near lossless parameter value.
        /// </value>
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
        /// The interleave mode.
        /// </value>
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
        /// Gets or sets the preset coding parameters.
        /// </summary>
        /// <value>
        /// The preset coding parameters.
        /// </value>
        /// <exception cref="ArgumentNullException">value.</exception>
        public JpegLSPresetCodingParameters? PresetCodingParameters
        {
            get => _presetCodingParameters;

            set
            {
                if (value is null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                var native = new JpegLSPresetCodingParametersNative
                {
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
        /// Gets the size of the estimated destination.
        /// </summary>
        /// <value>
        /// The size of the estimated destination.
        /// </value>
        public long EstimatedDestinationSize
        {
            get
            {
                HandleJpegLSError(CharLSGetEstimatedDestinationSize(_encoder, out UIntPtr sizeInBytes));
                return (long)sizeInBytes;
            }
        }

        /// <summary>
        /// Gets or sets the destination.
        /// </summary>
        /// <value>
        /// The destination.
        /// </value>
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
                            (byte*)_destinationPin.Pointer, (UIntPtr)value.Length));
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
        /// Gets the bytes written.
        /// </summary>
        /// <value>
        /// The bytes written.
        /// </value>
        public int BytesWritten
        {
            get
            {
                HandleJpegLSError(CharLSGetBytesWritten(_encoder, out UIntPtr bytesWritten));
                return (int)bytesWritten;
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _encoder.Dispose();
            _destinationPin.Dispose();
        }

        /// <summary>
        /// Encodes the specified source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="stride">The stride.</param>
        public void Encode(ReadOnlySpan<byte> source, int stride = 0)
        {
            HandleJpegLSError(CharLSEncodeFromBuffer(_encoder, ref MemoryMarshal.GetReference(source), (UIntPtr)source.Length, (uint)stride));
        }

        /// <summary>
        /// Writes a standard SPIFF header to the destination. The additional values are computed from the current encoder settings.
        /// A SPIFF header is optional, but recommended for standalone JPEG-LS files.
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
        /// Writes a SPIFF header to the destination.
        /// </summary>
        /// <param name="spiffHeader">Reference to a SPIFF header that will be written to the destination.</param>
        public void WriteSpiffHeader(SpiffHeader spiffHeader)
        {
            var headerNative = new SpiffHeaderNative(spiffHeader);
            HandleJpegLSError(CharLSWriteSpiffHeader(_encoder, ref headerNative));
        }

        private static SafeHandleJpegLSEncoder CreateEncoder()
        {
            SafeHandleJpegLSEncoder encoder = CharLSCreateEncoder();
            return encoder.IsInvalid ? throw new OutOfMemoryException() : encoder;
        }
    }
}
