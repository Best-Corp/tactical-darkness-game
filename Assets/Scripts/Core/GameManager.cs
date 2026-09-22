using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int maxPlayers = 10;
    public int playersAlive;
    public float shrinkAmount = 1f;

    public float mapSize = 100f;
    private float currentMapSize;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        playersAlive = maxPlayers;
        currentMapSize = mapSize;
        Debug.Log("Game Started! Players alive: " + playersAlive);
    }

    public void PlayerDied()
    {
        playersAlive--;
        currentMapSize -= shrinkAmount;

        Debug.Log("Player Eliminated! " + playersAlive + " remaining");

        if (playersAlive <= 1)
        {
            Debug.Log("Game Over! Winner!");
        }
    }

    public float GetCurrentMapSize()
    {
        return currentMapSize;
    }
}
