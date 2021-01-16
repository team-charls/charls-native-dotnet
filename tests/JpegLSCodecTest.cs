// Copyright (c) Team CharLS.
// SPDX-License-Identifier: BSD-3-Clause

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using NUnit.Framework;

namespace CharLS.Native.Test
{
    [TestFixture]
    public class JpegLSCodecTest
    {
        [Test]
        public void GetMetadataInfoFromLosslessEncodedColorImage()
        {
            using JpegLSDecoder decoder = new(ReadAllBytes("t8c0e0.jls"));

            Assert.AreEqual(decoder.FrameInfo.Height, 256);
            Assert.AreEqual(decoder.FrameInfo.Width, 256);
            Assert.AreEqual(decoder.FrameInfo.BitsPerSample, 8);
            Assert.AreEqual(decoder.FrameInfo.ComponentCount, 3);
            Assert.AreEqual(decoder.NearLossless, 0);
        }

        [Test]
        public void GetMetadataInfoFromNearLosslessEncodedColorImage()
        {
            using JpegLSDecoder decoder = new(ReadAllBytes("t8c0e3.jls"));

            var frameInfo = decoder.FrameInfo;
            Assert.AreEqual(frameInfo.Height, 256);
            Assert.AreEqual(frameInfo.Width, 256);
            Assert.AreEqual(frameInfo.BitsPerSample, 8);
            Assert.AreEqual(frameInfo.ComponentCount, 3);
            Assert.AreEqual(decoder.NearLossless, 3);
        }

        [Test]
        public void Decode()
        {
            var source = ReadAllBytes("t8c0e0.jls");
            var expected = ReadAllBytes("test8.ppm", 15);
            var uncompressed = Decode(source);

            using JpegLSDecoder decoder = new(source);

            var frameInfo = decoder.FrameInfo;
            if (decoder.InterleaveMode == JpegLSInterleaveMode.None && frameInfo.ComponentCount == 3)
            {
                expected = TripletToPlanar(expected, frameInfo.Width, frameInfo.Height);
            }

            Assert.AreEqual(expected, uncompressed);
        }

        [Test]
        public void Encode()
        {
            FrameInfo info = new(256, 256, 8, 3);

            var uncompressedOriginal = ReadAllBytes("test8.ppm", 15);
            uncompressedOriginal = TripletToPlanar(uncompressedOriginal, info.Width, info.Height);

            using JpegLSEncoder encoder = new() { FrameInfo = info };

            encoder.Destination = new byte[encoder.EstimatedDestinationSize];
            encoder.Encode(uncompressedOriginal);

            using JpegLSDecoder decoder = new(encoder.EncodedData);
            Assert.AreEqual(info, decoder.FrameInfo);

            var uncompressed = decoder.Decode();
            Assert.AreEqual(uncompressedOriginal.Length, uncompressed.Length);
            Assert.AreEqual(uncompressedOriginal, uncompressed);
        }

        [Test]
        public void EncodeWithPresetCodingParameters()
        {
            FrameInfo info = new(256, 256, 8, 3);

            var uncompressedOriginal = ReadAllBytes("test8.ppm", 15);
            uncompressedOriginal = TripletToPlanar(uncompressedOriginal, info.Width, info.Height);

            JpegLSPresetCodingParameters presetCodingParameters = new(255, 9, 10, 11, 31);
            using JpegLSEncoder encoder = new() { FrameInfo = info, PresetCodingParameters = presetCodingParameters };

            encoder.Destination = new byte[encoder.EstimatedDestinationSize];
            encoder.Encode(uncompressedOriginal);

            using JpegLSDecoder decoder = new(encoder.EncodedData);
            var pcp = decoder.PresetCodingParameters;

            Assert.AreEqual(presetCodingParameters, pcp);

            var uncompressed = decoder.Decode();
            Assert.AreEqual(uncompressedOriginal.Length, uncompressed.Length);
            Assert.AreEqual(uncompressedOriginal, uncompressed);
        }

