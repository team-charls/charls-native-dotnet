
# CharLS.Native .NET

CharLS.Native .NET is an adapter assembly that provides access to the native CharLS JPEG-LS C++ implementation for .NET based applications.  
JPEG-LS (ISO-14495-1) is a lossless/near-lossless compression standard for continuous-tone images.

## Features

* Support for .NET 9.0, .NET 8.0 and .NET Framework 4.8.
* Support for the .NET platforms: Windows, Linux and macOS.

## How to use

CharLS.Native can be added to your C# project using the dotnet command line or the NuGet Package Manager in Visual Studio.

### Install using the dotnet command line

```bash
dotnet add package CharLS.Native
```

### Windows specific installation steps

The NuGet package comes with prebuilt CharLS DLLs for x86, x64 and ARM64 targets.
The Microsoft Visual C++ Redistributable for Visual Studio 2015-2022 (v14.38 or newer) needs to be installed on the target system.

### Linux specific installation steps

A prebuild CharLS shared library can be installed using the Apt package manager:

```bash
sudo apt install libcharls2
```

### MacOS specific installation steps

A prebuild CharLS shared library can be installed using the Homebrew package manager:

```bash
brew install team-charls/tap/charls
```

### How to use the C# classes in the NuGet package

A sample application is included in the GitHub repository that demonstrates how to convert common image types like .bmp, .png and .jpg to .jls (JPEG-LS).

## About the JPEG-LS image compression standard

More information about JPEG-LS can be found in the [README](https://github.com/team-charls/charls/blob/master/README.md) from the C++ CharLS project.
This repository also contains instructions how the build the native C++ CharLS shared library from source.
