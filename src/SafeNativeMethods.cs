// Copyright (c) Team CharLS.
// SPDX-License-Identifier: BSD-3-Clause

using System;
using System.Runtime.InteropServices;
using CharLS.Native;
using size_t = System.UIntPtr;

namespace CharLS
{
    internal static class SafeNativeMethods
    {
        private const string NativeX86Library = "charls-2-x86.dll";
        private const string NativeX64Library = "charls-2-x64.dll";

        [DllImport(NativeX86Library, SetLastError = false, CharSet = CharSet.Ansi, BestFitMapping = false,
            ThrowOnUnmappableChar = true, EntryPoint = "charls_get_error_message")]
        internal static extern IntPtr CharLSGetErrorMessageX86(int errorValue);

        [DllImport(NativeX64Library, SetLastError = false, CharSet = CharSet.Ansi, BestFitMapping = false,
            ThrowOnUnmappableChar = true, EntryPoint = "charls_get_error_message")]
        internal static extern IntPtr CharLSGetErrorMessageX64(int errorValue);

        [DllImport(NativeX86Library, SetLastError = false, EntryPoint = "charls_jpegls_encoder_set_frame_info")]
        internal static extern JpegLSError CharLSSetFrameInfoX86(SafeHandleJpegLSEncoder encoder, [In] ref FrameInfoNative frameInfo);

        [DllImport(NativeX64Library, SetLastError = false, EntryPoint = "charls_jpegls_encoder_set_frame_info")]
        internal static extern JpegLSError CharLSSetFrameInfoX64(SafeHandleJpegLSEncoder encoder, [In] ref FrameInfoNative frameInfo);

        [DllImport(NativeX64Library, SetLastError = false, EntryPoint = "charls_jpegls_encoder_set_near_lossless")]
        internal static extern JpegLSError CharLSSetNearLosslessX64(SafeHandleJpegLSEncoder encoder, [In] int nearLossless);

        [DllImport(NativeX86Library, SetLastError = false, EntryPoint = "charls_jpegls_encoder_set_near_lossless")]
        internal static extern JpegLSError CharLSSetNearLosslessX86(SafeHandleJpegLSEncoder encoder, [In] int nearLossless);

        [DllImport(NativeX86Library, SetLastError = false, EntryPoint = "charls_jpegls_encoder_create")]
        internal static extern SafeHandleJpegLSEncoder CharLSCreateEncoderX86();

        [DllImport(NativeX64Library, SetLastError = false, EntryPoint = "charls_jpegls_encoder_create")]
        internal static extern SafeHandleJpegLSEncoder CharLSCreateEncoderX64();

        [DllImport(NativeX86Library, SetLastError = false, EntryPoint = "charls_jpegls_encoder_destroy")]
        internal static extern void CharLSDestroyEncoderX86(IntPtr encoder);

        [DllImport(NativeX64Library, SetLastError = false, EntryPoint = "charls_jpegls_encoder_destroy")]
        internal static extern void CharLSDestroyEncoderX64(IntPtr encoder);

        [DllImport(NativeX86Library, SetLastError = false, EntryPoint = "charls_jpegls_encoder_set_destination_buffer")]
        internal static extern unsafe JpegLSError CharLSSetDestinationBufferX86(SafeHandleJpegLSEncoder encoder, byte* destination, size_t destinationLength);

        [DllImport(NativeX64Library, SetLastError = false, EntryPoint = "charls_jpegls_encoder_set_destination_buffer")]
        internal static extern unsafe JpegLSError CharLSSetDestinationBufferX64(SafeHandleJpegLSEncoder encoder, byte* destination, size_t destinationLength);

        [DllImport(NativeX86Library, SetLastError = false, EntryPoint = "charls_jpegls_encoder_get_estimated_destination_size")]
        internal static extern JpegLSError CharLSGetEstimatedDestinationSizeX86(SafeHandleJpegLSEncoder encoder, [Out] out size_t sizeInBytes);

        [DllImport(NativeX64Library, SetLastError = false, EntryPoint = "charls_jpegls_encoder_get_estimated_destination_size")]
        internal static extern JpegLSError CharLSGetEstimatedDestinationSizeX64(SafeHandleJpegLSEncoder encoder, [Out] out size_t sizeInBytes);

