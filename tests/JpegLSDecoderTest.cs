// Copyright (c) Team CharLS.
// SPDX-License-Identifier: BSD-3-Clause

using System.Reflection;
using NUnit.Framework;

namespace CharLS.Native.Test;

[TestFixture]
public class JpegLSDecoderTest
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

        Assert.DoesNotThrow(() => {
            using JpegLSDecoder _ = new(Memory<byte>.Empty, false);
        });
    }

    [Test]
    public void SetSourceWithEmptyBuffer()
    {
        if (!CanHandleEmptyBuffer())
            return;

        using JpegLSDecoder decoder = new();
        Assert.DoesNotThrow(() => {
            decoder.Source = Memory<byte>.Empty;
        });
    }

    [Test]
    public void SetSourceTwice()
    {
        using JpegLSDecoder decoder = new() { Source = new byte[10] };

        _ = Assert.Throws<InvalidOperationException>(() => {
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
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => {
            var _ = decoder.GetDestinationSize(-1);
        });
    }

    [Test]
    public void DecodeToBufferWithNegativeStride()
    {
        byte[] source = ReadAllBytes("t8nde0.jls");

        using JpegLSDecoder decoder = new(source);
        var buffer = new byte[decoder.GetDestinationSize()];
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => {
            decoder.Decode(buffer, -1);
        });
    }

    [Test]
    public void DecodeToBufferWithNull()
    {
        byte[] source = ReadAllBytes("t8nde0.jls");

        using JpegLSDecoder decoder = new(source);
        _ = Assert.Throws<ArgumentException>(() => {
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
    public void UseAfterDispose()
    {
        JpegLSDecoder decoder = new();
        decoder.Dispose();

        _ = Assert.Throws<ObjectDisposedException>(() => {
            var _ = decoder.TryReadSpiffHeader(out var _);
        });

        Assert.IsNotNull(decoder.Source);

        _ = Assert.Throws<ObjectDisposedException>(() => {
            var _ = decoder.GetDestinationSize();
        });

        _ = Assert.Throws<ObjectDisposedException>(() => {
            var _ = decoder.Decode();
        });
    }

    [Test]
    public void UseBeforeReadHeader()
    {
        using JpegLSDecoder decoder = new();

        _ = Assert.Throws<InvalidOperationException>(() => {
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

        void CommentHandler(object? _, CommentEventArgs e)
        {
            comment2 = e.Data.ToArray();
        }

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
    }

    [Test]
    public void CommentHandlerSubscribeWithNull()
    {
        using JpegLSDecoder decoder = new();
        decoder.Comment += null;
    }

    [Test]
    public void CommentHandlerUnsubscribeWithNull()
    {
        using JpegLSDecoder decoder = new();
        decoder.Comment -= null;
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
    public void ReadHeaderWithoutSpiffHeader()
    {
        using JpegLSEncoder encoder = new(new FrameInfo(1, 1, 8, 1));
        encoder.WriteStandardSpiffHeader(SpiffColorSpace.Grayscale);
        encoder.Encode(new byte[1]);

        using JpegLSDecoder decoder = new(encoder.EncodedData, false);
        decoder.ReadHeader(false);
        Assert.IsNull(decoder.SpiffHeader);
    }

    internal static bool CanHandleEmptyBuffer()
    {
        Interop.CharLSGetVersionNumber(out int _, out int minor, out int patch);
        return minor > 2 || patch > 0;
    }

    private static byte[] ReadAllBytes(string path, int bytesToSkip = 0)
    {
        var fullPath = Path.Join(DataFileDirectory, path);

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
            return Path.Join(Path.GetDirectoryName(assemblyLocation.LocalPath), "DataFiles");
        }
    }
}
