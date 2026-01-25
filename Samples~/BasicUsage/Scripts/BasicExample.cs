using UnityEngine;
using Boilerplate.Foundation;
using Boilerplate.Foundation.Unity;

/// <summary>
/// Basic example showing domain types and adapter usage
/// </summary>
public class BasicExample : MonoBehaviour
{
    void Start()
    {
        // Domain type creation
        Float3 position = new Float3(10f, 5f, 0f);
        Angle rotation = Angle.FromDegrees(45f);
        ColorRgba color = new ColorRgba(1f, 0f, 0f, 1f);
        
        // Convert to Unity at the boundary
        transform.position = position.ToUnity();
        transform.rotation = rotation.ToUnityRotationZ();
        GetComponent<Renderer>().material.color = color.ToUnity();
        
        Debug.Log($"Position: {position}");
        Debug.Log($"Rotation: {rotation}");
    }
}
