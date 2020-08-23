// Copyright (c) Team CharLS.
// SPDX-License-Identifier: BSD-3-Clause

using System;
using System.Runtime.InteropServices;

namespace CharLS.Native
{
    /// <summary>
    /// Encapsulates the parameters that will be written to the JFIF header.
    /// Since JFIF 1.02 thumbnails should preferable be created in extended segments.
    /// </summary>
    /// <remarks>
    /// Some fields are not used but defined to ensure memory layout and size is identical with the native structure.
    /// </remarks>
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct FrameInfoNative : IEquatable<FrameInfoNative>
    {
        private uint _width;
        private uint _height;
        private int _bitsPerSample;
        private int _componentCount;

        public uint Width
        {
            get
            {
                return _width;
            }

            set
            {
                _width = value;
            }
        }

        public uint Height
        {
            get => _height;
            set => _height = value;
        }

        public int BitsPerSample
        {
            get => _bitsPerSample;
            set => _bitsPerSample = value;
        }

        public int ComponentCount
        {
            get => _componentCount;
            set => _componentCount = value;
        }

        public static bool operator ==(FrameInfoNative frameInfo1, FrameInfoNative frameInfo2)
        {
            return frameInfo1.Equals(frameInfo2);
        }

        public static bool operator !=(FrameInfoNative frameInfo1, FrameInfoNative frameInfo2)
        {
            return !frameInfo1.Equals(frameInfo2);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is FrameInfoNative))
                return false;

            return Equals((FrameInfoNative)obj);
        }

        public bool Equals(FrameInfoNative other)
        {
            return _height == other._height &&
                   _width == other._width;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return HashCode.Combine(_width, _height);
        }
    }
}
