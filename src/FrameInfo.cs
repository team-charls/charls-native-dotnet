// Copyright (c) Team CharLS.
// SPDX-License-Identifier: BSD-3-Clause

namespace CharLS.Native;

/// <summary>
/// Hold information about an image frame.
/// </summary>
public sealed record FrameInfo
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FrameInfo"/> record.
    /// </summary>
    public FrameInfo()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="FrameInfo"/> record.
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
    /// Gets the width of the image, valid range is [1, 65535].
    /// </summary>
    public int Width { get; init; }

    /// <summary>
    /// Gets the height of the image, valid range is [1, 65535].
    /// </summary>
    public int Height { get; init; }

    /// <summary>
    /// Gets the number of bits per sample, valid range is [2, 16].
    /// </summary>
    public int BitsPerSample { get; init; }

    /// <summary>
    /// Gets the number of components contained in the frame, valid range is [1, 255].
    /// </summary>
    public int ComponentCount { get; init; }
}
