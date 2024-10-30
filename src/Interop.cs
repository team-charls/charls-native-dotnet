// Copyright (c) Team CharLS.
// SPDX-License-Identifier: BSD-3-Clause

using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
#if NET8_0_OR_GREATER
using System.Reflection;
#endif
using System.Runtime.InteropServices;

namespace CharLS.Native;


#if NET8_0_OR_GREATER
internal static partial class Interop
#else
internal static class Interop
#endif
{
#if NET8_0_OR_GREATER
    private const string NativeLibraryName = "charls-2";
#else
    private const string NativeLibraryNameX86 = "charls-2-x86.dll";
    private const string NativeLibraryNameX64 = "charls-2-x64.dll";
    private const string NativeLibraryNameArm64 = "charls-2-arm64.dll";

    internal delegate void CharLSGetVersionNumberFn(out int major, out int minor, out int patch);
    internal delegate JpegLSError CharLSValidateSpiffHeaderFn(ref SpiffHeaderNative spiffHeader, ref FrameInfoNative frameInfo);
    internal delegate IntPtr CharLSGetErrorMessageFn(int errorValue);
    internal delegate JpegLSError CharLSSetFrameInfoFn(SafeHandleJpegLSEncoder encoder, ref FrameInfoNative frameInfo);
    internal delegate JpegLSError CharLSSetNearLosslessFn(SafeHandleJpegLSEncoder encoder, int nearLossless);
    internal delegate JpegLSError CharLSSetInterleaveModeFn(SafeHandleJpegLSEncoder encoder, JpegLSInterleaveMode interleaveMode);
    internal delegate JpegLSError CharLSSetEncodingOptionsFn(SafeHandleJpegLSEncoder encoder, EncodingOptions encodingOptions);
    internal delegate JpegLSError CharLSSetPresetCodingParametersFn(SafeHandleJpegLSEncoder encoder, ref JpegLSPresetCodingParametersNative presetCodingParameters);
    internal unsafe delegate JpegLSError CharLSSetDestinationBufferFn(SafeHandleJpegLSEncoder encoder, byte* destination, nuint destinationLength);
    internal delegate JpegLSError CharLSGetEstimatedDestinationSizeFn(SafeHandleJpegLSEncoder encoder, out nuint sizeInBytes);
    internal delegate JpegLSError CharLSEncodeFromBufferFn(SafeHandleJpegLSEncoder encoder, ref byte source, nuint sourceSizeBytes, uint stride);
    internal delegate JpegLSError CharLSWriteStandardSpiffHeaderFn(SafeHandleJpegLSEncoder encoder, SpiffColorSpace colorSpace, SpiffResolutionUnit resolutionUnit, uint verticalResolution, uint horizontalResolution);
    internal delegate JpegLSError CharLSWriteSpiffHeaderFn(SafeHandleJpegLSEncoder encoder, ref SpiffHeaderNative spiffHeader);
    internal delegate JpegLSError CharLSWriteCommentFn(SafeHandleJpegLSEncoder encoder, ref byte comment, nuint commentLength);
    internal delegate JpegLSError CharLSWriteApplicationDataFn(SafeHandleJpegLSEncoder encoder, int applicationDataId, ref byte applicationData, nuint applicationDataLength);
    internal delegate JpegLSError CharLSGetBytesWrittenFn(SafeHandleJpegLSEncoder encoder, out nuint bytesWritten);
    internal delegate JpegLSError CharLSRewindFn(SafeHandleJpegLSEncoder encoder);
    internal delegate void CharLSDestroyEncoderFn(IntPtr encoder);
    internal delegate SafeHandleJpegLSEncoder CharLSCreateEncoderFn();
    internal delegate SafeHandleJpegLSDecoder CharLSCreateDecoderFn();
    internal delegate void CharLSDestroyDecoderFn(IntPtr decoder);
    internal unsafe delegate JpegLSError CharLSSetSourceBufferFn(SafeHandleJpegLSDecoder decoder, byte* source, nuint sourceLength);
    internal delegate JpegLSError CharLSReadSpiffHeaderFn(SafeHandleJpegLSDecoder decoder, out SpiffHeaderNative spiffHeader, out int headerFound);
    internal delegate JpegLSError CharLSReadHeaderFn(SafeHandleJpegLSDecoder decoder);
    internal delegate JpegLSError CharLSGetFrameInfoFn(SafeHandleJpegLSDecoder decoder, out FrameInfoNative frameInfo);
    internal delegate JpegLSError CharLSGetNearLosslessFn(SafeHandleJpegLSDecoder decoder, int component, out int nearLossless);
    internal delegate JpegLSError CharLSGetInterleaveModeFn(SafeHandleJpegLSDecoder decoder, out JpegLSInterleaveMode interleaveMode);
    internal delegate JpegLSError CharLSGetPresetCodingParametersFn(SafeHandleJpegLSDecoder decoder, int reserved, out JpegLSPresetCodingParametersNative presetCodingParameters);
    internal delegate JpegLSError CharLSGetDestinationSizeFn(SafeHandleJpegLSDecoder decoder, uint stride, out nuint destinationSize);
    internal delegate JpegLSError CharLSDecodeToBufferFn(SafeHandleJpegLSDecoder decoder, ref byte destination, nuint destinationSize, uint stride);
    internal delegate JpegLSError CharLSAtCommentFn(SafeHandleJpegLSDecoder decoder, AtCommentHandler handler, IntPtr userContext);
    internal delegate JpegLSError CharLSAtApplicationDataFn(SafeHandleJpegLSDecoder decoder, AtApplicationDataHandler handler, IntPtr userContext);

    internal static CharLSGetVersionNumberFn CharLSGetVersionNumber = GetFunctionCharLSGetVersionNumber();
    internal static CharLSValidateSpiffHeaderFn CharLSValidateSpiffHeader = GetFunctionCharLSValidateSpiffHeader();
    internal static CharLSGetErrorMessageFn CharLSGetErrorMessage = GetFunctionCharLSGetErrorMessage();
    internal static CharLSSetFrameInfoFn CharLSSetFrameInfo = GetFunctionCharLSSetFrameInfo();
    internal static CharLSSetNearLosslessFn CharLSSetNearLossless = GetFunctionCharLSSetNearLossless();
    internal static CharLSSetInterleaveModeFn CharLSSetInterleaveMode = GetFunctionCharLSSetInterleaveMode();
    internal static CharLSSetEncodingOptionsFn CharLSSetEncodingOptions = GetFunctionCharLSSetEncodingOptions();
    internal static CharLSSetPresetCodingParametersFn CharLSSetPresetCodingParameters = GetFunctionCharLSSetPresetCodingParameters();
    internal static CharLSSetDestinationBufferFn CharLSSetDestinationBuffer = GetFunctionCharLSSetDestinationBuffer();
    internal static CharLSGetEstimatedDestinationSizeFn CharLSGetEstimatedDestinationSize = GetFunctionCharLSGetEstimatedDestinationSize();
    internal static CharLSEncodeFromBufferFn CharLSEncodeFromBuffer = GetFunctionCharLSEncodeFromBuffer();
    internal static CharLSWriteStandardSpiffHeaderFn CharLSWriteStandardSpiffHeader = GetFunctionCharLSWriteStandardSpiffHeader();
    internal static CharLSWriteSpiffHeaderFn CharLSWriteSpiffHeader = GetFunctionCharLSWriteSpiffHeader();
    internal static CharLSWriteCommentFn CharLSWriteComment = GetFunctionCharLSWriteComment();
    internal static CharLSWriteApplicationDataFn CharLSWriteApplicationData = GetFunctionCharLSWriteApplicationData();
    internal static CharLSGetBytesWrittenFn CharLSGetBytesWritten = GetFunctionCharLSGetBytesWritten();
    internal static CharLSRewindFn CharLSRewind = GetFunctionCharLSRewind();
    internal static CharLSDestroyEncoderFn CharLSDestroyEncoder = GetFunctionCharLSDestroyEncoder();
    internal static CharLSCreateEncoderFn CharLSCreateEncoder = GetFunctionCharLSCreateEncoder();
    internal static CharLSCreateDecoderFn CharLSCreateDecoder = GetFunctionCharLSCreateDecoder();
    internal static CharLSDestroyDecoderFn CharLSDestroyDecoder = GetFunctionCharLSDestroyDecoder();
    internal static CharLSSetSourceBufferFn CharLSSetSourceBuffer = GetFunctionCharLSSetSourceBuffer();
    internal static CharLSReadSpiffHeaderFn CharLSReadSpiffHeader = GetFunctionCharLSReadSpiffHeader();
    internal static CharLSReadHeaderFn CharLSReadHeader = GetFunctionCharLSReadHeader();
    internal static CharLSGetFrameInfoFn CharLSGetFrameInfo = GetFunctionCharLSGetFrameInfo();
    internal static CharLSGetNearLosslessFn CharLSGetNearLossless = GetFunctionCharLSGetNearLossless();
    internal static CharLSGetInterleaveModeFn CharLSGetInterleaveMode = GetFunctionCharLSGetInterleaveMode();
    internal static CharLSGetPresetCodingParametersFn CharLSGetPresetCodingParameters = GetFunctionCharLSGetPresetCodingParameters();
    internal static CharLSGetDestinationSizeFn CharLSGetDestinationSize = GetFunctionCharLSGetDestinationSize();
    internal static CharLSDecodeToBufferFn CharLSDecodeToBuffer = GetFunctionCharLSDecodeToBuffer();
    internal static CharLSAtCommentFn CharLSAtComment = GetFunctionCharLSAtComment();
    internal static CharLSAtApplicationDataFn CharLSAtApplicationData = GetFunctionCharLSAtApplicationData();
#endif

    internal delegate int AtCommentHandler(nint dataPtr, nuint dataSize, nint userContextPtr);

    internal delegate int AtApplicationDataHandler(int applicationDataId, nint applicationDataPtr,
        nuint applicationDataSize, nint userContextPtr);

    [SuppressMessage("Design", "CA1065:Do not raise exceptions in unexpected locations",
        Justification = "Type is unusable if native DLL doesn't match")]
    static Interop()
    {
#if NET8_0_OR_GREATER
        NativeLibrary.SetDllImportResolver(Assembly.GetExecutingAssembly(), DllImportResolver);
#endif
        CharLSGetVersionNumber(out int major, out int minor, out int _);
        if (major != 2 || minor < 4)
        {
            throw new DllNotFoundException(
                $"Native DLL version mismatch: expected minimal v2.4, found v{major}.{minor}");
        }
    }

#if NET8_0_OR_GREATER
    [LibraryImport(NativeLibraryName, SetLastError = false, EntryPoint = "charls_get_version_number")]
    internal static partial void CharLSGetVersionNumber(out int major, out int minor, out int patch);

    [LibraryImport(NativeLibraryName, SetLastError = false, EntryPoint = "charls_validate_spiff_header")]
    internal static partial JpegLSError CharLSValidateSpiffHeader(ref SpiffHeaderNative spiffHeader, ref FrameInfoNative frameInfo);

    [LibraryImport(NativeLibraryName, SetLastError = false, StringMarshalling = StringMarshalling.Utf8, EntryPoint = "charls_get_error_message")]
    internal static partial nint CharLSGetErrorMessage(int errorValue);

    [LibraryImport(NativeLibraryName, SetLastError = false, EntryPoint = "charls_jpegls_encoder_set_frame_info")]
    internal static partial JpegLSError CharLSSetFrameInfo(SafeHandleJpegLSEncoder encoder, ref FrameInfoNative frameInfo);

    [LibraryImport(NativeLibraryName, SetLastError = false, EntryPoint = "charls_jpegls_encoder_set_near_lossless")]
    internal static partial JpegLSError CharLSSetNearLossless(SafeHandleJpegLSEncoder encoder, int nearLossless);

    [LibraryImport(NativeLibraryName, SetLastError = false, EntryPoint = "charls_jpegls_encoder_set_interleave_mode")]
    internal static partial JpegLSError CharLSSetInterleaveMode(SafeHandleJpegLSEncoder encoder, JpegLSInterleaveMode interleaveMode);

    [LibraryImport(NativeLibraryName, SetLastError = false, EntryPoint = "charls_jpegls_encoder_set_encoding_options")]
    internal static partial JpegLSError CharLSSetEncodingOptions(SafeHandleJpegLSEncoder encoder, EncodingOptions encodingOptions);

    [LibraryImport(NativeLibraryName, SetLastError = false, EntryPoint = "charls_jpegls_encoder_set_preset_coding_parameters")]
    internal static partial JpegLSError CharLSSetPresetCodingParameters(SafeHandleJpegLSEncoder encoder, ref JpegLSPresetCodingParametersNative presetCodingParameters);

    [LibraryImport(NativeLibraryName, SetLastError = false, EntryPoint = "charls_jpegls_encoder_create")]
    internal static partial SafeHandleJpegLSEncoder CharLSCreateEncoder();

    [LibraryImport(NativeLibraryName, SetLastError = false, EntryPoint = "charls_jpegls_encoder_destroy")]
    internal static partial void CharLSDestroyEncoder(nint encoder);

    [LibraryImport(NativeLibraryName, SetLastError = false, EntryPoint = "charls_jpegls_encoder_set_destination_buffer")]
    internal static unsafe partial JpegLSError CharLSSetDestinationBuffer(SafeHandleJpegLSEncoder encoder, byte* destination, nuint destinationLength);

    [LibraryImport(NativeLibraryName, SetLastError = false, EntryPoint = "charls_jpegls_encoder_get_estimated_destination_size")]
    internal static partial JpegLSError CharLSGetEstimatedDestinationSize(SafeHandleJpegLSEncoder encoder, out nuint sizeInBytes);

    [LibraryImport(NativeLibraryName, SetLastError = false, EntryPoint = "charls_jpegls_encoder_encode_from_buffer")]
    internal static partial JpegLSError CharLSEncodeFromBuffer(SafeHandleJpegLSEncoder encoder, ref byte source, nuint sourceSizeBytes, uint stride);

    [LibraryImport(NativeLibraryName, SetLastError = false, EntryPoint = "charls_jpegls_encoder_write_standard_spiff_header")]
    internal static partial JpegLSError CharLSWriteStandardSpiffHeader(SafeHandleJpegLSEncoder encoder,
        SpiffColorSpace colorSpace, SpiffResolutionUnit resolutionUnit, uint verticalResolution, uint horizontalResolution);

    [LibraryImport(NativeLibraryName, SetLastError = false, EntryPoint = "charls_jpegls_encoder_write_spiff_header")]
    internal static partial JpegLSError CharLSWriteSpiffHeader(SafeHandleJpegLSEncoder encoder, ref SpiffHeaderNative spiffHeader);

    [LibraryImport(NativeLibraryName, SetLastError = false, EntryPoint = "charls_jpegls_encoder_write_comment")]
    internal static partial JpegLSError CharLSWriteComment(SafeHandleJpegLSEncoder encoder, ref byte comment, nuint commentLength);

    [LibraryImport(NativeLibraryName, SetLastError = false, EntryPoint = "charls_jpegls_encoder_write_application_data")]
    internal static partial JpegLSError CharLSWriteApplicationData(SafeHandleJpegLSEncoder encoder, int applicationDataId,
        ref byte applicationData, nuint applicationDataLength);

    [LibraryImport(NativeLibraryName, SetLastError = false, EntryPoint = "charls_jpegls_encoder_get_bytes_written")]
    internal static partial JpegLSError CharLSGetBytesWritten(SafeHandleJpegLSEncoder encoder, out nuint bytesWritten);

    [LibraryImport(NativeLibraryName, SetLastError = false, EntryPoint = "charls_jpegls_encoder_rewind")]
    internal static partial JpegLSError CharLSRewind(SafeHandleJpegLSEncoder encoder);

    [LibraryImport(NativeLibraryName, SetLastError = false, EntryPoint = "charls_jpegls_decoder_create")]
    internal static partial SafeHandleJpegLSDecoder CharLSCreateDecoder();

    [LibraryImport(NativeLibraryName, SetLastError = false, EntryPoint = "charls_jpegls_decoder_destroy")]
    internal static partial void CharLSDestroyDecoder(nint decoder);

    [LibraryImport(NativeLibraryName, SetLastError = false, EntryPoint = "charls_jpegls_decoder_set_source_buffer")]
    internal static unsafe partial JpegLSError CharLSSetSourceBuffer(SafeHandleJpegLSDecoder decoder, byte* source, nuint sourceLength);

    [LibraryImport(NativeLibraryName, SetLastError = false, EntryPoint = "charls_jpegls_decoder_read_spiff_header")]
    internal static partial JpegLSError CharLSReadSpiffHeader(SafeHandleJpegLSDecoder decoder,
        out SpiffHeaderNative spiffHeader, out int headerFound);

    [LibraryImport(NativeLibraryName, SetLastError = false, EntryPoint = "charls_jpegls_decoder_read_header")]
    internal static partial JpegLSError CharLSReadHeader(SafeHandleJpegLSDecoder decoder);

    [LibraryImport(NativeLibraryName, SetLastError = false, EntryPoint = "charls_jpegls_decoder_get_frame_info")]
    internal static partial JpegLSError CharLSGetFrameInfo(SafeHandleJpegLSDecoder decoder, out FrameInfoNative frameInfo);

    [LibraryImport(NativeLibraryName, SetLastError = false, EntryPoint = "charls_jpegls_decoder_get_near_lossless")]
    internal static partial JpegLSError CharLSGetNearLossless(SafeHandleJpegLSDecoder decoder, int component, out int nearLossless);

    [LibraryImport(NativeLibraryName, SetLastError = false, EntryPoint = "charls_jpegls_decoder_get_interleave_mode")]
    internal static partial JpegLSError CharLSGetInterleaveMode(SafeHandleJpegLSDecoder decoder, out JpegLSInterleaveMode interleaveMode);

    [LibraryImport(NativeLibraryName, SetLastError = false, EntryPoint = "charls_jpegls_decoder_get_preset_coding_parameters")]
    internal static partial JpegLSError CharLSGetPresetCodingParameters(SafeHandleJpegLSDecoder decoder, int reserved, out JpegLSPresetCodingParametersNative presetCodingParameters);

    [LibraryImport(NativeLibraryName, SetLastError = false, EntryPoint = "charls_jpegls_decoder_get_destination_size")]
    internal static partial JpegLSError CharLSGetDestinationSize(SafeHandleJpegLSDecoder decoder, uint stride, out nuint destinationSize);

    [LibraryImport(NativeLibraryName, SetLastError = false, EntryPoint = "charls_jpegls_decoder_decode_to_buffer")]
    internal static partial JpegLSError CharLSDecodeToBuffer(SafeHandleJpegLSDecoder decoder, ref byte destination, nuint destinationSize, uint stride);

    [LibraryImport(NativeLibraryName, SetLastError = false, EntryPoint = "charls_jpegls_decoder_at_comment")]
    internal static partial JpegLSError CharLSAtComment(SafeHandleJpegLSDecoder decoder, AtCommentHandler handler, nint userContext);

    [LibraryImport(NativeLibraryName, SetLastError = false, EntryPoint = "charls_jpegls_decoder_at_application_data")]
    internal static partial JpegLSError CharLSAtApplicationData(SafeHandleJpegLSDecoder decoder, AtApplicationDataHandler handler, nint userContext);
#else
#pragma warning disable CA5393 // Do not use unsafe DllImportSearchPath value (needed for .NET 4.8)
    [DllImport(NativeLibraryNameX86, SetLastError = false, EntryPoint = "charls_get_version_number")]
    private static extern void CharLSGetVersionNumberX86(out int major, out int minor, out int patch);

    [DllImport(NativeLibraryNameX64, SetLastError = false, EntryPoint = "charls_get_version_number")]
    private static extern void CharLSGetVersionNumberX64(out int major, out int minor, out int patch);

    [DllImport(NativeLibraryNameArm64, SetLastError = false, EntryPoint = "charls_get_version_number")]
    private static extern void CharLSGetVersionNumberArm64(out int major, out int minor, out int patch);

    [DllImport(NativeLibraryNameX86, SetLastError = false, EntryPoint = "charls_validate_spiff_header")]
    internal static extern JpegLSError CharLSValidateSpiffHeaderX86([In] ref SpiffHeaderNative spiffHeader,
        [In] ref FrameInfoNative frameInfo);

    [DllImport(NativeLibraryNameX64, SetLastError = false, EntryPoint = "charls_validate_spiff_header")]
    internal static extern JpegLSError CharLSValidateSpiffHeaderX64([In] ref SpiffHeaderNative spiffHeader,
        [In] ref FrameInfoNative frameInfo);

    [DllImport(NativeLibraryNameArm64, SetLastError = false, EntryPoint = "charls_validate_spiff_header")]
    internal static extern JpegLSError CharLSValidateSpiffHeaderArm64([In] ref SpiffHeaderNative spiffHeader,
        [In] ref FrameInfoNative frameInfo);

    [DllImport(NativeLibraryNameX86, SetLastError = false, CharSet = CharSet.Ansi, BestFitMapping = false,
        ThrowOnUnmappableChar = true, EntryPoint = "charls_get_error_message")]
    internal static extern IntPtr CharLSGetErrorMessageX86(int errorValue);

    [DllImport(NativeLibraryNameX64, SetLastError = false, CharSet = CharSet.Ansi, BestFitMapping = false,
        ThrowOnUnmappableChar = true, EntryPoint = "charls_get_error_message")]
    internal static extern IntPtr CharLSGetErrorMessageX64(int errorValue);

    [DllImport(NativeLibraryNameArm64, SetLastError = false, CharSet = CharSet.Ansi, BestFitMapping = false,
        ThrowOnUnmappableChar = true, EntryPoint = "charls_get_error_message")]
    internal static extern IntPtr CharLSGetErrorMessageArm64(int errorValue);

    [DllImport(NativeLibraryNameX86, SetLastError = false, EntryPoint = "charls_jpegls_encoder_set_frame_info")]
    internal static extern JpegLSError CharLSSetFrameInfoX86(SafeHandleJpegLSEncoder encoder,
        [In] ref FrameInfoNative frameInfo);

    [DllImport(NativeLibraryNameX64, SetLastError = false, EntryPoint = "charls_jpegls_encoder_set_frame_info")]
    internal static extern JpegLSError CharLSSetFrameInfoX64(SafeHandleJpegLSEncoder encoder,
        [In] ref FrameInfoNative frameInfo);

    [DllImport(NativeLibraryNameArm64, SetLastError = false, EntryPoint = "charls_jpegls_encoder_set_frame_info")]
    internal static extern JpegLSError CharLSSetFrameInfoArm64(SafeHandleJpegLSEncoder encoder,
        [In] ref FrameInfoNative frameInfo);

    [DllImport(NativeLibraryNameX86, SetLastError = false, EntryPoint = "charls_jpegls_encoder_set_near_lossless")]
    internal static extern JpegLSError CharLSSetNearLosslessX86(SafeHandleJpegLSEncoder encoder, [In] int nearLossless);

    [DllImport(NativeLibraryNameX64, SetLastError = false, EntryPoint = "charls_jpegls_encoder_set_near_lossless")]
    internal static extern JpegLSError CharLSSetNearLosslessX64(SafeHandleJpegLSEncoder encoder, [In] int nearLossless);

    [DllImport(NativeLibraryNameArm64, SetLastError = false, EntryPoint = "charls_jpegls_encoder_set_near_lossless")]
    internal static extern JpegLSError CharLSSetNearLosslessArm64(SafeHandleJpegLSEncoder encoder, [In] int nearLossless);

    [DllImport(NativeLibraryNameX86, SetLastError = false, EntryPoint = "charls_jpegls_encoder_set_interleave_mode")]
    internal static extern JpegLSError CharLSSetInterleaveModeX86(SafeHandleJpegLSEncoder encoder,
        [In] JpegLSInterleaveMode interleaveMode);

    [DllImport(NativeLibraryNameX64, SetLastError = false, EntryPoint = "charls_jpegls_encoder_set_interleave_mode")]
    internal static extern JpegLSError CharLSSetInterleaveModeX64(SafeHandleJpegLSEncoder encoder,
        [In] JpegLSInterleaveMode interleaveMode);

    [DllImport(NativeLibraryNameArm64, SetLastError = false, EntryPoint = "charls_jpegls_encoder_set_interleave_mode")]
    internal static extern JpegLSError CharLSSetInterleaveModeArm64(SafeHandleJpegLSEncoder encoder,
        [In] JpegLSInterleaveMode interleaveMode);

    [DllImport(NativeLibraryNameX86, SetLastError = false, EntryPoint = "charls_jpegls_encoder_set_encoding_options")]
    internal static extern JpegLSError CharLSSetEncodingOptionsX86(SafeHandleJpegLSEncoder encoder,
        [In] EncodingOptions encodingOptions);

    [DllImport(NativeLibraryNameX64, SetLastError = false, EntryPoint = "charls_jpegls_encoder_set_encoding_options")]
    internal static extern JpegLSError CharLSSetEncodingOptionsX64(SafeHandleJpegLSEncoder encoder,
        [In] EncodingOptions encodingOptions);

    [DllImport(NativeLibraryNameArm64, SetLastError = false, EntryPoint = "charls_jpegls_encoder_set_encoding_options")]
    internal static extern JpegLSError CharLSSetEncodingOptionsArm64(SafeHandleJpegLSEncoder encoder,
        [In] EncodingOptions encodingOptions);

    [DllImport(NativeLibraryNameX86, SetLastError = false,
        EntryPoint = "charls_jpegls_encoder_set_preset_coding_parameters")]
    internal static extern JpegLSError CharLSSetPresetCodingParametersX86(SafeHandleJpegLSEncoder encoder,
        [In] ref JpegLSPresetCodingParametersNative presetCodingParameters);

    [DllImport(NativeLibraryNameX64, SetLastError = false,
        EntryPoint = "charls_jpegls_encoder_set_preset_coding_parameters")]
    internal static extern JpegLSError CharLSSetPresetCodingParametersX64(SafeHandleJpegLSEncoder encoder,
        [In] ref JpegLSPresetCodingParametersNative presetCodingParameters);

    [DllImport(NativeLibraryNameArm64, SetLastError = false,
        EntryPoint = "charls_jpegls_encoder_set_preset_coding_parameters")]
    internal static extern JpegLSError CharLSSetPresetCodingParametersArm64(SafeHandleJpegLSEncoder encoder,
        [In] ref JpegLSPresetCodingParametersNative presetCodingParameters);

    [DllImport(NativeLibraryNameX86, SetLastError = false, EntryPoint = "charls_jpegls_encoder_create")]
    internal static extern SafeHandleJpegLSEncoder CharLSCreateEncoderX86();

    [DllImport(NativeLibraryNameX64, SetLastError = false, EntryPoint = "charls_jpegls_encoder_create")]
    internal static extern SafeHandleJpegLSEncoder CharLSCreateEncoderX64();

    [DllImport(NativeLibraryNameArm64, SetLastError = false, EntryPoint = "charls_jpegls_encoder_create")]
    internal static extern SafeHandleJpegLSEncoder CharLSCreateEncoderArm64();

    [DllImport(NativeLibraryNameX86, SetLastError = false, EntryPoint = "charls_jpegls_encoder_destroy")]
    internal static extern void CharLSDestroyEncoderX86(IntPtr encoder);

    [DllImport(NativeLibraryNameX64, SetLastError = false, EntryPoint = "charls_jpegls_encoder_destroy")]
    internal static extern void CharLSDestroyEncoderX64(IntPtr encoder);

    [DllImport(NativeLibraryNameArm64, SetLastError = false, EntryPoint = "charls_jpegls_encoder_destroy")]
    internal static extern void CharLSDestroyEncoderArm64(IntPtr encoder);

    [DllImport(NativeLibraryNameX86, SetLastError = false, EntryPoint = "charls_jpegls_encoder_set_destination_buffer")]
    internal static extern unsafe JpegLSError CharLSSetDestinationBufferX86(SafeHandleJpegLSEncoder encoder,
        byte* destination, nuint destinationLength);

    [DllImport(NativeLibraryNameX64, SetLastError = false, EntryPoint = "charls_jpegls_encoder_set_destination_buffer")]
    internal static extern unsafe JpegLSError CharLSSetDestinationBufferX64(SafeHandleJpegLSEncoder encoder,
        byte* destination, nuint destinationLength);

    [DllImport(NativeLibraryNameArm64, SetLastError = false, EntryPoint = "charls_jpegls_encoder_set_destination_buffer")]
    internal static extern unsafe JpegLSError CharLSSetDestinationBufferArm64(SafeHandleJpegLSEncoder encoder,
        byte* destination, nuint destinationLength);

    [DllImport(NativeLibraryNameX86, SetLastError = false,
        EntryPoint = "charls_jpegls_encoder_get_estimated_destination_size")]
    internal static extern JpegLSError CharLSGetEstimatedDestinationSizeX86(SafeHandleJpegLSEncoder encoder,
        [Out] out nuint sizeInBytes);

    [DllImport(NativeLibraryNameX64, SetLastError = false,
        EntryPoint = "charls_jpegls_encoder_get_estimated_destination_size")]
    internal static extern JpegLSError CharLSGetEstimatedDestinationSizeX64(SafeHandleJpegLSEncoder encoder,
        [Out] out nuint sizeInBytes);

    [DllImport(NativeLibraryNameArm64, SetLastError = false,
        EntryPoint = "charls_jpegls_encoder_get_estimated_destination_size")]
    internal static extern JpegLSError CharLSGetEstimatedDestinationSizeArm64(SafeHandleJpegLSEncoder encoder,
        [Out] out nuint sizeInBytes);

    [DllImport(NativeLibraryNameX86, SetLastError = false, EntryPoint = "charls_jpegls_encoder_encode_from_buffer")]
    internal static extern JpegLSError CharLSEncodeFromBufferX86(SafeHandleJpegLSEncoder encoder, ref byte source,
        nuint sourceSizeBytes, uint stride);

    [DllImport(NativeLibraryNameX64, SetLastError = false, EntryPoint = "charls_jpegls_encoder_encode_from_buffer")]
    internal static extern JpegLSError CharLSEncodeFromBufferX64(SafeHandleJpegLSEncoder encoder, ref byte source,
        nuint sourceSizeBytes, uint stride);

    [DllImport(NativeLibraryNameArm64, SetLastError = false, EntryPoint = "charls_jpegls_encoder_encode_from_buffer")]
    internal static extern JpegLSError CharLSEncodeFromBufferArm64(SafeHandleJpegLSEncoder encoder, ref byte source,
        nuint sourceSizeBytes, uint stride);

    [DllImport(NativeLibraryNameX86, SetLastError = false,
        EntryPoint = "charls_jpegls_encoder_write_standard_spiff_header")]
    internal static extern JpegLSError CharLSWriteStandardSpiffHeaderX86(SafeHandleJpegLSEncoder encoder,
        SpiffColorSpace colorSpace, SpiffResolutionUnit resolutionUnit, uint verticalResolution,
        uint horizontalResolution);

    [DllImport(NativeLibraryNameX64, SetLastError = false,
        EntryPoint = "charls_jpegls_encoder_write_standard_spiff_header")]
    internal static extern JpegLSError CharLSWriteStandardSpiffHeaderX64(SafeHandleJpegLSEncoder encoder,
        SpiffColorSpace colorSpace, SpiffResolutionUnit resolutionUnit, uint verticalResolution,
        uint horizontalResolution);

    [DllImport(NativeLibraryNameArm64, SetLastError = false,
        EntryPoint = "charls_jpegls_encoder_write_standard_spiff_header")]
    internal static extern JpegLSError CharLSWriteStandardSpiffHeaderArm64(SafeHandleJpegLSEncoder encoder,
        SpiffColorSpace colorSpace, SpiffResolutionUnit resolutionUnit, uint verticalResolution,
        uint horizontalResolution);

    [DllImport(NativeLibraryNameX86, SetLastError = false, EntryPoint = "charls_jpegls_encoder_write_spiff_header")]
    internal static extern JpegLSError CharLSWriteSpiffHeaderX86(SafeHandleJpegLSEncoder encoder,
        [In] ref SpiffHeaderNative spiffHeader);

    [DllImport(NativeLibraryNameX64, SetLastError = false, EntryPoint = "charls_jpegls_encoder_write_spiff_header")]
    internal static extern JpegLSError CharLSWriteSpiffHeaderX64(SafeHandleJpegLSEncoder encoder,
        [In] ref SpiffHeaderNative spiffHeader);

    [DllImport(NativeLibraryNameArm64, SetLastError = false, EntryPoint = "charls_jpegls_encoder_write_spiff_header")]
    internal static extern JpegLSError CharLSWriteSpiffHeaderArm64(SafeHandleJpegLSEncoder encoder,
        [In] ref SpiffHeaderNative spiffHeader);

    [DllImport(NativeLibraryNameX86, SetLastError = false, EntryPoint = "charls_jpegls_encoder_write_comment")]
    internal static extern JpegLSError CharLSWriteCommentX86(SafeHandleJpegLSEncoder encoder, ref byte comment,
        nuint commentLength);

    [DllImport(NativeLibraryNameX64, SetLastError = false, EntryPoint = "charls_jpegls_encoder_write_comment")]
    internal static extern JpegLSError CharLSWriteCommentX64(SafeHandleJpegLSEncoder encoder, ref byte comment,
        nuint commentLength);

    [DllImport(NativeLibraryNameArm64, SetLastError = false, EntryPoint = "charls_jpegls_encoder_write_comment")]
    internal static extern JpegLSError CharLSWriteCommentArm64(SafeHandleJpegLSEncoder encoder, ref byte comment,
        nuint commentLength);

    [DllImport(NativeLibraryNameX86, SetLastError = false, EntryPoint = "charls_jpegls_encoder_write_application_data")]
    internal static extern JpegLSError CharLSWriteApplicationDataX86(SafeHandleJpegLSEncoder encoder,
        int applicationDataId,
        ref byte applicationData, nuint applicationDataLength);

    [DllImport(NativeLibraryNameX64, SetLastError = false, EntryPoint = "charls_jpegls_encoder_write_application_data")]
    internal static extern JpegLSError CharLSWriteApplicationDataX64(SafeHandleJpegLSEncoder encoder,
        int applicationDataId,
        ref byte applicationData, nuint applicationDataLength);

    [DllImport(NativeLibraryNameArm64, SetLastError = false, EntryPoint = "charls_jpegls_encoder_write_application_data")]
    internal static extern JpegLSError CharLSWriteApplicationDataArm64(SafeHandleJpegLSEncoder encoder,
        int applicationDataId,
        ref byte applicationData, nuint applicationDataLength);

    [DllImport(NativeLibraryNameX86, SetLastError = false, EntryPoint = "charls_jpegls_encoder_get_bytes_written")]
    internal static extern JpegLSError CharLSGetBytesWrittenX86(SafeHandleJpegLSEncoder encoder,
        [Out] out nuint bytesWritten);

    [DllImport(NativeLibraryNameX64, SetLastError = false, EntryPoint = "charls_jpegls_encoder_get_bytes_written")]
    internal static extern JpegLSError CharLSGetBytesWrittenX64(SafeHandleJpegLSEncoder encoder,
        [Out] out nuint bytesWritten);

    [DllImport(NativeLibraryNameArm64, SetLastError = false, EntryPoint = "charls_jpegls_encoder_get_bytes_written")]
    internal static extern JpegLSError CharLSGetBytesWrittenArm64(SafeHandleJpegLSEncoder encoder,
        [Out] out nuint bytesWritten);

    [DllImport(NativeLibraryNameX86, SetLastError = false, EntryPoint = "charls_jpegls_encoder_rewind")]
    internal static extern JpegLSError CharLSRewindX86(SafeHandleJpegLSEncoder encoder);

    [DllImport(NativeLibraryNameX64, SetLastError = false, EntryPoint = "charls_jpegls_encoder_rewind")]
    internal static extern JpegLSError CharLSRewindX64(SafeHandleJpegLSEncoder encoder);

    [DllImport(NativeLibraryNameArm64, SetLastError = false, EntryPoint = "charls_jpegls_encoder_rewind")]
    internal static extern JpegLSError CharLSRewindArm64(SafeHandleJpegLSEncoder encoder);

    [DllImport(NativeLibraryNameX86, SetLastError = false, EntryPoint = "charls_jpegls_decoder_create")]
    internal static extern SafeHandleJpegLSDecoder CharLSCreateDecoderX86();

    [DllImport(NativeLibraryNameX64, SetLastError = false, EntryPoint = "charls_jpegls_decoder_create")]
    internal static extern SafeHandleJpegLSDecoder CharLSCreateDecoderX64();

    [DllImport(NativeLibraryNameArm64, SetLastError = false, EntryPoint = "charls_jpegls_decoder_create")]
    internal static extern SafeHandleJpegLSDecoder CharLSCreateDecoderArm64();

    [DllImport(NativeLibraryNameX86, SetLastError = false, EntryPoint = "charls_jpegls_decoder_destroy")]
    internal static extern void CharLSDestroyDecoderX86(IntPtr decoder);

    [DllImport(NativeLibraryNameX64, SetLastError = false, EntryPoint = "charls_jpegls_decoder_destroy")]
    internal static extern void CharLSDestroyDecoderX64(IntPtr decoder);

    [DllImport(NativeLibraryNameArm64, SetLastError = false, EntryPoint = "charls_jpegls_decoder_destroy")]
    internal static extern void CharLSDestroyDecoderArm64(IntPtr decoder);

    [DllImport(NativeLibraryNameX86, SetLastError = false, EntryPoint = "charls_jpegls_decoder_set_source_buffer")]
    internal static extern unsafe JpegLSError CharLSSetSourceBufferX86(SafeHandleJpegLSDecoder decoder, byte* source,
        nuint sourceLength);

    [DllImport(NativeLibraryNameX64, SetLastError = false, EntryPoint = "charls_jpegls_decoder_set_source_buffer")]
    internal static extern unsafe JpegLSError CharLSSetSourceBufferX64(SafeHandleJpegLSDecoder decoder, byte* source,
        nuint sourceLength);

    [DllImport(NativeLibraryNameArm64, SetLastError = false, EntryPoint = "charls_jpegls_decoder_set_source_buffer")]
    internal static extern unsafe JpegLSError CharLSSetSourceBufferArm64(SafeHandleJpegLSDecoder decoder, byte* source,
        nuint sourceLength);

    [DllImport(NativeLibraryNameX86, SetLastError = false, EntryPoint = "charls_jpegls_decoder_read_spiff_header")]
    internal static extern JpegLSError CharLSReadSpiffHeaderX86(SafeHandleJpegLSDecoder decoder,
        [Out] out SpiffHeaderNative spiffHeader, [Out] out int headerFound);

    [DllImport(NativeLibraryNameX64, SetLastError = false, EntryPoint = "charls_jpegls_decoder_read_spiff_header")]
    internal static extern JpegLSError CharLSReadSpiffHeaderX64(SafeHandleJpegLSDecoder decoder,
        [Out] out SpiffHeaderNative spiffHeader, [Out] out int headerFound);

    [DllImport(NativeLibraryNameArm64, SetLastError = false, EntryPoint = "charls_jpegls_decoder_read_spiff_header")]
    internal static extern JpegLSError CharLSReadSpiffHeaderArm64(SafeHandleJpegLSDecoder decoder,
        [Out] out SpiffHeaderNative spiffHeader, [Out] out int headerFound);

    [DllImport(NativeLibraryNameX86, SetLastError = false, EntryPoint = "charls_jpegls_decoder_read_header")]
    internal static extern JpegLSError CharLSReadHeaderX86(SafeHandleJpegLSDecoder decoder);

    [DllImport(NativeLibraryNameX64, SetLastError = false, EntryPoint = "charls_jpegls_decoder_read_header")]
    internal static extern JpegLSError CharLSReadHeaderX64(SafeHandleJpegLSDecoder decoder);

    [DllImport(NativeLibraryNameArm64, SetLastError = false, EntryPoint = "charls_jpegls_decoder_read_header")]
    internal static extern JpegLSError CharLSReadHeaderArm64(SafeHandleJpegLSDecoder decoder);

    [DllImport(NativeLibraryNameX86, SetLastError = false, EntryPoint = "charls_jpegls_decoder_get_frame_info")]
    internal static extern JpegLSError CharLSGetFrameInfoX86(SafeHandleJpegLSDecoder decoder,
        [Out] out FrameInfoNative frameInfo);

    [DllImport(NativeLibraryNameX64, SetLastError = false, EntryPoint = "charls_jpegls_decoder_get_frame_info")]
    internal static extern JpegLSError CharLSGetFrameInfoX64(SafeHandleJpegLSDecoder decoder,
        [Out] out FrameInfoNative frameInfo);

    [DllImport(NativeLibraryNameArm64, SetLastError = false, EntryPoint = "charls_jpegls_decoder_get_frame_info")]
    internal static extern JpegLSError CharLSGetFrameInfoArm64(SafeHandleJpegLSDecoder decoder,
        [Out] out FrameInfoNative frameInfo);

    [DllImport(NativeLibraryNameX86, SetLastError = false, EntryPoint = "charls_jpegls_decoder_get_near_lossless")]
    internal static extern JpegLSError CharLSGetNearLosslessX86(SafeHandleJpegLSDecoder decoder, int component,
        [Out] out int nearLossless);

    [DllImport(NativeLibraryNameX64, SetLastError = false, EntryPoint = "charls_jpegls_decoder_get_near_lossless")]
    internal static extern JpegLSError CharLSGetNearLosslessX64(SafeHandleJpegLSDecoder decoder, int component,
        [Out] out int nearLossless);

    [DllImport(NativeLibraryNameArm64, SetLastError = false, EntryPoint = "charls_jpegls_decoder_get_near_lossless")]
    internal static extern JpegLSError CharLSGetNearLosslessArm64(SafeHandleJpegLSDecoder decoder, int component,
        [Out] out int nearLossless);

    [DllImport(NativeLibraryNameX86, SetLastError = false, EntryPoint = "charls_jpegls_decoder_get_interleave_mode")]
    internal static extern JpegLSError CharLSGetInterleaveModeX86(SafeHandleJpegLSDecoder decoder,
        [Out] out JpegLSInterleaveMode interleaveMode);

    [DllImport(NativeLibraryNameX64, SetLastError = false, EntryPoint = "charls_jpegls_decoder_get_interleave_mode")]
    internal static extern JpegLSError CharLSGetInterleaveModeX64(SafeHandleJpegLSDecoder decoder,
        [Out] out JpegLSInterleaveMode interleaveMode);

    [DllImport(NativeLibraryNameArm64, SetLastError = false, EntryPoint = "charls_jpegls_decoder_get_interleave_mode")]
    internal static extern JpegLSError CharLSGetInterleaveModeArm64(SafeHandleJpegLSDecoder decoder,
        [Out] out JpegLSInterleaveMode interleaveMode);

    [DllImport(NativeLibraryNameX86, SetLastError = false,
        EntryPoint = "charls_jpegls_decoder_get_preset_coding_parameters")]
    internal static extern JpegLSError CharLSGetPresetCodingParametersX86(SafeHandleJpegLSDecoder decoder, int reserved,
        [Out] out JpegLSPresetCodingParametersNative presetCodingParameters);

    [DllImport(NativeLibraryNameX64, SetLastError = false,
        EntryPoint = "charls_jpegls_decoder_get_preset_coding_parameters")]
    internal static extern JpegLSError CharLSGetPresetCodingParametersX64(SafeHandleJpegLSDecoder decoder, int reserved,
        [Out] out JpegLSPresetCodingParametersNative presetCodingParameters);

    [DllImport(NativeLibraryNameArm64, SetLastError = false,
        EntryPoint = "charls_jpegls_decoder_get_preset_coding_parameters")]
    internal static extern JpegLSError CharLSGetPresetCodingParametersArm64(SafeHandleJpegLSDecoder decoder, int reserved,
        [Out] out JpegLSPresetCodingParametersNative presetCodingParameters);

    [DllImport(NativeLibraryNameX86, SetLastError = false, EntryPoint = "charls_jpegls_decoder_get_destination_size")]
    internal static extern JpegLSError CharLSGetDestinationSizeX86(SafeHandleJpegLSDecoder decoder, uint stride,
        [Out] out nuint destinationSize);

    [DllImport(NativeLibraryNameX64, SetLastError = false, EntryPoint = "charls_jpegls_decoder_get_destination_size")]
    internal static extern JpegLSError CharLSGetDestinationSizeX64(SafeHandleJpegLSDecoder decoder, uint stride,
        [Out] out nuint destinationSize);

    [DllImport(NativeLibraryNameArm64, SetLastError = false, EntryPoint = "charls_jpegls_decoder_get_destination_size")]
    internal static extern JpegLSError CharLSGetDestinationSizeArm64(SafeHandleJpegLSDecoder decoder, uint stride,
        [Out] out nuint destinationSize);

    [DllImport(NativeLibraryNameX86, SetLastError = false, EntryPoint = "charls_jpegls_decoder_decode_to_buffer")]
    internal static extern JpegLSError CharLSDecodeToBufferX86(SafeHandleJpegLSDecoder decoder, ref byte destination,
        nuint destinationSize, uint stride);

    [DllImport(NativeLibraryNameX64, SetLastError = false, EntryPoint = "charls_jpegls_decoder_decode_to_buffer")]
    internal static extern JpegLSError CharLSDecodeToBufferX64(SafeHandleJpegLSDecoder decoder, ref byte destination,
        nuint destinationSize, uint stride);

    [DllImport(NativeLibraryNameArm64, SetLastError = false, EntryPoint = "charls_jpegls_decoder_decode_to_buffer")]
    internal static extern JpegLSError CharLSDecodeToBufferArm64(SafeHandleJpegLSDecoder decoder, ref byte destination,
        nuint destinationSize, uint stride);

    [DllImport(NativeLibraryNameX86, SetLastError = false, EntryPoint = "charls_jpegls_decoder_at_comment")]
    internal static extern JpegLSError CharLSAtCommentX86(SafeHandleJpegLSDecoder decoder, AtCommentHandler handler,
        IntPtr userContext);

    [DllImport(NativeLibraryNameX64, SetLastError = false, EntryPoint = "charls_jpegls_decoder_at_comment")]
    internal static extern JpegLSError CharLSAtCommentX64(SafeHandleJpegLSDecoder decoder, AtCommentHandler handler,
        IntPtr userContext);

    [DllImport(NativeLibraryNameArm64, SetLastError = false, EntryPoint = "charls_jpegls_decoder_at_comment")]
    internal static extern JpegLSError CharLSAtCommentArm64(SafeHandleJpegLSDecoder decoder, AtCommentHandler handler,
        IntPtr userContext);

    [DllImport(NativeLibraryNameX86, SetLastError = false, EntryPoint = "charls_jpegls_decoder_at_application_data")]
    internal static extern JpegLSError CharLSAtApplicationDataX86(SafeHandleJpegLSDecoder decoder,
        AtApplicationDataHandler handler, IntPtr userContext);

    [DllImport(NativeLibraryNameX64, SetLastError = false, EntryPoint = "charls_jpegls_decoder_at_application_data")]
    internal static extern JpegLSError CharLSAtApplicationDataX64(SafeHandleJpegLSDecoder decoder,
        AtApplicationDataHandler handler, IntPtr userContext);

    [DllImport(NativeLibraryNameArm64, SetLastError = false, EntryPoint = "charls_jpegls_decoder_at_application_data")]
    internal static extern JpegLSError CharLSAtApplicationDataArm64(SafeHandleJpegLSDecoder decoder,
        AtApplicationDataHandler handler, IntPtr userContext);
#pragma warning restore CA5393 // Do not use unsafe DllImportSearchPath value


    private static CharLSGetVersionNumberFn GetFunctionCharLSGetVersionNumber()
    {
        return RuntimeInformation.ProcessArchitecture switch
        {
            Architecture.X86 => CharLSGetVersionNumberX86,
            Architecture.X64 => CharLSGetVersionNumberX64,
            Architecture.Arm64 => CharLSGetVersionNumberArm64,
            Architecture.Arm => throw new NotSupportedException(CreateNotSupportedErrorMessage()),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    internal static CharLSValidateSpiffHeaderFn GetFunctionCharLSValidateSpiffHeader()
    {
        return RuntimeInformation.ProcessArchitecture switch
        {
            Architecture.X86 => CharLSValidateSpiffHeaderX86,
            Architecture.X64 => CharLSValidateSpiffHeaderX64,
            Architecture.Arm64 => CharLSValidateSpiffHeaderArm64,
            Architecture.Arm => throw new NotSupportedException(CreateNotSupportedErrorMessage()),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private static CharLSGetErrorMessageFn GetFunctionCharLSGetErrorMessage()
    {
        return RuntimeInformation.ProcessArchitecture switch
        {
            Architecture.X86 => CharLSGetErrorMessageX86,
            Architecture.X64 => CharLSGetErrorMessageX64,
            Architecture.Arm64 => CharLSGetErrorMessageArm64,
            Architecture.Arm => throw new NotSupportedException(CreateNotSupportedErrorMessage()),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private static CharLSSetFrameInfoFn GetFunctionCharLSSetFrameInfo()
    {
        return RuntimeInformation.ProcessArchitecture switch
        {
            Architecture.X86 => CharLSSetFrameInfoX86,
            Architecture.X64 => CharLSSetFrameInfoX64,
            Architecture.Arm64 => CharLSSetFrameInfoArm64,
            Architecture.Arm => throw new NotSupportedException(CreateNotSupportedErrorMessage()),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private static CharLSSetNearLosslessFn GetFunctionCharLSSetNearLossless()
    {
        return RuntimeInformation.ProcessArchitecture switch
        {
            Architecture.X86 => CharLSSetNearLosslessX86,
            Architecture.X64 => CharLSSetNearLosslessX64,
            Architecture.Arm64 => CharLSSetNearLosslessArm64,
            Architecture.Arm => throw new NotSupportedException(CreateNotSupportedErrorMessage()),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private static CharLSSetInterleaveModeFn GetFunctionCharLSSetInterleaveMode()
    {
        return RuntimeInformation.ProcessArchitecture switch
        {
            Architecture.X86 => CharLSSetInterleaveModeX86,
            Architecture.X64 => CharLSSetInterleaveModeX64,
            Architecture.Arm64 => CharLSSetInterleaveModeArm64,
            Architecture.Arm => throw new NotSupportedException(CreateNotSupportedErrorMessage()),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private static CharLSSetEncodingOptionsFn GetFunctionCharLSSetEncodingOptions()
    {
        return RuntimeInformation.ProcessArchitecture switch
        {
            Architecture.X86 => CharLSSetEncodingOptionsX86,
            Architecture.X64 => CharLSSetEncodingOptionsX64,
            Architecture.Arm64 => CharLSSetEncodingOptionsArm64,
            Architecture.Arm => throw new NotSupportedException(CreateNotSupportedErrorMessage()),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private static CharLSSetPresetCodingParametersFn GetFunctionCharLSSetPresetCodingParameters()
    {
        return RuntimeInformation.ProcessArchitecture switch
        {
            Architecture.X86 => CharLSSetPresetCodingParametersX86,
            Architecture.X64 => CharLSSetPresetCodingParametersX64,
            Architecture.Arm64 => CharLSSetPresetCodingParametersArm64,
            Architecture.Arm => throw new NotSupportedException(CreateNotSupportedErrorMessage()),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private static unsafe CharLSSetDestinationBufferFn GetFunctionCharLSSetDestinationBuffer()
    {
        return RuntimeInformation.ProcessArchitecture switch
        {
            Architecture.X86 => CharLSSetDestinationBufferX86,
            Architecture.X64 => CharLSSetDestinationBufferX64,
            Architecture.Arm64 => CharLSSetDestinationBufferArm64,
            Architecture.Arm => throw new NotSupportedException(CreateNotSupportedErrorMessage()),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private static CharLSGetEstimatedDestinationSizeFn GetFunctionCharLSGetEstimatedDestinationSize()
    {
        return RuntimeInformation.ProcessArchitecture switch
        {
            Architecture.X86 => CharLSGetEstimatedDestinationSizeX86,
            Architecture.X64 => CharLSGetEstimatedDestinationSizeX64,
            Architecture.Arm64 => CharLSGetEstimatedDestinationSizeArm64,
            Architecture.Arm => throw new NotSupportedException(CreateNotSupportedErrorMessage()),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private static CharLSEncodeFromBufferFn GetFunctionCharLSEncodeFromBuffer()
    {
        return RuntimeInformation.ProcessArchitecture switch
        {
            Architecture.X86 => CharLSEncodeFromBufferX86,
            Architecture.X64 => CharLSEncodeFromBufferX64,
            Architecture.Arm64 => CharLSEncodeFromBufferArm64,
            Architecture.Arm => throw new NotSupportedException(CreateNotSupportedErrorMessage()),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private static CharLSWriteStandardSpiffHeaderFn GetFunctionCharLSWriteStandardSpiffHeader()
    {
        return RuntimeInformation.ProcessArchitecture switch
        {
            Architecture.X86 => CharLSWriteStandardSpiffHeaderX86,
            Architecture.X64 => CharLSWriteStandardSpiffHeaderX64,
            Architecture.Arm64 => CharLSWriteStandardSpiffHeaderArm64,
            Architecture.Arm => throw new NotSupportedException(CreateNotSupportedErrorMessage()),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private static CharLSWriteSpiffHeaderFn GetFunctionCharLSWriteSpiffHeader()
    {
        return RuntimeInformation.ProcessArchitecture switch
        {
            Architecture.X86 => CharLSWriteSpiffHeaderX86,
            Architecture.X64 => CharLSWriteSpiffHeaderX64,
            Architecture.Arm64 => CharLSWriteSpiffHeaderArm64,
            Architecture.Arm => throw new NotSupportedException(CreateNotSupportedErrorMessage()),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private static CharLSWriteCommentFn GetFunctionCharLSWriteComment()
    {
        return RuntimeInformation.ProcessArchitecture switch
        {
            Architecture.X86 => CharLSWriteCommentX86,
            Architecture.X64 => CharLSWriteCommentX64,
            Architecture.Arm64 => CharLSWriteCommentArm64,
            Architecture.Arm => throw new NotSupportedException(CreateNotSupportedErrorMessage()),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private static CharLSWriteApplicationDataFn GetFunctionCharLSWriteApplicationData()
    {
        return RuntimeInformation.ProcessArchitecture switch
        {
            Architecture.X86 => CharLSWriteApplicationDataX86,
            Architecture.X64 => CharLSWriteApplicationDataX64,
            Architecture.Arm64 => CharLSWriteApplicationDataArm64,
            Architecture.Arm => throw new NotSupportedException(CreateNotSupportedErrorMessage()),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private static CharLSGetBytesWrittenFn GetFunctionCharLSGetBytesWritten()
    {
        return RuntimeInformation.ProcessArchitecture switch
        {
            Architecture.X86 => CharLSGetBytesWrittenX86,
            Architecture.X64 => CharLSGetBytesWrittenX64,
            Architecture.Arm64 => CharLSGetBytesWrittenArm64,
            Architecture.Arm => throw new NotSupportedException(CreateNotSupportedErrorMessage()),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private static CharLSRewindFn GetFunctionCharLSRewind()
    {
        return RuntimeInformation.ProcessArchitecture switch
        {
            Architecture.X86 => CharLSRewindX86,
            Architecture.X64 => CharLSRewindX64,
            Architecture.Arm64 => CharLSRewindArm64,
            Architecture.Arm => throw new NotSupportedException(CreateNotSupportedErrorMessage()),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private static CharLSDestroyEncoderFn GetFunctionCharLSDestroyEncoder()
    {
        return RuntimeInformation.ProcessArchitecture switch
        {
            Architecture.X86 => CharLSDestroyEncoderX86,
            Architecture.X64 => CharLSDestroyEncoderX64,
            Architecture.Arm64 => CharLSDestroyEncoderArm64,
            Architecture.Arm => throw new NotSupportedException(CreateNotSupportedErrorMessage()),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private static CharLSCreateEncoderFn GetFunctionCharLSCreateEncoder()
    {
        return RuntimeInformation.ProcessArchitecture switch
        {
            Architecture.X86 => CharLSCreateEncoderX86,
            Architecture.X64 => CharLSCreateEncoderX64,
            Architecture.Arm64 => CharLSCreateEncoderArm64,
            Architecture.Arm => throw new NotSupportedException(CreateNotSupportedErrorMessage()),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private static CharLSCreateDecoderFn GetFunctionCharLSCreateDecoder()
    {
        return RuntimeInformation.ProcessArchitecture switch
        {
            Architecture.X86 => CharLSCreateDecoderX86,
            Architecture.X64 => CharLSCreateDecoderX64,
            Architecture.Arm64 => CharLSCreateDecoderArm64,
            Architecture.Arm => throw new NotSupportedException(CreateNotSupportedErrorMessage()),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private static CharLSDestroyDecoderFn GetFunctionCharLSDestroyDecoder()
    {
        return RuntimeInformation.ProcessArchitecture switch
        {
            Architecture.X86 => CharLSDestroyDecoderX86,
            Architecture.X64 => CharLSDestroyDecoderX64,
            Architecture.Arm64 => CharLSDestroyDecoderArm64,
            Architecture.Arm => throw new NotSupportedException(CreateNotSupportedErrorMessage()),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private static unsafe CharLSSetSourceBufferFn GetFunctionCharLSSetSourceBuffer()
    {
        return RuntimeInformation.ProcessArchitecture switch
        {
            Architecture.X86 => CharLSSetSourceBufferX86,
            Architecture.X64 => CharLSSetSourceBufferX64,
            Architecture.Arm64 => CharLSSetSourceBufferArm64,
            Architecture.Arm => throw new NotSupportedException(CreateNotSupportedErrorMessage()),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private static CharLSReadSpiffHeaderFn GetFunctionCharLSReadSpiffHeader()
    {
        return RuntimeInformation.ProcessArchitecture switch
        {
            Architecture.X86 => CharLSReadSpiffHeaderX86,
            Architecture.X64 => CharLSReadSpiffHeaderX64,
            Architecture.Arm64 => CharLSReadSpiffHeaderArm64,
            Architecture.Arm => throw new NotSupportedException(CreateNotSupportedErrorMessage()),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private static CharLSReadHeaderFn GetFunctionCharLSReadHeader()
    {
        return RuntimeInformation.ProcessArchitecture switch
        {
            Architecture.X86 => CharLSReadHeaderX86,
            Architecture.X64 => CharLSReadHeaderX64,
            Architecture.Arm64 => CharLSReadHeaderArm64,
            Architecture.Arm => throw new NotSupportedException(CreateNotSupportedErrorMessage()),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private static CharLSGetFrameInfoFn GetFunctionCharLSGetFrameInfo()
    {
        return RuntimeInformation.ProcessArchitecture switch
        {
            Architecture.X86 => CharLSGetFrameInfoX86,
            Architecture.X64 => CharLSGetFrameInfoX64,
            Architecture.Arm64 => CharLSGetFrameInfoArm64,
            Architecture.Arm => throw new NotSupportedException(CreateNotSupportedErrorMessage()),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private static CharLSGetNearLosslessFn GetFunctionCharLSGetNearLossless()
    {
        return RuntimeInformation.ProcessArchitecture switch
        {
            Architecture.X86 => CharLSGetNearLosslessX86,
            Architecture.X64 => CharLSGetNearLosslessX64,
            Architecture.Arm64 => CharLSGetNearLosslessArm64,
            Architecture.Arm => throw new NotSupportedException(CreateNotSupportedErrorMessage()),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private static CharLSGetInterleaveModeFn GetFunctionCharLSGetInterleaveMode()
    {
        return RuntimeInformation.ProcessArchitecture switch
        {
            Architecture.X86 => CharLSGetInterleaveModeX86,
            Architecture.X64 => CharLSGetInterleaveModeX64,
            Architecture.Arm64 => CharLSGetInterleaveModeArm64,
            Architecture.Arm => throw new NotSupportedException(CreateNotSupportedErrorMessage()),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private static CharLSGetPresetCodingParametersFn GetFunctionCharLSGetPresetCodingParameters()
    {
        return RuntimeInformation.ProcessArchitecture switch
        {
            Architecture.X86 => CharLSGetPresetCodingParametersX86,
            Architecture.X64 => CharLSGetPresetCodingParametersX64,
            Architecture.Arm64 => CharLSGetPresetCodingParametersArm64,
            Architecture.Arm => throw new NotSupportedException(CreateNotSupportedErrorMessage()),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private static CharLSGetDestinationSizeFn GetFunctionCharLSGetDestinationSize()
    {
        return RuntimeInformation.ProcessArchitecture switch
        {
            Architecture.X86 => CharLSGetDestinationSizeX86,
            Architecture.X64 => CharLSGetDestinationSizeX64,
            Architecture.Arm64 => CharLSGetDestinationSizeArm64,
            Architecture.Arm => throw new NotSupportedException(CreateNotSupportedErrorMessage()),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private static CharLSDecodeToBufferFn GetFunctionCharLSDecodeToBuffer()
    {
        return RuntimeInformation.ProcessArchitecture switch
        {
            Architecture.X86 => CharLSDecodeToBufferX86,
            Architecture.X64 => CharLSDecodeToBufferX64,
            Architecture.Arm64 => CharLSDecodeToBufferArm64,
            Architecture.Arm => throw new NotSupportedException(CreateNotSupportedErrorMessage()),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private static CharLSAtCommentFn GetFunctionCharLSAtComment()
    {
        return RuntimeInformation.ProcessArchitecture switch
        {
            Architecture.X86 => CharLSAtCommentX86,
            Architecture.X64 => CharLSAtCommentX64,
            Architecture.Arm64 => CharLSAtCommentArm64,
            Architecture.Arm => throw new NotSupportedException(CreateNotSupportedErrorMessage()),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private static CharLSAtApplicationDataFn GetFunctionCharLSAtApplicationData()
    {
        return RuntimeInformation.ProcessArchitecture switch
        {
            Architecture.X86 => CharLSAtApplicationDataX86,
            Architecture.X64 => CharLSAtApplicationDataX64,
            Architecture.Arm64 => CharLSAtApplicationDataArm64,
            Architecture.Arm => throw new NotSupportedException(CreateNotSupportedErrorMessage()),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private static string CreateNotSupportedErrorMessage()
    {
        return "No native JPEG-LS codec is available for " + RuntimeInformation.ProcessArchitecture;
    }
#endif

    internal static void HandleJpegLSError(JpegLSError error)
    {
        Exception exception;

        switch (error)
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
            case JpegLSError.UnexpectedMarkerFound:
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
            case JpegLSError.InvalidParameterNearLossless:
            case JpegLSError.InvalidParameterJpeglsPresetCodingParameters:
            case JpegLSError.UnexpectedFailure:
            case JpegLSError.NotEnoughMemory:
            case JpegLSError.UnexpectedRestartMarker:
            case JpegLSError.RestartMarkerNotFound:
            case JpegLSError.CallbackFailed:
            case JpegLSError.EndOfImageMarkerNotFound:
            case JpegLSError.InvalidSpiffHeader:
                exception = new InvalidDataException(GetErrorMessage(error));
                break;

            case JpegLSError.InvalidArgument:
            case JpegLSError.DestinationBufferTooSmall:
                exception = new ArgumentException(GetErrorMessage(error));
                break;

            case JpegLSError.InvalidArgumentWidth:
            case JpegLSError.InvalidArgumentHeight:
            case JpegLSError.InvalidArgumentComponentCount:
            case JpegLSError.InvalidArgumentBitsPerSample:
            case JpegLSError.InvalidArgumentNearLossless:
            case JpegLSError.InvalidArgumentPresetCodingParameters:
            case JpegLSError.InvalidArgumentSpiffEntrySize:
            case JpegLSError.InvalidArgumentStride:
                exception = new ArgumentOutOfRangeException(GetErrorMessage(error));
                break;

            case JpegLSError.InvalidArgumentColorTransformation:
            case JpegLSError.InvalidArgumentInterleaveMode:
            case JpegLSError.InvalidArgumentEncodingOptions:
                exception = new InvalidEnumArgumentException(GetErrorMessage(error));
                break;

            case JpegLSError.InvalidOperation:
                exception = new InvalidOperationException(GetErrorMessage(error));
                break;

            default:
                Debug.Assert(false, "C# and native implementation mismatch");

                // ReSharper disable once HeuristicUnreachableCode
                exception = new InvalidOperationException(GetErrorMessage(error));
                break;
        }

        exception.SetJpegLSError(error);
        throw exception;
    }

    private static string GetErrorMessage(JpegLSError error)
    {
        nint message = CharLSGetErrorMessage((int)error);

        return Marshal.PtrToStringAnsi(message) ?? string.Empty;
    }

#if NET8_0_OR_GREATER
    private static nint DllImportResolver(string libraryName, Assembly assembly, DllImportSearchPath? searchPath)
    {
        return libraryName != NativeLibraryName
            ? IntPtr.Zero
            : NativeLibrary.Load(GetLibraryName(), assembly, DllImportSearchPath.AssemblyDirectory);
    }

    private static string GetLibraryName()
    {
        return OperatingSystem.IsWindows() ? GetLibraryNameForWindows() : GetLibraryNameForLinuxVariant();
    }

    private static string GetLibraryNameForWindows()
    {
        return RuntimeInformation.ProcessArchitecture == Architecture.Arm64
            ? "charls-2-arm64"
            : GetLibraryNameForWindowsOnIntel();
    }

    private static string GetLibraryNameForWindowsOnIntel()
    {
        return Environment.Is64BitProcess ? "charls-2-x64" : "charls-2-x86";
    }

    private static string GetLibraryNameForLinuxVariant()
    {
        return OperatingSystem.IsLinux()
            ? "charls.so.2"
            : OperatingSystem.IsMacOS()
            ? "charls.2"
            : throw new NotSupportedException("No native JPEG-LS codec is available for " + Environment.OSVersion);
    }
#endif
}
