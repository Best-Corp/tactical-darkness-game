using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class PlayerSetup : EditorWindow
{
    [MenuItem("Tactical Darkness/Setup Full 2D Scene")]
    static void SetupFullScene()
    {
        EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);

        // Fix Camera
        Camera cam = Camera.main;
        cam.orthographic = true;
        cam.orthographicSize = 15f;
        cam.transform.position = new Vector3(0, 20, -10);
        cam.transform.rotation = Quaternion.Euler(90, 0, 0);
        cam.backgroundColor = Color.black;
        cam.clearFlags = CameraClearFlags.SolidColor;

        // REMOVE directional light (key for darkness!)
        Light dirLight = Object.FindObjectOfType<Light>();
        if (dirLight != null && dirLight.type == LightType.Directional)
        {
            Object.DestroyImmediate(dirLight.gameObject);
        }

        SetupPlayer();
        SetupGameManager();
        SetupDarkMap();

        string scenePath = "Assets/Scenes/MainScene.unity";
        System.IO.Directory.CreateDirectory("Assets/Scenes");
        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene(), scenePath);

        Debug.Log("Dark 2D scene created! No directional light = darkness everywhere except halo.");
    }

    static void SetupPlayer()
    {
        GameObject player = new GameObject("Player");
        player.tag = "Player";

        // 2D physics
        CircleCollider2D col = player.AddComponent<CircleCollider2D>();
        col.radius = 0.5f;

        Rigidbody2D rb = player.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.freezeRotation = true;

        // Player sprite (will be lit by point light)
        SpriteRenderer sr = player.AddComponent<SpriteRenderer>();
        sr.color = Color.cyan;
        sr.sprite = CreateSquareSprite();

        // Scripts
        player.AddComponent<PlayerMovement>();
        player.AddComponent<LightHalo>();
        player.AddComponent<Shooting>();
        player.AddComponent<Health>();

        player.transform.position = new Vector3(0, 0, 0);
    }

    static Sprite CreateSquareSprite()
    {
        Texture2D tex = new Texture2D(32, 32);
        Color[] pixels = new Color[32 * 32];
        for (int i = 0; i < pixels.Length; i++)
            pixels[i] = Color.white;
        tex.SetPixels(pixels);
        tex.Apply();
        tex.filterMode = FilterMode.Point;

        return Sprite.Create(tex, new Rect(0, 0, 32, 32), new Vector2(0.5f, 0.5f), 32);
    }

    static void SetupGameManager()
    {
        GameObject gm = new GameObject("GameManager");
        gm.AddComponent<GameManager>();
    }

    static void SetupDarkMap()
    {
        // Ground (dark)
        GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
        ground.name = "Ground";
        ground.transform.localScale = new Vector3(10, 1, 10);
        ground.transform.position = Vector3.zero;
        ground.AddComponent<MapShrink>();
        Object.DestroyImmediate(ground.GetComponent<BoxCollider>());
        ground.GetComponent<Renderer>().sharedMaterial.color = new Color(0.02f, 0.02f, 0.02f);

        // Walls (dark)
        CreateWall("WallNorth", new Vector3(0, 0.5f, 50), new Vector3(100, 1, 1));
        CreateWall("WallSouth", new Vector3(0, 0.5f, -50), new Vector3(100, 1, 1));
        CreateWall("WallEast", new Vector3(50, 0.5f, 0), new Vector3(1, 1, 100));
        CreateWall("WallWest", new Vector3(-50, 0.5f, 0), new Vector3(1, 1, 100));

        // Cover (dark)
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
