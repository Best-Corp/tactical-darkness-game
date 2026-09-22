using UnityEngine;

public class LightHalo : MonoBehaviour
{
    public float haloRadius = 5f;
    public LayerMask dynamicLayer;
    public Light haloLight;

    void Start()
    {
        // Create point light for halo
        GameObject lightObj = new GameObject("HaloLight");
        lightObj.transform.SetParent(transform);
        lightObj.transform.localPosition = Vector3.zero;
        
        haloLight = lightObj.AddComponent<Light>();
        haloLight.type = LightType.Point;
        haloLight.range = haloRadius;
        haloLight.intensity = 2f;
        haloLight.color = Color.white;
    }

    void Update()
    {
        // Check for dynamic objects in halo
        Collider[] hits = Physics.OverlapSphere(transform.position, haloRadius, dynamicLayer);
        
        foreach (Collider hit in hits)
        {
            // Reveal hidden objects
            Renderer renderer = hit.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.enabled = true;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        // Draw halo radius in editor
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, haloRadius);
    }
}
