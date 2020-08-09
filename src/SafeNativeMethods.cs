// Copyright (c) Team CharLS.
// SPDX-License-Identifier: BSD-3-Clause

using System;
using System.Runtime.InteropServices;
using CharLS.Native;

namespace CharLS
{
    internal static class SafeNativeMethods
    {
        private const string NativeX86Library = "charls-2-x86.dll";
        private const string NativeX64Library = "charls-2-x64.dll";

        [DllImport(NativeX86Library, SetLastError = false, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true, EntryPoint = "JpegLsReadHeader")]
        internal static extern JpegLSError JpegLsReadHeaderX86([In] byte[] compressedSource, int compressedLength, out JlsParameters info, IntPtr reserved);

        [DllImport(NativeX64Library, SetLastError = false, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true, EntryPoint = "JpegLsReadHeader")]
        internal static extern JpegLSError JpegLsReadHeaderX64([In] byte[] compressedSource, long compressedLength, out JlsParameters info, IntPtr reserved);

        [DllImport(NativeX86Library, SetLastError = false, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true, EntryPoint = "JpegLsDecode")]
        internal static extern JpegLSError JpegLsDecodeX86(
            [Out] byte[] uncompressedData,
            int uncompressedLength,
            [In] byte[] compressedData,
            int compressedLength,
            IntPtr info,
            IntPtr reserved);

        [DllImport(NativeX64Library, SetLastError = false, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true, EntryPoint = "JpegLsDecode")]
        internal static extern JpegLSError JpegLsDecodeX64(
            [Out] byte[] uncompressedData,
            long uncompressedLength,
            [In] byte[] compressedData,
            long compressedLength,
            IntPtr info,
            IntPtr reserved);

        [DllImport(NativeX86Library, SetLastError = false, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true, EntryPoint = "JpegLsEncode")]
        internal static extern JpegLSError JpegLsEncodeX86(
            [Out] byte[] compressedData,
            int compressedLength,
            out int byteCountWritten,
            [In] byte[] uncompressedData,
            int uncompressedLength,
            [In] ref JlsParameters info,
            IntPtr reserved);

        [DllImport(NativeX64Library, SetLastError = false, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true, EntryPoint = "JpegLsEncode")]
        internal static extern JpegLSError JpegLsEncodeX64(
            [Out] byte[] compressedData,
            long compressedLength,
            out long byteCountWritten,
            [In] byte[] uncompressedData,
            long uncompressedLength,
            [In] ref JlsParameters info,
            IntPtr reserved);

        [DllImport(NativeX86Library, SetLastError = false, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true, EntryPoint = "charls_get_error_message")]
        internal static extern IntPtr CharLSGetErrorMessageX86(int errorValue);

        [DllImport(NativeX64Library, SetLastError = false, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true, EntryPoint = "charls_get_error_message")]
        internal static extern IntPtr CharLSGetErrorMessageX64(int errorValue);

        [DllImport(NativeX86Library, SetLastError = false, CharSet = CharSet.Ansi, BestFitMapping = false,
            ThrowOnUnmappableChar = true, EntryPoint = "charls_jpegls_encoder_set_frame_info")]
        internal static extern JpegLSError CharLSSetFrameInfoX86(IntPtr encoder, [In] ref FrameInfoNative frameInfo);

        [DllImport(NativeX64Library, SetLastError = false, CharSet = CharSet.Ansi, BestFitMapping = false,
            ThrowOnUnmappableChar = true, EntryPoint = "charls_jpegls_encoder_set_frame_info")]
        internal static extern JpegLSError CharLSSetFrameInfoX64(IntPtr encoder, [In] ref FrameInfoNative frameInfo);

        [DllImport(NativeX86Library, SetLastError = false, CharSet = CharSet.Ansi, BestFitMapping = false,
            ThrowOnUnmappableChar = true, EntryPoint = "charls_jpegls_encoder_create")]
        internal static extern IntPtr CharLSCreateEncoderX86();

        [DllImport(NativeX64Library, SetLastError = false, CharSet = CharSet.Ansi, BestFitMapping = false,
            ThrowOnUnmappableChar = true, EntryPoint = "charls_jpegls_encoder_create")]
        internal static extern IntPtr CharLSCreateEncoderX64();

        [DllImport(NativeX86Library, SetLastError = false, CharSet = CharSet.Ansi, BestFitMapping = false,
            ThrowOnUnmappableChar = true, EntryPoint = "charls_jpegls_decoder_create")]
        internal static extern IntPtr CharLSCreateDecoderX86();

        [DllImport(NativeX64Library, SetLastError = false, CharSet = CharSet.Ansi, BestFitMapping = false,
            ThrowOnUnmappableChar = true, EntryPoint = "charls_jpegls_decoder_create")]
        internal static extern IntPtr CharLSCreateDecoderX64();

        [DllImport(NativeX86Library, SetLastError = false, CharSet = CharSet.Ansi, BestFitMapping = false,
            ThrowOnUnmappableChar = true, EntryPoint = "charls_jpegls_decoder_read_spiff_header")]
        internal static extern IntPtr CharLSReadSpiffHeaderX86(IntPtr decoder);

        [DllImport(NativeX64Library, SetLastError = false, CharSet = CharSet.Ansi, BestFitMapping = false,
            ThrowOnUnmappableChar = true, EntryPoint = "charls_jpegls_decoder_read_spiff_header")]
        internal static extern IntPtr CharLSReadSpiffHeaderX64(IntPtr decoder);

        [DllImport(NativeX86Library, SetLastError = false, CharSet = CharSet.Ansi, BestFitMapping = false,
            ThrowOnUnmappableChar = true, EntryPoint = "charls_jpegls_decoder_read_header")]
        internal static extern JpegLSError JpegLSDecoderReadHeaderX86(IntPtr decoder);

        [DllImport(NativeX64Library, SetLastError = false, CharSet = CharSet.Ansi, BestFitMapping = false,
            ThrowOnUnmappableChar = true, EntryPoint = "charls_jpegls_decoder_read_header")]
        internal static extern JpegLSError JpegLSDecoderReadHeaderX64(IntPtr decoder);
    }
}