        [DllImport(NativeX86Library, SetLastError = false, EntryPoint = "charls_jpegls_encoder_encode_from_buffer")]
        internal static extern JpegLSError CharLSEncodeFromBufferX86(SafeHandleJpegLSEncoder encoder, ref byte source, size_t sourceSizeBytes, uint stride);

        [DllImport(NativeX64Library, SetLastError = false, EntryPoint = "charls_jpegls_encoder_encode_from_buffer")]
        internal static extern JpegLSError CharLSEncodeFromBufferX64(SafeHandleJpegLSEncoder encoder, ref byte source, size_t sourceSizeBytes, uint stride);

        [DllImport(NativeX86Library, SetLastError = false, EntryPoint = "charls_jpegls_encoder_write_standard_spiff_header")]
        internal static extern JpegLSError CharLSWriteStandardSpiffHeaderX86(SafeHandleJpegLSEncoder encoder,
            SpiffColorSpace colorSpace, SpiffResolutionUnit resolutionUnit, uint verticalResolution, uint horizontalResolution);

        [DllImport(NativeX64Library, SetLastError = false, EntryPoint = "charls_jpegls_encoder_write_standard_spiff_header")]
        internal static extern JpegLSError CharLSWriteStandardSpiffHeaderX64(SafeHandleJpegLSEncoder encoder,
            SpiffColorSpace colorSpace, SpiffResolutionUnit resolutionUnit, uint verticalResolution, uint horizontalResolution);

        [DllImport(NativeX86Library, SetLastError = false, EntryPoint = "charls_jpegls_encoder_write_spiff_header")]
        internal static extern JpegLSError CharLSWriteSpiffHeaderX86(SafeHandleJpegLSEncoder encoder, [In] ref SpiffHeaderNative spiffHeader);

        [DllImport(NativeX64Library, SetLastError = false, EntryPoint = "charls_jpegls_encoder_write_spiff_header")]
        internal static extern JpegLSError CharLSWriteSpiffHeaderX64(SafeHandleJpegLSEncoder encoder, [In] ref SpiffHeaderNative spiffHeader);

        [DllImport(NativeX86Library, SetLastError = false, EntryPoint = "charls_jpegls_encoder_get_bytes_written")]
        internal static extern JpegLSError CharLSGetBytesWrittenX86(SafeHandleJpegLSEncoder encoder, [Out] out size_t bytesWritten);

        [DllImport(NativeX64Library, SetLastError = false, EntryPoint = "charls_jpegls_encoder_get_bytes_written")]
        internal static extern JpegLSError CharLSGetBytesWrittenX64(SafeHandleJpegLSEncoder encoder, [Out] out size_t bytesWritten);

        [DllImport(NativeX86Library, SetLastError = false, EntryPoint = "charls_jpegls_decoder_create")]
        internal static extern SafeHandleJpegLSDecoder CharLSCreateDecoderX86();

        [DllImport(NativeX64Library, SetLastError = false, EntryPoint = "charls_jpegls_decoder_create")]
        internal static extern SafeHandleJpegLSDecoder CharLSCreateDecoderX64();

        [DllImport(NativeX86Library, SetLastError = false, EntryPoint = "charls_jpegls_decoder_destroy")]
        internal static extern void CharLSDestroyDecoderX86(IntPtr decoder);

        [DllImport(NativeX64Library, SetLastError = false, EntryPoint = "charls_jpegls_decoder_destroy")]
        internal static extern void CharLSDestroyDecoderX64(IntPtr decoder);

        [DllImport(NativeX86Library, SetLastError = false, EntryPoint = "charls_jpegls_decoder_set_source_buffer")]
        internal static extern unsafe JpegLSError CharLSSetSourceBufferX86(SafeHandleJpegLSDecoder decoder, byte* source, size_t sourceLength);

        [DllImport(NativeX64Library, SetLastError = false, EntryPoint = "charls_jpegls_decoder_set_source_buffer")]
        internal static extern unsafe JpegLSError CharLSSetSourceBufferX64(SafeHandleJpegLSDecoder decoder, byte* source, size_t sourceLength);

