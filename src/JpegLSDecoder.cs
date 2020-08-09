// Copyright (c) Team CharLS.
// SPDX-License-Identifier: BSD-3-Clause

using System;

namespace CharLS.Native
{
    public class JpegLSDecoder
    {
        private readonly IntPtr _decoder = CreateDecoder();
        private FrameInfoNative _frameInfo;

        public JpegLSDecoder()
        {
        }

        public FrameInfoNative FrameInfo
        {
            get
            {
                return _frameInfo;
            }

            set
            {
                var error = Environment.Is64BitProcess
                    ? SafeNativeMethods.CharLSSetFrameInfoX64(_decoder, ref value)
                    : SafeNativeMethods.CharLSSetFrameInfoX86(_decoder, ref value);
                JpegLSCodec.HandleResult(error);

                _frameInfo = value;
            }
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

        private static IntPtr CreateDecoder()
        {
            var encoder = Environment.Is64BitProcess
                ? SafeNativeMethods.CharLSCreateDecoderX64()
                : SafeNativeMethods.CharLSCreateDecoderX86();
            if (encoder == IntPtr.Zero)
                throw new OutOfMemoryException();

            return encoder;
        }
    }
}
