// Copyright (c) Team CharLS.
// SPDX-License-Identifier: BSD-3-Clause

using System.Runtime.InteropServices;

namespace CharLS.Native
{
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    internal struct FrameInfoNative
    {
        internal uint Width;
        internal uint Height;
        internal int BitsPerSample;
        internal int ComponentCount;
    }
}
