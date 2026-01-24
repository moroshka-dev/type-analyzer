# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.0.1] - 2026-01-24

### Changed

- Updated dependency `com.moroshka.protect` to 1.1.0

### Fixed

- Fixed Samples configuration (TypeAnalyzerSample)
- Fixed `Moroshka.TypeAnalyzer.csproj` configuration

## [1.0.0] - 2026-01-06

### Added

- `ITypeAnalyzer` interface for type analysis
- `TypeAnalyzer` class - main implementation of the type analyzer
- `TypeAnalysisResult` class - analysis result containing information about type members
- `TypeAnalysisOptions` enum - options for selective analysis of different type members (constructors, methods, properties, fields)
- Caching of analysis results for improved performance
- Support for analyzing type constructors
- Support for analyzing type methods
- Support for analyzing type properties
- Support for analyzing type fields
- `ClearCache()` method for clearing the analysis results cache
