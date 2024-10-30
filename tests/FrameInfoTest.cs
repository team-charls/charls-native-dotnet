// Copyright (c) Team CharLS.
// SPDX-License-Identifier: BSD-3-Clause

using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;

namespace CharLS.Native.Test;

[TestFixture]
internal sealed class FrameInfoTest
{
    [Test]
    public void ConstructDefault()
    {
        FrameInfo frameInfo = new(256, 1024, 8, 3);

        Assert.Multiple(() =>
        {
            Assert.That(frameInfo.Width, Is.EqualTo(256));
            Assert.That(frameInfo.Height, Is.EqualTo(1024));
            Assert.That(frameInfo.BitsPerSample, Is.EqualTo(8));
            Assert.That(frameInfo.ComponentCount, Is.EqualTo(3));
        });
    }

    [Test]
    public void ConstructWithProperties()
    {
        FrameInfo frameInfo = new()
        {
            Width = 256,
            Height = 1024,
            BitsPerSample = 8,
            ComponentCount = 3
        };

        Assert.Multiple(() =>
        {
            Assert.That(frameInfo.Width, Is.EqualTo(256));
            Assert.That(frameInfo.Height, Is.EqualTo(1024));
            Assert.That(frameInfo.BitsPerSample, Is.EqualTo(8));
            Assert.That(frameInfo.ComponentCount, Is.EqualTo(3));
        });
    }

    [Test]
    public void EquatableSameObjects()
    {
        FrameInfo a = new(256, 1024, 8, 3);
        FrameInfo b = new(256, 1024, 8, 3);

        bool equals = a.Equals(b);
        bool equalsObject = a.Equals((object)b);
        bool equalsOperator = a == b;

        Assert.Multiple(() =>
        {
            Assert.That(equals, Is.True);
            Assert.That(equalsObject, Is.True);
            Assert.That(b, Is.EqualTo(a));
            Assert.That(a, Is.EqualTo(b));
        });
        Assert.Multiple(() =>
        {
            Assert.That(b.GetHashCode(), Is.EqualTo(a.GetHashCode()));
            Assert.That(equalsOperator, Is.True);
        });
    }

    [Test]
    public void EquatableDifferentObjects()
    {
        FrameInfo a = new(256, 1024, 8, 3);
        FrameInfo b = new(256, 1024, 8, 4);

        bool equals = a.Equals(b);
        bool equalsObject = a.Equals((object)b);
        bool equalsOperator = a == b;

        Assert.Multiple(() =>
        {
            Assert.That(equals, Is.False);
            Assert.That(equalsObject, Is.False);
            Assert.That(equalsOperator, Is.False);
        });
    }

    [Test]
    [SuppressMessage("Maintainability", "CA1508:Avoid dead conditional code", Justification = "Unit test code")]
    public void EquatableWithNull()
    {
        FrameInfo a = new(256, 1024, 8, 3);

        bool equals = a.Equals(null!);

        Assert.Multiple(() =>
        {
            Assert.That(equals, Is.False);
        });
    }

    [Test]
    public void NativeWidthTooLarge()
    {
        FrameInfoNative frameInfoNative = new()
        {
            Width = uint.MaxValue
        };

        _ = Assert.Throws<OverflowException>(() => {
            FrameInfo _ = new(frameInfoNative);
        });
    }

    [Test]
    public void NativeHeightTooLarge()
    {
        FrameInfoNative frameInfoNative = new()
        {
            Height = uint.MaxValue
        };

        _ = Assert.Throws<OverflowException>(() => {
            FrameInfo _ = new(frameInfoNative);
        });
    }
}
