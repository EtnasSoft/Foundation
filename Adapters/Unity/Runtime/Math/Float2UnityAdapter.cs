using EtnasSoft.Foundation.Validation;
using UnityEngine;

namespace EtnasSoft.Foundation.Unity {
    public static class Float2UnityAdapter {
        public static Vector2 ToUnity(
            this Float2 v
        ) {
            return new Vector2(v.X, v.Y);
        }

        public static Vector2 ToUnity(
            this Float2 v,
            UnityAdapterDiagnostics diag
        ) {
            return v.ToUnity(ValidationPolicy.Safe, diag);
        }

        public static Vector2 ToUnityUnchecked(
            this Float2 v,
            UnityAdapterDiagnostics diag
        ) {
            if (!Float2Validation.IsFinite(v)) {
                diag.Warn?.Invoke(
                    $"[Float2UnityAdapter] Float2 contains invalid numbers: {v}"
                );
            }

            return new Vector2(v.X, v.Y);
        }

        public static Float2 ToDomain(
            this Vector2 v
        ) {
            return new Float2(v.x, v.y);
        }

        public static Vector2 ToUnity(
            this Float2 v,
            ValidationPolicy policy,
            UnityAdapterDiagnostics diag
        ) {
            if (!Float2Sanitizer.TrySanitize(v, policy, out var sanitized,
                    out var status)) {
                diag.Warn?.Invoke(
                    $"[Float2UnityAdapter] Float2 sanitization failed ({policy.InvalidNumber}): {v} (status: {status}). Returning unsanitized."
                );
                sanitized = v;
            }

            if (!sanitized.Equals(v)) {
                diag.Warn?.Invoke(
                    $"[Float2UnityAdapter] Float2 sanitized ({policy.InvalidNumber}): {v} -> {sanitized}"
                );
            }

            return new Vector2(sanitized.X, sanitized.Y);
        }

        public static Float2 ToDomain(
            this Vector2 v,
            ValidationPolicy policy,
            UnityAdapterDiagnostics diag
        ) {
            var domain = new Float2(v.x, v.y);
            if (!Float2Sanitizer.TrySanitize(domain, policy, out var sanitized,
                    out var status)) {
                diag.Warn?.Invoke(
                    $"[Float2UnityAdapter] Vector2 sanitization failed ({policy.InvalidNumber}): ({v.x}, {v.y}) (status: {status}). Returning unsanitized."
                );
                sanitized = domain;
            }

            if (!sanitized.Equals(domain)) {
                diag.Warn?.Invoke(
                    $"[Float2UnityAdapter] Vector2 sanitized ({policy.InvalidNumber}): ({v.x}, {v.y}) -> {sanitized}"
                );
            }

            return sanitized;
        }
    }
}
