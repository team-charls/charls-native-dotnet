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

        [DllImport(NativeX86Library, SetLastError = false, CharSet = CharSet.Ansi, BestFitMapping = false,
            ThrowOnUnmappableChar = true, EntryPoint = "charls_get_error_message")]
        internal static extern IntPtr CharLSGetErrorMessageX86(int errorValue);

        [DllImport(NativeX64Library, SetLastError = false, CharSet = CharSet.Ansi, BestFitMapping = false,
            ThrowOnUnmappableChar = true, EntryPoint = "charls_get_error_message")]
        internal static extern IntPtr CharLSGetErrorMessageX64(int errorValue);

        [DllImport(NativeX86Library, SetLastError = false, CharSet = CharSet.Ansi, BestFitMapping = false,
            ThrowOnUnmappableChar = true, EntryPoint = "charls_jpegls_encoder_set_frame_info")]
        internal static extern JpegLSError CharLSSetFrameInfoX86(SafeHandleJpegLSEncoder encoder, [In] ref FrameInfoNative frameInfo);

        [DllImport(NativeX64Library, SetLastError = false, CharSet = CharSet.Ansi, BestFitMapping = false,
            ThrowOnUnmappableChar = true, EntryPoint = "charls_jpegls_encoder_set_frame_info")]
        internal static extern JpegLSError CharLSSetFrameInfoX64(SafeHandleJpegLSEncoder encoder, [In] ref FrameInfoNative frameInfo);

        [DllImport(NativeX64Library, SetLastError = false, CharSet = CharSet.Ansi, BestFitMapping = false,
            ThrowOnUnmappableChar = true, EntryPoint = "charls_jpegls_encoder_set_near_lossless")]
        internal static extern JpegLSError CharLSSetNearLosslessX64(SafeHandleJpegLSEncoder encoder, [In] int nearLossless);

        [DllImport(NativeX86Library, SetLastError = false, CharSet = CharSet.Ansi, BestFitMapping = false,
            ThrowOnUnmappableChar = true, EntryPoint = "charls_jpegls_encoder_set_near_lossless")]
        internal static extern JpegLSError CharLSSetNearLosslessX86(SafeHandleJpegLSEncoder encoder, [In] int nearLossless);

        [DllImport(NativeX86Library, SetLastError = false, CharSet = CharSet.Ansi, BestFitMapping = false,
            ThrowOnUnmappableChar = true, EntryPoint = "charls_jpegls_encoder_create")]
        internal static extern SafeHandleJpegLSEncoder CharLSCreateEncoderX86();

        [DllImport(NativeX64Library, SetLastError = false, CharSet = CharSet.Ansi, BestFitMapping = false,
            ThrowOnUnmappableChar = true, EntryPoint = "charls_jpegls_encoder_create")]
        internal static extern SafeHandleJpegLSEncoder CharLSCreateEncoderX64();

        [DllImport(NativeX86Library, SetLastError = false, CharSet = CharSet.Ansi, BestFitMapping = false,
            ThrowOnUnmappableChar = true, EntryPoint = "charls_jpegls_encoder_destroy")]
        internal static extern void CharLSDestroyEncoderX86(IntPtr encoder);

        [DllImport(NativeX64Library, SetLastError = false, CharSet = CharSet.Ansi, BestFitMapping = false,
            ThrowOnUnmappableChar = true, EntryPoint = "charls_jpegls_encoder_destroy")]
        internal static extern void CharLSDestroyEncoderX64(IntPtr encoder);

        [DllImport(NativeX86Library, SetLastError = false, CharSet = CharSet.Ansi, BestFitMapping = false,
            ThrowOnUnmappableChar = true, EntryPoint = "charls_jpegls_encoder_set_destination_buffer")]
        internal static extern JpegLSError CharLSSetDestinationBufferX86(SafeHandleJpegLSEncoder encoder, [In] byte[] destination,
            uint destinationLength);

        [DllImport(NativeX64Library, SetLastError = false, CharSet = CharSet.Ansi, BestFitMapping = false,
            ThrowOnUnmappableChar = true, EntryPoint = "charls_jpegls_encoder_set_destination_buffer")]
        internal static extern JpegLSError CharLSSetDestinationBufferX64(SafeHandleJpegLSEncoder encoder, [In] byte[] destination,
            uint destinationLength);

        [DllImport(NativeX86Library, SetLastError = false, CharSet = CharSet.Ansi, BestFitMapping = false,
            ThrowOnUnmappableChar = true, EntryPoint = "charls_jpegls_encoder_get_estimated_destination_size")]
        internal static extern JpegLSError CharLSGetEstimatedDestinationSizeX86(SafeHandleJpegLSEncoder encoder, [Out] out IntPtr sizeInBytes);

        [DllImport(NativeX64Library, SetLastError = false, CharSet = CharSet.Ansi, BestFitMapping = false,
            ThrowOnUnmappableChar = true, EntryPoint = "charls_jpegls_encoder_get_estimated_destination_size")]
        internal static extern JpegLSError CharLSGetEstimatedDestinationSizeX64(SafeHandleJpegLSEncoder encoder, [Out] out IntPtr sizeInBytes);

        [DllImport(NativeX86Library, SetLastError = false, CharSet = CharSet.Ansi, BestFitMapping = false,
            ThrowOnUnmappableChar = true, EntryPoint = "charls_jpegls_encoder_encode_from_buffer")]
        internal static extern JpegLSError CharLSEncodeFromBufferX86(SafeHandleJpegLSEncoder encoder, [In] byte[] source, IntPtr sourceSizeBytes, uint stride);

        [DllImport(NativeX64Library, SetLastError = false, CharSet = CharSet.Ansi, BestFitMapping = false,
            ThrowOnUnmappableChar = true, EntryPoint = "charls_jpegls_encoder_encode_from_buffer")]
        internal static extern JpegLSError CharLSEncodeFromBufferX64(SafeHandleJpegLSEncoder encoder, [In] byte[] source, IntPtr sourceSizeBytes, uint stride);

        [DllImport(NativeX86Library, SetLastError = false, CharSet = CharSet.Ansi, BestFitMapping = false,
            ThrowOnUnmappableChar = true, EntryPoint = "charls_jpegls_encoder_get_bytes_written")]
        internal static extern JpegLSError CharLSGetBytesWrittenX86(SafeHandleJpegLSEncoder encoder, [Out] out IntPtr bytesWritten);

        [DllImport(NativeX64Library, SetLastError = false, CharSet = CharSet.Ansi, BestFitMapping = false,
            ThrowOnUnmappableChar = true, EntryPoint = "charls_jpegls_encoder_get_bytes_written")]
        internal static extern JpegLSError CharLSGetBytesWrittenX64(SafeHandleJpegLSEncoder encoder, [Out] out IntPtr bytesWritten);

        [DllImport(NativeX86Library, SetLastError = false, CharSet = CharSet.Ansi, BestFitMapping = false,
            ThrowOnUnmappableChar = true, EntryPoint = "charls_jpegls_decoder_create")]
        internal static extern SafeHandleJpegLSDecoder CharLSCreateDecoderX86();

        [DllImport(NativeX64Library, SetLastError = false, CharSet = CharSet.Ansi, BestFitMapping = false,
            ThrowOnUnmappableChar = true, EntryPoint = "charls_jpegls_decoder_create")]
        internal static extern SafeHandleJpegLSDecoder CharLSCreateDecoderX64();

        [DllImport(NativeX86Library, SetLastError = false, CharSet = CharSet.Ansi, BestFitMapping = false,
            ThrowOnUnmappableChar = true, EntryPoint = "charls_jpegls_decoder_destroy")]
        internal static extern void CharLSDestroyDecoderX86(IntPtr decoder);

        [DllImport(NativeX64Library, SetLastError = false, CharSet = CharSet.Ansi, BestFitMapping = false,
            ThrowOnUnmappableChar = true, EntryPoint = "charls_jpegls_decoder_destroy")]
        internal static extern void CharLSDestroyDecoderX64(IntPtr decoder);

        [DllImport(NativeX86Library, SetLastError = false, CharSet = CharSet.Ansi, BestFitMapping = false,
        ThrowOnUnmappableChar = true, EntryPoint = "charls_jpegls_decoder_set_source_buffer")]
        internal static extern JpegLSError CharLSSetSourceBufferX86(SafeHandleJpegLSDecoder decoder, [In] byte[] source,
            int sourceLength);

        [DllImport(NativeX64Library, SetLastError = false, CharSet = CharSet.Ansi, BestFitMapping = false,
        ThrowOnUnmappableChar = true, EntryPoint = "charls_jpegls_decoder_set_source_buffer")]
        internal static extern JpegLSError CharLSSetSourceBufferX64(SafeHandleJpegLSDecoder decoder, [In] byte[] source,
            int sourceLength);

        [DllImport(NativeX86Library, SetLastError = false, CharSet = CharSet.Ansi, BestFitMapping = false,
            ThrowOnUnmappableChar = true, EntryPoint = "charls_jpegls_decoder_read_spiff_header")]
        internal static extern IntPtr CharLSReadSpiffHeaderX86(SafeHandleJpegLSDecoder decoder);

        [DllImport(NativeX64Library, SetLastError = false, CharSet = CharSet.Ansi, BestFitMapping = false,
            ThrowOnUnmappableChar = true, EntryPoint = "charls_jpegls_decoder_read_spiff_header")]
        internal static extern IntPtr CharLSReadSpiffHeaderX64(SafeHandleJpegLSDecoder decoder);

        [DllImport(NativeX86Library, SetLastError = false, CharSet = CharSet.Ansi, BestFitMapping = false,
            ThrowOnUnmappableChar = true, EntryPoint = "charls_jpegls_decoder_read_header")]
        internal static extern JpegLSError JpegLSDecoderReadHeaderX86(SafeHandleJpegLSDecoder decoder);

        [DllImport(NativeX64Library, SetLastError = false, CharSet = CharSet.Ansi, BestFitMapping = false,
            ThrowOnUnmappableChar = true, EntryPoint = "charls_jpegls_decoder_read_header")]
        internal static extern JpegLSError JpegLSDecoderReadHeaderX64(SafeHandleJpegLSDecoder decoder);

        [DllImport(NativeX86Library, SetLastError = false, CharSet = CharSet.Ansi, BestFitMapping = false,
            ThrowOnUnmappableChar = true, EntryPoint = "charls_jpegls_decoder_get_frame_info")]
        internal static extern JpegLSError CharLSGetFrameInfoX86(SafeHandleJpegLSDecoder decoder, [Out] out FrameInfoNative frameInfo);

        [DllImport(NativeX64Library, SetLastError = false, CharSet = CharSet.Ansi, BestFitMapping = false,
            ThrowOnUnmappableChar = true, EntryPoint = "charls_jpegls_decoder_get_frame_info")]
        internal static extern JpegLSError CharLSGetFrameInfoX64(SafeHandleJpegLSDecoder decoder, [Out] out FrameInfoNative frameInfo);

        [DllImport(NativeX86Library, SetLastError = false, CharSet = CharSet.Ansi, BestFitMapping = false,
            ThrowOnUnmappableChar = true, EntryPoint = "charls_jpegls_decoder_get_near_lossless")]
        internal static extern JpegLSError CharLSGetNearLosslessX86(SafeHandleJpegLSDecoder decoder, int component, [Out] out int nearLossless);

        [DllImport(NativeX64Library, SetLastError = false, CharSet = CharSet.Ansi, BestFitMapping = false,
            ThrowOnUnmappableChar = true, EntryPoint = "charls_jpegls_decoder_get_near_lossless")]
        internal static extern JpegLSError CharLSGetNearLosslessX64(SafeHandleJpegLSDecoder decoder, int component, [Out] out int nearLossless);

        [DllImport(NativeX64Library, SetLastError = false, CharSet = CharSet.Ansi, BestFitMapping = false,
            ThrowOnUnmappableChar = true, EntryPoint = "charls_jpegls_decoder_get_interleave_mode")]
        internal static extern JpegLSError CharLSGetInterleaveModeX64(SafeHandleJpegLSDecoder decoder, [Out] out JpegLSInterleaveMode interleaveMode);

        [DllImport(NativeX86Library, SetLastError = false, CharSet = CharSet.Ansi, BestFitMapping = false,
            ThrowOnUnmappableChar = true, EntryPoint = "charls_jpegls_decoder_get_interleave_mode")]
        internal static extern JpegLSError CharLSGetInterleaveModeX86(SafeHandleJpegLSDecoder decoder, [Out] out JpegLSInterleaveMode interleaveMode);

        [DllImport(NativeX86Library, SetLastError = false, CharSet = CharSet.Ansi, BestFitMapping = false,
            ThrowOnUnmappableChar = true, EntryPoint = "charls_jpegls_decoder_get_destination_size")]
        internal static extern JpegLSError CharLSGetDestinationSizeX86(SafeHandleJpegLSDecoder decoder, uint stride, [Out] out UIntPtr destinationSize);

        [DllImport(NativeX64Library, SetLastError = false, CharSet = CharSet.Ansi, BestFitMapping = false,
            ThrowOnUnmappableChar = true, EntryPoint = "charls_jpegls_decoder_get_destination_size")]
        internal static extern JpegLSError CharLSGetDestinationSizeX64(SafeHandleJpegLSDecoder decoder, uint stride, [Out] out UIntPtr destinationSize);

        [DllImport(NativeX86Library, SetLastError = false, CharSet = CharSet.Ansi, BestFitMapping = false,
            ThrowOnUnmappableChar = true, EntryPoint = "charls_jpegls_decoder_decode_to_buffer")]
        internal static extern JpegLSError CharLSDecodeToBufferX86(SafeHandleJpegLSDecoder decoder, byte[] destination, UIntPtr destinationSize, uint stride);

        [DllImport(NativeX64Library, SetLastError = false, CharSet = CharSet.Ansi, BestFitMapping = false,
            ThrowOnUnmappableChar = true, EntryPoint = "charls_jpegls_decoder_decode_to_buffer")]
        internal static extern JpegLSError CharLSDecodeToBufferX64(SafeHandleJpegLSDecoder decoder, byte[] destination, UIntPtr destinationSize, uint stride);
    }
}
