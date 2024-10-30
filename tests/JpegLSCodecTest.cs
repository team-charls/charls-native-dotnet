// Copyright (c) Team CharLS.
// SPDX-License-Identifier: BSD-3-Clause

using System.Reflection;
using NUnit.Framework;

namespace CharLS.Native.Test;

[TestFixture]
internal sealed class JpegLSCodecTest
{
    [Test]
    public void GetMetadataInfoFromLosslessEncodedColorImage()
    {
        using JpegLSDecoder decoder = new(ReadAllBytes("t8c0e0.jls"));

        Assert.Multiple(() =>
        {
            Assert.That(decoder.FrameInfo.Height, Is.EqualTo(256));
            Assert.That(decoder.FrameInfo.Width, Is.EqualTo(256));
            Assert.That(decoder.FrameInfo.BitsPerSample, Is.EqualTo(8));
            Assert.That(decoder.FrameInfo.ComponentCount, Is.EqualTo(3));
            Assert.That(decoder.NearLossless, Is.EqualTo(0));
        });
    }

    [Test]
    public void GetMetadataInfoFromNearLosslessEncodedColorImage()
    {
        using JpegLSDecoder decoder = new(ReadAllBytes("t8c0e3.jls"));

        var frameInfo = decoder.FrameInfo;
        Assert.Multiple(() =>
        {
            Assert.That(frameInfo.Height, Is.EqualTo(256));
            Assert.That(frameInfo.Width, Is.EqualTo(256));
            Assert.That(frameInfo.BitsPerSample, Is.EqualTo(8));
            Assert.That(frameInfo.ComponentCount, Is.EqualTo(3));
            Assert.That(decoder.NearLossless, Is.EqualTo(3));
        });
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

        Assert.That(uncompressed, Is.EqualTo(expected));
    }

    [Test]
    public void Encode()
    {
        FrameInfo info = new(256, 256, 8, 3);

        var uncompressedOriginal = ReadAllBytes("test8.ppm", 15);
        uncompressedOriginal = TripletToPlanar(uncompressedOriginal, info.Width, info.Height);

        using JpegLSEncoder encoder = new();
        encoder.FrameInfo = info;

        encoder.Destination = new byte[encoder.EstimatedDestinationSize];
        encoder.Encode(uncompressedOriginal);

        using JpegLSDecoder decoder = new(encoder.EncodedData);
        Assert.That(decoder.FrameInfo, Is.EqualTo(info));

        var uncompressed = decoder.Decode();
        Assert.That(uncompressed, Has.Length.EqualTo(uncompressedOriginal.Length));
        Assert.That(uncompressed, Is.EqualTo(uncompressedOriginal));
    }

    [Test]
    public void EncodeWithPresetCodingParameters()
    {
        FrameInfo info = new(256, 256, 8, 3);

        var uncompressedOriginal = ReadAllBytes("test8.ppm", 15);
        uncompressedOriginal = TripletToPlanar(uncompressedOriginal, info.Width, info.Height);

        JpegLSPresetCodingParameters presetCodingParameters = new(255, 9, 10, 11, 31);
        using JpegLSEncoder encoder = new(info, false);
        encoder.PresetCodingParameters = presetCodingParameters;

        encoder.Destination = new byte[encoder.EstimatedDestinationSize];
        encoder.Encode(uncompressedOriginal);

        using JpegLSDecoder decoder = new(encoder.EncodedData);
        var pcp = decoder.PresetCodingParameters;

        Assert.That(pcp, Is.EqualTo(presetCodingParameters));

        var uncompressed = decoder.Decode();
        Assert.That(uncompressed, Has.Length.EqualTo(uncompressedOriginal.Length));
        Assert.That(uncompressed, Is.EqualTo(uncompressedOriginal));
    }

