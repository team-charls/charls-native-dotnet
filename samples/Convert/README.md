# CharLS.Native .NET Convert Sample

This sample demonstrates how to convert common image formats that .NET can read by default (.bmp, .png, .jpg, etc.) to the JPEG-LS format.

## How to use

Usage: Convert "input-image-filename"

The sample will create at the same location an output file called "input-image-filename".jls

## Remarks

- This sample uses the NuGet package System.Drawing.Common. This NuGet package is only supported on Windows.
A good simple cross-platform alternative is unfortunately not available.
