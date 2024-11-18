// Copyright (c) Team CharLS.
// SPDX-License-Identifier: BSD-3-Clause

using CharLS.Native;

var source = new byte[] { 7 };
using var encoder = new JpegLSEncoder(1, 1, 8, 1);
encoder.Encode(source);

var destination = new byte[1];
using var decoder = new JpegLSDecoder(encoder.EncodedData);
decoder.Decode(destination);

Console.WriteLine(source[0] != destination[0]
    ? "Error: source and destination are not equal."
    : "Success: source and destination are equal.");
