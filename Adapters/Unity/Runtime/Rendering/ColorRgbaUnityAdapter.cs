using EtnasSoft.Foundation.Validation;
using UnityEngine;

namespace EtnasSoft.Foundation.Unity {
    public static class ColorRgbaUnityAdapter {
        public static Color ToUnity(
            this ColorRgba c
        ) {
            return new Color(c.R, c.G, c.B, c.A);
        }

        public static Color ToUnity(
            this ColorRgba c,
            UnityAdapterDiagnostics diag
        ) {
            return c.ToUnity(ValidationPolicy.Safe, diag);
        }

        public static Color ToUnityUnchecked(
            this ColorRgba c,
            UnityAdapterDiagnostics diag
        ) {
            if (!ColorValidation.IsFinite(c)) {
                diag.Warn?.Invoke(
                    $"[ColorRgbaUnityAdapter] Color contains invalid numbers: ({c.R},{c.G},{c.B},{c.A})"
                );
            } else if (!ColorValidation.IsInUnitRange(c)) {
                diag.Warn?.Invoke(
                    $"[ColorRgbaUnityAdapter] Color out of [0..1]: ({c.R},{c.G},{c.B},{c.A})"
                );
            }

            return new Color(c.R, c.G, c.B, c.A);
        }

        public static ColorRgba ToDomain(
            this Color c
        ) {
            return new ColorRgba(c.r, c.g, c.b, c.a);
        }

        public static Color ToUnity(
            this ColorRgba c,
            ValidationPolicy policy,
            UnityAdapterDiagnostics diag
        ) {
            if (!ColorRgbaSanitizer.TrySanitize(c, policy, out var sanitized,
                    out var status)) {
                LogSanitizationFailure(diag, c, status, policy);

                return new Color(c.R, c.G, c.B, c.A);
            }

            if (!sanitized.Equals(c)) {
                diag.Warn?.Invoke(
                    $"[ColorRgbaUnityAdapter] Color sanitized ({policy.InvalidNumber}, {policy.ColorUnitRange}): " +
                    $"({c.R},{c.G},{c.B},{c.A}) -> ({sanitized.R},{sanitized.G},{sanitized.B},{sanitized.A})"
                );
            }

            return new Color(sanitized.R, sanitized.G, sanitized.B,
                sanitized.A);
        }

        private static void LogSanitizationFailure(
            UnityAdapterDiagnostics diag,
            in ColorRgba color,
            SanitizeStatus status,
            ValidationPolicy policy
        ) {
            var policyDetails = $"InvalidNumber: {policy.InvalidNumber}";
            var rangeDetails = $"InvalidNumber: {policy.InvalidNumber}, Range: {policy.ColorUnitRange}";

            switch (status) {
                case SanitizeStatus.NaN:
                case SanitizeStatus.NotFinite:
                    diag.Warn?.Invoke(
                        $"[ColorRgbaUnityAdapter] Color contains invalid numbers ({policyDetails}): ({color.R},{color.G},{color.B},{color.A})"
                    );
                    break;

                case SanitizeStatus.Infinity:
                    diag.Warn?.Invoke(
                        $"[ColorRgbaUnityAdapter] Color contains infinite values ({policyDetails}): ({color.R},{color.G},{color.B},{color.A})"
                    );
                    break;

                case SanitizeStatus.OutOfRange:
                    diag.Warn?.Invoke(
                        $"[ColorRgbaUnityAdapter] Color out of [0..1] ({rangeDetails}): ({color.R},{color.G},{color.B},{color.A})"
                    );
                    break;
            }
        }
    }
}
