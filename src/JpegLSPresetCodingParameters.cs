// Copyright (c) Team CharLS.
// SPDX-License-Identifier: BSD-3-Clause

using System;

namespace CharLS.Native
{
    /// <summary>
    /// Hold information about JPEG-LS preset coding parameters.
    /// </summary>
    public sealed class JpegLSPresetCodingParameters : IEquatable<JpegLSPresetCodingParameters>
    {
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

        public int MaximumSampleValue { get; }

        /// <summary>
        /// Gets the Threshold1.
        /// </summary>
        public int Threshold1 { get; }

        /// <summary>
        /// Gets the Threshold2.
        /// </summary>
        public int Threshold2 { get; }

        /// <summary>
        /// Gets the Threshold3.
        /// </summary>
        public int Threshold3 { get; }

        /// <summary>
        /// Gets the ResetValue.
        /// </summary>
        public int ResetValue { get; }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="parameters1">The frame info1.</param>
        /// <param name="parameters2">The frame info2.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(JpegLSPresetCodingParameters parameters1, JpegLSPresetCodingParameters parameters2)
        {
            return parameters1.Equals(parameters2);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="parameters1">The frame info1.</param>
        /// <param name="parameters2">The frame info2.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(JpegLSPresetCodingParameters parameters1, JpegLSPresetCodingParameters parameters2)
        {
            return !parameters1.Equals(parameters2);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        ///   <see langword="true" /> if the current object is equal to the <paramref name="other" /> parameter; otherwise, <see langword="false" />.
        /// </returns>
        public bool Equals(JpegLSPresetCodingParameters? other)
        {
            return !(other is null) &&
                   MaximumSampleValue == other.MaximumSampleValue &&
                   Threshold1 == other.Threshold1 &&
                   Threshold2 == other.Threshold2 &&
                   Threshold3 == other.Threshold3 &&
                   ResetValue == other.ResetValue;
        }

        /// <summary>
        /// Determines whether the specified <see cref="object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object? obj)
        {
            return Equals((obj as JpegLSPresetCodingParameters) !);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(MaximumSampleValue, Threshold1, Threshold2, Threshold3, ResetValue);
        }
    }
}