using System;

namespace EtnasSoft.Foundation.Validation {
    internal static class SanitizeCommon {
        public static SanitizeStatus GetNonFiniteStatus(
            bool hasNaN,
            bool hasInfinity
        ) {
            if (hasNaN && hasInfinity) {
                return SanitizeStatus.NotFinite;
            }

            if (hasNaN) {
                return SanitizeStatus.NaN;
            }

            if (hasInfinity) {
                return SanitizeStatus.Infinity;
            }

            return SanitizeStatus.NotFinite;
        }

        public static bool ApplyInvalidNumberPolicy<T>(
            in T original,
            InvalidNumberPolicy policy,
            Func<T> getDefault,
            out T sanitized
        ) {
            switch (policy) {
                case InvalidNumberPolicy.Ignore:
                    sanitized = original;
                    return true;

                case InvalidNumberPolicy.ReturnDefault:
                    sanitized = getDefault();
                    return true;

                case InvalidNumberPolicy.Throw:
                    sanitized = original;
                    return false;
            }

            sanitized = original;
            return true;
        }

        public static bool ApplyRangePolicy<T>(
            in T original,
            RangePolicy policy,
            System.Func<T> clamp,
            out T sanitized
        ) {
            switch (policy) {
                case RangePolicy.Ignore:
                    sanitized = original;
                    return true;

                case RangePolicy.Clamp:
                    sanitized = clamp();
                    return true;

                case RangePolicy.Throw:
                    sanitized = original;
                    return false;
            }

            sanitized = original;
            return true;
        }
    }
}
