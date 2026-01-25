using System.Text.RegularExpressions;
using EtnasSoft.Foundation.Validation;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace EtnasSoft.Foundation.Unity.Tests {
    /// <summary>
    ///     Tests angle adapter conversions to ensure precision and error handling
    ///     in Unity integration.
    /// </summary>
    public class AngleUnityAdapterTests {
        [Test]
        public void Angle_ToUnityDegrees_ConvertsCorrectly() {
            var domain = Angle.FromDegrees(180f);
            var unityDegrees = domain.ToUnityDegrees();
            Assert.AreEqual(180f, unityDegrees, 1e-5f);
        }

        [Test]
        public void Angle_ToUnityRotationZ_CreatesCorrectQuaternion() {
            var domain = Angle.FromDegrees(90f);
            var rotation = domain.ToUnityRotationZ();

            // 90 degrees in Z should be ~ (0, 0, 0.707, 0.707)
            Assert.AreEqual(90f, rotation.eulerAngles.z, 1e-5f);
        }

        /// <summary>
        ///     Verifies policy-driven sanitization to prevent invalid rotations.
        /// </summary>
        [Test]
        public void Angle_PolicyPath_SanitizesNaN() {
            // Arrange: Force NaN by creating angle with NaN radians,
            // assuming Angle allows NaN internally for this test.
            var badAngle = Angle.FromRadians(float.NaN);

            // Assert
            LogAssert.Expect(LogType.Warning,
                new Regex("Angle sanitized"));

            // Act
            var result = badAngle.ToUnityDegrees(ValidationPolicy.Safe,
                UnityAdapterDiagnostics.UnityDebug);

            Assert.AreEqual(0f, result);
        }

        [Test]
        public void Angle_DiagnosticsPath_UsesSafePolicy_SanitizesNaN() {
            var badAngle = Angle.FromRadians(float.NaN);

            LogAssert.Expect(LogType.Warning,
                new Regex("Angle sanitized"));

            var result = badAngle.ToUnityDegrees(
                UnityAdapterDiagnostics.UnityDebug);

            Assert.AreEqual(0f, result);
        }

        [Test]
        public void Angle_ToUnityDegreesUnchecked_WarnsAndPassesThrough() {
            var badAngle = Angle.FromRadians(float.NaN);

            LogAssert.Expect(LogType.Warning,
                new Regex("contains invalid numbers"));

            var result = badAngle.ToUnityDegreesUnchecked(
                UnityAdapterDiagnostics.UnityDebug);

            Assert.IsTrue(float.IsNaN(result));
        }
    }
}
