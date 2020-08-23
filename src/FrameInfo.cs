// Copyright (c) Team CharLS.
// SPDX-License-Identifier: BSD-3-Clause

namespace CharLS.Native
{
    /// <summary>
    /// Hold information about the frame.
    /// </summary>
    public sealed class FrameInfo
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
    }
}
