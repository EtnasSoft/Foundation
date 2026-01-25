using EtnasSoft.Foundation.Validation;
using UnityEngine;

namespace EtnasSoft.Foundation.Unity {
    public static class Float3UnityAdapter {
        public static Vector3 ToUnity(
            this Float3 v
        ) {
            return new Vector3(v.X, v.Y, v.Z);
        }

        public static Vector3 ToUnity(
            this Float3 v,
            UnityAdapterDiagnostics diag
        ) {
            return v.ToUnity(ValidationPolicy.Safe, diag);
        }

        public static Vector3 ToUnityUnchecked(
            this Float3 v,
            UnityAdapterDiagnostics diag
        ) {
            if (!Float3Validation.IsFinite(v)) {
                diag.Warn?.Invoke(
                    $"[Float3UnityAdapter] Float3 contains invalid numbers: {v}"
                );
            }

            return new Vector3(v.X, v.Y, v.Z);
        }

        public static Float3 ToDomain(
            this Vector3 v
        ) {
            return new Float3(v.x, v.y, v.z);
        }

        public static Vector3 ToUnity(
            this Float3 v,
            ValidationPolicy policy,
            UnityAdapterDiagnostics diag
        ) {
            if (!Float3Sanitizer.TrySanitize(v, policy, out var sanitized,
                    out var status)) {
                diag.Warn?.Invoke(
                    $"[Float3UnityAdapter] Float3 sanitization failed ({policy.InvalidNumber}): {v} (status: {status}). Returning unsanitized."
                );
                sanitized = v;
            }

            if (!sanitized.Equals(v)) {
                diag.Warn?.Invoke(
                    $"[Float3UnityAdapter] Float3 sanitized ({policy.InvalidNumber}): {v} -> {sanitized}"
                );
            }

            return new Vector3(sanitized.X, sanitized.Y, sanitized.Z);
        }

        public static Float3 ToDomain(
            this Vector3 v,
            ValidationPolicy policy,
            UnityAdapterDiagnostics diag
        ) {
            var domain = new Float3(v.x, v.y, v.z);
            if (!Float3Sanitizer.TrySanitize(domain, policy, out var sanitized,
                    out var status)) {
                diag.Warn?.Invoke(
                    $"[Float3UnityAdapter] Vector3 sanitization failed ({policy.InvalidNumber}): ({v.x}, {v.y}, {v.z}) (status: {status}). Returning unsanitized."
                );
                sanitized = domain;
            }

            if (!sanitized.Equals(domain)) {
                diag.Warn?.Invoke(
                    $"[Float3UnityAdapter] Vector3 sanitized ({policy.InvalidNumber}): ({v.x}, {v.y}, {v.z}) -> {sanitized}"
                );
            }

            return sanitized;
        }
    }
}
