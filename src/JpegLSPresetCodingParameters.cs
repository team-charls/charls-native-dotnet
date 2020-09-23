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
        /// <param name="maximumSampleValue">The maximumSampleValue.</param>
        /// <param name="threshold1">The threshold1.</param>
        /// <param name="threshold2">The threshold2.</param>
        /// <param name="threshold3">The threshold3.</param>
        /// <param name="resetValue">The resetValue.</param>
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
        /// Gets the MaximumSampleValue.
        /// </summary>
        public int MaximumSampleValue { get; init; }

        /// <summary>
        /// Gets the Threshold1.
        /// </summary>
        public int Threshold1 { get; init; }

        /// <summary>
        /// Gets the Threshold2.
        /// </summary>
        public int Threshold2 { get; init; }

        /// <summary>
        /// Gets the Threshold3.
        /// </summary>
        public int Threshold3 { get; init; }

        /// <summary>
        /// Gets the ResetValue.
        /// </summary>
        public int ResetValue { get; init; }
    }
}