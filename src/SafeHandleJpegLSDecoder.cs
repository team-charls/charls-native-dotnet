// Copyright (c) Team CharLS.
// SPDX-License-Identifier: BSD-3-Clause

using Microsoft.Win32.SafeHandles;

namespace CharLS.Native;

/// <summary>
/// Helper class that manages the native JpegLSDecoder resource.
/// </summary>
internal sealed class SafeHandleJpegLSDecoder : SafeHandleZeroOrMinusOneIsInvalid
{
    public SafeHandleJpegLSDecoder()
        : base(true)
    {
    }

    protected override bool ReleaseHandle()
    {
        Interop.CharLSDestroyDecoder(handle);
        return true;
    }
}
