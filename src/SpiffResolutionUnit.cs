// Copyright (c) Team CharLS.
// SPDX-License-Identifier: BSD-3-Clause

namespace CharLS.Native
{
    /// <summary>
    /// Defines the resolution units for the VRES and HRES parameters, as defined in ISO/IEC 10918-3, F.2.1.
    /// </summary>
    public enum SpiffResolutionUnit
    {
        /// <summary>
        /// VRES and HRES are to be interpreted as aspect ratio.
        /// </summary>
        /// <remark>
        /// If vertical or horizontal resolutions are not known, use this option and set VRES and HRES
        /// both to 1 to indicate that pixels in the image should be assumed to be square.
        /// </remark>
        AspectRatio = 0,

        /// <summary>
        /// Units of dots/samples per inch
        /// </summary>
        DotsPerInch = 1,

        /// <summary>
        /// Units of dots/samples per centimeter.
        /// </summary>
        DotsPerCentimeter = 2
    }
}