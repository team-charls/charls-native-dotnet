// Copyright (c) Team CharLS.
// SPDX-License-Identifier: BSD-3-Clause

using System;
using System.Globalization;

namespace CharLS.Native
{
    /// <summary>
    /// Holds the information for a SPIFF header.
    /// </summary>
    public sealed class SpiffHeader : IEquatable<SpiffHeader>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpiffHeader"/> class.
        /// </summary>
        public SpiffHeader()
        {
            ComponentCount = 1;
            Width = 1;
            Height = 1;
            ColorSpace = SpiffColorSpace.None;
            BitsPerSample = 8;
            CompressionType = SpiffCompressionType.JpegLS;
            ResolutionUnit = SpiffResolutionUnit.AspectRatio;
            VerticalResolution = 1;
            HorizontalResolution = 1;
        }

        /// <summary>
        /// Gets or sets the application profile identifier.
        /// </summary>
        /// <value>
        /// The profile identifier.
        /// </value>
        public SpiffProfileId ProfileId { get; set; }

        /// <summary>
        /// Gets or sets the number of color component count, range [1, 255].
        /// </summary>
        /// <value>
        /// The component count.
        /// </value>
        public int ComponentCount { get; set; }

        /// <summary>
        /// Gets or sets the height, range [1, 4294967295].
        /// </summary>
        /// <value>
        /// The height.
        /// </value>
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets the width, range [1, 4294967295].
        /// </summary>
        /// <value>
        /// The width.
        /// </value>
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the color space.
        /// </summary>
        /// <value>
        /// The color space.
        /// </value>
        public SpiffColorSpace ColorSpace { get; set; }

        /// <summary>
        /// Gets or sets the bits per sample, range (1, 2, 4, 8, 12, 16).
        /// </summary>
        /// <value>
        /// The bits per sample.
        /// </value>
        public int BitsPerSample { get; set; }

        /// <summary>
        /// Gets or sets the type of the compression.
        /// </summary>
        /// <value>
        /// The type of the compression.
        /// </value>
        public SpiffCompressionType CompressionType { get; set; }

        /// <summary>
        /// Gets or sets the resolution unit.
        /// </summary>
        /// <value>
        /// The resolution unit.
        /// </value>
        public SpiffResolutionUnit ResolutionUnit { get; set; }

        /// <summary>
        /// Gets or sets the vertical resolution.
        /// </summary>
        /// <value>
        /// The vertical resolution.
        /// </value>
        public int VerticalResolution { get; set; }

        /// <summary>
        /// Gets or sets the horizontal resolution.
        /// </summary>
        /// <value>
        /// The horizontal resolution.
        /// </value>
        public int HorizontalResolution { get; set; }

        /// <summary>
        /// Returns a <see cref="string"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "ProfileId = {0}, Width = {1}, Height = {2}, ColorSpace = {3}, ComponentCount = {4}, CompressionType = {5}",
                ProfileId, Width, Height, ColorSpace, ComponentCount, CompressionType);
        }

        /// <summary>
        /// Determines whether the specified <see cref="object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to compare with this instance.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object? obj)
        {
            return Equals(obj as SpiffHeader);
        }

        /// <summary>
        /// Determines whether the specified <see cref="SpiffHeader" /> is equal to this instance.
        /// </summary>
        /// <param name="other">The <see cref="SpiffHeader" /> to compare with this instance.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(SpiffHeader? other)
        {
            return other != null &&
                   ProfileId == other.ProfileId &&
                   ComponentCount == other.ComponentCount &&
                   Height == other.Height &&
                   Width == other.Width &&
                   ColorSpace == other.ColorSpace &&
                   BitsPerSample == other.BitsPerSample &&
                   CompressionType == other.CompressionType &&
                   ResolutionUnit == other.ResolutionUnit &&
                   VerticalResolution == other.VerticalResolution &&
                   HorizontalResolution == other.HorizontalResolution;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            var hashCode = default(HashCode);
            hashCode.Add((int)ProfileId);
            hashCode.Add(ComponentCount);
            hashCode.Add(Height);
            hashCode.Add(Width);
            hashCode.Add((int)ColorSpace);
            hashCode.Add(BitsPerSample);
            hashCode.Add((int)CompressionType);
            hashCode.Add((int)ResolutionUnit);
            hashCode.Add(VerticalResolution);
            hashCode.Add(HorizontalResolution);
            return hashCode.ToHashCode();
        }

        internal static bool TryCreate(in SpiffHeaderNative headerNative, out SpiffHeader? spiffHeader)
        {
            if (headerNative.Height > int.MaxValue ||
                headerNative.Width > int.MaxValue ||
                headerNative.VerticalResolution > int.MaxValue ||
                headerNative.HorizontalResolution > int.MaxValue)
            {
                spiffHeader = default;
                return false;
            }

            spiffHeader = new SpiffHeader
            {
                ProfileId = headerNative.ProfileId,
                ComponentCount = headerNative.ComponentCount,
                Height = (int)headerNative.Height,
                Width = (int)headerNative.Width,
                ColorSpace = headerNative.ColorSpace,
                BitsPerSample = headerNative.BitsPerSample,
                CompressionType = headerNative.CompressionType,
                ResolutionUnit = headerNative.ResolutionUnit,
                VerticalResolution = (int)headerNative.VerticalResolution,
                HorizontalResolution = (int)headerNative.HorizontalResolution
            };

            return true;
        }
    }
}