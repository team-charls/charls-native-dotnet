// Copyright (c) Team CharLS.
// SPDX-License-Identifier: BSD-3-Clause

using System;
using System.Reflection;
using System.Runtime.InteropServices;
using CharLS.Native;
using size_t = System.UIntPtr;

namespace CharLS
{
    internal static class SafeNativeMethods
    {
        private const string NativeLibraryName = "charls-2";

        static SafeNativeMethods()
        {
            NativeLibrary.SetDllImportResolver(Assembly.GetExecutingAssembly(), DllImportResolver);
        }

        [DllImport(NativeLibraryName, SetLastError = false, CharSet = CharSet.Ansi, BestFitMapping = false,
            ThrowOnUnmappableChar = true, EntryPoint = "charls_get_error_message")]
        internal static extern IntPtr CharLSGetErrorMessage(int errorValue);

        [DllImport(NativeLibraryName, SetLastError = false, EntryPoint = "charls_jpegls_encoder_set_frame_info")]
        internal static extern JpegLSError CharLSSetFrameInfo(SafeHandleJpegLSEncoder encoder, [In] ref FrameInfoNative frameInfo);

        [DllImport(NativeLibraryName, SetLastError = false, EntryPoint = "charls_jpegls_encoder_set_near_lossless")]
        internal static extern JpegLSError CharLSSetNearLossless(SafeHandleJpegLSEncoder encoder, [In] int nearLossless);

        [DllImport(NativeLibraryName, SetLastError = false, EntryPoint = "charls_jpegls_encoder_create")]
        internal static extern SafeHandleJpegLSEncoder CharLSCreateEncoder();

        [DllImport(NativeLibraryName, SetLastError = false, EntryPoint = "charls_jpegls_encoder_destroy")]
        internal static extern void CharLSDestroyEncoder(IntPtr encoder);

        [DllImport(NativeLibraryName, SetLastError = false, EntryPoint = "charls_jpegls_encoder_set_destination_buffer")]
        internal static extern unsafe JpegLSError CharLSSetDestinationBuffer(SafeHandleJpegLSEncoder encoder, byte* destination, size_t destinationLength);

        [DllImport(NativeLibraryName, SetLastError = false, EntryPoint = "charls_jpegls_encoder_get_estimated_destination_size")]
        internal static extern JpegLSError CharLSGetEstimatedDestinationSize(SafeHandleJpegLSEncoder encoder, [Out] out size_t sizeInBytes);

        [DllImport(NativeLibraryName, SetLastError = false, EntryPoint = "charls_jpegls_encoder_encode_from_buffer")]
        internal static extern JpegLSError CharLSEncodeFromBuffer(SafeHandleJpegLSEncoder encoder, ref byte source, size_t sourceSizeBytes, uint stride);

        [DllImport(NativeLibraryName, SetLastError = false, EntryPoint = "charls_jpegls_encoder_write_standard_spiff_header")]
        internal static extern JpegLSError CharLSWriteStandardSpiffHeader(SafeHandleJpegLSEncoder encoder,
            SpiffColorSpace colorSpace, SpiffResolutionUnit resolutionUnit, uint verticalResolution, uint horizontalResolution);

        [DllImport(NativeLibraryName, SetLastError = false, EntryPoint = "charls_jpegls_encoder_write_spiff_header")]
        internal static extern JpegLSError CharLSWriteSpiffHeader(SafeHandleJpegLSEncoder encoder, [In] ref SpiffHeaderNative spiffHeader);

        [DllImport(NativeLibraryName, SetLastError = false, EntryPoint = "charls_jpegls_encoder_get_bytes_written")]
        internal static extern JpegLSError CharLSGetBytesWritten(SafeHandleJpegLSEncoder encoder, [Out] out size_t bytesWritten);

        [DllImport(NativeLibraryName, SetLastError = false, EntryPoint = "charls_jpegls_decoder_create")]
        internal static extern SafeHandleJpegLSDecoder CharLSCreateDecoder();

        [DllImport(NativeLibraryName, SetLastError = false, EntryPoint = "charls_jpegls_decoder_destroy")]
        internal static extern void CharLSDestroyDecoder(IntPtr decoder);

        [DllImport(NativeLibraryName, SetLastError = false, EntryPoint = "charls_jpegls_decoder_set_source_buffer")]
        internal static extern unsafe JpegLSError CharLSSetSourceBuffer(SafeHandleJpegLSDecoder decoder, byte* source, size_t sourceLength);

        [DllImport(NativeLibraryName, SetLastError = false, EntryPoint = "charls_jpegls_decoder_read_spiff_header")]
        internal static extern JpegLSError CharLSReadSpiffHeader(SafeHandleJpegLSDecoder decoder,
            [Out] out SpiffHeaderNative spiffHeader, [Out] out int headerFound);

        [DllImport(NativeLibraryName, SetLastError = false, EntryPoint = "charls_jpegls_decoder_read_header")]
        internal static extern JpegLSError JpegLSDecoderReadHeader(SafeHandleJpegLSDecoder decoder);

        [DllImport(NativeLibraryName, SetLastError = false, EntryPoint = "charls_jpegls_decoder_get_frame_info")]
        internal static extern JpegLSError CharLSGetFrameInfo(SafeHandleJpegLSDecoder decoder, [Out] out FrameInfoNative frameInfo);

        [DllImport(NativeLibraryName, SetLastError = false, EntryPoint = "charls_jpegls_decoder_get_near_lossless")]
        internal static extern JpegLSError CharLSGetNearLossless(SafeHandleJpegLSDecoder decoder, int component, [Out] out int nearLossless);

        [DllImport(NativeLibraryName, SetLastError = false, EntryPoint = "charls_jpegls_decoder_get_interleave_mode")]
        internal static extern JpegLSError CharLSGetInterleaveMode(SafeHandleJpegLSDecoder decoder, [Out] out JpegLSInterleaveMode interleaveMode);

        [DllImport(NativeLibraryName, SetLastError = false, EntryPoint = "charls_jpegls_decoder_get_destination_size")]
        internal static extern JpegLSError CharLSGetDestinationSize(SafeHandleJpegLSDecoder decoder, uint stride, [Out] out size_t destinationSize);

        [DllImport(NativeLibraryName, SetLastError = false, EntryPoint = "charls_jpegls_decoder_decode_to_buffer")]
        internal static extern JpegLSError CharLSDecodeToBuffer(SafeHandleJpegLSDecoder decoder, ref byte destination, size_t destinationSize, uint stride);

        private static IntPtr DllImportResolver(string libraryName, Assembly assembly, DllImportSearchPath? searchPath)
        {
            IntPtr libHandle = IntPtr.Zero;
            if (libraryName == NativeLibraryName)
            {
                string library;

                if (Environment.Is64BitProcess)
                {
                    library = "charls-2-x64.dll";
                }
                else
                {
                    library = "charls-2-x86.dll";
                }

                var libraryLoaded = NativeLibrary.TryLoad(library, assembly, DllImportSearchPath.AssemblyDirectory, out libHandle);
                if (!libraryLoaded)
                {
                    throw new Exception($"Unable to Load Library: {library}");
                }
            }

            return libHandle;
        }
    }
}
