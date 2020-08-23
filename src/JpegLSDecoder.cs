// Copyright (c) Team CharLS.
// SPDX-License-Identifier: BSD-3-Clause

using System;
using System.IO;

namespace CharLS.Native
{
    /// <summary>
    /// JPEG-LS Decoder.
    /// </summary>
    public class JpegLSDecoder
    {
        private readonly SafeHandleJpegLSDecoder _decoder = CreateDecoder();
        private FrameInfoNative? _frameInfo;
        private int? _nearLossless;
        private JpegLSInterleaveMode? _interleaveMode;

        /// <summary>
        /// Initializes a new instance of the <see cref="JpegLSDecoder"/> class.
        /// </summary>
        /// <param name="source">The source buffer.</param>
        /// <param name="sourceLength">Length of the source buffer.</param>
        public JpegLSDecoder(byte[] source, int sourceLength = 0)
        {
            SetSource(source, sourceLength);
        }

        /// <summary>
        /// Gets the frame information.
        /// </summary>
        /// <value>
        /// The frame information.
        /// </value>
        public FrameInfoNative FrameInfo
        {
            get
            {
                if (!_frameInfo.HasValue)
                {
                    FrameInfoNative frameInfo;

                    var error = Environment.Is64BitProcess
                        ? SafeNativeMethods.CharLSGetFrameInfoX64(_decoder, out frameInfo)
                        : SafeNativeMethods.CharLSGetFrameInfoX86(_decoder, out frameInfo);
                    JpegLSCodec.HandleResult(error);

                    _frameInfo = frameInfo;
                }

                return _frameInfo.Value;
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
                    int nearLossless;

                    var error = Environment.Is64BitProcess
                        ? SafeNativeMethods.CharLSGetNearLosslessX64(_decoder, 0, out nearLossless)
                        : SafeNativeMethods.CharLSGetNearLosslessX86(_decoder, 0, out nearLossless);
                    JpegLSCodec.HandleResult(error);

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
                    JpegLSInterleaveMode interleaveMode;

                    var error = Environment.Is64BitProcess
                        ? SafeNativeMethods.CharLSGetInterleaveModeX64(_decoder, out interleaveMode)
                        : SafeNativeMethods.CharLSGetInterleaveModeX86(_decoder, out interleaveMode);
                    JpegLSCodec.HandleResult(error);

                    _interleaveMode = interleaveMode;
                }

                return _interleaveMode.Value;
            }
        }

        /// <summary>
        /// Decompresses the JPEG-LS encoded data passed in the source byte array.
        /// </summary>
        /// <param name="source">The byte array that contains the JPEG-LS encoded data to decompress.</param>
        /// <returns>A byte array with the pixel data.</returns>
        /// <exception cref="ArgumentNullException">source is null.</exception>
        /// <exception cref="InvalidDataException">Thrown when the source array contains invalid compressed data.</exception>
        public static byte[] Decode(byte[] source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            var decoder = new JpegLSDecoder(source);
            decoder.ReadHeader();

            var destination = new byte[decoder.GetDestinationSize()];
            decoder.DecodeToBuffer(destination);

            return destination;
        }

        /// <summary>
        /// Sets the source buffer that contains the encoded JPEG-LS bytes.
        /// </summary>
        /// <param name="source">The source buffer.</param>
        /// <param name="sourceLength">Length of the source buffer, when 0 .</param>
        public void SetSource(byte[] source, int sourceLength)
        {
            if (sourceLength == 0)
            {
                sourceLength = source.Length;
            }

            var error = Environment.Is64BitProcess
                ? SafeNativeMethods.CharLSSetSourceBufferX64(_decoder, source, sourceLength)
                : SafeNativeMethods.CharLSSetSourceBufferX86(_decoder, source, sourceLength);
            JpegLSCodec.HandleResult(error);
        }

        /// <summary>
        /// Gets the required size of the destination buffer.
        /// </summary>
        /// <param name="stride">The stride.</param>
        /// <returns>The size of the destination buffer in bytes.</returns>
        public long GetDestinationSize(int stride = 0)
        {
            UIntPtr destinationSize;

            var error = Environment.Is64BitProcess
                ? SafeNativeMethods.CharLSGetDestinationSizeX64(_decoder, (uint)stride, out destinationSize)
                : SafeNativeMethods.CharLSGetDestinationSizeX86(_decoder, (uint)stride, out destinationSize);
            JpegLSCodec.HandleResult(error);

            return (long)destinationSize;
        }

        /// <summary>
        /// Reads the SPIFF header.
        /// </summary>
        public void ReadSpiffHeader()
        {
            if (Environment.Is64BitProcess)
                SafeNativeMethods.CharLSReadSpiffHeaderX64(_decoder);
            else
                SafeNativeMethods.CharLSReadSpiffHeaderX86(_decoder);
        }

        /// <summary>
        /// Reads the header.
        /// </summary>
        public void ReadHeader()
        {
            var error = Environment.Is64BitProcess
                ? SafeNativeMethods.JpegLSDecoderReadHeaderX64(_decoder)
                : SafeNativeMethods.JpegLSDecoderReadHeaderX86(_decoder);
            JpegLSCodec.HandleResult(error);
        }

        /// <summary>
        /// Decodes the encoded JPEG-LS source to a byte buffer.
        /// </summary>
        /// <param name="destination">The destination.</param>
        /// <param name="destinationSize">Size of the destination.</param>
        /// <param name="stride">The stride.</param>
        public void DecodeToBuffer(byte[] destination, long destinationSize = 0, int stride = 0)
        {
            if (destinationSize == 0)
            {
                destinationSize = destination.Length;
            }

            var error = Environment.Is64BitProcess
                ? SafeNativeMethods.CharLSDecodeToBufferX64(_decoder, destination, (UIntPtr)destinationSize, (uint)stride)
                : SafeNativeMethods.CharLSDecodeToBufferX86(_decoder, destination, (UIntPtr)destinationSize, (uint)stride);
            JpegLSCodec.HandleResult(error);
        }

        private static SafeHandleJpegLSDecoder CreateDecoder()
        {
            var encoder = Environment.Is64BitProcess
                ? SafeNativeMethods.CharLSCreateDecoderX64()
                : SafeNativeMethods.CharLSCreateDecoderX86();
            if (encoder.IsInvalid)
                throw new OutOfMemoryException();

            return encoder;
        }
    }
}
