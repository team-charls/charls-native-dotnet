// Copyright (c) Team CharLS.
// SPDX-License-Identifier: BSD-3-Clause

namespace CharLS.Native
{
    /// <summary>
    /// Defines the compression options that can be used in a SPIFF header v2, as defined in ISO/IEC 10918-3, F.2.1.
    /// </summary>
    public enum SpiffCompressionType
    {
        /// <summary>
        /// Picture data is stored in component interleaved format, encoded at BPS per sample.
        /// </summary>
        Uncompressed = 0,

        /// <summary>
        /// Recommendation T.4, the basic algorithm commonly known as MH (Modified Huffman), only allowed for bi-level images.
        /// </summary>
        ModifiedHuffman = 1,

        /// <summary>
        /// Recommendation T.4, commonly known as MR (Modified READ), only allowed for bi-level images.
        /// </summary>
        ModifiedRead = 2,

        /// <summary>
        /// Recommendation T .6, commonly known as MMR (Modified Modified READ), only allowed for bi-level images.
        /// </summary>
        ModifiedModifiedRead = 3,

        /// <summary>
        /// ISO/IEC 11544, commonly known as JBIG, only allowed for bi-level images.
        /// </summary>
        JBig = 4,

        /// <summary>
        /// ISO/IEC 10918-1 or ISO/IEC 10918-3, commonly known as JPEG.
        /// </summary>
        Jpeg = 5,

        /// <summary>
        /// ISO/IEC 14495-1 or ISO/IEC 14495-2, commonly known as JPEG-LS. (extension defined in ISO/IEC 14495-1)
        /// </summary>
        JpegLS = 6
    }
}