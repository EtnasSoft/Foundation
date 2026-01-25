using EtnasSoft.Foundation.Validation;
using NUnit.Framework;

namespace EtnasSoft.Foundation.Unity.Tests {
    public class ColorRgbaSanitizerTests {
        [Test]
        public void TrySanitize_ColorRgba_Safe_NaN_ReturnsDefault() {
            var value = new ColorRgba(float.NaN, 0f, 0f);

            var ok = ColorRgbaSanitizer.TrySanitize(value, ValidationPolicy.Safe,
                out var sanitized, out var status);

            Assert.IsTrue(ok);
            Assert.AreEqual(SanitizeStatus.NaN, status);
            Assert.AreEqual(new ColorRgba(0f, 0f, 0f, 1f), sanitized);
        }

        [Test]
        public void TrySanitize_ColorRgba_Safe_Infinity_ReturnsDefault() {
            var value = new ColorRgba(float.PositiveInfinity, 0f, 0f);

            var ok = ColorRgbaSanitizer.TrySanitize(value, ValidationPolicy.Safe,
                out var sanitized, out var status);

            Assert.IsTrue(ok);
            Assert.AreEqual(SanitizeStatus.Infinity, status);
            Assert.AreEqual(new ColorRgba(0f, 0f, 0f, 1f), sanitized);
        }

        [Test]
        public void TrySanitize_ColorRgba_Safe_NotFinite_ReturnsDefault() {
            var value = new ColorRgba(float.NaN, float.PositiveInfinity, 0f);

            var ok = ColorRgbaSanitizer.TrySanitize(value, ValidationPolicy.Safe,
                out var sanitized, out var status);

            Assert.IsTrue(ok);
            Assert.AreEqual(SanitizeStatus.NotFinite, status);
            Assert.AreEqual(new ColorRgba(0f, 0f, 0f, 1f), sanitized);
        }

        [Test]
        public void TrySanitize_ColorRgba_Safe_OutOfRange_Clamps() {
            var value = new ColorRgba(2f, -0.5f, 0.5f, 2f);

            var ok = ColorRgbaSanitizer.TrySanitize(value, ValidationPolicy.Safe,
                out var sanitized, out var status);

            Assert.IsTrue(ok);
            Assert.AreEqual(SanitizeStatus.OutOfRange, status);
            Assert.AreEqual(new ColorRgba(1f, 0f, 0.5f, 1f), sanitized);
        }
    }
}
