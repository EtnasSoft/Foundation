using System.Text.RegularExpressions;
using EtnasSoft.Foundation.Validation;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace EtnasSoft.Foundation.Unity.Tests {
    /// <summary>
    ///     Validates color adapters to ensure accurate conversions and safe
    ///     handling of edge cases in Unity rendering.
    /// </summary>
    public class ColorUnityAdapterTests {
        /// <summary>
        ///     Verifies roundtrip accuracy to maintain color fidelity.
        /// </summary>
        [Test]
        public void ColorRgba_Roundtrip_IsAccurate() {
            var domain = new ColorRgba(0.1f, 0.5f, 0.9f);
            var unity = domain.ToUnity();
            var back = unity.ToDomain();

            Assert.AreEqual(domain.R, back.R, 1e-5f);
            Assert.AreEqual(domain.G, back.G, 1e-5f);
            Assert.AreEqual(domain.B, back.B, 1e-5f);
            Assert.AreEqual(domain.A, back.A, 1e-5f);
        }

        /// <summary>
        ///     Ensures NaN colors are sanitized by policy.
        /// </summary>
        [Test]
        public void ColorRgba_PolicyPath_SanitizesNaN() {
            // Arrange
            var badColor = new ColorRgba(float.NaN, 0, 0);

            // Assert: Expect Unity to receive a specific Warning
            LogAssert.Expect(LogType.Warning, new Regex("Color sanitized"));

            // Act: Use UnityDebug configuration that writes to Debug.LogWarning
            var result = badColor.ToUnity(ValidationPolicy.Safe,
                UnityAdapterDiagnostics.UnityDebug);

            Assert.AreEqual(new Color(0f, 0f, 0f, 1f), result,
                "Must return default color in case of invalid numbers");
        }

        /// <summary>
        ///     Checks value clamping to avoid color overflow in shaders.
        /// </summary>
        [Test]
        public void ColorRgba_PolicyPath_ClampsValues() {
            // Arrange: Color out of range
            var hdrColor = new ColorRgba(2.0f, -0.5f, 0.5f);

            // Assert: Expect clamp warning
            LogAssert.Expect(LogType.Warning,
                new Regex("Color sanitized"));

            // Act
            var result = hdrColor.ToUnity(ValidationPolicy.Safe,
                UnityAdapterDiagnostics.UnityDebug);

            Assert.AreEqual(1.0f, result.r);
            Assert.AreEqual(0.0f, result.g);
        }

        /// <summary>
        ///     Confirms byte color roundtrip for exact representation.
        /// </summary>
        [Test]
        public void ColorRgba32_Roundtrip_IsExact() {
            var domain = new ColorRgba32(255, 128, 0);
            var unity = domain.ToUnity();
            var back = unity.ToDomain();

            Assert.AreEqual(domain.R, back.R);
            Assert.AreEqual(domain.G, back.G);
        }
    }
}
