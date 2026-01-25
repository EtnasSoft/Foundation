namespace EtnasSoft.Foundation.Validation {
    public static class AngleValidation {
        public static bool IsFinite(
            in Angle a
        ) {
            return float.IsFinite(a.Radians);
        }

        public static bool HasNaN(
            in Angle a
        ) {
            return float.IsNaN(a.Radians);
        }

        public static bool HasInfinity(
            in Angle a
        ) {
            return float.IsInfinity(a.Radians);
        }
    }
}
