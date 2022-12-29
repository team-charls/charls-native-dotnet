// Copyright (c) Team CharLS.
// SPDX-License-Identifier: BSD-3-Clause

using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.InteropServices;

namespace CharLS.Native;

#if  NET7_0
internal static partial class Interop
#else
internal static class Interop
#endif
{
    private const string NativeLibraryName = "charls-2";

    internal delegate int AtCommentHandler(nint dataPtr, nuint dataSize);

    internal delegate int AtApplicationDataHandler(int applicationDataId, nint applicationDataPtr, nuint applicationDataSize);

    [SuppressMessage("Design", "CA1065:Do not raise exceptions in unexpected locations", Justification = "Type is unusable if native DLL doesn't match")]
    static Interop()
    {
        NativeLibrary.SetDllImportResolver(Assembly.GetExecutingAssembly(), DllImportResolver);

        CharLSGetVersionNumber(out int major, out int minor, out int _);
        if (major != 2 || minor < 4)
        {
            throw new DllNotFoundException($"Native DLL version mismatch: expected minimal v2.4, found v{major}.{minor}");
        }
    }

#if  NET7_0
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
    internal static partial JpegLSError JpegLSDecoderReadHeader(SafeHandleJpegLSDecoder decoder);

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
    [DllImport(NativeLibraryName, SetLastError = false, EntryPoint = "charls_get_version_number")]
    internal static extern void CharLSGetVersionNumber(out int major, out int minor, out int patch);

    [DllImport(NativeLibraryName, SetLastError = false, EntryPoint = "charls_validate_spiff_header")]
    internal static extern JpegLSError CharLSValidateSpiffHeader([In] ref SpiffHeaderNative spiffHeader, [In] ref FrameInfoNative frameInfo);

    [DllImport(NativeLibraryName, SetLastError = false, CharSet = CharSet.Ansi, BestFitMapping = false,
        ThrowOnUnmappableChar = true, EntryPoint = "charls_get_error_message")]
    internal static extern IntPtr CharLSGetErrorMessage(int errorValue);

    [DllImport(NativeLibraryName, SetLastError = false, EntryPoint = "charls_jpegls_encoder_set_frame_info")]
    internal static extern JpegLSError CharLSSetFrameInfo(SafeHandleJpegLSEncoder encoder, [In] ref FrameInfoNative frameInfo);

    [DllImport(NativeLibraryName, SetLastError = false, EntryPoint = "charls_jpegls_encoder_set_near_lossless")]
    internal static extern JpegLSError CharLSSetNearLossless(SafeHandleJpegLSEncoder encoder, [In] int nearLossless);

    [DllImport(NativeLibraryName, SetLastError = false, EntryPoint = "charls_jpegls_encoder_set_interleave_mode")]
    internal static extern JpegLSError CharLSSetInterleaveMode(SafeHandleJpegLSEncoder encoder, [In] JpegLSInterleaveMode interleaveMode);

    [DllImport(NativeLibraryName, SetLastError = false, EntryPoint = "charls_jpegls_encoder_set_encoding_options")]
    internal static extern JpegLSError CharLSSetEncodingOptions(SafeHandleJpegLSEncoder encoder, [In] EncodingOptions encodingOptions);

    [DllImport(NativeLibraryName, SetLastError = false, EntryPoint = "charls_jpegls_encoder_set_preset_coding_parameters")]
    internal static extern JpegLSError CharLSSetPresetCodingParameters(SafeHandleJpegLSEncoder encoder, [In] ref JpegLSPresetCodingParametersNative presetCodingParameters);

    [DllImport(NativeLibraryName, SetLastError = false, EntryPoint = "charls_jpegls_encoder_create")]
    internal static extern SafeHandleJpegLSEncoder CharLSCreateEncoder();

    [DllImport(NativeLibraryName, SetLastError = false, EntryPoint = "charls_jpegls_encoder_destroy")]
    internal static extern void CharLSDestroyEncoder(IntPtr encoder);

    [DllImport(NativeLibraryName, SetLastError = false, EntryPoint = "charls_jpegls_encoder_set_destination_buffer")]
    internal static extern unsafe JpegLSError CharLSSetDestinationBuffer(SafeHandleJpegLSEncoder encoder, byte* destination, nuint destinationLength);

    [DllImport(NativeLibraryName, SetLastError = false, EntryPoint = "charls_jpegls_encoder_get_estimated_destination_size")]
    internal static extern JpegLSError CharLSGetEstimatedDestinationSize(SafeHandleJpegLSEncoder encoder, [Out] out nuint sizeInBytes);

    [DllImport(NativeLibraryName, SetLastError = false, EntryPoint = "charls_jpegls_encoder_encode_from_buffer")]
    internal static extern JpegLSError CharLSEncodeFromBuffer(SafeHandleJpegLSEncoder encoder, ref byte source, nuint sourceSizeBytes, uint stride);

    [DllImport(NativeLibraryName, SetLastError = false, EntryPoint = "charls_jpegls_encoder_write_standard_spiff_header")]
    internal static extern JpegLSError CharLSWriteStandardSpiffHeader(SafeHandleJpegLSEncoder encoder,
        SpiffColorSpace colorSpace, SpiffResolutionUnit resolutionUnit, uint verticalResolution, uint horizontalResolution);

    [DllImport(NativeLibraryName, SetLastError = false, EntryPoint = "charls_jpegls_encoder_write_spiff_header")]
    internal static extern JpegLSError CharLSWriteSpiffHeader(SafeHandleJpegLSEncoder encoder, [In] ref SpiffHeaderNative spiffHeader);

    [DllImport(NativeLibraryName, SetLastError = false, EntryPoint = "charls_jpegls_encoder_write_comment")]
    internal static extern JpegLSError CharLSWriteComment(SafeHandleJpegLSEncoder encoder, ref byte comment, nuint commentLength);

    [DllImport(NativeLibraryName, SetLastError = false, EntryPoint = "charls_jpegls_encoder_write_application_data")]
    internal static extern JpegLSError CharLSWriteApplicationData(SafeHandleJpegLSEncoder encoder, int applicationDataId,
        ref byte applicationData, nuint applicationDataLength);

    [DllImport(NativeLibraryName, SetLastError = false, EntryPoint = "charls_jpegls_encoder_get_bytes_written")]
    internal static extern JpegLSError CharLSGetBytesWritten(SafeHandleJpegLSEncoder encoder, [Out] out nuint bytesWritten);

    [DllImport(NativeLibraryName, SetLastError = false, EntryPoint = "charls_jpegls_encoder_rewind")]
    internal static extern JpegLSError CharLSRewind(SafeHandleJpegLSEncoder encoder);

    [DllImport(NativeLibraryName, SetLastError = false, EntryPoint = "charls_jpegls_decoder_create")]
    internal static extern SafeHandleJpegLSDecoder CharLSCreateDecoder();

    [DllImport(NativeLibraryName, SetLastError = false, EntryPoint = "charls_jpegls_decoder_destroy")]
    internal static extern void CharLSDestroyDecoder(IntPtr decoder);

    [DllImport(NativeLibraryName, SetLastError = false, EntryPoint = "charls_jpegls_decoder_set_source_buffer")]
    internal static extern unsafe JpegLSError CharLSSetSourceBuffer(SafeHandleJpegLSDecoder decoder, byte* source, nuint sourceLength);

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
    internal static extern JpegLSError CharLSGetDestinationSize(SafeHandleJpegLSDecoder decoder, uint stride, [Out] out nuint destinationSize);

    [DllImport(NativeLibraryName, SetLastError = false, EntryPoint = "charls_jpegls_decoder_decode_to_buffer")]
    internal static extern JpegLSError CharLSDecodeToBuffer(SafeHandleJpegLSDecoder decoder, ref byte destination, nuint destinationSize, uint stride);

    [DllImport(NativeLibraryName, SetLastError = false, EntryPoint = "charls_jpegls_decoder_at_comment")]
    internal static extern JpegLSError CharLSAtComment(SafeHandleJpegLSDecoder decoder, AtCommentHandler handler, IntPtr userContext);

    [DllImport(NativeLibraryName, SetLastError = false, EntryPoint = "charls_jpegls_decoder_at_application_data")]
    internal static extern JpegLSError CharLSAtApplicationData(SafeHandleJpegLSDecoder decoder, AtApplicationDataHandler handler, IntPtr userContext);
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

        exception.Data.Add(nameof(JpegLSError), error);
        throw exception;
    }

    private static string GetErrorMessage(JpegLSError error)
    {
        nint message = CharLSGetErrorMessage((int)error);

        return Marshal.PtrToStringAnsi(message) ?? string.Empty;
    }

    private static nint DllImportResolver(string libraryName, Assembly assembly, DllImportSearchPath? searchPath)
    {
        return libraryName != NativeLibraryName
            ? IntPtr.Zero
            : NativeLibrary.Load(GetLibraryName(), assembly, DllImportSearchPath.AssemblyDirectory);
    }

    private static string GetLibraryName()
    {
        return OperatingSystem.IsWindows()
            ? Environment.Is64BitProcess ?
                RuntimeInformation.ProcessArchitecture == Architecture.Arm64 ? "charls-2-arm64" : "charls-2-x64" : "charls-2-x86"
            : OperatingSystem.IsLinux()
                ? "charls.so.2"
                : OperatingSystem.IsMacOS()
                    ? "charls.2"
                    : throw new NotSupportedException("No native JPEG-LS codec is available for " + Environment.OSVersion);
    }
}
