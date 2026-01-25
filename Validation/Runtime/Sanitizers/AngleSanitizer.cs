using System;

namespace EtnasSoft.Foundation.Validation {
    public static class AngleSanitizer {
        public static bool TrySanitize(
            in Angle a,
            ValidationPolicy policy,
            out Angle sanitized,
            out SanitizeStatus status
        ) {
            status = SanitizeStatus.None;

            if (AngleValidation.IsFinite(a)) {
                sanitized = a;
                return true;
            }

            var hasNaN = AngleValidation.HasNaN(a);
            var hasInfinity = AngleValidation.HasInfinity(a);
            status = SanitizeCommon.GetNonFiniteStatus(hasNaN, hasInfinity);

            return SanitizeCommon.ApplyInvalidNumberPolicy(
                original: a,
                policy: policy.InvalidNumber,
                getDefault: static () => Angle.Zero,
                sanitized: out sanitized
            );
        }

        public static Angle Sanitize(
            in Angle a,
            ValidationPolicy policy
        ) {
            if (TrySanitize(a, policy, out var sanitized, out _)) {
                return sanitized;
            }

            throw new ArgumentException(
                $"Invalid Angle ({policy.InvalidNumber}): {a.Radians} rad");
        }
    }
}
