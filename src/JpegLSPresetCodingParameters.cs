// Copyright (c) Team CharLS.
// SPDX-License-Identifier: BSD-3-Clause

namespace CharLS.Native
{
    /// <summary>
    /// Hold information about JPEG-LS preset coding parameters.
    /// </summary>
    public sealed record JpegLSPresetCodingParameters
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JpegLSPresetCodingParameters"/> class.
        /// </summary>
        public JpegLSPresetCodingParameters()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JpegLSPresetCodingParameters"/> class.
        /// </summary>
        /// <param name="maximumSampleValue">The maximum sample value of the pixels.</param>
        /// <param name="threshold1">The threshold 1 parameter.</param>
        /// <param name="threshold2">The threshold 2 parameter.</param>
        /// <param name="threshold3">The threshold 3 parameter.</param>
        /// <param name="resetValue">The reset value parameter.</param>
        public JpegLSPresetCodingParameters(int maximumSampleValue, int threshold1, int threshold2, int threshold3, int resetValue)
        {
            MaximumSampleValue = maximumSampleValue;
            Threshold1 = threshold1;
            Threshold2 = threshold2;
            Threshold3 = threshold3;
            ResetValue = resetValue;
        }

        internal JpegLSPresetCodingParameters(in JpegLSPresetCodingParametersNative native)
        {
            MaximumSampleValue = native.MaximumSampleValue;
            Threshold1 = native.Threshold1;
            Threshold2 = native.Threshold2;
            Threshold3 = native.Threshold3;
            ResetValue = native.ResetValue;
        }

        /// <summary>
        /// Gets the maximum sample value of the pixel data.
        /// </summary>
        public int MaximumSampleValue { get; init; }

        /// <summary>
        /// Gets the threshold 1 parameter.
        /// </summary>
        public int Threshold1 { get; init; }

        /// <summary>
        /// Gets the threshold 2 parameter.
        /// </summary>
        public int Threshold2 { get; init; }

        /// <summary>
        /// Gets the threshold 3 parameter.
        /// </summary>
        public int Threshold3 { get; init; }

        /// <summary>
        /// Gets the reset value parameter.
        /// </summary>
        public int ResetValue { get; init; }
    }
}