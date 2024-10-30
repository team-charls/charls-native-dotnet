// Copyright (c) Team CharLS.
// SPDX-License-Identifier: BSD-3-Clause

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using NUnit.Framework;

namespace CharLS.Native.Test;

[TestFixture]
internal sealed class JpegLSDecoderTest
{
    [Test]
    public void ReadPresetCodingParameters()
    {
        byte[] source = ReadAllBytes("t8nde0.jls");
        using JpegLSDecoder decoder = new(source);
        var presetCodingParameters = decoder.PresetCodingParameters;

        Assert.Multiple(() =>
        {
            Assert.That(presetCodingParameters.MaximumSampleValue, Is.EqualTo(255));
            Assert.That(presetCodingParameters.Threshold1, Is.EqualTo(9));
            Assert.That(presetCodingParameters.Threshold2, Is.EqualTo(9));
            Assert.That(presetCodingParameters.Threshold3, Is.EqualTo(9));
            Assert.That(presetCodingParameters.ResetValue, Is.EqualTo(31));
        });
    }

    [Test]
    public void CreateWithEmptyBuffer()
    {
        if (!CanHandleEmptyBuffer())
            return;

        Assert.DoesNotThrow(() =>
        {
            using JpegLSDecoder _ = new(Memory<byte>.Empty, false);
        });
    }

    [Test]
    public void SetSourceWithEmptyBuffer()
    {
        if (!CanHandleEmptyBuffer())
            return;

        using JpegLSDecoder decoder = new();
        Assert.DoesNotThrow(() =>
        {
            decoder.Source = Memory<byte>.Empty;
        });
    }

    [Test]
    public void SetSourceTwice()
    {
        using JpegLSDecoder decoder = new(new byte[10], false);

        _ = Assert.Throws<InvalidOperationException>(() =>
        {
            decoder.Source = new byte[20];
        });
    }

