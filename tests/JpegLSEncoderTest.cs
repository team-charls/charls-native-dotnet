// Copyright (c) Team CharLS.
// SPDX-License-Identifier: BSD-3-Clause

using System.ComponentModel;
using System.Text;
using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;

namespace CharLS.Native.Test;

[TestFixture]
internal sealed class JpegLSEncoderTest
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

        Assert.Multiple(() =>
        {
            Assert.That(encoder.FrameInfo, Is.EqualTo(expected));
            Assert.That(encoder.Destination.IsEmpty, Is.False);
        });
    }

    [Test]
    public void CreateWithFrameInfoConstructor()
    {
        FrameInfo expected = new(256, 500, 8, 3);
        using JpegLSEncoder encoder = new(expected);

        Assert.Multiple(() =>
        {
            Assert.That(encoder.FrameInfo, Is.EqualTo(expected));
            Assert.That(encoder.Destination.IsEmpty, Is.False);
        });
    }

    [Test]
    public void GetAndSetNearLossless()
    {
        using JpegLSEncoder encoder = new();

        Assert.That(encoder.NearLossless, Is.EqualTo(0));

        encoder.NearLossless = 1;
        Assert.That(encoder.NearLossless, Is.EqualTo(1));
    }

    [Test]
    public void GetAndSetInterleaveMode()
    {
        using JpegLSEncoder encoder = new();

        Assert.That(encoder.InterleaveMode, Is.EqualTo(JpegLSInterleaveMode.None));

        encoder.InterleaveMode = JpegLSInterleaveMode.Line;
        Assert.That(encoder.InterleaveMode, Is.EqualTo(JpegLSInterleaveMode.Line));
    }

    [Test]
    public void GetAndSetPresetCodingParameters()
    {
        using JpegLSEncoder encoder = new();

        Assert.That(encoder.PresetCodingParameters, Is.Null);

        var presetCodingParameters = new JpegLSPresetCodingParameters(255, 9, 10, 11, 31);
        encoder.PresetCodingParameters = presetCodingParameters;
        Assert.That(encoder.PresetCodingParameters, Is.EqualTo(presetCodingParameters));
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
        Assert.That(encoder.EncodingOptions, Is.EqualTo(EncodingOptions.IncludePCParametersJai));
    }

    [Test]
    public void EncodingOptionsCanBeChanged()
    {
        using JpegLSEncoder encoder = new();

        encoder.EncodingOptions = EncodingOptions.EvenDestinationSize | EncodingOptions.IncludeVersionNumber;
        Assert.That(encoder.EncodingOptions, Is.EqualTo(EncodingOptions.EvenDestinationSize | EncodingOptions.IncludeVersionNumber));

        encoder.EncodingOptions = EncodingOptions.None;
        Assert.That(encoder.EncodingOptions, Is.EqualTo(EncodingOptions.None));
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

        Assert.That(encoder.BytesWritten, Is.EqualTo(100));
    }

    [Test]
    public void EncodingOptionsEvenDestinationSizeNotEnabled()
    {
        using JpegLSEncoder encoder = new(new FrameInfo(512, 512, 8, 1));

        var source = new byte[512 * 512];
        encoder.Encode(source);

        Assert.That(encoder.BytesWritten, Is.EqualTo(99));
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

        Assert.That(versionComment, Is.Not.Null);

        string versionString = Encoding.UTF8.GetString(versionComment!);
#if NET8_0_OR_GREATER
        versionString = versionString[..7];
#else
        versionString = versionString.Substring(0, 7);
#endif

        Assert.That(versionString, Is.EqualTo("charls "));
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

        Assert.That(versionComment, Is.Null);
    }

    [Test]
    public void EncodingOptionsIncludePCParametersJaiEnabled()
    {
        using JpegLSEncoder encoder = new(new FrameInfo(100, 100, 14, 1));
        encoder.EncodingOptions |= EncodingOptions.IncludePCParametersJai;
        var source = new byte[100 * 100 * 2];
        encoder.Encode(source);

        Assert.That(encoder.BytesWritten, Is.EqualTo(59));
    }

    [Test]
    public void EncodingOptionsIncludePCParametersJaiNotEnabled()
    {
        using JpegLSEncoder encoder = new(new FrameInfo(100, 100, 14, 1));
        encoder.EncodingOptions = EncodingOptions.None;
        var source = new byte[100 * 100 * 2];
        encoder.Encode(source);

        Assert.That(encoder.BytesWritten, Is.EqualTo(44));
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

        Assert.That(comment2, Is.Not.Null);
        Assert.That(comment2!, Has.Length.EqualTo(4));
        Assert.Multiple(() =>
        {
            Assert.That(comment2![0], Is.EqualTo(1));
            Assert.That(comment2![1], Is.EqualTo(2));
            Assert.That(comment2![2], Is.EqualTo(3));
            Assert.That(comment2![3], Is.EqualTo(4));
        });
    }

    [Test]
    [SuppressMessage("Style", "IDE0301:Simplify collection initialization", Justification = "Not possible for .NET 6.0 build")]
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

        Assert.That(comment, Is.Not.Null);
        Assert.That(comment!, Is.Empty);
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

        Assert.Multiple(() =>
        {
            Assert.That(comment.IsEmpty, Is.False);
            Assert.That(comment!.Length, Is.EqualTo(6));
        });

        Assert.Multiple(() =>
        {
            Assert.That(comment.Span[0], Is.EqualTo((byte)'H'));
            Assert.That(comment.Span[1], Is.EqualTo((byte)'e'));
            Assert.That(comment.Span[2], Is.EqualTo((byte)'l'));
            Assert.That(comment.Span[3], Is.EqualTo((byte)'l'));
            Assert.That(comment.Span[4], Is.EqualTo((byte)'o'));
            Assert.That(comment.Span[5], Is.EqualTo(0));
        });
    }

    [Test]
    [SuppressMessage("Style", "IDE0301:Simplify collection initialization", Justification = "Not possible for .NET 6.0 build")]
    public void FailCommentEvent()
    {
        using JpegLSEncoder encoder = new(new FrameInfo(1, 1, 8, 1), true, 100);
        encoder.WriteComment(Array.Empty<byte>());
        encoder.Encode(new byte[1]);

        using JpegLSDecoder decoder = new(encoder.EncodedData, false);
        decoder.Comment += (_, _) => throw new InvalidCastException();

        var exception = Assert.Throws<InvalidDataException>(() =>
        {
            decoder.ReadHeader();
        });

        Assert.That(exception!.GetJpegLSError(), Is.EqualTo(JpegLSError.CallbackFailed));
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

        Assert.Multiple(() =>
        {
            Assert.That(applicationDataId, Is.EqualTo(3));
            Assert.That(applicationData2, Is.Not.Null);
        });
        Assert.That(applicationData2!, Has.Length.EqualTo(4));
        Assert.Multiple(() =>
        {
            Assert.That(applicationData2![0], Is.EqualTo(1));
            Assert.That(applicationData2![1], Is.EqualTo(2));
            Assert.That(applicationData2![2], Is.EqualTo(3));
            Assert.That(applicationData2![3], Is.EqualTo(4));
        });
    }

    [Test]
    public void WriteEmptyApplicationData()
    {
        using JpegLSEncoder encoder = new(new FrameInfo(1, 1, 8, 1), true, 100);

        encoder.WriteApplicationData(15, []);
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

        Assert.Multiple(() =>
        {
            Assert.That(applicationDataId, Is.EqualTo(15));
            Assert.That(applicationData, Is.Not.Null);
        });
        Assert.That(applicationData!, Is.Empty);
    }

    [Test]
    public void FailApplicationDataEvent()
    {
        using JpegLSEncoder encoder = new(new FrameInfo(1, 1, 8, 1), true, 100);
        encoder.WriteApplicationData(3, []);
        encoder.Encode(new byte[1]);

        using JpegLSDecoder decoder = new(encoder.EncodedData, false);
        decoder.ApplicationData += (_, _) => throw new InvalidCastException();

        var exception = Assert.Throws<InvalidDataException>(() =>
        {
            decoder.ReadHeader();
        });

        Assert.That(exception!.GetJpegLSError(), Is.EqualTo(JpegLSError.CallbackFailed));
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

        Assert.That(result2, Has.Length.EqualTo(result1.Length));
    }

    [Test]
    public void BadExtraBytesThrows()
    {
        FrameInfo frameInfo = new(1, 1, 8, 1);

        _ = Assert.Throws<ArgumentOutOfRangeException>(() => { _ = new JpegLSEncoder(frameInfo, true, -1); });
    }
}
