// Copyright (c) Team CharLS.
// SPDX-License-Identifier: BSD-3-Clause

using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;

namespace CharLS.Native.Test;

[TestFixture]
public sealed class JpegLSPresetCodingParametersTest
{
    [Test]
    public void ConstructDefault()
    {
        JpegLSPresetCodingParameters presetCodingParameters = new(255, 9, 10, 11, 31);

        Assert.Multiple(() =>
        {
            Assert.That(presetCodingParameters.MaximumSampleValue, Is.EqualTo(255));
            Assert.That(presetCodingParameters.Threshold1, Is.EqualTo(9));
            Assert.That(presetCodingParameters.Threshold2, Is.EqualTo(10));
            Assert.That(presetCodingParameters.Threshold3, Is.EqualTo(11));
            Assert.That(presetCodingParameters.ResetValue, Is.EqualTo(31));
        });
    }

    [Test]
    public void ConstructWithProperties()
    {
        JpegLSPresetCodingParameters presetCodingParameters = new()
        {
            MaximumSampleValue = 255,
            Threshold1 = 9,
            Threshold2 = 10,
            Threshold3 = 11,
            ResetValue = 31
        };

        Assert.Multiple(() =>
        {
            Assert.That(presetCodingParameters.MaximumSampleValue, Is.EqualTo(255));
            Assert.That(presetCodingParameters.Threshold1, Is.EqualTo(9));
            Assert.That(presetCodingParameters.Threshold2, Is.EqualTo(10));
            Assert.That(presetCodingParameters.Threshold3, Is.EqualTo(11));
            Assert.That(presetCodingParameters.ResetValue, Is.EqualTo(31));
        });
    }

    [Test]
    public void EquatableSameObjects()
    {
        JpegLSPresetCodingParameters a = new(255, 9, 10, 11, 31);
        JpegLSPresetCodingParameters b = new(255, 9, 10, 11, 31);

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
        JpegLSPresetCodingParameters a = new(255, 9, 10, 11, 31);
        JpegLSPresetCodingParameters b = new(255, 9, 10, 11, 32);

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
        JpegLSPresetCodingParameters a = new(2556, 9, 10, 11, 31);

        bool equals = a.Equals(null!);
        bool equalsObject = a!.Equals((object)null!);

        Assert.Multiple(() =>
        {
            Assert.That(equals, Is.False);
            Assert.That(equalsObject, Is.False);
        });
    }
}
