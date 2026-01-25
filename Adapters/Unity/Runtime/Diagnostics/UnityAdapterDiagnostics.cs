using System;
using UnityEngine;

namespace EtnasSoft.Foundation.Unity {
    public sealed class UnityAdapterDiagnostics {
        public Action<string>? Warn { get; init; }
        public Action<string>? Info { get; init; }

        public static UnityAdapterDiagnostics UnityDebug =>
            new() {
                Warn = Debug.LogWarning,
                Info = Debug.Log
            };

        public static UnityAdapterDiagnostics None => new();
    }
}
