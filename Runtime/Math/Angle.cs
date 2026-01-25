using System;
using System.Runtime.CompilerServices;

namespace EtnasSoft.Foundation {
    /// <summary>
    ///     Encapsulates angles in radians for precise trigonometric calculations,
    ///     while providing degree conversions for human readability.
    /// </summary>
    public readonly struct Angle : IEquatable<Angle>, IComparable<Angle> {
        public readonly float Radians;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Angle(
            float radians
        ) {
            Radians = radians;
        }

        /// <summary>
        ///     Creates angle from radians to ensure internal consistency in math ops.
        /// </summary>
        public static Angle FromRadians(
            float radians
        ) {
            return new Angle(radians);
        }

        /// <summary>
        ///     Creates angle from degrees for user input convenience.
        /// </summary>
        public static Angle FromDegrees(
            float degrees
        ) {
            return new Angle(degrees * (MathF.PI / 180f));
        }

        /// <summary>
        ///     Converts to degrees for display and Unity compatibility.
        /// </summary>
        public float Degrees {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Radians * (180f / MathF.PI);
        }

        public static Angle Zero => new(0f);

        // Explicit operators to avoid confusion with float arithmetic
        /// <summary>
        ///     Defines angle addition to maintain type safety in angle operations.
        /// </summary>
        public static Angle operator +(
            Angle a,
            Angle b
        ) {
            return new Angle(a.Radians + b.Radians);
        }

        /// <summary>
        ///     Defines angle subtraction to maintain type safety in angle operations.
        /// </summary>
        public static Angle operator -(
            Angle a,
            Angle b
        ) {
            return new Angle(a.Radians - b.Radians);
        }

        public bool Equals(
            Angle other
        ) {
            return Radians == other.Radians;
        }

        public int CompareTo(
            Angle other
        ) {
            return Radians.CompareTo(other.Radians);
        }

        /// <summary>
        ///     Formats angle in degrees for user-friendly string representation.
        /// </summary>
        public override string ToString() {
            return $"{Degrees:F1}°";
        }
    }
}
