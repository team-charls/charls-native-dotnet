// Copyright (c) Team CharLS.
// SPDX-License-Identifier: BSD-3-Clause

using System.ComponentModel;
using System.Text;
using NUnit.Framework;

namespace CharLS.Native.Test;

[TestFixture]
public sealed class JpegLSEncoderTest
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
        // Note: use the same default as the native CharLS implementation. Will change in the future.
        using JpegLSEncoder encoder = new();
        Assert.AreEqual(EncodingOptions.IncludePCParametersJai, encoder.EncodingOptions);
    }

    [Test]
    public void EncodingOptionsCanBeChanged()
    {
        using JpegLSEncoder encoder = new();

        encoder.EncodingOptions = EncodingOptions.EvenDestinationSize | EncodingOptions.IncludeVersionNumber;
        Assert.AreEqual(EncodingOptions.EvenDestinationSize | EncodingOptions.IncludeVersionNumber, encoder.EncodingOptions);

        encoder.EncodingOptions = EncodingOptions.None;
        Assert.AreEqual(EncodingOptions.None, encoder.EncodingOptions);
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
    public void EncodingOptionsEvenDestinationSizeEnabled()
    {
        using JpegLSEncoder encoder = new(new FrameInfo(512, 512, 8, 1));
        encoder.EncodingOptions = EncodingOptions.EvenDestinationSize;

        var source = new byte[512 * 512];
        encoder.Encode(source);

        Assert.AreEqual(100, encoder.BytesWritten);
    }

    [Test]
    public void EncodingOptionsEvenDestinationSizeNotEnabled()
    {
        using JpegLSEncoder encoder = new(new FrameInfo(512, 512, 8, 1));

        var source = new byte[512 * 512];
        encoder.Encode(source);

        Assert.AreEqual(99, encoder.BytesWritten);
    }

    [Test]
    public void EncodingOptionsIncludeVersionNumberEnabled()
    {
        using JpegLSEncoder encoder = new(new FrameInfo(512, 512, 8, 1));
        encoder.EncodingOptions |= EncodingOptions.IncludeVersionNumber;

        var source = new byte[512 * 512];
        encoder.Encode(source);

        byte[]? versionComment = null;
        using JpegLSDecoder decoder = new(encoder.EncodedData, false);
        decoder.Comment += (_, e) =>
        {
            versionComment = e.Data.ToArray();
        };
        decoder.ReadHeader();

        Assert.IsNotNull(versionComment);

        string versionString = Encoding.UTF8.GetString(versionComment!);
        versionString = versionString[..7];

        Assert.AreEqual("charls ", versionString);
    }

    [Test]
    public void EncodingOptionsIncludeVersionNumberNotEnabled()
    {
        using JpegLSEncoder encoder = new(new FrameInfo(512, 512, 8, 1));
        var source = new byte[512 * 512];
        encoder.Encode(source);

        byte[]? versionComment = null;
        using JpegLSDecoder decoder = new(encoder.EncodedData, false);
        decoder.Comment += (_, e) =>
        {
            versionComment = e.Data.ToArray();
        };
        decoder.ReadHeader();

        Assert.IsNull(versionComment);
    }

    [Test]
    public void EncodingOptionsIncludePCParametersJaiEnabled()
    {
        using JpegLSEncoder encoder = new(new FrameInfo(100, 100, 14, 1));
        encoder.EncodingOptions |= EncodingOptions.IncludePCParametersJai;
        var source = new byte[100 * 100 * 2];
        encoder.Encode(source);

        Assert.AreEqual(59, encoder.BytesWritten);
    }

    [Test]
    public void EncodingOptionsIncludePCParametersJaiNotEnabled()
    {
        using JpegLSEncoder encoder = new(new FrameInfo(100, 100, 14, 1));
        encoder.EncodingOptions = EncodingOptions.None;
        var source = new byte[100 * 100 * 2];
        encoder.Encode(source);

        Assert.AreEqual(44, encoder.BytesWritten);
    }

    [Test]
    public void WriteComment()
    {
        using JpegLSEncoder encoder = new(1, 1, 8, 1, true, 100);

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
        using JpegLSEncoder encoder = new(new FrameInfo(1, 1, 8, 1), true, 100);

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
        using JpegLSEncoder encoder = new(new FrameInfo(1, 1, 8, 1), true, 100);

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
        using JpegLSEncoder encoder = new(new FrameInfo(1, 1, 8, 1), true, 100);
        encoder.WriteComment(Array.Empty<byte>());
        encoder.Encode(new byte[1]);

        using JpegLSDecoder decoder = new(encoder.EncodedData, false);
        decoder.Comment += (_, e) =>
        {
            e.Failed = true;
        };

        var exception = Assert.Throws<InvalidDataException>(() =>
        {
            decoder.ReadHeader();
        });

        Assert.AreEqual(JpegLSError.CallbackFailed, exception!.GetJpegLSError());
    }

    [Test]
    public void WriteApplicationData()
    {
        using JpegLSEncoder encoder = new(1, 1, 8, 1, true, 100);

        var applicationData1 = new byte[] { 1, 2, 3, 4 };
        encoder.WriteApplicationData(3, applicationData1);
        encoder.Encode(new byte[1]);

        int applicationDataId = -1;
        byte[]? applicationData2 = null;
        using JpegLSDecoder decoder = new(encoder.EncodedData, false);
        decoder.ApplicationData += (_, e) =>
        {
            applicationDataId = e.Id;
            applicationData2 = e.Data.ToArray();
        };
        decoder.ReadHeader();

        Assert.AreEqual(3, applicationDataId);
        Assert.IsNotNull(applicationData2);
        Assert.AreEqual(4, applicationData2!.Length);
        Assert.AreEqual(1, applicationData2![0]);
        Assert.AreEqual(2, applicationData2![1]);
        Assert.AreEqual(3, applicationData2![2]);
        Assert.AreEqual(4, applicationData2![3]);
    }

    [Test]
    public void WriteEmptyApplicationData()
    {
        using JpegLSEncoder encoder = new(new FrameInfo(1, 1, 8, 1), true, 100);

        encoder.WriteApplicationData(15, Array.Empty<byte>());
        encoder.Encode(new byte[1]);

        int applicationDataId = -1;
        byte[]? applicationData = null;
        using JpegLSDecoder decoder = new(encoder.EncodedData, false);
        decoder.ApplicationData += (_, e) =>
        {
            applicationDataId = e.Id;
            applicationData = e.Data.ToArray();
        };
        decoder.ReadHeader();

        Assert.AreEqual(15, applicationDataId);
        Assert.IsNotNull(applicationData);
        Assert.AreEqual(0, applicationData!.Length);
    }

    [Test]
    public void FailApplicationDataEvent()
    {
        using JpegLSEncoder encoder = new(new FrameInfo(1, 1, 8, 1), true, 100);
        encoder.WriteApplicationData(3, Array.Empty<byte>());
        encoder.Encode(new byte[1]);

        using JpegLSDecoder decoder = new(encoder.EncodedData, false);
        decoder.ApplicationData += (_, e) =>
        {
            e.Failed = true;
        };

        var exception = Assert.Throws<InvalidDataException>(() =>
        {
            decoder.ReadHeader();
        });

        Assert.AreEqual(JpegLSError.CallbackFailed, exception!.GetJpegLSError());
    }

    [Test]
    public void RewindAndDecodeAgain()
    {
        using JpegLSEncoder encoder = new(new FrameInfo(1, 1, 8, 1), true, 100);

        encoder.Encode(new byte[1]);
        var result1 = encoder.Destination.ToArray();

        encoder.Rewind();

        encoder.Encode(new byte[1]);
        var result2 = encoder.Destination.ToArray();

        Assert.AreEqual(result1.Length, result2.Length);
    }

    [Test]
    public void BadExtraBytesThrows()
    {
        FrameInfo frameInfo = new(1, 1, 8, 1);

        _ = Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            var _ = new JpegLSEncoder(frameInfo, true, -1);
        });
    }
}
