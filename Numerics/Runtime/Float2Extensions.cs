using System;
using System.Runtime.CompilerServices;

namespace EtnasSoft.Foundation.Numerics {
    /// <summary>
    ///     Mathematical operations for <see cref="Float2" /> that live outside the value object
    ///     to keep the struct focused on identity and equality.
    /// </summary>
    public static class Float2Extensions {
        /// <summary>
        ///     Moves <paramref name="current" /> towards <paramref name="target" /> by at most
        ///     <paramref name="maxDistanceDelta" /> units, without overshooting.
        /// </summary>
        /// <param name="current">The starting position.</param>
        /// <param name="target">The desired destination.</param>
        /// <param name="maxDistanceDelta">
        ///     Maximum distance to advance this step.
        ///     Values less than or equal to zero return <paramref name="current" /> unchanged.
        /// </param>
        /// <returns>
        ///     <paramref name="target" /> when the remaining distance is within
        ///     <paramref name="maxDistanceDelta" /> (no overshoot); otherwise a position
        ///     advanced exactly <paramref name="maxDistanceDelta" /> towards
        ///     <paramref name="target" />.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Float2 MoveTowards(
            this Float2 current,
            Float2 target,
            float maxDistanceDelta
        ) {
            if (maxDistanceDelta <= 0f) return current;

            float dx = target.X - current.X;
            float dy = target.Y - current.Y;

            float sqrDist = dx * dx + dy * dy;
            if (sqrDist == 0f) return target;

            float maxDeltaSqr = maxDistanceDelta * maxDistanceDelta;
            if (sqrDist <= maxDeltaSqr) return target;

            float scale = maxDistanceDelta / MathF.Sqrt(sqrDist);
            return new Float2(current.X + dx * scale, current.Y + dy * scale);
        }
    }
}
