using System.Text.RegularExpressions;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace EtnasSoft.Foundation.Unity.Tests {
    /// <summary>
    ///     Tests Float3 adapter to ensure conversions maintain precision
    ///     in Unity's coordinate system.
    /// </summary>
    public class Float3UnityAdapterTests {
        /// <summary>
        ///     Confirms roundtrip conversions to avoid precision loss in vectors.
        /// </summary>
        [Test]
        public void Roundtrip_IsStable() {
            var d = new Float3(1, 2, 3);
            var u = d.ToUnity();
            var back = u.ToDomain();
            Assert.AreEqual(d, back);
        }

        [Test]
        public void Float3_ToUnity_Diagnostics_UsesSafePolicy_SanitizesNaN() {
            var bad = new Float3(float.NaN, 1f, 2f);

            LogAssert.Expect(LogType.Warning,
                new Regex("Float3 sanitized"));

            var result = bad.ToUnity(UnityAdapterDiagnostics.UnityDebug);

            Assert.AreEqual(Vector3.zero, result);
        }

        [Test]
        public void Float3_ToUnityUnchecked_WarnsAndPassesThrough() {
            var bad = new Float3(float.NaN, 1f, 2f);

            LogAssert.Expect(LogType.Warning,
                new Regex("contains invalid numbers"));

            var result = bad.ToUnityUnchecked(UnityAdapterDiagnostics.UnityDebug);

            Assert.IsTrue(float.IsNaN(result.x));
            Assert.AreEqual(1f, result.y);
            Assert.AreEqual(2f, result.z);
        }
    }
}
