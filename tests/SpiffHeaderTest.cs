// Copyright (c) Team CharLS.
// SPDX-License-Identifier: BSD-3-Clause

using NUnit.Framework;

namespace CharLS.Native.Test
{
    [TestFixture]
    public class SpiffHeaderTest
    {
        [Test]
        public void ConstructDefault()
        {
            SpiffHeader header = new();

            Assert.AreEqual(SpiffProfileId.None, header.ProfileId);
            Assert.AreEqual(0, header.ComponentCount);
            Assert.AreEqual(0, header.Width);
            Assert.AreEqual(0, header.Height);
            Assert.AreEqual(SpiffColorSpace.None, header.ColorSpace);
            Assert.AreEqual(0, header.BitsPerSample);
            Assert.AreEqual(SpiffCompressionType.JpegLS, header.CompressionType);
            Assert.AreEqual(SpiffResolutionUnit.AspectRatio, header.ResolutionUnit);
            Assert.AreEqual(1, header.VerticalResolution);
            Assert.AreEqual(1, header.HorizontalResolution);
        }

        [Test]
        public void ConstructAndModify()
        {
            SpiffHeader header = new() {
                ComponentCount = 3,
                Width = 512,
                Height = 1024,
                ColorSpace = SpiffColorSpace.Rgb
            };

            Assert.AreEqual(3, header.ComponentCount);
            Assert.AreEqual(512, header.Width);
            Assert.AreEqual(1024, header.Height);
            Assert.AreEqual(SpiffColorSpace.Rgb, header.ColorSpace);
        }

        [Test]
        public void ConstructFromUnsupportedNative()
        {
            SpiffHeaderNative native = new() {
                Height = uint.MaxValue,
                ColorSpace = SpiffColorSpace.Rgb
            };

            bool result = SpiffHeader.TryCreate(native, out var header);
            Assert.IsFalse(result);
            Assert.IsNull(header);
        }

        [Test]
        public void ToStringIsNotDefault()
        {
            SpiffHeader header = new() {
                ComponentCount = 3,
                Width = 512,
                Height = 1024,
                ColorSpace = SpiffColorSpace.Rgb
            };

            var usefulText = header.ToString();
            Assert.IsFalse(string.IsNullOrWhiteSpace(usefulText));
        }

        [Test]
        public void EquatableSameObjects()
        {
            SpiffHeader a = new();
            SpiffHeader b = new();

            Assert.IsTrue(a.Equals(b));
            Assert.IsTrue(a.Equals((object)b));
            Assert.AreEqual(a, b);
            Assert.AreEqual(b, a);
            Assert.AreEqual(a.GetHashCode(), b.GetHashCode());
        }

        [Test]
        public void EquatableDifferentObjects()
        {
            SpiffHeader a = new();
            SpiffHeader b = new() { Height = 2 };

            Assert.IsFalse(a.Equals(b));
            Assert.IsFalse(a.Equals((object)b));
        }

        [Test]
        public void EquatableWithNull()
        {
            SpiffHeader a = new();

            Assert.IsFalse(a.Equals(null));
            Assert.IsFalse(a!.Equals((object)null!));
        }
    }
}