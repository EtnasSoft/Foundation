using EtnasSoft.Foundation.Validation;
using NUnit.Framework;

namespace EtnasSoft.Foundation.Unity.Tests {
    public class Float3SanitizerTests {
        [Test]
        public void TrySanitize_Float3_Strict_NaN_ReturnsFalse() {
            var value = new Float3(float.NaN, 0f, 0f);

            var ok = Float3Sanitizer.TrySanitize(value, ValidationPolicy.Strict,
                out var sanitized, out var status);

            Assert.IsFalse(ok);
            Assert.AreEqual(SanitizeStatus.NaN, status);
            Assert.AreEqual(value, sanitized);
        }

        [Test]
        public void TrySanitize_Float3_Safe_NotFinite_ReturnsDefault() {
            var value = new Float3(float.NaN, float.PositiveInfinity, 0f);

            var ok = Float3Sanitizer.TrySanitize(value, ValidationPolicy.Safe,
                out var sanitized, out var status);

            Assert.IsTrue(ok);
            Assert.AreEqual(SanitizeStatus.NotFinite, status);
            Assert.AreEqual(Float3.Zero, sanitized);
        }
    }
}
