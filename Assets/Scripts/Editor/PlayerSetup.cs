using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.Tilemaps;

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

        // Setup Camera - TOP DOWN
        Camera cam = Camera.main;
        cam.orthographic = true;
        cam.orthographicSize = 12f;
        cam.transform.position = new Vector3(0, 20, 0);
        cam.transform.rotation = Quaternion.Euler(90f, 0, 0);
        cam.backgroundColor = Color.black;
        cam.clearFlags = CameraClearFlags.SolidColor;

        CameraFollow camFollow = cam.gameObject.AddComponent<CameraFollow>();

        SetupPlayer(camFollow);
        SetupGameManager();
        SetupTilemapMap();

        string scenePath = "Assets/Scenes/MainScene.unity";
        System.IO.Directory.CreateDirectory("Assets/Scenes");
        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene(), scenePath);

        Debug.Log("Carte Tilemap créée!");
    }

    static void SetupPlayer(CameraFollow camFollow)
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

        GameObject lightObj = new GameObject("HaloLight");
        lightObj.transform.SetParent(player.transform);
        lightObj.transform.localPosition = new Vector3(0, 5, 0);

        Light haloLight = lightObj.AddComponent<Light>();
        haloLight.type = LightType.Point;
        haloLight.range = 10f;
        haloLight.intensity = 3f;
        haloLight.color = Color.white;
        haloLight.shadows = LightShadows.None;

        player.transform.position = new Vector3(0, 0, 0);

        if (camFollow != null)
            camFollow.target = player.transform;
    }

    static void SetupGameManager()
    {
        GameObject gm = new GameObject("GameManager");
        gm.AddComponent<GameManager>();
    }

    static void SetupTilemapMap()
    {
        // Create Tilemap Grid
        GameObject grid = new GameObject("Grid");
        Grid gridComp = grid.AddComponent<Grid>();
        gridComp.cellSize = new Vector3(1, 1, 1);

        // Ground Tilemap
        GameObject groundTilemap = new GameObject("GroundTilemap");
        groundTilemap.transform.SetParent(grid.transform);
        Tilemap groundMap = groundTilemap.AddComponent<Tilemap>();
        TilemapRenderer groundRenderer = groundTilemap.AddComponent<TilemapRenderer>();
        groundRenderer.sortingOrder = 0;
        groundTilemap.AddComponent<TilemapCollider2D>();

        // Walls Tilemap
        GameObject wallsTilemap = new GameObject("WallsTilemap");
        wallsTilemap.transform.SetParent(grid.transform);
        Tilemap wallsMap = wallsTilemap.AddComponent<Tilemap>();
        TilemapRenderer wallsRenderer = wallsTilemap.AddComponent<TilemapRenderer>();
        wallsRenderer.sortingOrder = 1;
        wallsTilemap.AddComponent<TilemapCollider2D>();

        // Load tiles
        TileBase floorTile = AssetDatabase.LoadAssetAtPath<TileBase>("Assets/Kenney/PNG/Tiles/tile_01.png");
        TileBase wallTile = AssetDatabase.LoadAssetAtPath<TileBase>("Assets/Kenney/PNG/Tiles/tile_03.png");
        TileBase wallCornerTile = AssetDatabase.LoadAssetAtPath<TileBase>("Assets/Kenney/PNG/Tiles/tile_04.png");

        if (floorTile == null || wallTile == null)
        {
            Debug.Log("Tiles non trouvés - utilisation de sprites simples");
            CreateSimpleMap(grid);
            return;
        }

        // Fill ground
        for (int x = -20; x <= 20; x++)
        {
            for (int y = -20; y <= 20; y++)
            {
                groundMap.SetTile(new Vector3Int(x, y, 0), floorTile);
            }
        }

        // Create walls
        for (int x = -20; x <= 20; x++)
        {
            wallsMap.SetTile(new Vector3Int(x, 20, 0), wallTile);
            wallsMap.SetTile(new Vector3Int(x, -20, 0), wallTile);
        }
        for (int y = -20; y <= 20; y++)
        {
            wallsMap.SetTile(new Vector3Int(20, y, 0), wallTile);
            wallsMap.SetTile(new Vector3Int(-20, y, 0), wallTile);
        }

        // Add some random obstacles
        for (int i = 0; i < 15; i++)
        {
            int x = Random.Range(-15, 15);
            int y = Random.Range(-15, 15);
            wallsMap.SetTile(new Vector3Int(x, y, 0), wallCornerTile);
        }

        Debug.Log("Tilemap Map created with Kenney tiles!");
    }

    static void CreateSimpleMap(GameObject grid)
    {
        // Fallback: use sprites on ground
        GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
        ground.name = "Ground";
        ground.transform.localScale = new Vector3(10, 1, 10);
        ground.transform.position = Vector3.zero;
        ground.AddComponent<MapShrink>();
        Object.DestroyImmediate(ground.GetComponent<BoxCollider>());
        ground.GetComponent<Renderer>().sharedMaterial.color = new Color(0.02f, 0.02f, 0.02f);

        CreateWall("WallNorth", new Vector3(0, 0.5f, 50), new Vector3(100, 1, 1));
        CreateWall("WallSouth", new Vector3(0, 0.5f, -50), new Vector3(100, 1, 1));
        CreateWall("WallEast", new Vector3(50, 0.5f, 0), new Vector3(1, 1, 100));
        CreateWall("WallWest", new Vector3(-50, 0.5f, 0), new Vector3(1, 1, 100));

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
