using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace EtnasSoft.Foundation.Unity.Tests {
    /// <summary>
    ///     Verifies diagnostics configuration to ensure proper logging setup
    ///     in adapter operations.
    /// </summary>
    public class UnityAdapterDiagnosticsTests {
        /// <summary>
        ///     Ensures UnityDebug preset is correctly wired for warning logs.
        /// </summary>
        [Test]
        public void UnityDebug_Preset_IsWireUpCorrectly() {
            // Arrange
            // Accessing UnityDebug exercises the 'private init' of Warn
            var diag = UnityAdapterDiagnostics.UnityDebug;

            // Assert: Properties should not be null
            Assert.IsNotNull(diag.Warn, "UnityDebug.Warn should not be null");
            Assert.IsNotNull(diag.Info, "UnityDebug.Info should not be null");

            // Assert: Check that they actually call Unity
            LogAssert.Expect(LogType.Warning, "Test Warning Invocation");
            LogAssert.Expect(LogType.Log, "Test Info Invocation");

            // Act
            diag.Warn?.Invoke("Test Warning Invocation");
            diag.Info?.Invoke("Test Info Invocation");
        }

        /// <summary>
        ///     Confirms None preset disables all diagnostics safely.
        /// </summary>
        [Test]
        public void None_Preset_HasNullCallbacks() {
            // Arrange
            var diag = UnityAdapterDiagnostics.None;

            // Assert
            Assert.IsNull(diag.Warn, "None.Warn should be null");
            Assert.IsNull(diag.Info, "None.Info should be null");

            // Act (Should not throw exception)
            diag.Warn?.Invoke("Silence");
            diag.Info?.Invoke("Silence");
        }

        /// <summary>
        ///     Validates manual initialization of Info for custom diagnostics.
        /// </summary>
        [Test]
        public void Can_Initialize_Info_Manually() {
            // This test validates that the 'Info' property has a public accessible 'init'

            // Arrange
            var infoCalled = false;

            var diag = new UnityAdapterDiagnostics {
                // Warn = ... // Would not compile because it is 'private init'
                Info = msg => infoCalled = true
            };

            // Assert
            Assert.IsNull(diag.Warn,
                "Warn must be null when instantiating manually");
            Assert.IsNotNull(diag.Info, "Info should have been assigned");

            // Act
            diag.Info.Invoke("Testing manual init");

            Assert.IsTrue(infoCalled,
                "The manual delegate should have been executed");
        }
    }
}
