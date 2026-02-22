# Changelog

All notable changes to this package will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Changed
- (pending)

### Fixed
- (pending)

## [0.3.0] - 2026-02-22

### Added
- `EtnasSoft.Foundation.Numerics` assembly: new module for pure mathematical operations on domain types, with no Unity dependencies
- `Float2Extensions.MoveTowards`: advances a `Float2` towards a target by at most `maxDistanceDelta` units per call, without overshooting (mirrors `Vector2.MoveTowards` semantics)
- `Float3Extensions.MoveTowards`: equivalent operation for `Float3` (mirrors `Vector3.MoveTowards` semantics)
- Tests: `Float2MoveTowardsTests` and `Float3MoveTowardsTests` covering zero delta, negative delta, same position, large delta, exact delta, partial advance, large coordinates, and nearly-equal positions


## [0.2.3] - 2026-02-22
## [0.2.2] - 2026-02-22

### Changed
- Cleaning some files

## [0.2.1] - 2026-02-22

### Added
- `.editorconfig`

### Changed
- Remove AGENTS.md.meta from version control

## [0.2.0] - 2026-02-22

### Added
- `Float2` domain type: immutable 2D vector (`readonly struct`) as a Unity-decoupled replacement for `Vector2`
- `Float2Validation`: finite/NaN/Infinity checks for `Float2`
- `Float2Sanitizer`: sanitization with `ValidationPolicy` support for `Float2`
- `Float2UnityAdapter`: three-variant extension methods for `Float2 ↔ Vector2` conversion (simple, safe, customizable)
- Tests: `Float2SanitizerTests` and `Float2UnityAdapterTests`


## [0.1.0] - 2026-01-25

### Added
- Core domain types: `Float3`, `Angle`, `ColorRgba`, `ColorRgba32`
- Validation system with `ValidationPolicy` and sanitizers
- Unity adapters with diagnostic support
- Comprehensive validation for NaN, Infinity, and range violations

### Documentation
- Complete README with usage patterns and examples
- XML documentation for all public APIs
