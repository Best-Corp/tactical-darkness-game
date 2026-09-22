using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    public int maxPlayers = 10;
    public int playersAlive;
    public float shrinkAmount = 1f;
    
    public TextMeshProUGUI playersAliveText;
    public TextMeshProUGUI announcementText;
    
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
        UpdateUI();
        ShowAnnouncement("Game Started!");
    }

    public void PlayerDied()
    {
        playersAlive--;
        currentMapSize -= shrinkAmount;
        
        UpdateUI();
        ShowAnnouncement($"Player Eliminated! {playersAlive} remaining");
        
        if (playersAlive <= 1)
        {
            ShowAnnouncement("Game Over!");
        }
    }

    void UpdateUI()
    {
        if (playersAliveText != null)
            playersAliveText.text = $"Players Alive: {playersAlive}";
    }

    void ShowAnnouncement(string message)
    {
        if (announcementText != null)
        {
            announcementText.text = message;
            announcementText.gameObject.SetActive(true);
            CancelInvoke(nameof(HideAnnouncement));
            Invoke(nameof(HideAnnouncement), 3f);
        }
    }

    void HideAnnouncement()
    {
        if (announcementText != null)
            announcementText.gameObject.SetActive(false);
    }

    public float GetCurrentMapSize()
    {
        return currentMapSize;
    }
}
