// Copyright (c) Team CharLS.
// SPDX-License-Identifier: BSD-3-Clause

using System;
using Microsoft.Win32.SafeHandles;

namespace CharLS.Native
{
    internal class SafeHandleJpegLSDecoder : SafeHandleZeroOrMinusOneIsInvalid
    {
        public SafeHandleJpegLSDecoder()
            : base(true)
        {
        }

        protected override bool ReleaseHandle()
        {
            if (Environment.Is64BitProcess)
                SafeNativeMethods.CharLSDestroyDecoderX64(handle);
            else
                SafeNativeMethods.CharLSDestroyDecoderX86(handle);

            return true;
        }
    }
}
