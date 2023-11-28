// Copyright (c) Team CharLS.
// SPDX-License-Identifier: BSD-3-Clause

using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;

namespace CharLS.Native.Test;

[TestFixture]
public sealed class SpiffHeaderTest
{
    [Test]
    public void ConstructDefault()
    {
        SpiffHeader header = new();

        Assert.Multiple(() =>
        {
            Assert.That(header.ProfileId, Is.EqualTo(SpiffProfileId.None));
            Assert.That(header.ComponentCount, Is.EqualTo(0));
            Assert.That(header.Width, Is.EqualTo(0));
            Assert.That(header.Height, Is.EqualTo(0));
            Assert.That(header.ColorSpace, Is.EqualTo(SpiffColorSpace.None));
            Assert.That(header.BitsPerSample, Is.EqualTo(0));
            Assert.That(header.CompressionType, Is.EqualTo(SpiffCompressionType.JpegLS));
            Assert.That(header.ResolutionUnit, Is.EqualTo(SpiffResolutionUnit.AspectRatio));
            Assert.That(header.VerticalResolution, Is.EqualTo(1));
            Assert.That(header.HorizontalResolution, Is.EqualTo(1));
        });
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

        Assert.Multiple(() =>
        {
            Assert.That(header.ComponentCount, Is.EqualTo(3));
            Assert.That(header.Width, Is.EqualTo(512));
            Assert.That(header.Height, Is.EqualTo(1024));
            Assert.That(header.ColorSpace, Is.EqualTo(SpiffColorSpace.Rgb));
        });
    }

