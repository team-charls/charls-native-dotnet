// Copyright (c) Team CharLS.
// SPDX-License-Identifier: BSD-3-Clause

namespace CharLS.Native;

/// <summary>
/// Defines the interleave mode for multi-component (color) pixel data.
/// </summary>
public enum JpegLSInterleaveMode
{
    /// <summary>
    /// The encoded pixel data is not interleaved but stored as component for component, for example: RRRGGGBBB.
    /// Also default option for pixel data with only 1 component.
    /// </summary>
    None = 0,

    /// <summary>
    /// The interleave mode is by line. A full line of each
    /// component is encoded before moving to the next line.
    /// </summary>
    Line = 1,

    /// <summary>
    /// The data is stored by sample (pixel). For RGB color image this is the format like RGBRGBRGB.
    /// </summary>
    Sample = 2
}
