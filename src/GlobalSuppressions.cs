// Copyright (c) Team CharLS.
// SPDX-License-Identifier: BSD-3-Clause

// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.
using System.Diagnostics.CodeAnalysis;

// The purpose of this file is to suppress false warnings, this to keep the code clean.
// Warnings that are suppressed by a design choice are suppressed in the source code.

[assembly: SuppressMessage("Design", "CA1027:Mark enums with FlagsAttribute", Justification = "False warnings (triggered by missing values), enum values cannot be combined", Scope = "type", Target = "~T:CharLS.Native.SpiffColorSpace")]
