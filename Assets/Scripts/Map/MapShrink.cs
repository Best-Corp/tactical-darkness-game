using UnityEngine;

public class MapShrink : MonoBehaviour
{
    public float shrinkSpeed = 1f;
    public float minSize = 10f;

    private Vector3 originalScale;
    private float targetSize;

    void Start()
    {
        originalScale = transform.localScale;
        targetSize = GameManager.Instance.mapSize;
    }

    void Update()
    {
        if (GameManager.Instance != null)
        {
            float currentMapSize = GameManager.Instance.GetCurrentMapSize();

            if (currentMapSize < targetSize)
            {
                targetSize = currentMapSize;
            }
        }

        if (transform.localScale.x > targetSize / 100f)
        {
            float newSize = Mathf.Lerp(transform.localScale.x, targetSize / 100f, shrinkSpeed * Time.deltaTime);
            transform.localScale = new Vector3(newSize, originalScale.y, newSize);
        }
    }
}
