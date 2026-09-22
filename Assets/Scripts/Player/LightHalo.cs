using UnityEngine;

public class LightHalo : MonoBehaviour
{
    public float haloRadius = 5f;
    public Light haloLight;

    void Start()
    {
        GameObject lightObj = new GameObject("HaloLight");
        lightObj.transform.SetParent(transform);
        lightObj.transform.localPosition = Vector3.zero;

        haloLight = lightObj.AddComponent<Light>();
        haloLight.type = LightType.Point;
        haloLight.range = haloRadius;
        haloLight.intensity = 2f;
        haloLight.color = Color.white;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, haloRadius);
    }
}
