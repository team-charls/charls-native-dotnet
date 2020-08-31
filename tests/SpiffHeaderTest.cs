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
            var header = new SpiffHeader();

            Assert.AreEqual(SpiffProfileId.None, header.ProfileId);
            Assert.AreEqual(1, header.ComponentCount);
            Assert.AreEqual(1, header.Width);
            Assert.AreEqual(1, header.Height);
            Assert.AreEqual(SpiffColorSpace.None, header.ColorSpace);
            Assert.AreEqual(8, header.BitsPerSample);
            Assert.AreEqual(SpiffCompressionType.JpegLS, header.CompressionType);
            Assert.AreEqual(SpiffResolutionUnit.AspectRatio, header.ResolutionUnit);
            Assert.AreEqual(1, header.VerticalResolution);
            Assert.AreEqual(1, header.HorizontalResolution);
        }

        [Test]
        public void ConstructAndModify()
        {
            var header = new SpiffHeader {
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
        public void EquatableSameObjects()
        {
            var a = new SpiffHeader();
            var b = new SpiffHeader();

            Assert.IsTrue(a.Equals(b));
            Assert.IsTrue(a.Equals((object)b));
            Assert.AreEqual(a, b);
            Assert.AreEqual(b, a);
            Assert.AreEqual(a.GetHashCode(), b.GetHashCode());
        }

        [Test]
        public void EquatableDifferentObjects()
        {
            var a = new SpiffHeader();
            var b = new SpiffHeader { Height = 2 };

            Assert.IsFalse(a.Equals(b));
            Assert.IsFalse(a.Equals((object)b));
        }

        [Test]
        public void EquatableWithNull()
        {
            var a = new SpiffHeader();

            Assert.IsFalse(a.Equals(null));
            Assert.IsFalse(a.Equals((object)null));
        }
    }
}