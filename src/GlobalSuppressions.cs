// Copyright (c) Team CharLS.
// SPDX-License-Identifier: BSD-3-Clause

// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.
using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Usage", "CA2201:Do not raise reserved exception types", Justification = "OutOfMemoryException is the best exception to throw", Scope = "member", Target = "~M:CharLS.Native.JpegLSEncoder.CreateEncoder~CharLS.Native.SafeHandleJpegLSEncoder")]
[assembly: SuppressMessage("Usage", "CA2201:Do not raise reserved exception types", Justification = "OutOfMemoryException is the best exception to throw", Scope = "member", Target = "~M:CharLS.Native.JpegLSDecoder.CreateDecoder~CharLS.Native.SafeHandleJpegLSDecoder")]
[assembly: SuppressMessage("Design", "CA1065:Do not raise exceptions in unexpected locations", Justification = "Type is unusable if native DLL doesn't match", Scope = "member", Target = "~M:CharLS.Native.SafeNativeMethods.#cctor")]