    [Test]
    public void EncodeOneByOneColor()
    {
        var uncompressedOriginal = new byte[] { 77, 33, 255 };
        using JpegLSEncoder encoder = new(1, 1, 8, 3);

        encoder.Encode(uncompressedOriginal);

        using JpegLSDecoder decoder = new(encoder.Destination);

        var uncompressed = decoder.Decode();
        Assert.That(uncompressed, Has.Length.EqualTo(uncompressedOriginal.Length));
        Assert.That(uncompressed, Is.EqualTo(uncompressedOriginal));
    }

    [Test]
    public void Encode2BitMonochrome()
    {
        var uncompressedOriginal = new byte[] { 1 };
        using JpegLSEncoder encoder = new(1, 1, 2, 1);

        encoder.Encode(uncompressedOriginal);

        using JpegLSDecoder decoder = new(encoder.Destination);

        var uncompressed = decoder.Decode();
        Assert.That(uncompressed, Has.Length.EqualTo(uncompressedOriginal.Length));
        Assert.That(uncompressed, Is.EqualTo(uncompressedOriginal));
    }

    [Test]
    public void WriteStandardSpiffHeader()
    {
        var uncompressedOriginal = new byte[] { 1 };
        using JpegLSEncoder encoder = new(1, 1, 2, 1);

        encoder.WriteStandardSpiffHeader(SpiffColorSpace.Grayscale);
        encoder.Encode(uncompressedOriginal);

        using JpegLSDecoder decoder = new(encoder.Destination, false);
        bool spiffHeaderPresent = decoder.TryReadSpiffHeader(out var spiffHeader);

        Assert.Multiple(() =>
        {
            Assert.That(spiffHeaderPresent, Is.True);

            Assert.That(spiffHeader!.ProfileId, Is.EqualTo(SpiffProfileId.None));
            Assert.That(spiffHeader.ComponentCount, Is.EqualTo(1));
            Assert.That(spiffHeader.Height, Is.EqualTo(1));
            Assert.That(spiffHeader.Width, Is.EqualTo(1));
            Assert.That(spiffHeader.ColorSpace, Is.EqualTo(SpiffColorSpace.Grayscale));
            Assert.That(spiffHeader.BitsPerSample, Is.EqualTo(2));
            Assert.That(spiffHeader.CompressionType, Is.EqualTo(SpiffCompressionType.JpegLS));
            Assert.That(spiffHeader.ResolutionUnit, Is.EqualTo(SpiffResolutionUnit.AspectRatio));
            Assert.That(spiffHeader.VerticalResolution, Is.EqualTo(1));
            Assert.That(spiffHeader.HorizontalResolution, Is.EqualTo(1));
        });
    }

    [Test]
    public void WriteStandardSpiffHeaderReadWithProperty()
    {
        var uncompressedOriginal = new byte[] { 1 };
        using JpegLSEncoder encoder = new(1, 1, 2, 1);

        encoder.WriteStandardSpiffHeader(SpiffColorSpace.Grayscale);
        encoder.Encode(uncompressedOriginal);

        using JpegLSDecoder decoder = new(encoder.Destination);

        var spiffHeader = decoder.SpiffHeader;
        Assert.That(spiffHeader, Is.Not.Null);
        if (decoder.SpiffHeader == null)
            return;

        Assert.Multiple(() =>
        {
            Assert.That(spiffHeader!.ProfileId, Is.EqualTo(SpiffProfileId.None));
            Assert.That(decoder.SpiffHeader.ComponentCount, Is.EqualTo(1));
            Assert.That(spiffHeader.Height, Is.EqualTo(1));
            Assert.That(spiffHeader.Width, Is.EqualTo(1));
            Assert.That(spiffHeader.ColorSpace, Is.EqualTo(SpiffColorSpace.Grayscale));
            Assert.That(spiffHeader.BitsPerSample, Is.EqualTo(2));
            Assert.That(spiffHeader.CompressionType, Is.EqualTo(SpiffCompressionType.JpegLS));
            Assert.That(spiffHeader.ResolutionUnit, Is.EqualTo(SpiffResolutionUnit.AspectRatio));
            Assert.That(spiffHeader.VerticalResolution, Is.EqualTo(1));
            Assert.That(spiffHeader.HorizontalResolution, Is.EqualTo(1));
        });
    }

