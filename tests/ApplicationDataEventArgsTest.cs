// Copyright (c) Team CharLS.
// SPDX-License-Identifier: BSD-3-Clause

using NUnit.Framework;

namespace CharLS.Native.Test;

[TestFixture]
internal class ApplicationDataEventArgsTest
{
    [Test]
    public void Create()
    {
        byte[] data = [1, 2, 3];
        var eventArgs = new ApplicationDataEventArgs(3, data);

        Assert.Multiple(() =>
        {
            Assert.That(eventArgs.Id, Is.EqualTo(3));
            Assert.That(eventArgs.Data.Span.SequenceEqual(data), Is.True);
        });
    }
}
