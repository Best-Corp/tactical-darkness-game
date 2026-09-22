using UnityEngine;

public class DarknessOverlay : MonoBehaviour
{
    public float haloRadius = 12f;
    public LayerMask occluderMask;

    private Material fogMat;
    private GameObject fogQuad;

    void Start()
    {
        // Create a large quad for the fog
        fogQuad = new GameObject("FogOverlay");
        fogQuad.transform.SetParent(transform);
        fogQuad.transform.localPosition = new Vector3(0, 0, 0.5f);

        MeshFilter mf = fogQuad.AddComponent<MeshFilter>();
        MeshRenderer mr = fogQuad.AddComponent<MeshRenderer>();

        // Create quad mesh
        Mesh mesh = new Mesh();
        float size = 100f;
        mesh.vertices = new Vector3[]
        {
            new Vector3(-size, -size, 0),
            new Vector3(size, -size, 0),
            new Vector3(size, size, 0),
            new Vector3(-size, size, 0)
        };
        mesh.uv = new Vector2[]
        {
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(1, 1),
            new Vector2(0, 1)
        };
        mesh.triangles = new int[] { 0, 2, 1, 0, 3, 2 };
        mesh.RecalculateNormals();
        mf.mesh = mesh;

        // Load shader
        Shader shader = Shader.Find("Custom/FogOverlay");
        if (shader == null)
        {
            Debug.LogError("Shader Custom/FogOverlay not found!");
            shader = Shader.Find("Sprites/Default");
        }

        fogMat = new Material(shader);
        fogMat.SetFloat("_Radius", haloRadius);
        fogMat.SetVector("_PlayerPos", Vector4.zero);
        fogMat.SetColor("_Color", new Color(0, 0, 0, 0.95f));
        fogMat.SetFloat("_Softness", 1.5f);

        mr.material = fogMat;
        mr.sortingLayerName = "Fog";
        mr.sortingOrder = 100;

        Debug.Log("DarknessOverlay created with radius " + haloRadius);
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
