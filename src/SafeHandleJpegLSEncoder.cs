using System;
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
            if (Environment.Is64BitProcess)
                SafeNativeMethods.CharLSDestroyEncoderX64(handle);
            else
                SafeNativeMethods.CharLSDestroyEncoderX86(handle);

            return true;
        }
    }
}