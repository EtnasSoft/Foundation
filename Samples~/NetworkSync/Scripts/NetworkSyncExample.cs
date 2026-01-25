using UnityEngine;
using Boilerplate.Foundation;
using Boilerplate.Foundation.Unity;
using Boilerplate.Foundation.Validation;

/// <summary>
/// Example showing validation of untrusted network data
/// </summary>
public class NetworkSyncExample : MonoBehaviour
{
    private readonly UnityAdapterDiagnostics _diag = 
        UnityAdapterDiagnostics.UnityDebug;
    
    public void SimulateNetworkUpdate()
    {
        // Simulate corrupted network data
        Float3 networkPosition = new Float3(float.NaN, 5f, 10f);
        
        // Safe policy sanitizes invalid values
        transform.position = networkPosition.ToUnity(_diag);
        
        // Check console for sanitization warning
        Debug.Log("Network position applied with sanitization");
    }
}
