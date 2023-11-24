// Copyright (c) Team CharLS.
// SPDX-License-Identifier: BSD-3-Clause

using System.Reflection;
using NUnit.Framework;

namespace CharLS.Native.Test;

[TestFixture]
public sealed class JpegLSDecoderTest
{
    [Test]
    public void ReadPresetCodingParameters()
    {
        byte[] source = ReadAllBytes("t8nde0.jls");
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

        Assert.IsFalse(result);
        Assert.IsNull(header);
    }

    [Test]
    public void TryReadSpiffHeaderWhenNotPresentUsingReadDirectConstructor()
    {
        byte[] source = ReadAllBytes("t8nde0.jls");

        using JpegLSDecoder decoder = new(source);

        Assert.IsNull(decoder.SpiffHeader);
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

        Assert.IsNotNull(decoder.Source);
    }

    [Test]
    public void UseAfterDisposeThrows()
    {
        JpegLSDecoder decoder = new();
        decoder.Dispose();

        _ = Assert.Throws<ObjectDisposedException>(() => { _ = decoder.TryReadSpiffHeader(out _); });

        Assert.IsNotNull(decoder.Source);

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

        Assert.IsNotNull(comment2);
        Assert.AreEqual(4, comment2!.Length);
        Assert.AreEqual(1, comment2![0]);
        Assert.AreEqual(2, comment2![1]);
        Assert.AreEqual(3, comment2![2]);
        Assert.AreEqual(4, comment2![3]);
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

        Assert.IsNotNull(applicationData2);
        Assert.AreEqual(4, applicationData2!.Length);
        Assert.AreEqual(1, applicationData2![0]);
        Assert.AreEqual(2, applicationData2![1]);
        Assert.AreEqual(3, applicationData2![2]);
        Assert.AreEqual(4, applicationData2![3]);
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

        encoder.WriteApplicationData(7, new byte[] { 1, 2, 3, 4 });
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

        Assert.IsNull(decoder.SpiffHeader);
    }

    [Test]
    public void ReadBadSpiffHeaderThrows()
    {
        using JpegLSEncoder encoder = new(new FrameInfo(1, 1, 8, 1));
        encoder.WriteStandardSpiffHeader(SpiffColorSpace.Rgb);
        encoder.Encode(new byte[1]);

        using JpegLSDecoder decoder = new(encoder.EncodedData, false);

        var exception = Assert.Throws<InvalidDataException>(() => { decoder.ReadHeader(); });
        Assert.AreEqual(JpegLSError.InvalidSpiffHeader, exception!.GetJpegLSError());
    }

    internal static bool CanHandleEmptyBuffer()
    {
        Interop.CharLSGetVersionNumber(out int _, out int minor, out int patch);
        return minor > 2 || patch > 0;
    }

    private static byte[] ReadAllBytes(string path, int bytesToSkip = 0)
    {
#if NET6_0_OR_GREATER
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
#if NET6_0_OR_GREATER
            return Path.Join(Path.GetDirectoryName(assemblyLocation.LocalPath), "DataFiles");
#else
            return Path.Combine(Path.GetDirectoryName(assemblyLocation.LocalPath), "DataFiles");
#endif
        }
    }
}
