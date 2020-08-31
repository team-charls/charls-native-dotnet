// Copyright (c) Team CharLS.
// SPDX-License-Identifier: BSD-3-Clause

namespace CharLS.Native
{
    /// <summary>
    /// Defines the color space options that can be used in a SPIFF header v2, as defined in ISO/IEC 10918-3, F.2.1.1.
    /// </summary>
    public enum SpiffColorSpace
    {
        /// <summary>
        /// Bi-level image. Each image sample is one bit: 0 = white and 1 = black.
        /// </summary>
        BiLevelBlack = 0,

        /// <summary>
        /// The color space is based on recommendation ITU-R BT.709.
        /// </summary>
        YcbcrItuBT709Video = 1,

        /// <summary>
        /// Color space interpretation of the coded sample is none of the other options.
        /// </summary>
        None = 2,

        /// <summary>
        /// The color space is based on recommendation ITU-R BT.601-1. (RGB).
        /// </summary>
        YcbcrItuBT6011Rgb = 3,

        /// <summary>
        /// The color space is based on recommendation ITU-R BT.601-1. (video).
        /// </summary>
        YcbcrItuBT6011Video = 4,

        /// <summary>
        /// Grayscale – This is a single component sample with interpretation as grayscale value, 0 is minimum, 2bps -1 is maximum.
        /// </summary>
        Grayscale = 8,

        /// <summary>
        /// This is the color encoding method used in the Photo CD™ system.
        /// </summary>
        PhotoYcc = 9,

        /// <summary>
        /// The encoded data consists of samples of (uncalibrated) R, G and B.
        /// </summary>
        Rgb = 10,

        /// <summary>
        /// The encoded data consists of samples of Cyan, Magenta and Yellow samples.
        /// </summary>
        Cmy = 11,

        /// <summary>
        /// The encoded data consists of samples of Cyan, Magenta, Yellow and Black samples.
        /// </summary>
        Cmyk = 12,

        /// <summary>
        /// Transformed CMYK type data (same as Adobe PostScript)
        /// </summary>
        Ycck = 13,

        /// <summary>
        /// The CIE 1976 (L* a* b*) color space.
        /// </summary>
        CieLab = 14,

        /// <summary>
        /// Bi-level image. Each image sample is one bit: 1 = white and 0 = black.
        /// </summary>
        BiLevelWhite = 15
    }
}