using UnityEngine;

public class DarknessOverlay : MonoBehaviour
{
    public float haloRadius = 12f;
    public LayerMask occluderMask;

    private Material fogMat;
    private GameObject fogQuad;

    void Start()
    {
        fogQuad = GameObject.CreatePrimitive(PrimitiveType.Quad);
        fogQuad.name = "FogOverlay";
        fogQuad.transform.SetParent(transform);
        fogQuad.transform.localPosition = new Vector3(0, 0, 0.5f);
        fogQuad.transform.localScale = new Vector3(200, 200, 1);

        // Remove collider
        Object.Destroy(fogQuad.GetComponent<BoxCollider>());

        // Setup renderer
        MeshRenderer mr = fogQuad.GetComponent<MeshRenderer>();

        Shader shader = Shader.Find("Custom/FogOverlay");
        if (shader == null)
            shader = Shader.Find("Sprites/Default");

        fogMat = new Material(shader);
        fogMat.SetFloat("_Radius", haloRadius);
        fogMat.SetVector("_PlayerPos", Vector4.zero);

        mr.material = fogMat;
        mr.sortingLayerName = "Fog";
        mr.sortingOrder = 100;
    }

    void LateUpdate()
    {
        if (fogMat != null)
        {
            fogMat.SetVector("_PlayerPos", transform.position);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, haloRadius);
    }
}
