// Copyright (c) Team CharLS.
// SPDX-License-Identifier: BSD-3-Clause

using System;

namespace CharLS.Native
{
    public class JpegLSDecoder
    {
        private readonly SafeHandleJpegLSDecoder _decoder = CreateDecoder();
        private FrameInfoNative? _frameInfo;
        private int? _nearLossless;

        public JpegLSDecoder(byte[] source, int sourceLength = 0) => SetSource(source, sourceLength);

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

        public long GetDestinationSize(int stride = 0)
        {
            UIntPtr destinationSize;

            var error = Environment.Is64BitProcess
                ? SafeNativeMethods.CharLSGetDestinationSizeX64(_decoder, (uint)stride, out destinationSize)
                : SafeNativeMethods.CharLSGetDestinationSizeX86(_decoder, (uint)stride, out destinationSize);
            JpegLSCodec.HandleResult(error);

            return (long)destinationSize;
        }

        public void ReadSpiffHeader()
        {
            if (Environment.Is64BitProcess)
                SafeNativeMethods.CharLSReadSpiffHeaderX64(_decoder);
            else
                SafeNativeMethods.CharLSReadSpiffHeaderX86(_decoder);
        }

        public void ReadHeader()
        {
            var error = Environment.Is64BitProcess
                ? SafeNativeMethods.JpegLSDecoderReadHeaderX64(_decoder)
                : SafeNativeMethods.JpegLSDecoderReadHeaderX86(_decoder);
            JpegLSCodec.HandleResult(error);
        }

        public void DecodeToBuffer(byte[] destination, long destinationSize = 0, int stride = 0)
        {
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
