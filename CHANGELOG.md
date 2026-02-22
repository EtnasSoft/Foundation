# Changelog

All notable changes to this package will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added
- `Float2` domain type: immutable 2D vector (`readonly struct`) as a Unity-decoupled replacement for `Vector2`
- `Float2Validation`: finite/NaN/Infinity checks for `Float2`
- `Float2Sanitizer`: sanitization with `ValidationPolicy` support for `Float2`
- `Float2UnityAdapter`: three-variant extension methods for `Float2 ↔ Vector2` conversion (simple, safe, customizable)
- Tests: `Float2SanitizerTests` and `Float2UnityAdapterTests`

### Changed
- (pending)

### Fixed
- (pending)

## [0.1.0] - 2026-01-25

### Added
- Core domain types: `Float3`, `Angle`, `ColorRgba`, `ColorRgba32`
- Validation system with `ValidationPolicy` and sanitizers
- Unity adapters with diagnostic support
- Comprehensive validation for NaN, Infinity, and range violations

### Documentation
- Complete README with usage patterns and examples
- XML documentation for all public APIs
