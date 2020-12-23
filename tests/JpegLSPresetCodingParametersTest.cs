// Copyright (c) Team CharLS.
// SPDX-License-Identifier: BSD-3-Clause

using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;

namespace CharLS.Native.Test
{
    [TestFixture]
    public class JpegLSPresetCodingParametersTest
    {
        [Test]
        public void ConstructDefault()
        {
            JpegLSPresetCodingParameters presetCodingParameters = new(255, 9, 10, 11, 31);

            Assert.AreEqual(255, presetCodingParameters.MaximumSampleValue);
            Assert.AreEqual(9, presetCodingParameters.Threshold1);
            Assert.AreEqual(10, presetCodingParameters.Threshold2);
            Assert.AreEqual(11, presetCodingParameters.Threshold3);
            Assert.AreEqual(31, presetCodingParameters.ResetValue);
        }

        [Test]
        public void ConstructWithProperties()
        {
            JpegLSPresetCodingParameters presetCodingParameters = new()
            {
                MaximumSampleValue = 255,
                Threshold1 = 9,
                Threshold2 = 10,
                Threshold3 = 11,
                ResetValue =  31
            };

            Assert.AreEqual(255, presetCodingParameters.MaximumSampleValue);
            Assert.AreEqual(9, presetCodingParameters.Threshold1);
            Assert.AreEqual(10, presetCodingParameters.Threshold2);
            Assert.AreEqual(11, presetCodingParameters.Threshold3);
            Assert.AreEqual(31, presetCodingParameters.ResetValue);
        }

        [Test]
        public void EquatableSameObjects()
        {
            JpegLSPresetCodingParameters a = new(255, 9, 10, 11, 31);
            JpegLSPresetCodingParameters b = new(255, 9, 10, 11, 31);

            Assert.IsTrue(a.Equals(b));
            Assert.IsTrue(a.Equals((object)b));
            Assert.AreEqual(a, b);
            Assert.AreEqual(b, a);
            Assert.AreEqual(a.GetHashCode(), b.GetHashCode());
            Assert.IsTrue(a == b);
        }

        [Test]
        public void EquatableDifferentObjects()
        {
            JpegLSPresetCodingParameters a = new(255, 9, 10, 11, 31);
            JpegLSPresetCodingParameters b = new(255, 9, 10, 11, 32);

            Assert.IsFalse(a.Equals(b));
            Assert.IsFalse(a.Equals((object)b));
            Assert.IsTrue(a != b);
        }

        [Test]
        [SuppressMessage("Maintainability", "CA1508:Avoid dead conditional code", Justification = "Unit test code")]
        public void EquatableWithNull()
        {
            JpegLSPresetCodingParameters a = new(2556, 9, 10, 11, 31);

            Assert.IsFalse(a.Equals(null!));
            Assert.IsFalse(a!.Equals((object)null!));
        }
    }
}
