using UnityEngine;

public class LightHalo : MonoBehaviour
{
    public float haloRadius = 5f;
    public Light haloLight;

    void Start()
    {
        // Create point light for halo
        GameObject lightObj = new GameObject("HaloLight");
        lightObj.transform.SetParent(transform);
        lightObj.transform.localPosition = new Vector3(0, 1, 0);

        haloLight = lightObj.AddComponent<Light>();
        haloLight.type = LightType.Point;
        haloLight.range = haloRadius;
        haloLight.intensity = 3f;
        haloLight.color = Color.white;
        haloLight.shadows = LightShadows.None;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, haloRadius);
    }
}
