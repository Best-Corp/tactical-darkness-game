using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class PlayerSetup : EditorWindow
{
    [MenuItem("Tactical Darkness/Setup Full 2D Scene")]
    static void SetupFullScene()
    {
        EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);

        // REMOVE ALL lights
        Light[] lights = Object.FindObjectsOfType<Light>();
        foreach (Light l in lights)
        {
            Object.DestroyImmediate(l.gameObject);
        }

        // Setup Camera
        Camera cam = Camera.main;
        cam.orthographic = true;
        cam.orthographicSize = 8f;
        cam.transform.position = new Vector3(0, 20, 0);
        cam.transform.rotation = Quaternion.Euler(90f, 0, 0);
        cam.backgroundColor = Color.black;
        cam.clearFlags = CameraClearFlags.SolidColor;

        CameraFollow camFollow = cam.gameObject.AddComponent<CameraFollow>();

        // Create player with halo
        GameObject player = CreatePlayer();
        camFollow.target = player.transform;

        // GameManager
        GameObject gm = new GameObject("GameManager");
        gm.AddComponent<GameManager>();

        // Map
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

        CircleCollider2D col = player.AddComponent<CircleCollider2D>();
        col.radius = 0.4f;

        Rigidbody2D rb = player.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.freezeRotation = true;

        SpriteRenderer sr = player.AddComponent<SpriteRenderer>();
        sr.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Kenney/PNG/Survivor 1/survivor1_stand.png");
        sr.sortingOrder = 10;

        player.AddComponent<PlayerMovement>();
        player.AddComponent<Shooting>();
        player.AddComponent<Health>();

        // === 360° LIGHT HALO ===
        GameObject lightObj = new GameObject("HaloLight");
        lightObj.transform.SetParent(player.transform);
        lightObj.transform.localPosition = Vector3.zero;

        Light haloLight = lightObj.AddComponent<Light>();
        haloLight.type = LightType.Point;
        haloLight.range = 12f;
        haloLight.intensity = 8f;
        haloLight.color = Color.white;
        haloLight.shadows = LightShadows.None;
        haloLight.renderMode = LightRenderMode.ForcePixel;

        player.transform.position = Vector3.zero;
        return player;
    }

    static void CreateMap()
    {
        Sprite floorSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Kenney/PNG/Tiles/tile_01.png");
        Sprite wallSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Kenney/PNG/Tiles/tile_03.png");
        Sprite boxSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Kenney/PNG/Tiles/tile_14.png");

        // Create Lit material for floor
        Material litMat = new Material(Shader.Find("Standard"));
        litMat.SetFloat("_Glossiness", 0);
        litMat.SetFloat("_Metallic", 0);

        // Floor
        GameObject floorParent = new GameObject("Floor");
        for (int x = -15; x <= 15; x++)
        {
            for (int y = -15; y <= 15; y++)
            {
                GameObject tile = new GameObject("Floor_" + x + "_" + y);
                tile.transform.SetParent(floorParent.transform);
                tile.transform.position = new Vector3(x, 0, y);

                SpriteRenderer sr = tile.AddComponent<SpriteRenderer>();
                sr.sprite = floorSprite;
                sr.sortingOrder = 0;
                sr.material = litMat;
            }
        }

        // Walls
        GameObject wallParent = new GameObject("Walls");
        for (int x = -16; x <= 16; x++)
        {
            CreateTile(wallParent, wallSprite, x, 16, new Color(0.3f, 0.3f, 0.3f), 1, litMat);
            CreateTile(wallParent, wallSprite, x, -16, new Color(0.3f, 0.3f, 0.3f), 1, litMat);
        }
        for (int y = -16; y <= 16; y++)
        {
            CreateTile(wallParent, wallSprite, 16, y, new Color(0.3f, 0.3f, 0.3f), 1, litMat);
            CreateTile(wallParent, wallSprite, -16, y, new Color(0.3f, 0.3f, 0.3f), 1, litMat);
        }

        // Boxes
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
            sr.material = litMat;

            BoxCollider2D boxCol = box.AddComponent<BoxCollider2D>();
        }
    }

    static void CreateTile(GameObject parent, Sprite sprite, int x, int y, Color color, int order, Material mat)
    {
        GameObject tile = new GameObject("Tile_" + x + "_" + y);
        tile.transform.SetParent(parent.transform);
        tile.transform.position = new Vector3(x, 0, y);

        SpriteRenderer sr = tile.AddComponent<SpriteRenderer>();
        sr.sprite = sprite;
        sr.sortingOrder = order;
        sr.color = color;
        sr.material = mat;

        BoxCollider2D col = tile.AddComponent<BoxCollider2D>();
    }
}
