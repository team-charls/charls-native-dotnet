// Copyright (c) Team CharLS.
// SPDX-License-Identifier: BSD-3-Clause

using System.ComponentModel;
using NUnit.Framework;

namespace CharLS.Native.Test;

[TestFixture]
public class JpegLSEncoderTest
{
    [Test]
    public void CreateEncoderWithBadWidth()
    {
        _ = Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            using JpegLSEncoder _ = new(0, 1, 2, 1);
        });
    }

    [Test]
    public void CreateWithExtendedConstructor()
    {
        FrameInfo expected = new(256, 500, 8, 3);
        using JpegLSEncoder encoder = new(256, 500, 8, 3);

        Assert.AreEqual(expected, encoder.FrameInfo);
        Assert.NotNull(encoder.Destination);
    }

    [Test]
    public void CreateWithFrameInfoConstructor()
    {
        FrameInfo expected = new(256, 500, 8, 3);
        using JpegLSEncoder encoder = new(expected);

        Assert.AreEqual(expected, encoder.FrameInfo);
        Assert.NotNull(encoder.Destination);
    }

    [Test]
    public void InitializeFrameInfoWithNull()
    {
        using JpegLSEncoder encoder = new();
        _ = Assert.Throws<ArgumentNullException>(() =>
        {
            encoder.FrameInfo = null;
        });
    }

    [Test]
    public void GetAndSetNearLossless()
    {
        using JpegLSEncoder encoder = new();

        Assert.AreEqual(0, encoder.NearLossless);

        encoder.NearLossless = 1;
        Assert.AreEqual(1, encoder.NearLossless);
    }

    [Test]
    public void GetAndSetInterleaveMode()
    {
        using JpegLSEncoder encoder = new();

        Assert.AreEqual(JpegLSInterleaveMode.None, encoder.InterleaveMode);

        encoder.InterleaveMode = JpegLSInterleaveMode.Line;
        Assert.AreEqual(JpegLSInterleaveMode.Line, encoder.InterleaveMode);
    }

    [Test]
    public void GetAndSetPresetCodingParameters()
    {
        using JpegLSEncoder encoder = new();

        Assert.IsNull(encoder.PresetCodingParameters);

        var presetCodingParameters = new JpegLSPresetCodingParameters(255, 9, 10, 11, 31);
        encoder.PresetCodingParameters = presetCodingParameters;
        Assert.AreEqual(presetCodingParameters, encoder.PresetCodingParameters);
    }

    [Test]
    public void InitializePresetCodingParametersWithNull()
    {
        using JpegLSEncoder encoder = new();
        _ = Assert.Throws<ArgumentNullException>(() =>
        {
            encoder.PresetCodingParameters = null;
        });
    }

    [Test]
    public void SetDestinationWithEmptyBuffer()
    {
        if (!JpegLSDecoderTest.CanHandleEmptyBuffer())
            return;

        using JpegLSEncoder encoder = new();
        Assert.DoesNotThrow(() =>
        {
            encoder.Destination = Memory<byte>.Empty;
        });
    }

    [Test]
    public void UseAfterDisposeThrows()
    {
        JpegLSEncoder encoder = new();
        encoder.Dispose();

        _ = Assert.Throws<ObjectDisposedException>(() =>
        {
            encoder.NearLossless = 0;
        });

        _ = Assert.Throws<ObjectDisposedException>(() =>
        {
            encoder.Encode(new byte[10]);
        });
    }

    [Test]
    public void DefaultEncodingOptionsIsIncludePCParametersJai()
    {
        using JpegLSEncoder encoder = new();
        Assert.AreEqual(EncodingOptions.IncludePCParametersJai, encoder.EncodingOptions);
    }

    [Test]
    public void BadEncodingOptionsThrows()
    {
        using JpegLSEncoder encoder = new();

        _ = Assert.Throws<InvalidEnumArgumentException>(() =>
        {
            encoder.EncodingOptions = (EncodingOptions)8;
        });
    }

    [Test]
    public void WriteComment()
    {
        FrameInfo frameInfo = new(1, 1, 8, 1);
        using JpegLSEncoder encoder = new(frameInfo, false);
        encoder.Destination = new byte[100];

        var comment1 = new byte[] { 1, 2, 3, 4 };
        encoder.WriteComment(comment1);
        encoder.Encode(new byte[1]);

        byte[]? comment2 = null;
        using JpegLSDecoder decoder = new(encoder.EncodedData, false);
        decoder.Comment += (_, e) =>
        {
            comment2 = e.Data.ToArray();
        };
        decoder.ReadHeader();

        Assert.IsNotNull(comment2);
        Assert.AreEqual(4, comment2!.Length);
        Assert.AreEqual(1, comment2![0]);
        Assert.AreEqual(2, comment2![1]);
        Assert.AreEqual(3, comment2![2]);
        Assert.AreEqual(4, comment2![3]);
    }

    [Test]
    public void WriteEmptyComment()
    {
        FrameInfo frameInfo = new(1, 1, 8, 1);
        using JpegLSEncoder encoder = new(frameInfo, false);
        encoder.Destination = new byte[100];

        encoder.WriteComment(Array.Empty<byte>());
        encoder.Encode(new byte[1]);

        byte[]? comment = null;
        using JpegLSDecoder decoder = new(encoder.EncodedData, false);
        decoder.Comment += (_, e) =>
        {
            comment = e.Data.ToArray();
        };
        decoder.ReadHeader();

        Assert.IsNotNull(comment);
        Assert.AreEqual(0, comment!.Length);
    }

    [Test]
    public void WriteStringComment()
    {
        FrameInfo frameInfo = new(1, 1, 8, 1);
        using JpegLSEncoder encoder = new(frameInfo, false);
        encoder.Destination = new byte[100];

        encoder.WriteComment("Hello");
        encoder.Encode(new byte[1]);

        ReadOnlyMemory<byte> comment = null;
        using JpegLSDecoder decoder = new(encoder.EncodedData, false);
        decoder.Comment += (_, e) =>
        {
            comment = e.Data;
        };
        decoder.ReadHeader();

        Assert.IsNotNull(comment);
        Assert.AreEqual(6, comment!.Length);

        var data = comment.Span;
        Assert.AreEqual((byte)'H', data[0]);
        Assert.AreEqual((byte)'e', data[1]);
        Assert.AreEqual((byte)'l', data[2]);
        Assert.AreEqual((byte)'l', data[3]);
        Assert.AreEqual((byte)'o', data[4]);
        Assert.AreEqual(0, data[5]);
    }

    [Test]
    public void FailCommentEvent()
    {
        FrameInfo frameInfo = new(1, 1, 8, 1);
        using JpegLSEncoder encoder = new(frameInfo, false);
        encoder.Destination = new byte[100];

        encoder.WriteComment(Array.Empty<byte>());
        encoder.Encode(new byte[1]);

        using JpegLSDecoder decoder = new(encoder.EncodedData, false);
        decoder.Comment += (_, e) =>
        {
            e.Failed = true;
        };

        _ = Assert.Throws<InvalidDataException>(() =>
        {
            decoder.ReadHeader();
        });
    }
}
