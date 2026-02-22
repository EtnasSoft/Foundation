# Unity Foundation Library

## Quick Start

1. Add the package via Git URL in Package Manager
2. Create `Assets/csc.rsp` with `-nullable:enable`
3. Reference assemblies in your `.asmdef` files

## Key Concepts

### Domain Types
    Use `Float2`, `Float3`, `Angle`, and `ColorRgba` instead of Unity types in your domain logic.

### Adapters
    Convert at boundaries using extension methods:
- `vector.ToDomain()` - Unity → Domain
    - `float3.ToUnity()` - Domain → Unity

### Validation
    Protect against invalid data:
    ```csharp
var diag = UnityAdapterDiagnostics.UnityDebug;
transform.position = networkData.ToUnity(ValidationPolicy.Safe, diag);
    ```

## Learn More

See the [full README](../README.md) for comprehensive documentation.
