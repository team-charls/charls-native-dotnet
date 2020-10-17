// Copyright (c) Team CharLS.
// SPDX-License-Identifier: BSD-3-Clause

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using CharLS.Native;

internal class Program
{
    private const int Success = 0;
    private const int Failure = 1;

    private static int Main(string[] args)
    {
        // This sample demonstrates how to convert another encoded image to a JPEG-LS encoded image.
        // The input path should be an absolute path to a file format .NET can read (.bmp, .png, etc).
        if (!TryParseArguments(args, out string inputPath))
        {
            Console.WriteLine("Usage: Convert <path to image file>");
            return Failure;
        }

        try
        {
            using Bitmap image = new(inputPath);

            var bitmapData = image.LockBits(
                new(0, 0, image.Width, image.Height),
                ImageLockMode.ReadWrite,
                PixelFormat.Format24bppRgb);
            if (bitmapData.Stride < 0)
            {
                Console.WriteLine("Image {inputPath} is not top down.");
                return Failure;
            }

            Span<byte> pixels;
            unsafe
            {
                pixels = new(bitmapData.Scan0.ToPointer(), bitmapData.Stride * image.Height);
            }

            // GDI+ returns bgr pixels, JPEG-LS (Spiff) only knows RGB as color space.
            ConvertBgrToRgb(pixels, image.Width, image.Height, bitmapData.Stride);

            using JpegLSEncoder jpeglsEncoder = new(bitmapData.Width, bitmapData.Height, 8, 3)
            {
                InterleaveMode = JpegLSInterleaveMode.Sample
            };

            jpeglsEncoder.WriteStandardSpiffHeader(SpiffColorSpace.Rgb);
            jpeglsEncoder.Encode(pixels, bitmapData.Stride);

            Save(GetOutputPath(inputPath), jpeglsEncoder.EncodedData.Span);

            return Success;
        }
        catch (IOException e)
        {
            Console.WriteLine("Error: " + e.Message);
        }

        return Failure;
    }

    private static string GetOutputPath(string inputPath)
    {
        return Path.ChangeExtension(inputPath, ".jls");
    }

    private static void Save(string path, ReadOnlySpan<byte> encodedData)
    {
        using FileStream output = new(path, FileMode.OpenOrCreate);
        output.Write(encodedData);
    }

    private static bool TryParseArguments(IReadOnlyList<string> args, out string inputPath)
    {
        inputPath = string.Empty;

        if (args.Count != 1)
            return false;

        inputPath = args[0];
        return true;
    }

    private static void ConvertBgrToRgb(Span<byte> pixels, int width, int height, int stride)
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

                byte temp = pixels[a];
                pixels[a] = pixels[b];
                pixels[b] = temp;
            }
        }
    }
}