        [Test]
        public void EncodeOneByOneColor()
        {
            var uncompressedOriginal = new byte[] { 77, 33, 255 };
            using JpegLSEncoder encoder = new() { FrameInfo = new(1, 1, 8, 3) };

            encoder.Destination = new byte[encoder.EstimatedDestinationSize];
            encoder.Encode(uncompressedOriginal);

            using JpegLSDecoder decoder = new(encoder.Destination);

            var uncompressed = decoder.Decode();
            Assert.AreEqual(uncompressedOriginal.Length, uncompressed.Length);
            Assert.AreEqual(uncompressedOriginal, uncompressed);
        }

        [Test]
        public void Encode2BitMonochrome()
        {
            var uncompressedOriginal = new byte[] { 1 };
            using JpegLSEncoder encoder = new() { FrameInfo = new(1, 1, 2, 1) };

            encoder.Destination = new byte[encoder.EstimatedDestinationSize];
            encoder.Encode(uncompressedOriginal);

            using JpegLSDecoder decoder = new(encoder.Destination);

            var uncompressed = decoder.Decode();
            Assert.AreEqual(uncompressedOriginal.Length, uncompressed.Length);
            Assert.AreEqual(uncompressedOriginal, uncompressed);
        }

        [Test]
        public void WriteStandardSpiffHeader()
        {
            var uncompressedOriginal = new byte[] { 1 };
            using JpegLSEncoder encoder = new() { FrameInfo = new(1, 1, 2, 1) };

            encoder.Destination = new byte[encoder.EstimatedDestinationSize];
            encoder.WriteStandardSpiffHeader(SpiffColorSpace.Grayscale);
            encoder.Encode(uncompressedOriginal);

            using JpegLSDecoder decoder = new(encoder.Destination, false);
            bool spiffHeaderPresent = decoder.TryReadSpiffHeader(out var spiffHeader);

            Assert.IsTrue(spiffHeaderPresent);

            Assert.AreEqual(SpiffProfileId.None, spiffHeader!.ProfileId);
            Assert.AreEqual(1, spiffHeader.ComponentCount);
            Assert.AreEqual(1, spiffHeader.Height);
            Assert.AreEqual(1, spiffHeader.Width);
            Assert.AreEqual(SpiffColorSpace.Grayscale, spiffHeader.ColorSpace);
            Assert.AreEqual(2, spiffHeader.BitsPerSample);
            Assert.AreEqual(SpiffCompressionType.JpegLS, spiffHeader.CompressionType);
            Assert.AreEqual(SpiffResolutionUnit.AspectRatio, spiffHeader.ResolutionUnit);
            Assert.AreEqual(1, spiffHeader.VerticalResolution);
            Assert.AreEqual(1, spiffHeader.HorizontalResolution);
        }

        [Test]
        public void WriteStandardSpiffHeaderReadWithProperty()
        {
            var uncompressedOriginal = new byte[] { 1 };
            using JpegLSEncoder encoder = new() { FrameInfo = new(1, 1, 2, 1) };

            encoder.Destination = new byte[encoder.EstimatedDestinationSize];
            encoder.WriteStandardSpiffHeader(SpiffColorSpace.Grayscale);
            encoder.Encode(uncompressedOriginal);

            using JpegLSDecoder decoder = new(encoder.Destination);

            var spiffHeader = decoder.SpiffHeader;
            Assert.IsNotNull(spiffHeader);
            if (decoder.SpiffHeader != null)
            {
                Assert.AreEqual(SpiffProfileId.None, spiffHeader!.ProfileId);
                Assert.AreEqual(1, decoder.SpiffHeader.ComponentCount);
                Assert.AreEqual(1, spiffHeader.Height);
                Assert.AreEqual(1, spiffHeader.Width);
                Assert.AreEqual(SpiffColorSpace.Grayscale, spiffHeader.ColorSpace);
                Assert.AreEqual(2, spiffHeader.BitsPerSample);
                Assert.AreEqual(SpiffCompressionType.JpegLS, spiffHeader.CompressionType);
                Assert.AreEqual(SpiffResolutionUnit.AspectRatio, spiffHeader.ResolutionUnit);
                Assert.AreEqual(1, spiffHeader.VerticalResolution);
                Assert.AreEqual(1, spiffHeader.HorizontalResolution);
            }
        }