    [Test]
    public void WriteSpiffHeader()
    {
        var uncompressedOriginal = new byte[] { 1 };
        using JpegLSEncoder encoder = new(1, 1, 2, 1);

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

        Assert.Multiple(() =>
        {
            Assert.That(spiffHeaderPresent, Is.True);

            Assert.That(spiffHeader!.ProfileId, Is.EqualTo(SpiffProfileId.None));
            Assert.That(spiffHeader.ComponentCount, Is.EqualTo(1));
            Assert.That(spiffHeader.Height, Is.EqualTo(1));
            Assert.That(spiffHeader.Width, Is.EqualTo(1));
            Assert.That(spiffHeader.ColorSpace, Is.EqualTo(SpiffColorSpace.Grayscale));
            Assert.That(spiffHeader.BitsPerSample, Is.EqualTo(2));
            Assert.That(spiffHeader.CompressionType, Is.EqualTo(SpiffCompressionType.JpegLS));
            Assert.That(spiffHeader.ResolutionUnit, Is.EqualTo(SpiffResolutionUnit.AspectRatio));
            Assert.That(spiffHeader.VerticalResolution, Is.EqualTo(1));
            Assert.That(spiffHeader.HorizontalResolution, Is.EqualTo(1));
        });
    }

    [Test]
    public void DecodeBitStreamWithNoMarkerStart()
    {
        var source = "33"u8.ToArray();

        var exception = Assert.Throws<InvalidDataException>(() => Decode(source));
        Assert.That(exception!.Data["JpegLSError"], Is.EqualTo(JpegLSError.JpegMarkerStartByteNotFound));
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
        Assert.That(exception!.Data["JpegLSError"], Is.EqualTo(JpegLSError.EncodingNotSupported));
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
        Assert.That(exception!.Data["JpegLSError"], Is.EqualTo(JpegLSError.UnknownJpegMarkerFound));
    }

    [Test]
    public void EncodeOversizedImageWidth()
    {
        const int size = ushort.MaxValue + 1;
        using JpegLSEncoder encoder = new(new FrameInfo(size, 1, 8, 1));
        var source = new byte[size];
        encoder.Encode(source);

        using JpegLSDecoder decoder = new(encoder.EncodedData);
        var destination = new byte[size];
        decoder.Decode(destination);

        Assert.That(destination, Is.EqualTo(source));
    }

    [Test]
    public void EncodeOversizedImageHeight()
    {
        const int size = ushort.MaxValue + 1;
        using JpegLSEncoder encoder = new(new FrameInfo(1, size, 8, 1));
        var source = new byte[size];
        encoder.Encode(source);

        using JpegLSDecoder decoder = new(encoder.EncodedData);
        var destination = new byte[size];
        decoder.Decode(destination);

        Assert.That(destination, Is.EqualTo(source));
    }

    private static byte[] TripletToPlanar(byte[] buffer, int width, int height)
    {
        var result = new byte[buffer.Length];

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
#if NET8_0_OR_GREATER
        var fullPath = Path.Join(DataFileDirectory, path);
#else
        var fullPath = Path.Combine(DataFileDirectory, path);
#endif

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
#if NET8_0_OR_GREATER
            return Path.Join(Path.GetDirectoryName(assemblyLocation.LocalPath), "DataFiles");
#else
            return Path.Combine(Path.GetDirectoryName(assemblyLocation.LocalPath), "DataFiles");
#endif
        }
    }

    private static byte[] Decode(byte[] source)
    {
        using JpegLSDecoder decoder = new(source);
        return decoder.Decode();
    }
}
