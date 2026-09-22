using UnityEngine;

public class DarknessOverlay : MonoBehaviour
{
    public float haloRadius = 12f;
    public LayerMask occluderMask;

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private Mesh fogMesh;
    private Material fogMat;

    private int segments = 64;
    private float fogSize = 100f;

    void Start()
    {
        GameObject fogObj = new GameObject("Fog");
        fogObj.transform.SetParent(transform);
        fogObj.transform.localPosition = new Vector3(0, 0, 0.5f);

        meshFilter = fogObj.AddComponent<MeshFilter>();
        meshRenderer = fogObj.AddComponent<MeshRenderer>();

        Shader shader = Shader.Find("Custom/FogOverlay");
        if (shader == null)
            shader = Shader.Find("Sprites/Default");

        fogMat = new Material(shader);
        fogMat.color = Color.black;

        meshRenderer.material = fogMat;
        meshRenderer.sortingLayerName = "Fog";
        meshRenderer.sortingOrder = 100;

        fogMesh = new Mesh();
        fogMesh.name = "FogMesh";
        meshFilter.mesh = fogMesh;

        GenerateFogMesh();
    }

    void LateUpdate()
    {
        GenerateFogMesh();
    }

    void GenerateFogMesh()
    {
        Vector3 center = transform.position;
        float r = fogSize;

        Vector3[] vertices = new Vector3[segments + 1];
        int[] triangles = new int[segments * 3];

        vertices[0] = new Vector3(0, 0, 0);

        for (int i = 0; i < segments; i++)
        {
            float angle = (float)i / segments * Mathf.PI * 2f;
            vertices[i + 1] = new Vector3(Mathf.Cos(angle) * r, Mathf.Sin(angle) * r, 0);
        }

        for (int i = 0; i < segments; i++)
        {
            triangles[i * 3] = 0;
            triangles[i * 3 + 1] = i + 1;
            triangles[i * 3 + 2] = (i + 1) % segments + 1;
        }

        fogMesh.Clear();
        fogMesh.vertices = vertices;
        fogMesh.triangles = triangles;
        fogMesh.RecalculateNormals();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, haloRadius);
    }
}
