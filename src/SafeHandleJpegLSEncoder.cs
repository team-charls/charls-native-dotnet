// Copyright (c) Team CharLS.
// SPDX-License-Identifier: BSD-3-Clause

using Microsoft.Win32.SafeHandles;

namespace CharLS.Native
{
    internal class SafeHandleJpegLSEncoder : SafeHandleZeroOrMinusOneIsInvalid
    {
        public SafeHandleJpegLSEncoder()
            : base(true)
        {
        }

        protected override bool ReleaseHandle()
        {
            SafeNativeMethods.CharLSDestroyEncoder(handle);
            return true;
        }
    }
}