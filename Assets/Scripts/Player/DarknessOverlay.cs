using UnityEngine;

public class DarknessOverlay : MonoBehaviour
{
    public float haloRadius = 5f;
    public Material darknessMaterial;

    private RenderTexture renderTexture;
    private Camera darkCam;

    void Start()
    {
        // Create darkness texture
        renderTexture = new RenderTexture(256, 256, 0);
        renderTexture.Create();

        // Create a circle texture for the halo
        Texture2D circleTex = CreateCircleTexture(256, (int)(haloRadius * 20));

        if (darknessMaterial == null)
        {
            darknessMaterial = new Material(Shader.Find("Sprites/Default"));
        }
        darknessMaterial.mainTexture = circleTex;
    }

    Texture2D CreateCircleTexture(int size, int radius)
    {
        Texture2D tex = new Texture2D(size, size);
        Color transparent = new Color(0, 0, 0, 0);
        Color opaque = new Color(0, 0, 0, 1);

        int center = size / 2;

        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                float dist = Vector2.Distance(new Vector2(x, y), new Vector2(center, center));
                if (dist < radius)
                    tex.SetPixel(x, y, transparent);
                else
                    tex.SetPixel(x, y, opaque);
            }
        }

        tex.Apply();
        return tex;
    }

    void OnDestroy()
    {
        if (renderTexture != null)
            renderTexture.Release();
    }
}
