# Change Log

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/) and this project adheres to [Semantic Versioning](http://semver.org/).

## [2.0.0 - 2022-6-5]

### Added

- Support for Charls v2.3.4. CharLS v2.3.0 is now the minimum required version.
- JpegLSEncoder constructor has an additional argument extraBytes to create a larger destination buffer.
- JpegLSEncoder.Rewind() method.

## Removed

- Support for .NET 5.0 (.NET 5.0 is end of support since 2022-5-10).

### Changed

- CharLS Windows DLLs updated to v2.3.4 + c6af80b.

## [1.2.0 - 2021-12-5]

### Added

- Support for .NET 6.0.

### Changed

- Updated source code to leverage C# 10 features.
- Added missing JpegLSError enumerations.
- CharLS Windows DLLs updated to v2.2.1 + bea1c0.

## [1.1.0 - 2021-5-8]

### Added

- Support for Linux and macOS.

### Changed

- CharLS Windows DLLs updated to v2.2.1+42c3a58.

## [1.0.0 - 2021-1-24]

### Added

- Initial release. Includes native CharLS library version 2.2.0.
