namespace EtnasSoft.Foundation.Validation {
    public static class ColorValidation {
        public static bool IsFinite(
            in ColorRgba c
        ) {
            return float.IsFinite(c.R) && float.IsFinite(c.G) &&
                   float.IsFinite(c.B) && float.IsFinite(c.A);
        }

        public static bool HasNaN(
            in ColorRgba c
        ) {
            return float.IsNaN(c.R) || float.IsNaN(c.G) ||
                   float.IsNaN(c.B) || float.IsNaN(c.A);
        }

        public static bool HasInfinity(
            in ColorRgba c
        ) {
            return float.IsInfinity(c.R) || float.IsInfinity(c.G) ||
                   float.IsInfinity(c.B) || float.IsInfinity(c.A);
        }

        public static bool IsInUnitRange(
            in ColorRgba c
        ) {
            return c.R is >= 0f and <= 1f &&
                   c.G is >= 0f and <= 1f &&
                   c.B is >= 0f and <= 1f &&
                   c.A is >= 0f and <= 1f;
        }

        public static ColorRgba Clamp01(
            in ColorRgba c
        ) {
            return new ColorRgba(Clamp01(c.R), Clamp01(c.G), Clamp01(c.B),
                Clamp01(c.A));
        }

        private static float Clamp01(
            float v
        ) {
            if (!float.IsFinite(v)) {
                return 0f;
            }

            return v < 0f ? 0f : v > 1f ? 1f : v;
        }
    }
}
