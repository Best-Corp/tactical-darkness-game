using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class PlayerSetup : EditorWindow
{
    [MenuItem("Tactical Darkness/Setup Full 2D Scene")]
    static void SetupFullScene()
    {
        EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);

        // REMOVE directional light
        Light dirLight = Object.FindObjectOfType<Light>();
        if (dirLight != null && dirLight.type == LightType.Directional)
        {
            Object.DestroyImmediate(dirLight.gameObject);
        }

        // Setup Camera
        Camera cam = Camera.main;
        cam.orthographic = true;
        cam.orthographicSize = 10f;
        cam.transform.position = new Vector3(0, 0, -10);
        cam.backgroundColor = Color.black;
        cam.clearFlags = CameraClearFlags.SolidColor;

        // Add camera follow
        CameraFollow camFollow = cam.gameObject.AddComponent<CameraFollow>();

        SetupPlayer(camFollow);
        SetupGameManager();
        SetupDarkMap();

        string scenePath = "Assets/Scenes/MainScene.unity";
        System.IO.Directory.CreateDirectory("Assets/Scenes");
        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene(), scenePath);

        Debug.Log("Scene ready! Player should be visible with halo light.");
    }

    static void SetupPlayer(CameraFollow camFollow)
    {
        GameObject player = new GameObject("Player");
        player.tag = "Player";

        // 2D physics
        CircleCollider2D col = player.AddComponent<CircleCollider2D>();
        col.radius = 0.5f;

        Rigidbody2D rb = player.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.freezeRotation = true;

        // Player sprite - BIG and VISIBLE
        SpriteRenderer sr = player.AddComponent<SpriteRenderer>();
        sr.sprite = CreateCircleSprite(64);
        sr.color = Color.cyan;

        // Make player bigger
        player.transform.localScale = new Vector3(1f, 1f, 1f);

        // Scripts
        player.AddComponent<PlayerMovement>();
        player.AddComponent<Shooting>();
        player.AddComponent<Health>();

        // Light Halo - BRIGHT
        GameObject lightObj = new GameObject("HaloLight");
        lightObj.transform.SetParent(player.transform);
        lightObj.transform.localPosition = new Vector3(0, 0, 2f);

        Light haloLight = lightObj.AddComponent<Light>();
        haloLight.type = LightType.Point;
        haloLight.range = 8f;
        haloLight.intensity = 5f;
        haloLight.color = Color.white;
        haloLight.shadows = LightShadows.None;

        player.transform.position = new Vector3(0, 0, 0);

        // Camera follows player
        if (camFollow != null)
        {
            camFollow.target = player.transform;
        }
    }

    static Sprite CreateCircleSprite(int size)
    {
        Texture2D tex = new Texture2D(size, size);
        Color transparent = new Color(0, 0, 0, 0);
        Color white = Color.white;

        int center = size / 2;
        float radius = size / 2f - 1;

        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                float dist = Vector2.Distance(new Vector2(x, y), new Vector2(center, center));
                if (dist <= radius)
                    tex.SetPixel(x, y, white);
                else
                    tex.SetPixel(x, y, transparent);
            }
        }

        tex.Apply();
        tex.filterMode = FilterMode.Point;
        return Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), size / 2);
    }

    static void SetupGameManager()
    {
        GameObject gm = new GameObject("GameManager");
        gm.AddComponent<GameManager>();
    }

    static void SetupDarkMap()
    {
        // Ground
        GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
        ground.name = "Ground";
        ground.transform.localScale = new Vector3(10, 1, 10);
        ground.transform.position = Vector3.zero;
        ground.AddComponent<MapShrink>();
        Object.DestroyImmediate(ground.GetComponent<BoxCollider>());
        ground.GetComponent<Renderer>().sharedMaterial.color = new Color(0.02f, 0.02f, 0.02f);

        // Walls
        CreateWall("WallNorth", new Vector3(0, 0.5f, 50), new Vector3(100, 1, 1));
        CreateWall("WallSouth", new Vector3(0, 0.5f, -50), new Vector3(100, 1, 1));
        CreateWall("WallEast", new Vector3(50, 0.5f, 0), new Vector3(1, 1, 100));
        CreateWall("WallWest", new Vector3(-50, 0.5f, 0), new Vector3(1, 1, 100));

        // Cover
        for (int i = 0; i < 10; i++)
        {
            float x = Random.Range(-35f, 35f);
            float z = Random.Range(-35f, 35f);
            CreateCover("Cover_" + i, new Vector3(x, 0.5f, z));
        }
    }

    static void CreateWall(string name, Vector3 position, Vector3 scale)
    {
        GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        wall.name = name;
        wall.transform.position = position;
        wall.transform.localScale = scale;
        wall.GetComponent<Renderer>().sharedMaterial.color = new Color(0.08f, 0.08f, 0.08f);
    }

    static void CreateCover(string name, Vector3 position)
    {
        GameObject cover = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cover.name = name;
        cover.transform.position = position;
        cover.transform.localScale = new Vector3(
            Random.Range(2f, 4f),
            Random.Range(1f, 2f),
            Random.Range(2f, 4f)
        );
        cover.GetComponent<Renderer>().sharedMaterial.color = new Color(0.1f, 0.1f, 0.1f);
    }
}
