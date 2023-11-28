// Copyright (c) Team CharLS.
// SPDX-License-Identifier: BSD-3-Clause

using NUnit.Framework;

namespace CharLS.Native.Test;

[TestFixture]
public sealed class ExceptionExtensionsTest
{
    [Test]
    public void SetAndGetJpegLSError()
    {
        var exception = new InvalidDataException();

        exception.SetJpegLSError(JpegLSError.DuplicateStartOfFrameMarker);

        Assert.That(exception.GetJpegLSError(), Is.EqualTo(JpegLSError.DuplicateStartOfFrameMarker));
    }

    [Test]
    public void GetNotAvailableJpegLSErrorReturnsNone()
    {
        var exception = new InvalidDataException();

        Assert.That(exception.GetJpegLSError(), Is.EqualTo(JpegLSError.None));
    }
}