    [Test]
    public void ConstructFromUnsupportedNative()
    {
        SpiffHeaderNative native = new() {
            Height = uint.MaxValue,
            ColorSpace = SpiffColorSpace.Rgb
        };

        bool result = SpiffHeader.TryCreate(native, out var header);
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.False);
            Assert.That(header, Is.Null);
        });
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
        Assert.That(string.IsNullOrWhiteSpace(usefulText), Is.False);
    }

    [Test]
    public void EquatableSameObjects()
    {
        SpiffHeader a = new();
        SpiffHeader b = new();

        var equal = a.Equals(b);
        var equalObject = a.Equals((object)b);

        Assert.Multiple(() =>
        {
            Assert.That(equal, Is.True);
            Assert.That(equalObject, Is.True);
            Assert.That(b, Is.EqualTo(a));
            Assert.That(a, Is.EqualTo(b));
        });
        Assert.That(b.GetHashCode(), Is.EqualTo(a.GetHashCode()));
    }

    [Test]
    public void EquatableDifferentObjects()
    {
        SpiffHeader a = new();
        SpiffHeader b = new() { Height = 2 };

        var equal = a.Equals(b);
        var equalObject = a.Equals((object)b);

        Assert.Multiple(() =>
        {
            Assert.That(equal, Is.False);
            Assert.That(equalObject, Is.False);
        });
    }

    [Test]
    [SuppressMessage("Maintainability", "CA1508:Avoid dead conditional code", Justification = "Unit test code")]
    public void EquatableWithNull()
    {
        SpiffHeader a = new();

        Assert.Multiple(() =>
        {
            Assert.That(a, Is.Not.EqualTo(null));
            Assert.That(a!, Is.Not.EqualTo(null!));
        });
    }

    [Test]
    public void UseAllSpiffColorSpaceValues()
    {
        SpiffHeader header = new() { ColorSpace = SpiffColorSpace.BiLevelBlack };
        Assert.That(header.ColorSpace, Is.EqualTo(SpiffColorSpace.BiLevelBlack));

        header = new() { ColorSpace = SpiffColorSpace.YcbcrItuBT709Video };
        Assert.That(header.ColorSpace, Is.EqualTo(SpiffColorSpace.YcbcrItuBT709Video));

        header = new() { ColorSpace = SpiffColorSpace.None };
        Assert.That(header.ColorSpace, Is.EqualTo(SpiffColorSpace.None));

        header = new() { ColorSpace = SpiffColorSpace.YcbcrItuBT6011Rgb };
        Assert.That(header.ColorSpace, Is.EqualTo(SpiffColorSpace.YcbcrItuBT6011Rgb));

        header = new() { ColorSpace = SpiffColorSpace.YcbcrItuBT6011Video };
        Assert.That(header.ColorSpace, Is.EqualTo(SpiffColorSpace.YcbcrItuBT6011Video));

        header = new() { ColorSpace = SpiffColorSpace.Grayscale };
        Assert.That(header.ColorSpace, Is.EqualTo(SpiffColorSpace.Grayscale));

        header = new() { ColorSpace = SpiffColorSpace.PhotoYcc };
        Assert.That(header.ColorSpace, Is.EqualTo(SpiffColorSpace.PhotoYcc));

        header = new() { ColorSpace = SpiffColorSpace.Rgb };
        Assert.That(header.ColorSpace, Is.EqualTo(SpiffColorSpace.Rgb));

        header = new() { ColorSpace = SpiffColorSpace.Cmy };
        Assert.That(header.ColorSpace, Is.EqualTo(SpiffColorSpace.Cmy));

        header = new() { ColorSpace = SpiffColorSpace.Cmyk };
        Assert.That(header.ColorSpace, Is.EqualTo(SpiffColorSpace.Cmyk));

        header = new() { ColorSpace = SpiffColorSpace.Ycck };
        Assert.That(header.ColorSpace, Is.EqualTo(SpiffColorSpace.Ycck));

        header = new() { ColorSpace = SpiffColorSpace.CieLab };
        Assert.That(header.ColorSpace, Is.EqualTo(SpiffColorSpace.CieLab));

        header = new() { ColorSpace = SpiffColorSpace.BiLevelWhite };
        Assert.That(header.ColorSpace, Is.EqualTo(SpiffColorSpace.BiLevelWhite));
    }

    [Test]
    public void UseAllSpiffCompressionTypeValues()
    {
        SpiffHeader header = new() { CompressionType = SpiffCompressionType.Uncompressed };
        Assert.That(header.CompressionType, Is.EqualTo(SpiffCompressionType.Uncompressed));

        header = new() { CompressionType = SpiffCompressionType.ModifiedHuffman };
        Assert.That(header.CompressionType, Is.EqualTo(SpiffCompressionType.ModifiedHuffman));

        header = new() { CompressionType = SpiffCompressionType.ModifiedRead };
        Assert.That(header.CompressionType, Is.EqualTo(SpiffCompressionType.ModifiedRead));

        header = new() { CompressionType = SpiffCompressionType.ModifiedModifiedRead };
        Assert.That(header.CompressionType, Is.EqualTo(SpiffCompressionType.ModifiedModifiedRead));

        header = new() { CompressionType = SpiffCompressionType.JBig };
        Assert.That(header.CompressionType, Is.EqualTo(SpiffCompressionType.JBig));

        header = new() { CompressionType = SpiffCompressionType.Jpeg };
        Assert.That(header.CompressionType, Is.EqualTo(SpiffCompressionType.Jpeg));

        header = new() { CompressionType = SpiffCompressionType.JpegLS };
        Assert.That(header.CompressionType, Is.EqualTo(SpiffCompressionType.JpegLS));
    }

    [Test]
    public void UseAllSpiffProfileIdValues()
    {
        SpiffHeader header = new() { ProfileId = SpiffProfileId.None };
        Assert.That(header.ProfileId, Is.EqualTo(SpiffProfileId.None));

        header = new() { ProfileId = SpiffProfileId.ContinuousToneBase };
        Assert.That(header.ProfileId, Is.EqualTo(SpiffProfileId.ContinuousToneBase));

        header = new() { ProfileId = SpiffProfileId.ContinuousToneProgressive };
        Assert.That(header.ProfileId, Is.EqualTo(SpiffProfileId.ContinuousToneProgressive));

        header = new() { ProfileId = SpiffProfileId.BiLevelFacsimile };
        Assert.That(header.ProfileId, Is.EqualTo(SpiffProfileId.BiLevelFacsimile));

        header = new() { ProfileId = SpiffProfileId.ContinuousToneFacsimile };
        Assert.That(header.ProfileId, Is.EqualTo(SpiffProfileId.ContinuousToneFacsimile));
    }

    [Test]
    public void UseAllSpiffResolutionUnitValues()
    {
        SpiffHeader header = new() { ResolutionUnit = SpiffResolutionUnit.AspectRatio };
        Assert.That(header.ResolutionUnit, Is.EqualTo(SpiffResolutionUnit.AspectRatio));

        header = new() { ResolutionUnit = SpiffResolutionUnit.DotsPerInch };
        Assert.That(header.ResolutionUnit, Is.EqualTo(SpiffResolutionUnit.DotsPerInch));

        header = new() { ResolutionUnit = SpiffResolutionUnit.DotsPerCentimeter };
        Assert.That(header.ResolutionUnit, Is.EqualTo(SpiffResolutionUnit.DotsPerCentimeter));
    }
}
