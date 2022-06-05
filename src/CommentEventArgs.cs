// Copyright (c) Team CharLS.
// SPDX-License-Identifier: BSD-3-Clause

namespace CharLS.Native;

/// <summary>
/// Encapsulates event data for Comment event
/// </summary>
public sealed class CommentEventArgs : EventArgs
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CommentEventArgs"/> class.
    /// </summary>
    /// <param name="data"></param>
    internal CommentEventArgs(byte[] data)
    {
        Data = data;
    }

    /// <summary>
    /// Returns the data of the comment.
    /// </summary>
    public ReadOnlyMemory<byte> Data { get; }

    /// <summary>
    /// `When set will abort the decoding process.
    /// </summary>
    public bool Failed { get; set; }
}
