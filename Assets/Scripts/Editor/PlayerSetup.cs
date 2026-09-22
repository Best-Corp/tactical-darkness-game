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
        Light[] lights = Object.FindObjectsOfType<Light>();
        foreach (Light l in lights)
        {
            if (l.type == LightType.Directional)
                Object.DestroyImmediate(l.gameObject);
        }

        // Setup Camera - TOP DOWN
        Camera cam = Camera.main;
        cam.orthographic = true;
        cam.orthographicSize = 8f;
        cam.transform.position = new Vector3(0, 15, 0);
        cam.transform.rotation = Quaternion.Euler(90f, 0, 0);
        cam.backgroundColor = new Color(0.02f, 0.02f, 0.02f);
        cam.clearFlags = CameraClearFlags.SolidColor;

        CameraFollow camFollow = cam.gameObject.AddComponent<CameraFollow>();

        // Create player
        GameObject player = CreatePlayer();
        camFollow.target = player.transform;

        // Create GameManager
        GameObject gm = new GameObject("GameManager");
        gm.AddComponent<GameManager>();

        // Create map using sprites
        CreateMap();

        string scenePath = "Assets/Scenes/MainScene.unity";
        System.IO.Directory.CreateDirectory("Assets/Scenes");
        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene(), scenePath);

        Debug.Log("Scène créée!");
    }

    static GameObject CreatePlayer()
    {
        GameObject player = new GameObject("Player");
        player.tag = "Player";

        // Physics
        CircleCollider2D col = player.AddComponent<CircleCollider2D>();
        col.radius = 0.4f;

        Rigidbody2D rb = player.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.freezeRotation = true;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        // Sprite
        SpriteRenderer sr = player.AddComponent<SpriteRenderer>();
        sr.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Kenney/PNG/Survivor 1/survivor1_stand.png");
        sr.sortingOrder = 10;

        // Scripts
        player.AddComponent<PlayerMovement>();
        player.AddComponent<Shooting>();
        player.AddComponent<Health>();

        // Light Halo - bright white light
        GameObject lightObj = new GameObject("HaloLight");
        lightObj.transform.SetParent(player.transform);
        lightObj.transform.localPosition = new Vector3(0, 0, 5);

        Light haloLight = lightObj.AddComponent<Light>();
        haloLight.type = LightType.Point;
        haloLight.range = 8f;
        haloLight.intensity = 4f;
        haloLight.color = Color.white;
        haloLight.shadows = LightShadows.None;

        player.transform.position = Vector3.zero;
        return player;
    }

    static void CreateMap()
    {
        // Load floor tile
        Sprite floorSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Kenney/PNG/Tiles/tile_01.png");
        Sprite wallSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Kenney/PNG/Tiles/tile_03.png");
        Sprite boxSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Kenney/PNG/Tiles/tile_14.png");

        // Create floor tiles
        GameObject floorParent = new GameObject("Floor");
        for (int x = -15; x <= 15; x++)
        {
            for (int y = -15; y <= 15; y++)
            {
                GameObject tile = new GameObject("Tile_" + x + "_" + y);
                tile.transform.SetParent(floorParent.transform);
                tile.transform.position = new Vector3(x, 0, y);

                SpriteRenderer sr = tile.AddComponent<SpriteRenderer>();
                sr.sprite = floorSprite;
                sr.sortingOrder = 0;
                sr.color = new Color(0.3f, 0.3f, 0.3f);
            }
        }

        // Create walls
        GameObject wallParent = new GameObject("Walls");
        for (int x = -16; x <= 16; x++)
        {
            CreateWallTile(wallParent, wallSprite, x, 16);
            CreateWallTile(wallParent, wallSprite, x, -16);
        }
        for (int y = -16; y <= 16; y++)
        {
            CreateWallTile(wallParent, wallSprite, 16, y);
            CreateWallTile(wallParent, wallSprite, -16, y);
        }

        // Create random boxes
        GameObject boxParent = new GameObject("Boxes");
        for (int i = 0; i < 20; i++)
        {
            int bx = Random.Range(-12, 12);
            int by = Random.Range(-12, 12);

            GameObject box = new GameObject("Box_" + i);
            box.transform.SetParent(boxParent.transform);
            box.transform.position = new Vector3(bx, 0, by);

            SpriteRenderer sr = box.AddComponent<SpriteRenderer>();
            sr.sprite = boxSprite;
            sr.sortingOrder = 2;
            sr.color = new Color(0.5f, 0.3f, 0.1f);

            BoxCollider2D boxCol = box.AddComponent<BoxCollider2D>();
        }
    }

    static void CreateWallTile(GameObject parent, Sprite sprite, int x, int y)
    {
        GameObject wall = new GameObject("Wall_" + x + "_" + y);
        wall.transform.SetParent(parent.transform);
        wall.transform.position = new Vector3(x, 0, y);

        SpriteRenderer sr = wall.AddComponent<SpriteRenderer>();
        sr.sprite = sprite;
        sr.sortingOrder = 1;
        sr.color = new Color(0.2f, 0.2f, 0.2f);

        BoxCollider2D col = wall.AddComponent<BoxCollider2D>();
    }
}
