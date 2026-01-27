# Unity Foundation Library

A domain-driven, Unity-decoupled foundation library providing primitive types with validation, sanitization, and safe Unity interoperability.

## Overview

This library implements a **Base Class Library (BCL)** for Unity projects following clean architecture principles. It provides domain-specific primitive types (`Angle`, `Float3`, `ColorRgba`, `ColorRgba32`) that are completely independent of Unity's engine types, along with adapters that enable safe, validated conversion between domain and Unity representations.

### Key Design Principles

1. **Unity Decoupling**: Core domain types have **zero Unity dependencies**. Unity types (`Vector3`, `Color`, `Quaternion`) exist only at the application boundaries.

2. **Explicit Boundaries**: Adapters mark the explicit boundary between domain logic and Unity infrastructure. No Unity types leak into your domain layer.

3. **Defensive Validation**: All conversions can validate and sanitize data using configurable policies, protecting against `NaN`, `Infinity`, and out-of-range values.

4. **Pay-for-What-You-Use**: Multiple adapter variants allow choosing between performance (unchecked) and safety (validated) based on trust level of data sources.

## Setup

### Nullable Reference Types

This library uses **C# nullable reference types** for improved null-safety. Unity does not enable this feature by default, so you must configure your project to support it.

Create a compiler response file at the root of your `Assets` folder:

**File**: `Assets/csc.rsp`
```
-nullable:enable
```

This enables nullable annotations throughout the module. Without this configuration, you'll encounter compiler errors when using the library.

> **Note**: If your project already has a `csc.rsp` file, append `-nullable:enable` to it rather than replacing the file.

## Architecture

```
┌─────────────────────────────────────────────────────────────┐
│  Unity Layer (MonoBehaviours, Components)            │
│  Uses: Vector3, Color, Quaternion                    │
└────────────────────┬────────────────────────────────────────┘
                     │
                     │ Adapters (Boundary)
                     ▼
┌─────────────────────────────────────────────────────────────┐
│  Domain Layer (Game Logic, Services, Systems)        │
│  Uses: Float3, ColorRgba, Angle                      │
│  Zero Unity Dependencies                             │
└─────────────────────────────────────────────────────────────┘
```

### Assembly Structure

- **`Boilerplate.Foundation`**: Core domain types (no Unity references)
- **`Boilerplate.Foundation.Validation`**: Validation and sanitization logic
- **`Boilerplate.Foundation.Unity`**: Unity adapters (references Unity + Foundation)

## Core Types

### `Float3`

Domain representation of a 3D vector. Use instead of `Vector3` in domain logic.

```csharp
public readonly struct Float3 {
    public readonly float X, Y, Z;
    public Float3(float x, float y, float z);
    public static Float3 Zero { get; }
}
```

### `Angle`

Type-safe angle representation storing radians internally.

```csharp
public readonly struct Angle {
    public readonly float Radians;
    public float Degrees { get; }
    
    public static Angle FromRadians(float radians);
    public static Angle FromDegrees(float degrees);
    public static Angle Zero { get; }
}
```

### `ColorRgba` / `ColorRgba32`

Domain color representations: floating-point (0-1 range) and byte-based (0-255).

```csharp
public readonly struct ColorRgba {
    public readonly float R, G, B, A;
    public ColorRgba(float r, float g, float b, float a = 1f);
}

public readonly struct ColorRgba32 {
    public readonly byte R, G, B, A;
    public ColorRgba32(byte r, byte g, byte b, byte a = 255);
}
```

## Validation Policies

The library provides three predefined validation policies:

```csharp
public readonly struct ValidationPolicy {
    public readonly InvalidNumberPolicy InvalidNumber;  // NaN, Infinity handling
    public readonly RangePolicy ColorUnitRange;         // Color [0..1] clamping
    
    public static ValidationPolicy None;    // Pass through everything
    public static ValidationPolicy Strict;  // Throw on invalid data
    public static ValidationPolicy Safe;    // Sanitize to safe defaults
}
```

