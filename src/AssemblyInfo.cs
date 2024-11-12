// Copyright (c) Team CharLS.
// SPDX-License-Identifier: BSD-3-Clause

using System.Runtime.InteropServices;

[assembly: ComVisible(false)]

#if NET8_0_OR_GREATER
[assembly: DefaultDllImportSearchPaths(DllImportSearchPath.SafeDirectories)]
#else
[assembly: DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
#endif
