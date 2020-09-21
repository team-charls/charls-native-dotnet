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

            using var decoder = new JpegLSDecoder(source);
            decoder.ReadHeader();

            var presetCodingParameters = decoder.PresetCodingParameters;

            Assert.AreEqual(255, presetCodingParameters.MaximumSampleValue);
            Assert.AreEqual(9, presetCodingParameters.Threshold1);
            Assert.AreEqual(9, presetCodingParameters.Threshold2);
            Assert.AreEqual(9, presetCodingParameters.Threshold3);
            Assert.AreEqual(31, presetCodingParameters.ResetValue);
        }

        [Test]
        public void SetSourceWithEmptyBuffer()
        {
            using var decoder = new JpegLSDecoder();
            _ = Assert.Throws<ArgumentException>(() => {
                decoder.SetSource(Memory<byte>.Empty);
            });
        }

        [Test]
        public void SetSourceTwice()
        {
            using var decoder = new JpegLSDecoder();

            decoder.SetSource(new byte[10]);

            _ = Assert.Throws<InvalidOperationException>(() => {
                decoder.SetSource(new byte[20]);
            });
        }

        [Test]
        public void TryReadSpiffHeaderWhenNotPresent()
        {
            byte[] source = ReadAllBytes("T8NDE0.JLS");

            using var decoder = new JpegLSDecoder(source);
            bool result = decoder.TryReadSpiffHeader(out var header);

            Assert.IsFalse(result);
            Assert.IsNull(header);
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
    }
}
