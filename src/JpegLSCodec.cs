// Copyright (c) Team CharLS.
// SPDX-License-Identifier: BSD-3-Clause

using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace CharLS.Native
{
    internal static class JpegLSCodec
    {
        internal static void HandleResult(JpegLSError result)
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

            var data = exception.Data;

            // ReSharper disable once PossibleNullReferenceException
            data.Add(nameof(JpegLSError), result);
            throw exception;
        }

        private static string GetErrorMessage(JpegLSError result)
        {
            var message = Environment.Is64BitProcess ?
                SafeNativeMethods.CharLSGetErrorMessageX64((int)result) :
                SafeNativeMethods.CharLSGetErrorMessageX86((int)result);
            return Marshal.PtrToStringAnsi(message);
        }
    }
}
