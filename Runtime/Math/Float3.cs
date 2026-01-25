using System;
using System.Runtime.CompilerServices;

namespace EtnasSoft.Foundation {
    /// <summary>
    ///     Encapsulates 3D coordinates in a struct to prevent coordinate mixing
    ///     and ensure type safety in vector operations.
    /// </summary>
    public readonly struct Float3 : IEquatable<Float3> {
        public readonly float X;
        public readonly float Y;
        public readonly float Z;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Float3(
            float x,
            float y,
            float z
        ) {
            (X, Y, Z) = (x, y, z);
        }

        public bool Equals(
            Float3 other
        ) {
            return X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z);
        }

        public override bool Equals(
            object? obj
        ) {
            return obj is Float3 other && Equals(other);
        }

        public override int GetHashCode() {
            return HashCode.Combine(X, Y, Z);
        }

        /// <summary>
        ///     Formats coordinates for debugging and logging to aid development.
        /// </summary>
        public override string ToString() {
            return $"({X}, {Y}, {Z})";
        }

        public static Float3 Zero => new(0f, 0f, 0f);
    }
}