        [DllImport(NativeX86Library, SetLastError = false, EntryPoint = "charls_jpegls_decoder_read_spiff_header")]
        internal static extern JpegLSError CharLSReadSpiffHeaderX86(SafeHandleJpegLSDecoder decoder,
            [Out] out SpiffHeaderNative spiffHeader, [Out] out int headerFound);

        [DllImport(NativeX64Library, SetLastError = false, EntryPoint = "charls_jpegls_decoder_read_spiff_header")]
        internal static extern JpegLSError CharLSReadSpiffHeaderX64(SafeHandleJpegLSDecoder decoder,
            [Out] out SpiffHeaderNative spiffHeader, [Out] out int headerFound);

        [DllImport(NativeX86Library, SetLastError = false, EntryPoint = "charls_jpegls_decoder_read_header")]
        internal static extern JpegLSError JpegLSDecoderReadHeaderX86(SafeHandleJpegLSDecoder decoder);

        [DllImport(NativeX64Library, SetLastError = false, EntryPoint = "charls_jpegls_decoder_read_header")]
        internal static extern JpegLSError JpegLSDecoderReadHeaderX64(SafeHandleJpegLSDecoder decoder);

        [DllImport(NativeX86Library, SetLastError = false, EntryPoint = "charls_jpegls_decoder_get_frame_info")]
        internal static extern JpegLSError CharLSGetFrameInfoX86(SafeHandleJpegLSDecoder decoder, [Out] out FrameInfoNative frameInfo);

        [DllImport(NativeX64Library, SetLastError = false, EntryPoint = "charls_jpegls_decoder_get_frame_info")]
        internal static extern JpegLSError CharLSGetFrameInfoX64(SafeHandleJpegLSDecoder decoder, [Out] out FrameInfoNative frameInfo);

        [DllImport(NativeX86Library, SetLastError = false, EntryPoint = "charls_jpegls_decoder_get_near_lossless")]
        internal static extern JpegLSError CharLSGetNearLosslessX86(SafeHandleJpegLSDecoder decoder, int component, [Out] out int nearLossless);

        [DllImport(NativeX64Library, SetLastError = false, EntryPoint = "charls_jpegls_decoder_get_near_lossless")]
        internal static extern JpegLSError CharLSGetNearLosslessX64(SafeHandleJpegLSDecoder decoder, int component, [Out] out int nearLossless);

        [DllImport(NativeX64Library, SetLastError = false, EntryPoint = "charls_jpegls_decoder_get_interleave_mode")]
        internal static extern JpegLSError CharLSGetInterleaveModeX64(SafeHandleJpegLSDecoder decoder, [Out] out JpegLSInterleaveMode interleaveMode);

        [DllImport(NativeX86Library, SetLastError = false, EntryPoint = "charls_jpegls_decoder_get_interleave_mode")]
        internal static extern JpegLSError CharLSGetInterleaveModeX86(SafeHandleJpegLSDecoder decoder, [Out] out JpegLSInterleaveMode interleaveMode);

        [DllImport(NativeX86Library, SetLastError = false, EntryPoint = "charls_jpegls_decoder_get_destination_size")]
        internal static extern JpegLSError CharLSGetDestinationSizeX86(SafeHandleJpegLSDecoder decoder, uint stride, [Out] out size_t destinationSize);

        [DllImport(NativeX64Library, SetLastError = false, EntryPoint = "charls_jpegls_decoder_get_destination_size")]
        internal static extern JpegLSError CharLSGetDestinationSizeX64(SafeHandleJpegLSDecoder decoder, uint stride, [Out] out size_t destinationSize);

        [DllImport(NativeX86Library, SetLastError = false, EntryPoint = "charls_jpegls_decoder_decode_to_buffer")]
        internal static extern JpegLSError CharLSDecodeToBufferX86(SafeHandleJpegLSDecoder decoder, ref byte destination, size_t destinationSize, uint stride);

        [DllImport(NativeX64Library, SetLastError = false, EntryPoint = "charls_jpegls_decoder_decode_to_buffer")]
        internal static extern JpegLSError CharLSDecodeToBufferX64(SafeHandleJpegLSDecoder decoder, ref byte destination, size_t destinationSize, uint stride);
    }
}