        [Test]
        public void WriteSpiffHeader()
        {
            var uncompressedOriginal = new byte[] { 1 };
            using JpegLSEncoder encoder = new() { FrameInfo = new(1, 1, 2, 1) };

            encoder.Destination = new byte[encoder.EstimatedDestinationSize];

            SpiffHeader originalSpiffHeader = new()
            {
                ColorSpace = SpiffColorSpace.Grayscale,
                Height = 1,
                Width = 1,
                ComponentCount = 1,
                BitsPerSample = 2
            };

            encoder.WriteSpiffHeader(originalSpiffHeader);
            encoder.Encode(uncompressedOriginal);

            using JpegLSDecoder decoder = new(encoder.Destination, false);
            bool spiffHeaderPresent = decoder.TryReadSpiffHeader(out var spiffHeader);

            Assert.IsTrue(spiffHeaderPresent);

            Assert.AreEqual(SpiffProfileId.None, spiffHeader!.ProfileId);
            Assert.AreEqual(1, spiffHeader.ComponentCount);
            Assert.AreEqual(1, spiffHeader.Height);
            Assert.AreEqual(1, spiffHeader.Width);
            Assert.AreEqual(SpiffColorSpace.Grayscale, spiffHeader.ColorSpace);
            Assert.AreEqual(2, spiffHeader.BitsPerSample);
            Assert.AreEqual(SpiffCompressionType.JpegLS, spiffHeader.CompressionType);
            Assert.AreEqual(SpiffResolutionUnit.AspectRatio, spiffHeader.ResolutionUnit);
            Assert.AreEqual(1, spiffHeader.VerticalResolution);
            Assert.AreEqual(1, spiffHeader.HorizontalResolution);
        }

        [Test]
        public void DecodeBitStreamWithNoMarkerStart()
        {
            var source = new byte[] { 0x33, 0x33 };

            var exception = Assert.Throws<InvalidDataException>(() => Decode(source));
            Assert.AreEqual(JpegLSError.JpegMarkerStartByteNotFound, exception!.Data["JpegLSError"]);
        }

        [Test]
        public void DecodeBitStreamWithUnsupportedEncoding()
        {
            var source = new byte[]
                {
                    0xFF, 0xD8, // Start Of Image (JPEG_SOI)
                    0xFF, 0xC3, // Start Of Frame (lossless, Huffman) (JPEG_SOF_3)
                    0x00, 0x00 // Length of data of the marker
                };
            var exception = Assert.Throws<InvalidDataException>(() => Decode(source));
            Assert.AreEqual(JpegLSError.EncodingNotSupported, exception!.Data["JpegLSError"]);
        }

        [Test]
        public void DecodeBitStreamWithUnknownJpegMarker()
        {
            var source = new byte[]
                {
                    0xFF, 0xD8, // Start Of Image (JPEG_SOI)
                    0xFF, 0x01, // Undefined marker
                    0x00, 0x00 // Length of data of the marker
                };

            var exception = Assert.Throws<InvalidDataException>(() => Decode(source));
            Assert.AreEqual(JpegLSError.UnknownJpegMarkerFound, exception!.Data["JpegLSError"]);
        }

        private static byte[] TripletToPlanar(IList<byte> buffer, int width, int height)
        {
            var result = new byte[buffer.Count];

            int bytePlaneCount = width * height;
            for (int i = 0; i < bytePlaneCount; i++)
            {
                result[i] = buffer[i * 3];
                result[i + bytePlaneCount] = buffer[(i * 3) + 1];
                result[i + (2 * bytePlaneCount)] = buffer[(i * 3) + 2];
            }

            return result;
        }

        private static byte[] ReadAllBytes(string path, int bytesToSkip = 0)
        {
            var fullPath = DataFileDirectory + path;

            if (bytesToSkip == 0)
                return File.ReadAllBytes(fullPath);

            using var stream = File.OpenRead(fullPath);
            var result = new byte[new FileInfo(fullPath).Length - bytesToSkip];

            _ = stream.Seek(bytesToSkip, SeekOrigin.Begin);
            _ = stream.Read(result, 0, result.Length);
            return result;
        }

        private static string DataFileDirectory
        {
            get
            {
                var assemblyLocation = new Uri(Assembly.GetExecutingAssembly().Location);
                return Path.GetDirectoryName(assemblyLocation.LocalPath) + @"\DataFiles\";
            }
        }

        private static byte[] Decode(byte[] source)
        {
            using JpegLSDecoder decoder = new(source);
            return decoder.Decode();
        }
    }
}
