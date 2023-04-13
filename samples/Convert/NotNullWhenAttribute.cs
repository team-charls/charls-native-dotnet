// Copyright (c) Team CharLS.
// SPDX-License-Identifier: BSD-3-Clause

#if !NET6_0_OR_GREATER

namespace System.Diagnostics.CodeAnalysis;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter | AttributeTargets.ReturnValue, AllowMultiple = true)]
internal sealed class NotNullWhenAttribute : Attribute
{
    public NotNullWhenAttribute(bool returnValue)
    {
        this.ReturnValue = returnValue;
    }

    public bool ReturnValue { get; }
}

#endif
