using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class PlayerSetup : EditorWindow
{
    [MenuItem("Tactical Darkness/Setup Full 2D Scene")]
    static void SetupFullScene()
    {
        EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);

        // Fix Camera for 2D top-down
        Camera cam = Camera.main;
        cam.orthographic = true;
        cam.orthographicSize = 15f;
        cam.transform.position = new Vector3(0, 20, -10);
        cam.transform.rotation = Quaternion.Euler(90, 0, 0);
        cam.backgroundColor = Color.black;
        cam.clearFlags = CameraClearFlags.SolidColor;

        SetupPlayer();
        SetupGameManager();
        SetupDarkMap();

        string scenePath = "Assets/Scenes/MainScene.unity";
        System.IO.Directory.CreateDirectory("Assets/Scenes");
        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene(), scenePath);

        Debug.Log("2D top-down scene created!");
    }

    static void SetupPlayer()
    {
        GameObject player = new GameObject("Player");
        player.tag = "Player";

        CircleCollider2D col = player.AddComponent<CircleCollider2D>();
        col.radius = 0.5f;

        Rigidbody2D rb = player.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.freezeRotation = true;

        SpriteRenderer sr = player.AddComponent<SpriteRenderer>();
        sr.color = Color.cyan;

        player.AddComponent<PlayerMovement>();
        player.AddComponent<LightHalo>();
        player.AddComponent<Shooting>();
        player.AddComponent<Health>();

        GameObject firePoint = new GameObject("FirePoint");
        firePoint.transform.SetParent(player.transform);
        firePoint.transform.localPosition = new Vector3(1f, 0, 0);
        player.GetComponent<Shooting>().firePoint = firePoint.transform;

        player.transform.position = new Vector3(0, 0, 0);
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

        Renderer groundRend = ground.GetComponent<Renderer>();
        groundRend.sharedMaterial.color = new Color(0.05f, 0.05f, 0.05f);

        // Walls
        CreateWall("WallNorth", new Vector3(0, 0.5f, 50), new Vector3(100, 1, 1));
        CreateWall("WallSouth", new Vector3(0, 0.5f, -50), new Vector3(100, 1, 1));
        CreateWall("WallEast", new Vector3(50, 0.5f, 0), new Vector3(1, 1, 100));
        CreateWall("WallWest", new Vector3(-50, 0.5f, 0), new Vector3(1, 1, 100));

        // Cover
        for (int i = 0; i < 10; i++)
        {
            float x = Random.Range(-40f, 40f);
            float z = Random.Range(-40f, 40f);
            CreateCover("Cover_" + i, new Vector3(x, 0.5f, z));
        }
    }

    static void CreateWall(string name, Vector3 position, Vector3 scale)
    {
        GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        wall.name = name;
        wall.transform.position = position;
        wall.transform.localScale = scale;
        Renderer rend = wall.GetComponent<Renderer>();
        rend.sharedMaterial.color = new Color(0.1f, 0.1f, 0.1f);
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
        Renderer rend = cover.GetComponent<Renderer>();
        rend.sharedMaterial.color = new Color(0.15f, 0.15f, 0.15f);
    }
}
