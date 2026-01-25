namespace EtnasSoft.Foundation.Validation {
    public static class Float3Validation {
        public static bool IsFinite(
            in Float3 v
        ) {
            return float.IsFinite(v.X) && float.IsFinite(v.Y) &&
                   float.IsFinite(v.Z);
        }

        public static bool HasNaN(
            in Float3 v
        ) {
            return float.IsNaN(v.X) || float.IsNaN(v.Y) || float.IsNaN(v.Z);
        }

        public static bool HasInfinity(
            in Float3 v
        ) {
            return float.IsInfinity(v.X) || float.IsInfinity(v.Y) ||
                   float.IsInfinity(v.Z);
        }
    }
}
