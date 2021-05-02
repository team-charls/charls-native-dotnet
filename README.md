
<img src="doc/jpeg_ls_logo.png" alt="JPEG-LS Logo" width="100"/>

# CharLS.Native .NET

[![License](https://img.shields.io/badge/License-BSD%203--Clause-blue.svg)](https://raw.githubusercontent.com/team-charls/charls-dotnet/master/LICENSE.md)
[![Build Status](https://dev.azure.com/team-charls/charls-native-dotnet/_apis/build/status/team-charls.charls-native-dotnet?branchName=master)](https://dev.azure.com/team-charls/charls-native-dotnet/_build/latest?definitionId=4&branchName=master)
[![NuGet](https://img.shields.io/nuget/v/CharLS.Native.svg)](https://www.nuget.org/packages/CharLS.Native)

CharLS.Native .NET is an adapter assembly that provides access to the native CharLS JPEG-LS C++ implementation for .NET based applications.
JPEG-LS (ISO-14495-1) is a lossless/near-lossless compression standard for continuous-tone images.

## Features

* .NET 5.0 class library.
* Supports the Windows platform in the x86 and x64 architecture, as well as Linux and macOS.  
  Note: the Microsoft Visual C++ Redistributable for Visual Studio 2019 needs to be installed on the target system.
        for Linux and macOS, the charls dynamic library needs to be installed on the target system.

## Installation

CharLS.Native can be installed using the NuGet command line or the NuGet Package Manager in Visual Studio.

### Install using the command line

```bash
Install-Package CharLS.Native
```

### How to use the NuGet package

A sample application is included in this repository that demonstrates how to convert common image types like .bmp, .png and .jpg to .jls (JPEG-LS).

## How to build this repository

* Use git to get a copy of this repository: git clone --recurse-submodules
* Use Visual Studio 2019 16.8 or newer to build the solution file CharLSNativeDotNet.sln

### Building and signing

To support code signing with a code signing certificate stored on a smart card a Windows command file is available: create-nuget-package.cmd.
Instructions:

* Open a Visual Studio Developer Command Prompt
* Go the root of the cloned repository
* Ensure the code signing certificate is available
* Execute the command create-nuget-package.cmd certificate-thumb-print time-stamp-url
 The certificate thumbprint and time stamp URL arguments are depending on the used certificate.

 All DLLs and the NuGet package itself will be signed.

## About the JPEG-LS image compression standard

More information about JPEG-LS can be found in the [README](https://github.com/team-charls/charls/blob/master/README.md) from the C++ CharLS project.
