using System;
using System.Runtime.CompilerServices;

namespace EtnasSoft.Foundation {
    /// <summary>
    ///     Encapsulates 2D coordinates in a struct to prevent coordinate mixing
    ///     and ensure type safety in vector operations.
    /// </summary>
    public readonly struct Float2 : IEquatable<Float2> {
        public readonly float X;
        public readonly float Y;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Float2(
            float x,
            float y
        ) {
            (X, Y) = (x, y);
        }

        public bool Equals(
            Float2 other
        ) {
            return X.Equals(other.X) && Y.Equals(other.Y);
        }

        public override bool Equals(
            object? obj
        ) {
            return obj is Float2 other && Equals(other);
        }

        public override int GetHashCode() {
            return HashCode.Combine(X, Y);
        }

        /// <summary>
        ///     Formats coordinates for debugging and logging to aid development.
        /// </summary>
        public override string ToString() {
            return $"({X}, {Y})";
        }

        public static Float2 Zero => new(0f, 0f);
    }
}
