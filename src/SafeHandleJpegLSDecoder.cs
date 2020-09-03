// Copyright (c) Team CharLS.
// SPDX-License-Identifier: BSD-3-Clause

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
            SafeNativeMethods.CharLSDestroyDecoder(handle);
            return true;
        }
    }
}
