// Copyright (c) Team CharLS.
// SPDX-License-Identifier: BSD-3-Clause

using System.Diagnostics.CodeAnalysis;
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
        [SuppressMessage("Maintainability", "CA1508:Avoid dead conditional code", Justification = "Unit test code")]
        public void EquatableWithNull()
        {
            SpiffHeader a = new();

            Assert.IsFalse(a.Equals(null));
            Assert.IsFalse(a!.Equals((object)null!));
        }

        [Test]
        public void UseAllSpiffColorSpaceValues()
        {
            SpiffHeader header = new() { ColorSpace = SpiffColorSpace.BiLevelBlack };
            Assert.AreEqual(SpiffColorSpace.BiLevelBlack, header.ColorSpace);

            header = new() { ColorSpace = SpiffColorSpace.YcbcrItuBT709Video };
            Assert.AreEqual(SpiffColorSpace.YcbcrItuBT709Video, header.ColorSpace);

            header = new() { ColorSpace = SpiffColorSpace.None };
            Assert.AreEqual(SpiffColorSpace.None, header.ColorSpace);

            header = new() { ColorSpace = SpiffColorSpace.YcbcrItuBT6011Rgb };
            Assert.AreEqual(SpiffColorSpace.YcbcrItuBT6011Rgb, header.ColorSpace);

            header = new() { ColorSpace = SpiffColorSpace.YcbcrItuBT6011Video };
            Assert.AreEqual(SpiffColorSpace.YcbcrItuBT6011Video, header.ColorSpace);

            header = new() { ColorSpace = SpiffColorSpace.Grayscale };
            Assert.AreEqual(SpiffColorSpace.Grayscale, header.ColorSpace);

            header = new() { ColorSpace = SpiffColorSpace.PhotoYcc };
            Assert.AreEqual(SpiffColorSpace.PhotoYcc, header.ColorSpace);

            header = new() { ColorSpace = SpiffColorSpace.Rgb };
            Assert.AreEqual(SpiffColorSpace.Rgb, header.ColorSpace);

            header = new() { ColorSpace = SpiffColorSpace.Cmy };
            Assert.AreEqual(SpiffColorSpace.Cmy, header.ColorSpace);

            header = new() { ColorSpace = SpiffColorSpace.Cmyk };
            Assert.AreEqual(SpiffColorSpace.Cmyk, header.ColorSpace);

            header = new() { ColorSpace = SpiffColorSpace.Ycck };
            Assert.AreEqual(SpiffColorSpace.Ycck, header.ColorSpace);

            header = new() { ColorSpace = SpiffColorSpace.CieLab };
            Assert.AreEqual(SpiffColorSpace.CieLab, header.ColorSpace);

            header = new() { ColorSpace = SpiffColorSpace.BiLevelWhite };
            Assert.AreEqual(SpiffColorSpace.BiLevelWhite, header.ColorSpace);
        }

        [Test]
        public void UseAllSpiffCompressionTypeValues()
        {
            SpiffHeader header = new() { CompressionType = SpiffCompressionType.Uncompressed };
            Assert.AreEqual(SpiffCompressionType.Uncompressed, header.CompressionType);

            header = new() { CompressionType = SpiffCompressionType.ModifiedHuffman };
            Assert.AreEqual(SpiffCompressionType.ModifiedHuffman, header.CompressionType);

            header = new() { CompressionType = SpiffCompressionType.ModifiedRead };
            Assert.AreEqual(SpiffCompressionType.ModifiedRead, header.CompressionType);

            header = new() { CompressionType = SpiffCompressionType.ModifiedModifiedRead };
            Assert.AreEqual(SpiffCompressionType.ModifiedModifiedRead, header.CompressionType);

            header = new() { CompressionType = SpiffCompressionType.JBig };
            Assert.AreEqual(SpiffCompressionType.JBig, header.CompressionType);

            header = new() { CompressionType = SpiffCompressionType.Jpeg };
            Assert.AreEqual(SpiffCompressionType.Jpeg, header.CompressionType);

            header = new() { CompressionType = SpiffCompressionType.JpegLS };
            Assert.AreEqual(SpiffCompressionType.JpegLS, header.CompressionType);
        }

        [Test]
        public void UseAllSpiffProfileIdValues()
        {
            SpiffHeader header = new() { ProfileId = SpiffProfileId.None };
            Assert.AreEqual(SpiffProfileId.None, header.ProfileId);

            header = new() { ProfileId = SpiffProfileId.ContinuousToneBase };
            Assert.AreEqual(SpiffProfileId.ContinuousToneBase, header.ProfileId);

            header = new() { ProfileId = SpiffProfileId.ContinuousToneProgressive };
            Assert.AreEqual(SpiffProfileId.ContinuousToneProgressive, header.ProfileId);

            header = new() { ProfileId = SpiffProfileId.BiLevelFacsimile };
            Assert.AreEqual(SpiffProfileId.BiLevelFacsimile, header.ProfileId);

            header = new() { ProfileId = SpiffProfileId.ContinuousToneFacsimile };
            Assert.AreEqual(SpiffProfileId.ContinuousToneFacsimile, header.ProfileId);
        }

        [Test]
        public void UseAllSpiffResolutionUnitValues()
        {
            SpiffHeader header = new() { ResolutionUnit = SpiffResolutionUnit.AspectRatio };
            Assert.AreEqual(SpiffResolutionUnit.AspectRatio, header.ResolutionUnit);

            header = new() { ResolutionUnit = SpiffResolutionUnit.DotsPerInch };
            Assert.AreEqual(SpiffResolutionUnit.DotsPerInch, header.ResolutionUnit);

            header = new() { ResolutionUnit = SpiffResolutionUnit.DotsPerCentimeter };
            Assert.AreEqual(SpiffResolutionUnit.DotsPerCentimeter, header.ResolutionUnit);
        }
    }
}
