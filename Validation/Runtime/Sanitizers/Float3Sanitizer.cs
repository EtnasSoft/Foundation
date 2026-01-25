using System;

namespace EtnasSoft.Foundation.Validation {
    public static class Float3Sanitizer {
        public static bool TrySanitize(
            in Float3 v,
            ValidationPolicy policy,
            out Float3 sanitized,
            out SanitizeStatus status
        ) {
            status = SanitizeStatus.None;

            if (Float3Validation.IsFinite(v)) {
                sanitized = v;
                return true;
            }

            var hasNaN = Float3Validation.HasNaN(v);
            var hasInfinity = Float3Validation.HasInfinity(v);
            status = SanitizeCommon.GetNonFiniteStatus(hasNaN, hasInfinity);

            return SanitizeCommon.ApplyInvalidNumberPolicy(
                original: v,
                policy: policy.InvalidNumber,
                getDefault: static () => Float3.Zero,
                sanitized: out sanitized
            );
        }

        public static Float3 Sanitize(
            in Float3 v,
            ValidationPolicy policy
        ) {
            if (TrySanitize(v, policy, out var sanitized, out _)) {
                return sanitized;
            }

            throw new ArgumentException(
                $"Invalid Float3 ({policy.InvalidNumber}): {v}");
        }
    }
}
