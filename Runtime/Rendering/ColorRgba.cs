using System;

namespace EtnasSoft.Foundation {
    /// <summary>
    ///     Represents colors in 0-1 float range to ensure compatibility with
    ///     Unity's Color type and prevent overflow issues.
    /// </summary>
    public readonly struct ColorRgba : IEquatable<ColorRgba> {
        public readonly float R, G, B, A; // contract: 0..1

        /// <summary>
        ///     Initializes color components with alpha defaulting to 1 for opaque.
        /// </summary>
        public ColorRgba(
            float r,
            float g,
            float b,
            float a = 1f
        ) {
            (R, G, B, A) = (r, g, b, a);
        }

        public bool Equals(
            ColorRgba other
        ) {
            return R.Equals(other.R) && G.Equals(other.G) &&
                   B.Equals(other.B) && A.Equals(other.A);
        }

        public override bool Equals(
            object? obj
        ) {
            return obj is ColorRgba other && Equals(other);
        }

        public override int GetHashCode() {
            return HashCode.Combine(R, G, B, A);
        }

        public override string ToString() {
            return $"({R}, {G}, {B}, {A})";
        }
    }
}
