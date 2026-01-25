using System;

namespace EtnasSoft.Foundation.Validation {
    public static class ColorRgbaSanitizer {
        public static bool TrySanitize(
            in ColorRgba c,
            ValidationPolicy policy,
            out ColorRgba sanitized,
            out SanitizeStatus status
        ) {
            status = SanitizeStatus.None;

            if (!ColorValidation.IsFinite(c)) {
                var hasNaN = ColorValidation.HasNaN(c);
                var hasInfinity = ColorValidation.HasInfinity(c);
                status = SanitizeCommon.GetNonFiniteStatus(hasNaN, hasInfinity);

                return SanitizeCommon.ApplyInvalidNumberPolicy(
                    c,
                    policy.InvalidNumber,
                    static () => new ColorRgba(0f, 0f, 0f),
                    out sanitized
                );
            }

            if (!ColorValidation.IsInUnitRange(c)) {
                status = SanitizeStatus.OutOfRange;
                var original = c;

                return SanitizeCommon.ApplyRangePolicy(
                    original,
                    policy.ColorUnitRange,
                    () => ColorValidation.Clamp01(original),
                    out sanitized
                );
            }

            sanitized = c;
            return true;
        }

        public static ColorRgba Sanitize(
            in ColorRgba c,
            ValidationPolicy policy
        ) {
            if (TrySanitize(c, policy, out var sanitized, out var status)) {
                return sanitized;
            }

            if (status == SanitizeStatus.OutOfRange) {
                throw new ArgumentOutOfRangeException(
                    nameof(c),
                    $"ColorRgba out of [0..1]: ({c.R},{c.G},{c.B},{c.A})"
                );
            }

            throw new ArgumentException(
                $"Invalid ColorRgba ({policy.InvalidNumber}) status: {status}"
            );
        }
    }
}