### Policy Behaviors

| Policy | Invalid Numbers (NaN/∞) | Out-of-Range Colors |
|--------|-------------------------|---------------------|
| `None` | Pass through unchanged | Pass through unchanged |
| `Safe` | Replace with default (Zero/Black) | Clamp to [0..1] |
| `Strict` | Throw exception | Throw exception |

## Usage Patterns

### Pattern 1: Trusted Internal Data (Performance)

When converting data you **own and trust** (e.g., constants, validated game state), use simple adapters:

```csharp
// Domain → Unity (trusted data)
Float3 position = new Float3(10f, 5f, 0f);
transform.position = position.ToUnity();

// Unity → Domain (trusted)
Float3 domainPos = transform.position.ToDomain();
```

**When to use**: Internal game state, pre-validated data, performance-critical paths where you control data sources.

### Pattern 2: Untrusted External Data (Safe Defaults)

When handling **external or untrusted data** (user input, network, serialization), use `Safe` policy:

```csharp
var diag = UnityAdapterDiagnostics.UnityDebug;

// Network data → Domain → Unity (sanitize invalid values)
Float3 networkPosition = DeserializeFromNetwork();
transform.position = networkPosition.ToUnity(diag); // Uses ValidationPolicy.Safe

// Custom policy for specific needs
ColorRgba userColor = GetUserInputColor();
material.color = userColor.ToUnity(ValidationPolicy.Safe, diag);
```

**When to use**: Network data, file I/O, user input, modding APIs, any external data source.

### Pattern 3: Development/Debug (Strict Validation)

During **development**, use `Strict` policy to catch bugs early:

```csharp
#if UNITY_EDITOR
var policy = ValidationPolicy.Strict;  // Throws on invalid data
#else
var policy = ValidationPolicy.Safe;    // Sanitizes in production
#endif

var diag = UnityAdapterDiagnostics.UnityDebug;
transform.position = calculatedPosition.ToUnity(policy, diag);
```

**When to use**: Development builds, automated tests, debugging suspicious calculations.

### Pattern 4: Unchecked Performance-Critical (Advanced)

For **maximum performance** when you need logging but not sanitization:

```csharp
var diag = UnityAdapterDiagnostics.UnityDebug;

// Logs warnings for invalid data but doesn't sanitize
transform.position = position.ToUnityUnchecked(diag);
```

⚠️ **Warning**: Only use when profiling proves adapters are a bottleneck. Invalid data will propagate to Unity!

## Practical Examples

### Example 1: Physics System (Domain Logic)

```csharp
// Domain service - NO Unity types
public class ProjectileCalculator {
    public Float3 CalculateTrajectory(Float3 start, Float3 velocity, float time) {
        // Pure domain logic using Float3
        var gravity = new Float3(0f, -9.81f, 0f);
        var displacement = new Float3(
            velocity.X * time,
            velocity.Y * time + 0.5f * gravity.Y * time * time,
            velocity.Z * time
        );
        
        return new Float3(
            start.X + displacement.X,
            start.Y + displacement.Y,
            start.Z + displacement.Z
        );
    }
}

// Unity presentation layer - adapters at boundary
public class ProjectileView : MonoBehaviour {
    private ProjectileCalculator _calculator = new();
    
    void Update() {
        // Adapter at boundary: Unity → Domain
        Float3 domainStart = transform.position.ToDomain();
        Float3 domainVelocity = new Float3(10f, 15f, 0f);
        
        // Pure domain logic
        Float3 domainNext = _calculator.CalculateTrajectory(
            domainStart, domainVelocity, Time.deltaTime
        );
        
        // Adapter at boundary: Domain → Unity
        transform.position = domainNext.ToUnity();
    }
}
```

### Example 2: Network Synchronization (Untrusted Data)

