using EtnasSoft.Foundation.Numerics;
using NUnit.Framework;

namespace EtnasSoft.Foundation.Unity.Tests {
    public class Float3MoveTowardsTests {
        // ── Zero / negative delta ────────────────────────────────────────────

        [Test]
        public void MoveTowards_ZeroDelta_ReturnsCurrent() {
            var current = new Float3(0f, 0f, 0f);
            var target = new Float3(5f, 5f, 5f);

            var result = current.MoveTowards(target, 0f);

            Assert.AreEqual(current, result);
        }

        [Test]
        public void MoveTowards_NegativeDelta_ReturnsCurrent() {
            var current = new Float3(1f, 1f, 1f);
            var target = new Float3(5f, 5f, 5f);

            var result = current.MoveTowards(target, -1f);

            Assert.AreEqual(current, result);
        }

        // ── Already at target ────────────────────────────────────────────────

        [Test]
        public void MoveTowards_SamePosition_ReturnsTarget() {
            var pos = new Float3(3f, 3f, 3f);

            var result = pos.MoveTowards(pos, 1f);

            Assert.AreEqual(pos, result);
        }

        // ── Delta larger than remaining distance ─────────────────────────────

        [Test]
        public void MoveTowards_LargeDelta_ReturnsTargetExactly() {
            var current = new Float3(0f, 0f, 0f);
            var target = new Float3(1f, 0f, 0f);

            var result = current.MoveTowards(target, 100f);

            Assert.AreEqual(target, result);
        }

        [Test]
        public void MoveTowards_ExactDelta_ReturnsTargetExactly() {
            var current = new Float3(0f, 0f, 0f);
            var target = new Float3(0f, 3f, 4f); // distance = 5

            var result = current.MoveTowards(target, 5f);

            Assert.AreEqual(target, result);
        }

        // ── Partial advance ───────────────────────────────────────────────────

        [Test]
        public void MoveTowards_PartialStep_AdvancesCorrectly() {
            var current = new Float3(0f, 0f, 0f);
            var target = new Float3(0f, 3f, 4f); // distance = 5, direction = (0, 0.6, 0.8)

            var result = current.MoveTowards(target, 2.5f); // half-way

            Assert.AreEqual(new Float3(0f, 1.5f, 2f), result);
        }

        // ── Edge cases ────────────────────────────────────────────────────────

        [Test]
        public void MoveTowards_LargeCoordinates_DoesNotOvershoot() {
            var current = new Float3(1000f, 1000f, 1000f);
            var target = new Float3(1001f, 1000f, 1000f); // distance = 1

            var result = current.MoveTowards(target, 100f);

            Assert.AreEqual(target, result);
        }

        [Test]
        public void MoveTowards_AlmostEqualPositions_DoesNotOvershoot() {
            var current = new Float3(0f, 0f, 0f);
            var target = new Float3(1e-6f, 1e-6f, 1e-6f);

            var result = current.MoveTowards(target, 1f);

            Assert.AreEqual(target, result);
        }
    }
}
