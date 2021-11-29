
<img src="doc/jpeg_ls_logo.png" alt="JPEG-LS Logo" width="100"/>

# CharLS.Native .NET

[![License](https://img.shields.io/badge/License-BSD%203--Clause-blue.svg)](https://raw.githubusercontent.com/team-charls/charls-dotnet/main/LICENSE.md)
[![Build Status](https://dev.azure.com/team-charls/charls-native-dotnet/_apis/build/status/team-charls.charls-native-dotnet?branchName=main)](https://dev.azure.com/team-charls/charls-native-dotnet/_build/latest?definitionId=4&branchName=main)
[![NuGet](https://img.shields.io/nuget/v/CharLS.Native.svg)](https://www.nuget.org/packages/CharLS.Native)

CharLS.Native .NET is an adapter assembly that provides access to the native CharLS JPEG-LS C++ implementation for .NET based applications.
JPEG-LS (ISO-14495-1) is a lossless/near-lossless compression standard for continuous-tone images.

## Features

* .NET 5.0  and .NET 6.0 class library.
* Support for the .NET platforms: Windows, Linux and macOS.

## How to use

CharLS.Native can be added to your C# project using the dotnet command line or the NuGet Package Manager in Visual Studio.

### Install using the dotnet command line

```bash
dotnet add package CharLS.Native
```

### Windows specific installation steps

The NuGet package comes with prebuild CharLS DLLs for the x86 and X64 targets.
The Microsoft Visual C++ 2015-2022 Redistributable (v14.30 or newer) needs to be installed on the target system.

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

## General steps to build this repository

* Use Git to get a clone of this repository:  

```bash
 git clone --recurse-submodules
```

* Use CMake to build the native C++ shared library, see the CharLS project how to do that. When building with Visual Studio, this step can be skipped.
* Use the .NET CLI or Visual Studio 2019 (v16.8 or newer) to build the solution file CharLSNativeDotNet.sln. For example: `dotnet build && dotnet test && dotnet publish` to build the nuget package.

### Building Windows DLLs and code signing all components

Building the NuGet package with signed Windows DLLs can only be done on the Window platform with Visual Studio 2019 or with Build tools for Visual Studio 2019.
To support code signing with a code signing certificate, stored on a smart card, a Windows command file is available: `create-signed-nuget-package.cmd`.
Instructions:

* Open a Visual Studio Developer Command Prompt
* Go the root of the cloned repository
* Ensure the code signing certificate is available
* Execute the command `create-signed-nuget-package.cmd certificate-thumb-print time-stamp-url`  
 The certificate thumbprint and time stamp URL arguments are depending on the used code signing certificate.

 All DLLs and the NuGet package itself will be signed.

## About the JPEG-LS image compression standard

More information about JPEG-LS can be found in the [README](https://github.com/team-charls/charls/blob/master/README.md) from the C++ CharLS project.
This repository also contains instructions how the build the native C++ CharLS shared library from source.
