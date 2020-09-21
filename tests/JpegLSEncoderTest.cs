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
    public class JpegLSEncoderTest
    {
        [Test]
        public void CreateEncoderWithBadWidth()
        {
            _ = Assert.Throws<ArgumentException>(() => {
                using var _ = new JpegLSEncoder(0, 1, 2, 1);
            });
        }

        [Test]
        public void CreateWithExtendedConstructor()
        {
            var expected = new FrameInfo(256, 500, 8, 3);
            using var encoder = new JpegLSEncoder(256, 500, 8, 3);

            Assert.AreEqual(expected, encoder.FrameInfo);
        }

        [Test]
        public void InitializeFrameInfoWithNull()
        {
            using var encoder = new JpegLSEncoder();
            _ = Assert.Throws<ArgumentNullException>(() => {
                encoder.FrameInfo = null;
            });
        }

        [Test]
        public void GetAndSetNearLossless()
        {
            using var encoder = new JpegLSEncoder();

            Assert.AreEqual(0, encoder.NearLossless);

            encoder.NearLossless = 1;
            Assert.AreEqual(1, encoder.NearLossless);
        }

        [Test]
        public void GetAndSetInterleaveMode()
        {
            using var encoder = new JpegLSEncoder();

            Assert.AreEqual(JpegLSInterleaveMode.None, encoder.InterleaveMode);

            encoder.InterleaveMode = JpegLSInterleaveMode.Line;
            Assert.AreEqual(JpegLSInterleaveMode.Line, encoder.InterleaveMode);
        }

        [Test]
        public void GetAndSetPresetCodingParameters()
        {
            using var encoder = new JpegLSEncoder();

            Assert.IsNull(encoder.PresetCodingParameters);

            var presetCodingParameters = new JpegLSPresetCodingParameters(255, 9, 10, 11, 31);
            encoder.PresetCodingParameters = presetCodingParameters;
            Assert.AreEqual(presetCodingParameters, encoder.PresetCodingParameters);
        }

        [Test]
        public void InitializePresetCodingParametersWithNull()
        {
            using var encoder = new JpegLSEncoder();
            _ = Assert.Throws<ArgumentNullException>(() => {
                encoder.PresetCodingParameters = null;
            });
        }

        [Test]
        public void SetDestinationWithEmptyBuffer()
        {
            using var encoder = new JpegLSEncoder();
            _ = Assert.Throws<ArgumentException>(() => {
                encoder.Destination = Memory<byte>.Empty; ;
            });
        }
    }
}