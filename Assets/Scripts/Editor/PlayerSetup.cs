using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.UI;

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

        CameraFollow camFollow = cam.gameObject.AddComponent<CameraFollow>();

        SetupPlayer(camFollow);
        SetupGameManager();
        SetupDarkMap();

        string scenePath = "Assets/Scenes/MainScene.unity";
        System.IO.Directory.CreateDirectory("Assets/Scenes");
        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene(), scenePath);

        Debug.Log("Scène créée avec Kenney sprites!");
    }

    static void SetupPlayer(CameraFollow camFollow)
    {
        GameObject player = new GameObject("Player");
        player.tag = "Player";

        // 2D physics
        CircleCollider2D col = player.AddComponent<CircleCollider2D>();
        col.radius = 0.4f;

        Rigidbody2D rb = player.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.freezeRotation = true;

        // Kenney sprite
        SpriteRenderer sr = player.AddComponent<SpriteRenderer>();
        sr.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Kenney/PNG/Survivor 1/survivor1_stand.png");
        sr.sortingOrder = 10;

        // Scripts
        player.AddComponent<PlayerMovement>();
        player.AddComponent<Shooting>();
        player.AddComponent<Health>();

        // Light Halo
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

        if (camFollow != null)
        {
            camFollow.target = player.transform;
        }
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
