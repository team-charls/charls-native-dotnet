// Copyright (c) Team CharLS.
// SPDX-License-Identifier: BSD-3-Clause

using System.Runtime.InteropServices;

namespace CharLS.Native
{
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    internal struct SpiffHeaderNative
    {
        internal SpiffProfileId ProfileId;
        internal int ComponentCount;
        internal uint Height;
        internal uint Width;
        internal SpiffColorSpace ColorSpace;
        internal int BitsPerSample;
        internal SpiffCompressionType CompressionType;
        internal SpiffResolutionUnit ResolutionUnit;
        internal uint VerticalResolution;
        internal uint HorizontalResolution;

        internal SpiffHeaderNative(SpiffHeader spiffHeader)
        {
            ProfileId = spiffHeader.ProfileId;
            ComponentCount = spiffHeader.ComponentCount;
            Height = (uint)spiffHeader.Height;
            Width = (uint)spiffHeader.Width;
            ColorSpace = spiffHeader.ColorSpace;
            BitsPerSample = spiffHeader.BitsPerSample;
            CompressionType = spiffHeader.CompressionType;
            ResolutionUnit = spiffHeader.ResolutionUnit;
            VerticalResolution = (uint)spiffHeader.VerticalResolution;
            HorizontalResolution = (uint)spiffHeader.HorizontalResolution;
        }
    }
}