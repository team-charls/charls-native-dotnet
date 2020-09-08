// Copyright (c) Team CharLS.
// SPDX-License-Identifier: BSD-3-Clause

using System;
using System.Drawing;
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
            // This sample demonstrates how to convert 8 bit monochrome images and 24 bit color images to a .jls
            // The input path should be a absolute path to a file format .NET can read (.bmp, .png, etc).
            string inputPath;
            if (!TryParseArguments(args, out inputPath))
            {
                Console.WriteLine("Usage: Converter <path to image file>");
                return Failure;
            }

            try
            {
                using var imageStream = new FileStream(inputPath, FileMode.Open, FileAccess.Read);
                using var image = new Bitmap(imageStream);

                // TODO: check pixel format.

                var rect = new Rectangle(0, 0, image.Width, image.Height);
                var bmpData = image.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, image.PixelFormat);

                // Declare an array to hold the bytes of the bitmap.
                int bytes = Math.Abs(bmpData.Stride) * image.Height;
                byte[] rgbValues = new byte[bytes];

                // Copy the RGB values into the array.
                System.Runtime.InteropServices.Marshal.Copy(bmpData.Scan0, rgbValues, 0, bytes);

                var encoder = new JpegLSEncoder {
                    FrameInfo = new FrameInfo(bmpData.Width, bmpData.Height, 8, 3)
                };

                var encodedData = new byte[encoder.EstimatedDestinationSize];
                encoder.SetDestination(encodedData);
                encoder.InterleaveMode = JpegLSInterleaveMode.Sample;
                encoder.Encode(rgbValues, bmpData.Stride);

                Save(encodedData, encoder.BytesWritten, GetOutputPath(inputPath));

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
    }
}
