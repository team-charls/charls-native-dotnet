// Copyright (c) Team CharLS.
// SPDX-License-Identifier: BSD-3-Clause

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using CharLS.Native;

[assembly: CLSCompliant(true)]

const int Success = 0;
const int Failure = 1;

// This sample demonstrates how to convert another encoded image to a JPEG-LS encoded image.
// The input path should be an absolute path to a file format .NET can read (.bmp, .png, etc.).
if (!TryParseArguments(args, out string inputPath))
{
    Console.WriteLine("Usage: Convert input-image-filename");
    return Failure;
}

try
{
    using Bitmap image = new (inputPath);

    if (!TryGetFrameInfoAndPixelFormat(image, out var frameInfo, out var filePixelFormat))
    {
        Console.WriteLine($"Conversion not supported: {image.PixelFormat}");
        return Failure;
    }

    var bitmapData = image.LockBits(
        new (0, 0, image.Width, image.Height),
        ImageLockMode.ReadOnly,
        filePixelFormat);

    if (bitmapData.Stride < 0)
    {
        Console.WriteLine($"Image {inputPath} is not top down.");
        return Failure;
    }

    Span<byte> pixels;
    unsafe
    {
        pixels = new(bitmapData.Scan0.ToPointer(), bitmapData.Stride * image.Height);
    }

    // GDI+ returns bgr pixels, JPEG-LS (Spiff) only knows RGB as color space.
    if (frameInfo.ComponentCount == 3)
        ConvertBgrToRgb(pixels, image.Width, image.Height, bitmapData.Stride);

    var interleaveMode = frameInfo.ComponentCount == 1 ? JpegLSInterleaveMode.None : JpegLSInterleaveMode.Sample;
    using JpegLSEncoder jpeglsEncoder = new (frameInfo)
    {
        InterleaveMode = interleaveMode
    };

    if (frameInfo.ComponentCount == 1)
        jpeglsEncoder.WriteStandardSpiffHeader(SpiffColorSpace.Grayscale);
    else if (frameInfo.ComponentCount == 3)
        jpeglsEncoder.WriteStandardSpiffHeader(SpiffColorSpace.Rgb);
    jpeglsEncoder.Encode(pixels, bitmapData.Stride);

    image.UnlockBits(bitmapData);

    Save(GetOutputPath(inputPath), jpeglsEncoder.EncodedData.Span);

    return Success;
}
catch (IOException e)
{
    Console.WriteLine("Error: " + e.Message);
    return Failure;
}
catch(ArgumentException e)
{
    Console.WriteLine($"Invalid path: {inputPath}.");
    Console.WriteLine("Error: " + e.Message);
    return Failure;
}

// GetPixelFormat() does not tell anything about the file format, it tells what the image codec
// chose for the in-memory representation of the bitmap data.
// PNG 8bits/grayscale is loaded as PixelFormat32bppARGB
// JPG/TIFF 8bits/grayscale are loaded as PixelFormat8bppIndexed
bool TryGetFrameInfoAndPixelFormat(Bitmap sourceImage, [NotNullWhen(true)] out FrameInfo? frameInfo, out PixelFormat filePixelFormat)
{
    var pixelFormat = sourceImage.PixelFormat;
    var flags = sourceImage.Flags;
    var colorSpaceGray = (flags & (int)ImageFlags.ColorSpaceGray) > 0;

    frameInfo = null;
    filePixelFormat = default;
    if (pixelFormat is PixelFormat.Format8bppIndexed
        or PixelFormat.Format24bppRgb)
    {
        filePixelFormat = pixelFormat;
    }
    else if (pixelFormat == PixelFormat.Format32bppArgb)
    {
        // Debug.Assert(image.RawFormat.Equals(ImageFormat.Png), "Only for PNG");
        filePixelFormat = colorSpaceGray ? PixelFormat.Format8bppIndexed : PixelFormat.Format24bppRgb;
    }

    if (filePixelFormat != default)
    {
        var componentCount = Image.GetPixelFormatSize(filePixelFormat) / 8;
        frameInfo = new FrameInfo
        {
            Width = sourceImage.Width,
            Height = sourceImage.Height,
            BitsPerSample = 8,
            ComponentCount = componentCount
        };
    }

    return frameInfo != null && filePixelFormat != default;
}

string GetOutputPath(string inputPath)
{
    return Path.ChangeExtension(inputPath, ".jls");
}

void Save(string path, ReadOnlySpan<byte> encodedData)
{
    using FileStream output = new (path, FileMode.OpenOrCreate);
    output.Write(encodedData);
}

bool TryParseArguments(IReadOnlyList<string> args, out string inputPath)
{
    if (args.Count != 1)
    {
        inputPath = string.Empty;
        return false;
    }

    inputPath = args[0];
    return true;
}

void ConvertBgrToRgb(Span<byte> pixels, int width, int height, int stride)
{
    const int bytesPerRgbPixel = 3;

    for (int line = 0; line < height; ++line)
    {
        int lineStart = line * stride;
        for (int pixel = 0; pixel < width; ++pixel)
        {
            int column = pixel * bytesPerRgbPixel;
            int a = lineStart + column;
            int b = lineStart + column + 2;

            (pixels[a], pixels[b]) = (pixels[b], pixels[a]);
        }
    }
}