    [Test]
    public void TryReadSpiffHeaderWhenNotPresent()
    {
        byte[] source = ReadAllBytes("t8nde0.jls");

        using JpegLSDecoder decoder = new(source, false);
        bool result = decoder.TryReadSpiffHeader(out var header);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.False);
            Assert.That(header, Is.Null);
        });
    }

    [Test]
    public void TryReadSpiffHeaderWhenNotPresentUsingReadDirectConstructor()
    {
        byte[] source = ReadAllBytes("t8nde0.jls");

        using JpegLSDecoder decoder = new(source);

        Assert.That(decoder.SpiffHeader, Is.Null);
    }

    [Test]
    public void GetDestinationSizeWithNegativeStride()
    {
        byte[] source = ReadAllBytes("t8nde0.jls");

        using JpegLSDecoder decoder = new(source);
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => { _ = decoder.GetDestinationSize(-1); });
    }

    [Test]
    public void DecodeToBufferWithNegativeStride()
    {
        byte[] source = ReadAllBytes("t8nde0.jls");

        using JpegLSDecoder decoder = new(source);
        var buffer = new byte[decoder.GetDestinationSize()];
        _ = Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            decoder.Decode(buffer, -1);
        });
    }

    [Test]
    public void DecodeToBufferWithNull()
    {
        byte[] source = ReadAllBytes("t8nde0.jls");

        using JpegLSDecoder decoder = new(source);
        _ = Assert.Throws<ArgumentException>(() =>
        {
            decoder.Decode(null);
        });
    }

    [Test]
    public void GetTheSource()
    {
        byte[] source = ReadAllBytes("t8nde0.jls");

        using JpegLSDecoder decoder = new(source);

        Assert.That(decoder.Source.IsEmpty, Is.False);
    }

    [Test]
    public void UseAfterDisposeThrows()
    {
        JpegLSDecoder decoder = new();
        decoder.Dispose();

        _ = Assert.Throws<ObjectDisposedException>(() => { _ = decoder.TryReadSpiffHeader(out _); });

        Assert.That(decoder.Source.ToArray(), Is.Not.Null);

        _ = Assert.Throws<ObjectDisposedException>(() => { _ = decoder.GetDestinationSize(); });

        _ = Assert.Throws<ObjectDisposedException>(() => { _ = decoder.Decode(); });
    }

    [Test]
    public void UseBeforeReadHeaderThrows()
    {
        using JpegLSDecoder decoder = new();

        _ = Assert.Throws<InvalidOperationException>(() =>
        {
            _ = decoder.GetDestinationSize();
        });
    }

    [Test]
    public void CommentHandlerReceivesCommentBytes()
    {
        using JpegLSEncoder encoder = new(1, 1, 8, 1, true, 100);

        var comment1 = new byte[] { 1, 2, 3, 4 };
        encoder.WriteComment(comment1);
        encoder.Encode(new byte[1]);

        byte[]? comment2 = null;
        using JpegLSDecoder decoder = new(encoder.EncodedData, false);

        decoder.Comment += CommentHandler;
        decoder.Comment -= CommentHandler;
        decoder.Comment += CommentHandler;
        decoder.ReadHeader();

        Assert.That(comment2, Is.Not.Null);
        Assert.That(comment2!, Has.Length.EqualTo(4));
        Assert.Multiple(() =>
        {
            Assert.That(comment2![0], Is.EqualTo(1));
            Assert.That(comment2![1], Is.EqualTo(2));
            Assert.That(comment2![2], Is.EqualTo(3));
            Assert.That(comment2![3], Is.EqualTo(4));
        });
        return;

        void CommentHandler(object? _, CommentEventArgs e)
        {
            comment2 = e.Data.ToArray();
        }
    }

    [Test]
    public void CommentHandlerSubscribeWithNull()
    {
        using JpegLSDecoder decoder = new();
        decoder.Comment += null;

        Assert.Pass();
    }

    [Test]
    public void CommentHandlerUnsubscribeWithNull()
    {
        using JpegLSDecoder decoder = new();
        decoder.Comment -= null;

        Assert.Pass();
    }

    [Test]
    public void ReadCommentWithoutHandler()
    {
        using JpegLSEncoder encoder = new(new FrameInfo(1, 1, 8, 1), true, 100);

        encoder.WriteComment("Hello");
        encoder.Encode(new byte[1]);

        using JpegLSDecoder decoder = new(encoder.EncodedData, false);
        decoder.ReadHeader();

        Assert.Pass();
    }

    [Test]
    public void ApplicationDataHandlerReceivesApplicationDataBytes()
    {
        using JpegLSEncoder encoder = new(1, 1, 8, 1, true, 100);

        var applicationData1 = new byte[] { 1, 2, 3, 4 };
        encoder.WriteApplicationData(12, applicationData1);
        encoder.Encode(new byte[1]);

        byte[]? applicationData2 = null;
        using JpegLSDecoder decoder = new(encoder.EncodedData, false);

        decoder.ApplicationData += ApplicationDataHandler;
        decoder.ApplicationData -= ApplicationDataHandler;
        decoder.ApplicationData += ApplicationDataHandler;
        decoder.ReadHeader();

        Assert.That(applicationData2, Is.Not.Null);
        Assert.That(applicationData2!, Has.Length.EqualTo(4));
        Assert.Multiple(() =>
        {
            Assert.That(applicationData2![0], Is.EqualTo(1));
            Assert.That(applicationData2![1], Is.EqualTo(2));
            Assert.That(applicationData2![2], Is.EqualTo(3));
            Assert.That(applicationData2![3], Is.EqualTo(4));
        });
        return;

        void ApplicationDataHandler(object? _, ApplicationDataEventArgs e)
        {
            applicationData2 = e.Data.ToArray();
        }
    }

    [Test]
    public void ApplicationDataHandlerSubscribeWithNull()
    {
        using JpegLSDecoder decoder = new();
        decoder.ApplicationData += null;

        Assert.Pass();
    }

    [Test]
    public void ApplicationDataHandlerUnsubscribeWithNull()
    {
        using JpegLSDecoder decoder = new();
        decoder.ApplicationData -= null;

        Assert.Pass();
    }

    [Test]
    public void ApplicationDataWithoutHandler()
    {
        using JpegLSEncoder encoder = new(new FrameInfo(1, 1, 8, 1), true, 100);

        encoder.WriteApplicationData(7, [1, 2, 3, 4]);
        encoder.Encode(new byte[1]);

        using JpegLSDecoder decoder = new(encoder.EncodedData, false);
        decoder.ReadHeader();

        Assert.Pass();
    }

    [Test]
    public void ReadHeaderWithoutSpiffHeader()
    {
        using JpegLSEncoder encoder = new(new FrameInfo(1, 1, 8, 1));
        encoder.WriteStandardSpiffHeader(SpiffColorSpace.Grayscale);
        encoder.Encode(new byte[1]);

        using JpegLSDecoder decoder = new(encoder.EncodedData, false);
        decoder.ReadHeader(false);

        Assert.That(decoder.SpiffHeader, Is.Null);
    }

    [Test]
    public void ReadBadSpiffHeaderThrows()
    {
        using JpegLSEncoder encoder = new(new FrameInfo(1, 1, 8, 1));
        encoder.WriteStandardSpiffHeader(SpiffColorSpace.Rgb);
        encoder.Encode(new byte[1]);

        using JpegLSDecoder decoder = new(encoder.EncodedData, false);

        var exception = Assert.Throws<InvalidDataException>(() => { decoder.ReadHeader(); });
        Assert.That(exception!.GetJpegLSError(), Is.EqualTo(JpegLSError.InvalidSpiffHeader));
    }

    [SuppressMessage("Structure", "NUnit1028:The non-test method is public", Justification = "shared code across tests")]
    internal static bool CanHandleEmptyBuffer()
    {
        Interop.CharLSGetVersionNumber(out int _, out int minor, out int patch);
        return minor > 2 || patch > 0;
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
            Uri assemblyLocation = new(Assembly.GetExecutingAssembly().Location);
#if NET8_0_OR_GREATER
            return Path.Join(Path.GetDirectoryName(assemblyLocation.LocalPath), "DataFiles");
#else
            return Path.Combine(Path.GetDirectoryName(assemblyLocation.LocalPath), "DataFiles");
#endif
        }
    }
}
