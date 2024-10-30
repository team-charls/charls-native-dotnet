// Copyright (c) Team CharLS.
// SPDX-License-Identifier: BSD-3-Clause

namespace CharLS.Native;

/// <summary>
/// Encapsulates event data for ApplicationData event
/// </summary>
public sealed class ApplicationDataEventArgs : EventArgs
{
    internal ApplicationDataEventArgs(int applicationDataId, byte[] data)
    {
        Id = applicationDataId;
        Data = data;
    }

    /// <summary>
    /// Identifies the type of application data, has the range [0,15]
    /// </summary>
    public int Id { get; }

    /// <summary>
    /// Gets the application data.
    /// </summary>
    public ReadOnlyMemory<byte> Data { get; }
}
