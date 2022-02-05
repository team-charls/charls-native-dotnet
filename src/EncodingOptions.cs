// Copyright (c) Team CharLS.
// SPDX-License-Identifier: BSD-3-Clause

namespace CharLS.Native;

/// <summary>
/// Defines options that can be enabled during the encoding process.
/// These options can be combined.
/// </summary>
[Flags]
public enum EncodingOptions
{
    /// <summary>
    /// No special encoding options are defined.
    /// </summary>
    None = 0,

    /// <summary>
    /// Ensures that the generated encoded data has an even size by adding
    /// an extra 0xFF byte to the End Of Image (EOI) marker.
    /// DICOM requires that data is always even. This can be done by adding a zero padding byte
    /// after the encoded data or with this option.
    /// This option is not default enabled.
    /// </summary>
    EvenDestinationSize = 1,

    /// <summary>
    /// Add a comment (COM) segment with the content: "charls [version-number]" to the encoded data.
    /// Storing the used encoder version can be helpful for long term archival of images.
    /// This option is not default enabled.
    /// </summary>
    IncludeVersionNumber = 2,

    /// <summary>
    /// Writes explicitly the default JPEG-LS preset coding parameters when the
    /// bits per sample is larger then 12 bits.
    /// The Java Advanced Imaging (JAI) JPEG-LS codec has a defect that causes it to use invalid
    /// preset coding parameters for these types of images.
    /// Most users of this codec are aware of this problem and have implemented a work-around.
    /// This option is default enabled. Will not be default enabled in the next major version upgrade.
    /// </summary>
    IncludePCParametersJai = 4
}
