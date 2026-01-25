using System;

namespace EtnasSoft.Foundation {
    /// <summary>
    ///     Encapsulates colors in byte format to ensure exact 0-255 values
    ///     and prevent floating-point precision issues.
    /// </summary>
    public readonly struct ColorRgba32 : IEquatable<ColorRgba32> {
        public readonly byte R, G, B, A; // 0..255

        /// <summary>
        ///     Initializes color components with alpha defaulting to 255 for opaque.
        /// </summary>
        public ColorRgba32(
            byte r,
            byte g,
            byte b,
            byte a = 255
        ) {
            (R, G, B, A) = (r, g, b, a);
        }

        public bool Equals(
            ColorRgba32 other
        ) {
            return R == other.R && G == other.G && B == other.B &&
                   A == other.A;
        }

        public override bool Equals(
            object? obj
        ) {
            return obj is ColorRgba32 other && Equals(other);
        }

        public override int GetHashCode() {
            return HashCode.Combine(R, G, B, A);
        }

        public override string ToString() {
            return $"({R}, {G}, {B}, {A})";
        }
    }
}
