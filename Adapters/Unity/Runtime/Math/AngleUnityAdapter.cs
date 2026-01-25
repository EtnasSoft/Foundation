using EtnasSoft.Foundation.Validation;
using UnityEngine;

namespace EtnasSoft.Foundation.Unity {
    public static class AngleUnityAdapter {
        public static float ToUnityDegrees(
            this Angle angle
        ) {
            return angle.Degrees;
        }

        public static float ToUnityDegrees(
            this Angle angle,
            UnityAdapterDiagnostics diag
        ) {
            return angle.ToUnityDegrees(ValidationPolicy.Safe, diag);
        }

        public static float ToUnityDegreesUnchecked(
            this Angle angle,
            UnityAdapterDiagnostics diag
        ) {
            if (!AngleValidation.IsFinite(angle)) {
                diag.Warn?.Invoke(
                    $"[AngleUnityAdapter] Angle contains invalid numbers: {angle.Radians} rad"
                );
            }

            return angle.Degrees;
        }

        public static Angle ToDomainAngle(
            this float unityDegrees
        ) {
            return Angle.FromDegrees(unityDegrees);
        }

        public static Quaternion ToUnityRotationZ(
            this Angle angle
        ) {
            return Quaternion.Euler(0f, 0f, angle.Degrees);
        }

        public static Angle ToDomainAngleZ(
            this Quaternion rotation
        ) {
            return Angle.FromDegrees(rotation.eulerAngles.z);
        }

        public static float ToUnityDegrees(
            this Angle angle,
            ValidationPolicy policy,
            UnityAdapterDiagnostics diag
        ) {
            if (!AngleSanitizer.TrySanitize(angle, policy, out var sanitized,
                    out var status)) {
                diag.Warn?.Invoke(
                    $"[AngleUnityAdapter] Angle sanitization failed ({policy.InvalidNumber}): {angle.Radians} rad (status: {status}). Returning unsanitized."
                );
                sanitized = angle;
            }

            if (!sanitized.Equals(angle)) {
                diag.Warn?.Invoke(
                    $"[AngleUnityAdapter] Angle sanitized ({policy.InvalidNumber}): {angle.Radians} rad -> {sanitized.Radians} rad"
                );
            }

            return sanitized.Degrees;
        }

        public static Quaternion ToUnityRotationZ(
            this Angle angle,
            ValidationPolicy policy,
            UnityAdapterDiagnostics diag
        ) {
            var degrees = angle.ToUnityDegrees(policy, diag);
            return Quaternion.Euler(0f, 0f, degrees);
        }

        public static Angle ToDomainAngleZ(
            this Quaternion rotation,
            ValidationPolicy policy,
            UnityAdapterDiagnostics diag
        ) {
            var angle = Angle.FromDegrees(rotation.eulerAngles.z);
            if (!AngleSanitizer.TrySanitize(angle, policy, out var sanitized,
                    out var status)) {
                diag.Warn?.Invoke(
                    $"[AngleUnityAdapter] Quaternion.Z angle sanitization failed ({policy.InvalidNumber}): {angle.Radians} rad (status: {status}). Returning unsanitized."
                );
                sanitized = angle;
            }

            if (!sanitized.Equals(angle)) {
                diag.Warn?.Invoke(
                    $"[AngleUnityAdapter] Quaternion.Z angle sanitized ({policy.InvalidNumber}): {angle.Radians} rad -> {sanitized.Radians} rad"
                );
            }

            return sanitized;
        }
    }
}
