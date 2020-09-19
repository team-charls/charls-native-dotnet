// Copyright (c) Team CharLS.
// SPDX-License-Identifier: BSD-3-Clause

using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using size_t = System.UIntPtr;

namespace CharLS.Native
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

        [DllImport(NativeLibraryName, SetLastError = false, EntryPoint = "charls_jpegls_encoder_set_interleave_mode")]
        internal static extern JpegLSError CharLSSetInterleaveMode(SafeHandleJpegLSEncoder encoder, [In] JpegLSInterleaveMode interleaveMode);

        [DllImport(NativeLibraryName, SetLastError = false, EntryPoint = "charls_jpegls_encoder_set_preset_coding_parameters")]
        internal static extern JpegLSError CharLSSetPresetCodingParameters(SafeHandleJpegLSEncoder encoder, [In] ref JpegLSPresetCodingParametersNative presetCodingParameters);

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

        [DllImport(NativeLibraryName, SetLastError = false, EntryPoint = "charls_jpegls_decoder_get_preset_coding_parameters")]
        internal static extern JpegLSError CharLSGetPresetCodingParameters(SafeHandleJpegLSDecoder decoder, int reserved, [Out] out JpegLSPresetCodingParametersNative presetCodingParameters);

        [DllImport(NativeLibraryName, SetLastError = false, EntryPoint = "charls_jpegls_decoder_get_destination_size")]
        internal static extern JpegLSError CharLSGetDestinationSize(SafeHandleJpegLSDecoder decoder, uint stride, [Out] out size_t destinationSize);

        [DllImport(NativeLibraryName, SetLastError = false, EntryPoint = "charls_jpegls_decoder_decode_to_buffer")]
        internal static extern JpegLSError CharLSDecodeToBuffer(SafeHandleJpegLSDecoder decoder, ref byte destination, size_t destinationSize, uint stride);

        internal static void HandleJpegLSError(JpegLSError result)
        {
            Exception exception;

            switch (result)
            {
                case JpegLSError.None:
                    return;

                case JpegLSError.TooMuchEncodedData:
                case JpegLSError.ParameterValueNotSupported:
                case JpegLSError.InvalidEncodedData:
                case JpegLSError.SourceBufferTooSmall:
                case JpegLSError.BitDepthForTransformNotSupported:
                case JpegLSError.ColorTransformNotSupported:
                case JpegLSError.EncodingNotSupported:
                case JpegLSError.UnknownJpegMarkerFound:
                case JpegLSError.JpegMarkerStartByteNotFound:
                case JpegLSError.StartOfImageMarkerNotFound:
                case JpegLSError.StartOfFrameMarkerNotFound:
                case JpegLSError.InvalidMarkerSegmentSize:
                case JpegLSError.DuplicateStartOfImageMarker:
                case JpegLSError.DuplicateStartOfFrameMarker:
                case JpegLSError.DuplicateComponentIdInStartOfFrameSegment:
                case JpegLSError.UnexpectedEndOfImageMarker:
                case JpegLSError.InvalidJpegLSPresetParameterType:
                case JpegLSError.JpeglsPresetExtendedParameterTypeNotSupported:
                case JpegLSError.MissingEndOfSpiffDirectory:
                case JpegLSError.InvalidParameterWidth:
                case JpegLSError.InvalidParameterHeight:
                case JpegLSError.InvalidParameterComponentCount:
                case JpegLSError.InvalidParameterBitsPerSample:
                case JpegLSError.InvalidParameterInterleaveMode:
                case JpegLSError.UnexpectedFailure:
                case JpegLSError.NotEnoughMemory:
                    exception = new InvalidDataException(GetErrorMessage(result));
                    break;

                case JpegLSError.InvalidArgument:
                case JpegLSError.DestinationBufferTooSmall:
                case JpegLSError.InvalidArgumentWidth:
                case JpegLSError.InvalidArgumentHeight:
                case JpegLSError.InvalidArgumentComponentCount:
                case JpegLSError.InvalidArgumentBitsPerSample:
                case JpegLSError.InvalidArgumentInterleaveMode:
                case JpegLSError.InvalidArgumentNearLossless:
                case JpegLSError.InvalidArgumentPresetCodingParameters:
                case JpegLSError.InvalidArgumentSpiffEntrySize:
                case JpegLSError.InvalidArgumentColorTransformation:
                    exception = new ArgumentException(GetErrorMessage(result));
                    break;

                case JpegLSError.InvalidOperation:
                    exception = new InvalidOperationException(GetErrorMessage(result));
                    break;

                default:
                    Debug.Assert(false, "C# and native implementation mismatch");

                    // ReSharper disable once HeuristicUnreachableCode
                    exception = new InvalidOperationException(GetErrorMessage(result));
                    break;
            }

            IDictionary? data = exception.Data;

            // ReSharper disable once PossibleNullReferenceException
            data.Add(nameof(JpegLSError), result);
            throw exception;
        }

        private static string GetErrorMessage(JpegLSError result)
        {
            IntPtr message = CharLSGetErrorMessage((int)result);

            return Marshal.PtrToStringAnsi(message) ?? string.Empty;
        }

        private static IntPtr DllImportResolver(string libraryName, Assembly assembly, DllImportSearchPath? searchPath)
        {
            if (libraryName != NativeLibraryName)
                return IntPtr.Zero;

            string library = Environment.Is64BitProcess ? "charls-2-x64.dll" : "charls-2-x86.dll";
            return NativeLibrary.Load(library, assembly, DllImportSearchPath.AssemblyDirectory);
        }
    }
}
