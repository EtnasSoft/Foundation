using System;

namespace EtnasSoft.Foundation.Validation {
    public static class Float2Sanitizer {
        public static bool TrySanitize(
            in Float2 v,
            ValidationPolicy policy,
            out Float2 sanitized,
            out SanitizeStatus status
        ) {
            status = SanitizeStatus.None;

            if (Float2Validation.IsFinite(v)) {
                sanitized = v;
                return true;
            }

            var hasNaN = Float2Validation.HasNaN(v);
            var hasInfinity = Float2Validation.HasInfinity(v);
            status = SanitizeCommon.GetNonFiniteStatus(hasNaN, hasInfinity);

            return SanitizeCommon.ApplyInvalidNumberPolicy(
                v,
                policy.InvalidNumber,
                static () => Float2.Zero,
                out sanitized
            );
        }

        public static Float2 Sanitize(
            in Float2 v,
            ValidationPolicy policy
        ) {
            if (TrySanitize(v, policy, out var sanitized, out _)) {
                return sanitized;
            }

            throw new ArgumentException(
                $"Invalid Float2 ({policy.InvalidNumber}): {v}");
        }
    }
}
