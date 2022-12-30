// Copyright (c) Team CharLS.
// SPDX-License-Identifier: BSD-3-Clause

namespace CharLS.Native;

/// <summary>
/// Provides helper methods for Exception objects and JpegLSError values.
/// </summary>
public static class ExceptionExtensions
{
    /// <summary>
    /// Retrieves the JpegLSError value from the Exception instance or None if no error value was present.
    /// </summary>
    public static JpegLSError GetJpegLSError(this Exception exception)
    {
        var value = exception.Data[nameof(JpegLSError)];
        return value != null ? (JpegLSError)value : default;
    }

    internal static void SetJpegLSError(this Exception exception, JpegLSError error)
    {
        exception.Data.Add(nameof(JpegLSError), error);
    }
}
