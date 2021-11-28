// Copyright (c) Team CharLS.
// SPDX-License-Identifier: BSD-3-Clause

using NUnit.Framework;

namespace CharLS.Native.Test;

[TestFixture]
public class JpegLSEncoderTest
{
    [Test]
    public void CreateEncoderWithBadWidth()
    {
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => {
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
        _ = Assert.Throws<ArgumentNullException>(() => {
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
        _ = Assert.Throws<ArgumentNullException>(() => {
            encoder.PresetCodingParameters = null;
        });
    }

    [Test]
    public void SetDestinationWithEmptyBuffer()
    {
        if (!JpegLSDecoderTest.CanHandleEmptyBuffer())
            return;

        using JpegLSEncoder encoder = new();
        Assert.DoesNotThrow(() => {
            encoder.Destination = Memory<byte>.Empty;
        });
    }

    [Test]
    public void UseAfterDispose()
    {
        JpegLSEncoder encoder = new();
        encoder.Dispose();

        _ = Assert.Throws<ObjectDisposedException>(() => {
            encoder.NearLossless = 0;
        });

        _ = Assert.Throws<ObjectDisposedException>(() => {
            encoder.Encode(new byte[10]);
        });
    }
}
