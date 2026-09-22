using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class PlayerSetup : EditorWindow
{
    [MenuItem("Tactical Darkness/Setup Full Scene")]
    static void SetupFullScene()
    {
        // Create new scene
        EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);

        // Setup Player
        SetupPlayer();
        SetupGameManager();
        SetupDarkMap();

        // Save scene
        string scenePath = "Assets/Scenes/MainScene.unity";
        System.IO.Directory.CreateDirectory("Assets/Scenes");
        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene(), scenePath);

        Debug.Log("Full scene created and saved to " + scenePath);
    }

    static void SetupPlayer()
    {
        GameObject player = new GameObject("Player");
        player.tag = "Player";

        CharacterController cc = player.AddComponent<CharacterController>();
        cc.height = 2f;
        cc.radius = 0.5f;
        cc.center = new Vector3(0, 1, 0);

        player.AddComponent<PlayerMovement>();
        player.AddComponent<LightHalo>();
        player.AddComponent<Shooting>();
        player.AddComponent<Health>();

        GameObject camObj = new GameObject("PlayerCamera");
        camObj.transform.SetParent(player.transform);
        camObj.transform.localPosition = new Vector3(0, 1.6f, 0);
        Camera cam = camObj.AddComponent<Camera>();
        cam.nearClipPlane = 0.1f;
        cam.farClipPlane = 100f;
        camObj.AddComponent<AudioListener>();

        player.transform.position = new Vector3(0, 1, 0);
    }

    static void SetupGameManager()
    {
        GameObject gm = new GameObject("GameManager");
        gm.AddComponent<GameManager>();
    }

    static void SetupDarkMap()
    {
        GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
        ground.name = "Ground";
        ground.transform.localScale = new Vector3(10, 1, 10);
        ground.AddComponent<MapShrink>();

        CreateWall("WallNorth", new Vector3(0, 2, 50), new Vector3(100, 4, 1));
        CreateWall("WallSouth", new Vector3(0, 2, -50), new Vector3(100, 4, 1));
        CreateWall("WallEast", new Vector3(50, 2, 0), new Vector3(1, 4, 100));
        CreateWall("WallWest", new Vector3(-50, 2, 0), new Vector3(1, 4, 100));

        for (int i = 0; i < 10; i++)
        {
            float x = Random.Range(-40f, 40f);
            float z = Random.Range(-40f, 40f);
            CreateCover("Cover_" + i, new Vector3(x, 1, z));
        }
    }

    static void CreateWall(string name, Vector3 position, Vector3 scale)
    {
        GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        wall.name = name;
        wall.transform.position = position;
        wall.transform.localScale = scale;
        Renderer rend = wall.GetComponent<Renderer>();
        rend.material.color = new Color(0.1f, 0.1f, 0.1f);
    }

    static void CreateCover(string name, Vector3 position)
    {
        GameObject cover = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cover.name = name;
        cover.transform.position = position;
        cover.transform.localScale = new Vector3(
            Random.Range(1f, 3f),
            Random.Range(1f, 2f),
            Random.Range(1f, 3f)
        );
        Renderer rend = cover.GetComponent<Renderer>();
        rend.material.color = new Color(0.15f, 0.15f, 0.15f);
    }
}
