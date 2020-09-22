// Copyright (c) Team CharLS.
// SPDX-License-Identifier: BSD-3-Clause

using System;

namespace CharLS.Native
{
    /// <summary>
    /// Hold information about the frame.
    /// </summary>
    public sealed class FrameInfo : IEquatable<FrameInfo>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FrameInfo"/> class.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="bitsPerSample">The number of bits per sample.</param>
        /// <param name="componentCount">The number of components contained in a frame.</param>
        public FrameInfo(int width, int height, int bitsPerSample, int componentCount)
        {
            Width = width;
            Height = height;
            BitsPerSample = bitsPerSample;
            ComponentCount = componentCount;
        }

        internal FrameInfo(in FrameInfoNative native)
        {
            Width = Convert.ToInt32(native.Width);
            Height = Convert.ToInt32(native.Height);
            BitsPerSample = native.BitsPerSample;
            ComponentCount = native.ComponentCount;
        }

        /// <summary>
        /// Gets the width of the image, range [1, 65535].
        /// </summary>
        public int Width { get; }

        /// <summary>
        /// Gets the height of the image, range [1, 65535].
        /// </summary>
        public int Height { get; }

        /// <summary>
        /// Gets the number of bits per sample, range [2, 16].
        /// </summary>
        public int BitsPerSample { get; }

        /// <summary>
        /// Gets the number of components contained in the frame, range [1, 255].
        /// </summary>
        public int ComponentCount { get; }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="frameInfo1">The frame info1.</param>
        /// <param name="frameInfo2">The frame info2.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(FrameInfo frameInfo1, FrameInfo frameInfo2)
        {
            return frameInfo1.Equals(frameInfo2);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="frameInfo1">The frame info1.</param>
        /// <param name="frameInfo2">The frame info2.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(FrameInfo frameInfo1, FrameInfo frameInfo2)
        {
            return !frameInfo1.Equals(frameInfo2);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        ///   <see langword="true" /> if the current object is equal to the <paramref name="other" /> parameter; otherwise, <see langword="false" />.
        /// </returns>
        public bool Equals(FrameInfo? other)
        {
            return !(other is null) &&
                   Width == other.Width &&
                   Height == other.Height &&
                   BitsPerSample == other.BitsPerSample &&
                   ComponentCount == other.ComponentCount;
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
            return Equals((obj as FrameInfo) !);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(Width, Height, BitsPerSample, ComponentCount);
        }
    }
}
