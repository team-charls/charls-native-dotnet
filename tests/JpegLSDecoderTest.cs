// Copyright (c) Team CharLS.
// SPDX-License-Identifier: BSD-3-Clause

using System;
using System.IO;
using System.Reflection;
using NUnit.Framework;

namespace CharLS.Native.Test
{
    [TestFixture]
    public class JpegLSDecoderTest
    {
        [Test]
        public void ReadPresetCodingParameters()
        {
            byte[] source = ReadAllBytes("T8NDE0.JLS");
            using JpegLSDecoder decoder = new(source);
            var presetCodingParameters = decoder.PresetCodingParameters;

            Assert.AreEqual(255, presetCodingParameters.MaximumSampleValue);
            Assert.AreEqual(9, presetCodingParameters.Threshold1);
            Assert.AreEqual(9, presetCodingParameters.Threshold2);
            Assert.AreEqual(9, presetCodingParameters.Threshold3);
            Assert.AreEqual(31, presetCodingParameters.ResetValue);
        }

        [Test]
        public void CreateWithEmptyBuffer()
        {
            _ = Assert.Throws<ArgumentException>(() => {
                using JpegLSDecoder _ = new(Memory<byte>.Empty, false);
            });
        }

        [Test]
        public void SetSourceWithEmptyBuffer()
        {
            using JpegLSDecoder decoder = new();
            _ = Assert.Throws<ArgumentException>(() => {
                decoder.Source = Memory<byte>.Empty;
            });
        }

        [Test]
        public void SetSourceTwice()
        {
            using JpegLSDecoder decoder = new() { Source = new byte[10] };

            _ = Assert.Throws<InvalidOperationException>(() => {
                decoder.Source = new byte[20];
            });
        }

        [Test]
        public void TryReadSpiffHeaderWhenNotPresent()
        {
            byte[] source = ReadAllBytes("T8NDE0.JLS");

            using JpegLSDecoder decoder = new(source, false);
            bool result = decoder.TryReadSpiffHeader(out var header);

            Assert.IsFalse(result);
            Assert.IsNull(header);
        }

        [Test]
        public void TryReadSpiffHeaderWhenNotPresentUsingReadDirectConstructor()
        {
            byte[] source = ReadAllBytes("T8NDE0.JLS");

            using JpegLSDecoder decoder = new(source);

            Assert.IsNull(decoder.SpiffHeader);
        }

        [Test]
        public void GetDestinationSizeWithNegativeStride()
        {
            byte[] source = ReadAllBytes("T8NDE0.JLS");

            using JpegLSDecoder decoder = new(source);
            _ = Assert.Throws<ArgumentOutOfRangeException>(() => {
                var _ = decoder.GetDestinationSize(-1);
            });
        }

        [Test]
        public void DecodeToBufferWithNegativeStride()
        {
            byte[] source = ReadAllBytes("T8NDE0.JLS");

            using JpegLSDecoder decoder = new(source);
            var buffer = new byte[decoder.GetDestinationSize()];
            _ = Assert.Throws<ArgumentOutOfRangeException>(() => {
                decoder.Decode(buffer, -1);
            });
        }

        [Test]
        public void DecodeToBufferWithNull()
        {
            byte[] source = ReadAllBytes("T8NDE0.JLS");

            using JpegLSDecoder decoder = new(source);
            _ = Assert.Throws<ArgumentException>(() => {
                decoder.Decode(null);
            });
        }

        [Test]
        public void GetTheSource()
        {
            byte[] source = ReadAllBytes("T8NDE0.JLS");

            using JpegLSDecoder decoder = new(source);

            Assert.IsNotNull(decoder.Source);
        }

        [Test]
        public void UseAfterDispose()
        {
            JpegLSDecoder decoder = new();
            decoder.Dispose();

            _ = Assert.Throws<ObjectDisposedException>(() => {
                var _ = decoder.TryReadSpiffHeader(out var _);
            });

            Assert.IsNotNull(decoder.Source);

            _ = Assert.Throws<ObjectDisposedException>(() => {
                var _ = decoder.GetDestinationSize();
            });

            _ = Assert.Throws<ObjectDisposedException>(() => {
                var _ = decoder.Decode();
            });
        }

        [Test]
        public void UseBeforeReadHeader()
        {
            using JpegLSDecoder decoder = new();

            _ = Assert.Throws<InvalidOperationException>(() => {
                var _ = decoder.GetDestinationSize();
            });
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
                Uri assemblyLocation = new(Assembly.GetExecutingAssembly().Location);
                return Path.GetDirectoryName(assemblyLocation.LocalPath) + @"\DataFiles\";
            }
        }
    }
}
