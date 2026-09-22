using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class PlayerSetup : EditorWindow
{
    [MenuItem("Tactical Darkness/Setup Full 2D Scene")]
    static void SetupFullScene()
    {
        EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);

        // REMOVE all lights
        Light[] lights = Object.FindObjectsOfType<Light>();
        foreach (Light l in lights)
            Object.DestroyImmediate(l.gameObject);

        // Camera 2D XY - no rotation
        Camera cam = Camera.main;
        cam.orthographic = true;
        cam.orthographicSize = 10f;
        cam.transform.position = new Vector3(0, 0, -10);
        cam.transform.rotation = Quaternion.identity;
        cam.backgroundColor = Color.black;
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.nearClipPlane = -10f;
        cam.farClipPlane = 10f;
        cam.orthographic = true;

        CameraFollow camFollow = cam.gameObject.AddComponent<CameraFollow>();

        // GameManager
        GameObject gm = new GameObject("GameManager");
        gm.AddComponent<GameManager>();

        // Map root
        GameObject mapRoot = new GameObject("Map");

        // Player
        GameObject player = CreateCombatant("Player", Vector2.zero, true);
        camFollow.target = player.transform;

        // Test target
        CreateCombatant("Target", new Vector2(5, 3), false);

        // Floor
        CreateFloor(mapRoot.transform);

        // Walls
        CreateWalls(mapRoot.transform);

        // Boxes
        CreateBoxes(mapRoot.transform);

        // Zone boundaries
        CreateBoundaries(mapRoot.transform);

        string scenePath = "Assets/Scenes/MainScene.unity";
        System.IO.Directory.CreateDirectory("Assets/Scenes");
        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene(), scenePath);

        Debug.Log("Scène 2D XY créée!");
    }

    static GameObject CreateCombatant(string name, Vector2 position, bool isPlayer)
    {
        // Root - physics only, never rotates
        GameObject root = new GameObject(name);
        root.layer = LayerMask.NameToLayer("Combatant");
        root.transform.position = new Vector3(position.x, position.y, 0);

        Rigidbody2D rb = root.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.freezeRotation = true;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        CircleCollider2D col = root.AddComponent<CircleCollider2D>();
        col.radius = 0.4f;
        col.isTrigger = false;

        // Visual child - rotates toward mouse
        GameObject visual = new GameObject("Visual");
        visual.transform.SetParent(root.transform);
        visual.transform.localPosition = Vector3.zero;
        SpriteRenderer sr = visual.AddComponent<SpriteRenderer>();
        sr.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Kenney/PNG/Survivor 1/survivor1_stand.png");
        sr.sortingLayerName = "Actors";
        sr.sortingOrder = 10;

        // FirePoint child
        GameObject firePoint = new GameObject("FirePoint");
        firePoint.transform.SetParent(root.transform);
        firePoint.transform.localPosition = new Vector3(0.8f, 0, 0);

        // Scripts
        PlayerMovement movement = root.AddComponent<PlayerMovement>();

        PlayerAim aim = root.AddComponent<PlayerAim>();
        aim.visual = visual.transform;
        aim.firePoint = firePoint.transform;

        if (isPlayer)
        {
            root.AddComponent<Shooting>();

            // DarknessOverlay
            DarknessOverlay fog = root.AddComponent<DarknessOverlay>();
            fog.haloRadius = 12f;
        }

        Health health = root.AddComponent<Health>();

        return root;
    }

    static void CreateFloor(Transform parent)
    {
        GameObject floor = new GameObject("Floor");
        floor.transform.SetParent(parent);
        floor.transform.position = Vector3.zero;

        Sprite floorSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Kenney/PNG/Tiles/tile_01.png");

        for (int x = -16; x <= 16; x++)
        {
            for (int y = -16; y <= 16; y++)
            {
                GameObject tile = new GameObject("Floor_" + x + "_" + y);
                tile.transform.SetParent(floor.transform);
                tile.transform.position = new Vector3(x, y, 0);
                tile.layer = LayerMask.NameToLayer("Default");

                SpriteRenderer sr = tile.AddComponent<SpriteRenderer>();
                sr.sprite = floorSprite;
                sr.sortingLayerName = "Ground";
                sr.sortingOrder = 0;
                sr.color = new Color(0.25f, 0.25f, 0.25f);
            }
        }
    }

    static void CreateWalls(Transform parent)
    {
        GameObject walls = new GameObject("Walls");
        walls.transform.SetParent(parent);

        Sprite wallSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Kenney/PNG/Tiles/tile_03.png");

        for (int x = -17; x <= 17; x++)
        {
            CreateWallTile(walls.transform, wallSprite, x, 17);
            CreateWallTile(walls.transform, wallSprite, x, -17);
        }
        for (int y = -16; y <= 16; y++)
        {
            CreateWallTile(walls.transform, wallSprite, 17, y);
            CreateWallTile(walls.transform, wallSprite, -17, y);
        }
    }

    static void CreateWallTile(Transform parent, Sprite sprite, int x, int y)
    {
        GameObject wall = new GameObject("Wall_" + x + "_" + y);
        wall.transform.SetParent(parent);
        wall.transform.position = new Vector3(x, y, 0);
        wall.layer = LayerMask.NameToLayer("Occluder");

        SpriteRenderer sr = wall.AddComponent<SpriteRenderer>();
        sr.sprite = sprite;
        sr.sortingLayerName = "World";
        sr.sortingOrder = 1;
        sr.color = new Color(0.35f, 0.35f, 0.35f);

        BoxCollider2D col = wall.AddComponent<BoxCollider2D>();
    }

    static void CreateBoxes(Transform parent)
    {
        GameObject boxes = new GameObject("Boxes");
        boxes.transform.SetParent(parent);

        Sprite boxSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Kenney/PNG/Tiles/tile_14.png");

        Vector2[] boxPositions = new Vector2[]
        {
            new Vector2(-8, -5), new Vector2(-3, 7), new Vector2(4, -8),
            new Vector2(9, 2), new Vector2(-12, 4), new Vector2(7, -11),
            new Vector2(-6, 10), new Vector2(11, -3), new Vector2(-10, -9),
            new Vector2(2, 12), new Vector2(-14, 0), new Vector2(13, 8),
            new Vector2(-4, -13), new Vector2(6, 6), new Vector2(-9, -2),
            new Vector2(0, -10), new Vector2(10, 10), new Vector2(-11, 11),
            new Vector2(5, -5), new Vector2(-7, 8)
        };

        for (int i = 0; i < boxPositions.Length; i++)
        {
            GameObject box = new GameObject("Box_" + i);
            box.transform.SetParent(boxes.transform);
            box.transform.position = new Vector3(boxPositions[i].x, boxPositions[i].y, 0);
            box.layer = LayerMask.NameToLayer("Occluder");

            SpriteRenderer sr = box.AddComponent<SpriteRenderer>();
            sr.sprite = boxSprite;
            sr.sortingLayerName = "World";
            sr.sortingOrder = 2;
            sr.color = new Color(0.6f, 0.4f, 0.2f);

            BoxCollider2D col = box.AddComponent<BoxCollider2D>();
        }
    }

    static void CreateBoundaries(Transform parent)
    {
        GameObject boundaries = new GameObject("Boundaries");
        boundaries.transform.SetParent(parent);

        float size = 34f;
        float half = size / 2f;

        CreateBoundary(boundaries.transform, "BoundaryTop", new Vector2(0, half), new Vector2(size, 1));
        CreateBoundary(boundaries.transform, "BoundaryBottom", new Vector2(0, -half), new Vector2(size, 1));
        CreateBoundary(boundaries.transform, "BoundaryLeft", new Vector2(-half, 0), new Vector2(1, size));
        CreateBoundary(boundaries.transform, "BoundaryRight", new Vector2(half, 0), new Vector2(1, size));
    }

    static void CreateBoundary(Transform parent, string name, Vector2 position, Vector2 size)
    {
        GameObject boundary = new GameObject(name);
        boundary.transform.SetParent(parent);
        boundary.transform.position = new Vector3(position.x, position.y, 0);
        boundary.layer = LayerMask.NameToLayer("Occluder");

        BoxCollider2D col = boundary.AddComponent<BoxCollider2D>();
        col.size = size;
        col.isTrigger = true;

        MapShrink ms = boundary.AddComponent<MapShrink>();
    }
}
