// Copyright (c) Team CharLS.
// SPDX-License-Identifier: BSD-3-Clause

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using CharLS.Native;

namespace Convert
{
    internal class Program
    {
        private const int Success = 0;
        private const int Failure = 1;

        private static int Main(string[] args)
        {
            // This sample demonstrates how to convert another encoded image to a JPEG-LS encoded image.
            // The input path should be an absolute path to a file format .NET can read (.bmp, .png, etc).
            string inputPath;
            if (!TryParseArguments(args, out inputPath))
            {
                Console.WriteLine("Usage: Convert <path to image file>");
                return Failure;
            }

            try
            {
                using var image = new Bitmap(inputPath);

                var bitmapData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height),
                    ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                if (bitmapData.Stride < 0)
                {
                    Console.WriteLine("Image {inputPath} is not top down.");
                    return Failure;
                }

                Span<byte> pixels;
                unsafe
                {
                    pixels = new Span<byte>(bitmapData.Scan0.ToPointer(), bitmapData.Stride * image.Height);
                }

                // GDI+ returns bgr pixels, JPEG-LS (Spiff) only knows RGB as colorspace.
                ConvertBgrToRgb(pixels, image.Width, image.Height, bitmapData.Stride);

                using var jpeglsEncoder = new JpegLSEncoder
                {
                    FrameInfo = new FrameInfo(bitmapData.Width, bitmapData.Height, 8, 3)
                };

                var encodedData = new byte[jpeglsEncoder.EstimatedDestinationSize];
                jpeglsEncoder.SetDestination(encodedData);
                jpeglsEncoder.InterleaveMode = JpegLSInterleaveMode.Sample;
                jpeglsEncoder.Encode(pixels, bitmapData.Stride);

                Save(encodedData, jpeglsEncoder.BytesWritten, GetOutputPath(inputPath));

                return Success;
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("Error: " + e.Message);
            }

            return Failure;
        }

        private static string GetOutputPath(string inputPath)
        {
            return Path.ChangeExtension(inputPath, ".jls");
        }

        private static void Save(byte[] pixels, long count, string path) // TODO: use ReadOnlySpan
        {
            using var output = new FileStream(path, FileMode.OpenOrCreate);
            output.Write(pixels, 0, (int)count); // TODO get rid of int cast.
        }

        private static bool TryParseArguments(string[] args, out string inputPath)
        {
            inputPath = string.Empty;

            if (args.Length != 1)
                return false;

            inputPath = args[0];
            return true;
        }

        private static void ConvertBgrToRgb(Span<byte> pixels, int width, int height, int stride)
        {
            const int BytesPerRgbPixel = 3;

            for (int line = 0; line < height; ++line)
            {
                int line_start = line * stride;
                for (int pixel = 0; pixel < width; ++pixel)
                {
                    int column = pixel * BytesPerRgbPixel;
                    int a = line_start + column;
                    int b = line_start + column + 2;

                    byte temp = pixels[a];
                    pixels[a] = pixels[b];
                    pixels[b] = temp;
                }
            }
        }
    }
}
