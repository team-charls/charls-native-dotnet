// Copyright (c) Team CharLS.
// SPDX-License-Identifier: BSD-3-Clause

using System;
using System.Buffers;
using System.Runtime.InteropServices;
using static CharLS.Native.SafeNativeMethods;

namespace CharLS.Native
{
    /// <summary>
    /// JPEG-LS Decoder.
    /// </summary>
    public sealed class JpegLSDecoder : IDisposable
    {
        private readonly SafeHandleJpegLSDecoder _decoder = CreateDecoder();
        private FrameInfo? _frameInfo;
        private int? _nearLossless;
        private JpegLSInterleaveMode? _interleaveMode;
        private MemoryHandle _sourcePin;

        /// <summary>
        /// Initializes a new instance of the <see cref="JpegLSDecoder"/> class.
        /// </summary>
        public JpegLSDecoder()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JpegLSDecoder"/> class.
        /// </summary>
        /// <param name="source">The source buffer.</param>
        public JpegLSDecoder(ReadOnlyMemory<byte> source)
        {
            SetSource(source);
        }

        /// <summary>
        /// Gets the frame information.
        /// </summary>
        /// <value>
        /// The frame information.
        /// </value>
        public FrameInfo FrameInfo
        {
            get
            {
                if (_frameInfo is null)
                {
                    HandleJpegLSError(CharLSGetFrameInfo(_decoder, out var frameInfoNative));
                    _frameInfo = new FrameInfo(frameInfoNative);
                }

                return _frameInfo;
            }
        }

        /// <summary>
        /// Gets the near lossless parameter used to encode the JPEG-LS stream.
        /// </summary>
        /// <value>
        /// The near lossless.
        /// </value>
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
        /// Function should be called after the JpegLS header is read.
        /// </remarks>
        /// <returns>The result of the operation: success or a failure code.</returns>
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
        public JpegLSPresetCodingParameters PresetCodingParameters
        {
            get
            {
                HandleJpegLSError(CharLSGetPresetCodingParameters(_decoder, 0, out var native));
                return new JpegLSPresetCodingParameters(native);
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _decoder.Dispose();
            _sourcePin.Dispose();
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        /// <returns>A byte array with the pixel data.</returns>
        public byte[] Decode()
        {
            var destination = new byte[GetDestinationSize()];
            DecodeToBuffer(destination);

            return destination;
        }

        /// <summary>
        /// Sets the source buffer that contains the encoded JPEG-LS bytes.
        /// </summary>
        /// <param name="source">The source buffer.</param>
        public void SetSource(ReadOnlyMemory<byte> source)
        {
            _sourcePin.Dispose();
            _sourcePin = source.Pin();

            try
            {
                unsafe
                {
                    HandleJpegLSError(CharLSSetSourceBuffer(_decoder, (byte*)_sourcePin.Pointer, (UIntPtr)source.Length));
                }
            }
            catch
            {
                _sourcePin.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Gets the required size of the destination buffer.
        /// </summary>
        /// <param name="stride">The stride.</param>
        /// <returns>The size of the destination buffer in bytes.</returns>
        public long GetDestinationSize(int stride = 0)
        {
            HandleJpegLSError(CharLSGetDestinationSize(_decoder, (uint)stride, out var destinationSize));
            return (long)destinationSize;
        }

        /// <summary>
        /// Reads the SPIFF header.
        /// </summary>
        /// <param name="spiffHeader">The header.</param>
        /// <returns>true if a SPIFF header was present and could be read.</returns>
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
        /// Reads the header.
        /// </summary>
        public void ReadHeader()
        {
            HandleJpegLSError(JpegLSDecoderReadHeader(_decoder));
        }

        /// <summary>
        /// Decodes the encoded JPEG-LS source to a byte buffer.
        /// </summary>
        /// <param name="destination">The destination.</param>
        /// <param name="stride">The stride.</param>
        public void DecodeToBuffer(Span<byte> destination, int stride = 0)
        {
            HandleJpegLSError(CharLSDecodeToBuffer(_decoder, ref MemoryMarshal.GetReference(destination), (UIntPtr)destination.Length, (uint)stride));
        }

        private static SafeHandleJpegLSDecoder CreateDecoder()
        {
            SafeHandleJpegLSDecoder encoder = CharLSCreateDecoder();
            return encoder.IsInvalid ? throw new OutOfMemoryException() : encoder;
        }
    }
}