```csharp
public class NetworkedEntity : MonoBehaviour {
    private readonly UnityAdapterDiagnostics _diag = 
        UnityAdapterDiagnostics.UnityDebug;
    
    public void OnNetworkUpdate(NetworkPacket packet) {
        // Network data is UNTRUSTED - could contain NaN, Infinity
        Float3 syncedPosition = packet.ReadFloat3();
        Angle syncedRotation = packet.ReadAngle();
        ColorRgba syncedColor = packet.ReadColor();
        
        // Safe policy: sanitizes invalid values, logs warnings
        transform.position = syncedPosition.ToUnity(_diag);
        transform.rotation = syncedRotation.ToUnityRotationZ(_diag);
        GetComponent<Renderer>().material.color = syncedColor.ToUnity(_diag);
        
        // If packet contained NaN positions, you'll see:
        // "Float3 sanitized (ReturnDefault): (NaN, 5, 10) -> (0, 0, 0)"
    }
}
```

### Example 3: Configuration File Loading

```csharp
public class ConfigLoader {
    private static readonly ValidationPolicy StrictDev = 
#if UNITY_EDITOR
        ValidationPolicy.Strict;  // Catch bad configs early
#else
        ValidationPolicy.Safe;    // Tolerate in production
#endif
    
    private static readonly UnityAdapterDiagnostics Diag = 
        UnityAdapterDiagnostics.UnityDebug;
    
    public LightConfig LoadLightConfig(JsonNode json) {
        // Parse from JSON (untrusted external format)
        var color = new ColorRgba(
            json["r"].GetValue<float>(),
            json["g"].GetValue<float>(),
            json["b"].GetValue<float>()
        );
        
        var angle = Angle.FromDegrees(json["angle"].GetValue<float>());
        
        // Validate during load - throws in editor, sanitizes in production
        return new LightConfig {
            UnityColor = color.ToUnity(StrictDev, Diag),
            UnityRotation = angle.ToUnityRotationZ(StrictDev, Diag)
        };
    }
}
```

### Example 4: Color Interpolation (Domain Logic)

```csharp
// Domain service - Unity-free
public static class ColorMath {
    public static ColorRgba Lerp(ColorRgba a, ColorRgba b, float t) {
        float Lerp(float x, float y, float t) => x + (y - x) * t;
        
        return new ColorRgba(
            Lerp(a.R, b.R, t),
            Lerp(a.G, b.G, t),
            Lerp(a.B, b.B, t),
            Lerp(a.A, b.A, t)
        );
    }
}

// Unity integration
public class ColorAnimator : MonoBehaviour {
    void Update() {
        // Domain logic with domain types
        var start = new ColorRgba(1f, 0f, 0f);
        var end = new ColorRgba(0f, 0f, 1f);
        var current = ColorMath.Lerp(start, end, Mathf.PingPong(Time.time, 1f));
        
        // Adapter only at the boundary
        GetComponent<Renderer>().material.color = current.ToUnity();
    }
}
```

## Common Pitfalls

### ❌ **Anti-Pattern: Unity Types in Domain Layer**

```csharp
// WRONG: Unity type in domain logic
public class EnemyAI {
    public Vector3 CalculateNextPosition() { ... }  // ❌ Unity dependency
}
```

```csharp
// CORRECT: Domain types in domain logic
public class EnemyAI {
    public Float3 CalculateNextPosition() { ... }  // ✓ Domain type
}
```

### ❌ **Anti-Pattern: Using `Safe` Policy for Trusted Data**

```csharp
// WRONG: Unnecessary validation overhead
void Update() {
    var pos = new Float3(0f, 0f, 0f);  // Literal constant
    transform.position = pos.ToUnity(ValidationPolicy.Safe, diag);  // ❌ Wasted CPU
}
```

```csharp
// CORRECT: Simple adapter for trusted data
void Update() {
    var pos = new Float3(0f, 0f, 0f);
    transform.position = pos.ToUnity();  // ✓ No overhead
}
```

