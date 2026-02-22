namespace EtnasSoft.Foundation.Validation {
    public static class Float2Validation {
        public static bool IsFinite(
            in Float2 v
        ) {
            return float.IsFinite(v.X) && float.IsFinite(v.Y);
        }

        public static bool HasNaN(
            in Float2 v
        ) {
            return float.IsNaN(v.X) || float.IsNaN(v.Y);
        }

        public static bool HasInfinity(
            in Float2 v
        ) {
            return float.IsInfinity(v.X) || float.IsInfinity(v.Y);
        }
    }
}
