using UnityEngine;

public class MapShrink : MonoBehaviour
{
    public float initialHalfSize = 16f;
    public float shrinkAmount = 2f;
    public float minHalfSize = 8f;

    private float currentHalfSize;
    private BoxCollider2D col;

    void Start()
    {
        currentHalfSize = initialHalfSize;
        col = GetComponent<BoxCollider2D>();
        UpdateBoundary();
    }

    public void Shrink()
    {
        currentHalfSize -= shrinkAmount;

        if (currentHalfSize < minHalfSize)
            currentHalfSize = minHalfSize;

        UpdateBoundary();

        Debug.Log("Zone réduite: " + currentHalfSize + " (min: " + minHalfSize + ")");
    }

    void UpdateBoundary()
    {
        if (col == null) return;

        Vector2 pos = transform.position;
        string name = gameObject.name;

        if (name.Contains("Top"))
        {
            pos.y = currentHalfSize;
            col.size = new Vector2(currentHalfSize * 2 + 2, 1);
        }
        else if (name.Contains("Bottom"))
        {
            pos.y = -currentHalfSize;
            col.size = new Vector2(currentHalfSize * 2 + 2, 1);
        }
        else if (name.Contains("Left"))
        {
            pos.x = -currentHalfSize;
            col.size = new Vector2(1, currentHalfSize * 2);
        }
        else if (name.Contains("Right"))
        {
            pos.x = currentHalfSize;
            col.size = new Vector2(1, currentHalfSize * 2);
        }

        transform.position = pos;
    }
}
