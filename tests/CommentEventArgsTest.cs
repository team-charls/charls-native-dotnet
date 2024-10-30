// Copyright (c) Team CharLS.
// SPDX-License-Identifier: BSD-3-Clause

using NUnit.Framework;

namespace CharLS.Native.Test;

[TestFixture]
internal class CommentEventArgsTest
{
    [Test]
    public void Create()
    {
        byte[] data = [1, 2, 3];
        var eventArgs = new CommentEventArgs(data);

        Assert.That(eventArgs.Data.Span.SequenceEqual(data), Is.True);
    }
}
