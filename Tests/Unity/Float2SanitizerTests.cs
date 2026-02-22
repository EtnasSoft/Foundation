using System;
using EtnasSoft.Foundation.Validation;
using NUnit.Framework;

namespace EtnasSoft.Foundation.Unity.Tests {
    public class Float2SanitizerTests {
        [Test]
        public void TrySanitize_Float2_ValidInput_ReturnsUnchanged() {
            var value = new Float2(1f, 2f);

            var ok = Float2Sanitizer.TrySanitize(value, ValidationPolicy.None,
                out var sanitized, out var status);

            Assert.IsTrue(ok);
            Assert.AreEqual(SanitizeStatus.None, status);
            Assert.AreEqual(value, sanitized);
        }

        [Test]
        public void TrySanitize_Float2_Strict_NaN_ReturnsFalse() {
            var value = new Float2(float.NaN, 0f);

            var ok = Float2Sanitizer.TrySanitize(value, ValidationPolicy.Strict,
                out var sanitized, out var status);

            Assert.IsFalse(ok);
            Assert.AreEqual(SanitizeStatus.NaN, status);
            Assert.AreEqual(value, sanitized);
        }

        [Test]
        public void TrySanitize_Float2_Strict_Infinity_ReturnsFalse() {
            var value = new Float2(0f, float.PositiveInfinity);

            var ok = Float2Sanitizer.TrySanitize(value, ValidationPolicy.Strict,
                out var sanitized, out var status);

            Assert.IsFalse(ok);
            Assert.AreEqual(SanitizeStatus.Infinity, status);
            Assert.AreEqual(value, sanitized);
        }

        [Test]
        public void TrySanitize_Float2_Safe_NaN_ReturnsZero() {
            var value = new Float2(float.NaN, 0f);

            var ok = Float2Sanitizer.TrySanitize(value, ValidationPolicy.Safe,
                out var sanitized, out var status);

            Assert.IsTrue(ok);
            Assert.AreEqual(SanitizeStatus.NaN, status);
            Assert.AreEqual(Float2.Zero, sanitized);
        }

        [Test]
        public void TrySanitize_Float2_Safe_NotFinite_ReturnsDefault() {
            var value = new Float2(float.NaN, float.PositiveInfinity);

            var ok = Float2Sanitizer.TrySanitize(value, ValidationPolicy.Safe,
                out var sanitized, out var status);

            Assert.IsTrue(ok);
            Assert.AreEqual(SanitizeStatus.NotFinite, status);
            Assert.AreEqual(Float2.Zero, sanitized);
        }

        [Test]
        public void TrySanitize_Float2_None_NaN_PassesThrough() {
            var value = new Float2(float.NaN, 1f);

            var ok = Float2Sanitizer.TrySanitize(value, ValidationPolicy.None,
                out var sanitized, out var status);

            Assert.IsTrue(ok);
            Assert.AreEqual(SanitizeStatus.NaN, status);
            Assert.AreEqual(value, sanitized);
        }

        [Test]
        public void Sanitize_Float2_Strict_NaN_ThrowsArgumentException() {
            var value = new Float2(float.NaN, 0f);

            Assert.Throws<ArgumentException>(() =>
                Float2Sanitizer.Sanitize(value, ValidationPolicy.Strict));
        }
    }
}
