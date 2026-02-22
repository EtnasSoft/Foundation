using System.Text.RegularExpressions;
using EtnasSoft.Foundation.Validation;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace EtnasSoft.Foundation.Unity.Tests {
    /// <summary>
    ///     Tests Float2 adapter to ensure conversions maintain precision
    ///     in Unity's coordinate system.
    /// </summary>
    public class Float2UnityAdapterTests {
        /// <summary>
        ///     Confirms roundtrip conversions to avoid precision loss in vectors.
        /// </summary>
        [Test]
        public void Roundtrip_IsStable() {
            var d = new Float2(1f, 2f);
            var u = d.ToUnity();
            var back = u.ToDomain();
            Assert.AreEqual(d, back);
        }

        [Test]
        public void Float2_ToUnity_Simple_MapsCorrectly() {
            var d = new Float2(3.5f, -1.25f);
            var u = d.ToUnity();
            Assert.AreEqual(3.5f, u.x);
            Assert.AreEqual(-1.25f, u.y);
        }

        [Test]
        public void Vector2_ToDomain_Simple_MapsCorrectly() {
            var u = new Vector2(7f, -4f);
            var d = u.ToDomain();
            Assert.AreEqual(new Float2(7f, -4f), d);
        }

        [Test]
        public void Float2_ToUnity_Diagnostics_UsesSafePolicy_SanitizesNaN() {
            var bad = new Float2(float.NaN, 1f);

            LogAssert.Expect(LogType.Warning,
                new Regex("Float2 sanitized"));

            var result = bad.ToUnity(UnityAdapterDiagnostics.UnityDebug);

            Assert.AreEqual(Vector2.zero, result);
        }

        [Test]
        public void
            Float2_ToUnity_Diagnostics_UsesSafePolicy_SanitizesInfinity() {
            var bad = new Float2(0f, float.NegativeInfinity);

            LogAssert.Expect(LogType.Warning,
                new Regex("Float2 sanitized"));

            var result = bad.ToUnity(UnityAdapterDiagnostics.UnityDebug);

            Assert.AreEqual(Vector2.zero, result);
        }

        [Test]
        public void Float2_ToUnityUnchecked_WarnsAndPassesThrough() {
            var bad = new Float2(float.NaN, 1f);

            LogAssert.Expect(LogType.Warning,
                new Regex("contains invalid numbers"));

            var result =
                bad.ToUnityUnchecked(UnityAdapterDiagnostics.UnityDebug);

            Assert.IsTrue(float.IsNaN(result.x));
            Assert.AreEqual(1f, result.y);
        }

        [Test]
        public void Float2_ToUnity_NonePolicy_PassesThroughNaN() {
            var bad = new Float2(float.NaN, 1f);
            var result = bad.ToUnity(ValidationPolicy.None,
                UnityAdapterDiagnostics.None);
            Assert.IsTrue(float.IsNaN(result.x));
        }

        [Test]
        public void Vector2_ToDomain_Safe_SanitizesNaN() {
            var bad = new Vector2(float.NaN, 2f);

            LogAssert.Expect(LogType.Warning,
                new Regex("Vector2 sanitized"));

            var result = bad.ToDomain(
                ValidationPolicy.Safe,
                UnityAdapterDiagnostics.UnityDebug);

            Assert.AreEqual(Float2.Zero, result);
        }

        [Test]
        public void Float2_Zero_RoundtripIsStable() {
            var d = Float2.Zero;
            Assert.AreEqual(d, d.ToUnity().ToDomain());
        }
    }
}