### ❌ **Anti-Pattern: Ignoring Sanitization Warnings**

```csharp
// WRONG: Silent data corruption
var diag = UnityAdapterDiagnostics.None;  // ❌ Suppresses warnings
transform.position = networkData.ToUnity(diag);
```

```csharp
// CORRECT: Log issues for debugging
var diag = UnityAdapterDiagnostics.UnityDebug;  // ✓ See problems
transform.position = networkData.ToUnity(diag);
```

## Diagnostics

The `UnityAdapterDiagnostics` class controls logging behavior:

```csharp
// Log to Unity console
var diag = UnityAdapterDiagnostics.UnityDebug;

// Silent (production/performance)
var diag = UnityAdapterDiagnostics.None;

// Custom logging
var diag = new UnityAdapterDiagnostics {
    Warn = msg => MyLogger.Warning(msg),
    Info = msg => MyLogger.Info(msg)
};
```

### Example Diagnostic Output

```
[Float3UnityAdapter] Float3 contains invalid numbers: (NaN, 5, 10)
[ColorRgbaUnityAdapter] Color sanitized (ReturnDefault, Clamp): (1.5, -0.2, 0.5, 1) -> (1, 0, 0.5, 1)
[AngleUnityAdapter] Angle sanitization failed (Throw): Infinity rad (status: Infinity). Returning unsanitized.
```

## Testing Strategies

### Unit Testing Domain Logic

```csharp
[Test]
public void ProjectileCalculator_PureLogic_NoUnityDependencies() {
    var calc = new ProjectileCalculator();
    var start = new Float3(0f, 10f, 0f);
    var velocity = new Float3(5f, 0f, 0f);
    
    var result = calc.CalculateTrajectory(start, velocity, 1f);
    
    // No Unity types involved - fast, isolated test
    Assert.AreEqual(5f, result.X, 0.01f);
}
```

### Integration Testing Adapters

```csharp
[Test]
public void Adapter_InvalidData_SanitizesWithSafePolicy() {
    var invalid = new Float3(float.NaN, 5f, 10f);
    var result = invalid.ToUnity(
        ValidationPolicy.Safe, 
        UnityAdapterDiagnostics.None
    );
    
    Assert.AreEqual(Vector3.zero, result);  // Sanitized to Zero
}
```

## Migration Guide

Converting existing Unity-coupled code:

### Before
```csharp
public class GameLogic : MonoBehaviour {
    public Vector3 ProcessMovement(Vector3 input) {
        return input * 2f;  // Unity types everywhere
    }
}
```

### After
```csharp
// Domain service (pure logic)
public class MovementCalculator {
    public Float3 ProcessMovement(Float3 input) {
        return new Float3(input.X * 2f, input.Y * 2f, input.Z * 2f);
    }
}

// Unity presenter (thin boundary layer)
public class MovementView : MonoBehaviour {
    private MovementCalculator _calc = new();
    
    void Update() {
        Float3 input = GetInputVector().ToDomain();
        Float3 result = _calc.ProcessMovement(input);
        transform.position = result.ToUnity();
    }
}
```

## Performance Considerations

- **Simple adapters** (`ToUnity()`, `ToDomain()`): Zero overhead, inline struct conversions
- **Validated adapters**: Add validation checks - use for untrusted data only
- **Unchecked adapters**: Logging only, minimal overhead - useful for debugging
- All domain types are `readonly struct` - no heap allocations, efficient passing

**Rule of thumb**: Use simple adapters by default. Add validation only where data trust is uncertain.

## License

This library is under the MIT License.

## Contributing

Contributions welcome! Please ensure:
- Domain types remain Unity-free
- Adapters live only in `Boilerplate.Foundation.Unity`
- All public APIs include XML documentation
- Tests cover validation edge cases (NaN, Infinity, range violations)
