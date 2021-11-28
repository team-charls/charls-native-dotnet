// Copyright (c) Team CharLS.
// SPDX-License-Identifier: BSD-3-Clause

using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;

namespace CharLS.Native.Test;

[TestFixture]
public class FrameInfoTest
{
    [Test]
    public void ConstructDefault()
    {
        FrameInfo frameInfo = new(256, 1024, 8, 3);

        Assert.AreEqual(256, frameInfo.Width);
        Assert.AreEqual(1024, frameInfo.Height);
        Assert.AreEqual(8, frameInfo.BitsPerSample);
        Assert.AreEqual(3, frameInfo.ComponentCount);
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

        Assert.AreEqual(256, frameInfo.Width);
        Assert.AreEqual(1024, frameInfo.Height);
        Assert.AreEqual(8, frameInfo.BitsPerSample);
        Assert.AreEqual(3, frameInfo.ComponentCount);
    }

    [Test]
    public void EquatableSameObjects()
    {
        FrameInfo a = new(256, 1024, 8, 3);
        FrameInfo b = new(256, 1024, 8, 3);

        Assert.IsTrue(a.Equals(b));
        Assert.IsTrue(a.Equals((object)b));
        Assert.AreEqual(a, b);
        Assert.AreEqual(b, a);
        Assert.AreEqual(a.GetHashCode(), b.GetHashCode());
        Assert.IsTrue(a == b);
    }

    [Test]
    public void EquatableDifferentObjects()
    {
        FrameInfo a = new(256, 1024, 8, 3);
        FrameInfo b = new(256, 1024, 8, 4);

        Assert.IsFalse(a.Equals(b));
        Assert.IsFalse(a.Equals((object)b));
        Assert.IsTrue(a != b);
    }

    [Test]
    [SuppressMessage("Maintainability", "CA1508:Avoid dead conditional code", Justification = "Unit test code")]
    public void EquatableWithNull()
    {
        FrameInfo a = new(256, 1024, 8, 3);

        Assert.IsFalse(a.Equals(null!));
        Assert.IsFalse(a!.Equals((object)null!));
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
