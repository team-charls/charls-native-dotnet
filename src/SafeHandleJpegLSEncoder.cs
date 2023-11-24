// Copyright (c) Team CharLS.
// SPDX-License-Identifier: BSD-3-Clause

using Microsoft.Win32.SafeHandles;

namespace CharLS.Native;

/// <summary>
/// Helper class that manages the native JpegLSEncoder resource.
/// </summary>
internal sealed class SafeHandleJpegLSEncoder() : SafeHandleZeroOrMinusOneIsInvalid(true)
{
    protected override bool ReleaseHandle()
    {
        Interop.CharLSDestroyEncoder(handle);
        return true;
    }
}
