// Copyright (c) Team CharLS.
// SPDX-License-Identifier: BSD-3-Clause

using System.Runtime.InteropServices;

namespace CharLS.Native;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct JpegLSPresetCodingParametersNative
{
    internal int MaximumSampleValue;
    internal int Threshold1;
    internal int Threshold2;
    internal int Threshold3;
    internal int ResetValue;
}
