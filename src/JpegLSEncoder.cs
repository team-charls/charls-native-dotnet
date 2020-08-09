// Copyright (c) Team CharLS.
// SPDX-License-Identifier: BSD-3-Clause

using System;

namespace CharLS.Native
{
    public class JpegLSEncoder
    {
        private readonly IntPtr _encoder = CreateEncoder();
        private FrameInfoNative _frameInfo;

        ////public int NearLossless { get; set; }

        ////public JpegLSInterleaveMode InterleaveMode { get; set; }

        ////public static byte[] Encode(byte[] source)
        ////{

        public FrameInfoNative FrameInfo
        {
            get
            {
                return _frameInfo;
            }

            set
            {
                var error = Environment.Is64BitProcess
                    ? SafeNativeMethods.CharLSSetFrameInfoX64(_encoder, ref value)
                    : SafeNativeMethods.CharLSSetFrameInfoX86(_encoder, ref value);
                JpegLSCodec.HandleResult(error);

                _frameInfo = value;
            }
        }

        private static IntPtr CreateEncoder()
        {
            var encoder = Environment.Is64BitProcess
                ? SafeNativeMethods.CharLSCreateEncoderX64()
                : SafeNativeMethods.CharLSCreateEncoderX86();
            if (encoder == IntPtr.Zero)
                throw new OutOfMemoryException();

            return encoder;
        }
    }
}
