using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    // Singleton: Easy access from anywhere (GameSession.Instance)
    public static GameSession Instance;

    [Header("Live Stats")]
    public int chest = 0;
    public int deaths = 0;
    public float timePlayed = 0f;
    public bool isTimerRunning = false;

    private void Awake()
    {
        // Singleton Pattern (Destroy duplicate if one exists)
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Keep me alive across levels!
    }

    private void Update()
    {
        // Only count time if we are NOT in the Main Menu
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            timePlayed += Time.deltaTime;
        }
    }

    // --- Helper Methods ---
   

    public void AddChest(int amount)
    {
        chest += amount;
    }

    public void AddDeath()
    {
        deaths++;
    }

    // Call this when you Load Game to restore saved stats
    public void LoadStats(int loadedChests, int loadedDeaths, float loadedTime)
    {
        chest = loadedChests;
        deaths = loadedDeaths;
        timePlayed = loadedTime;
    }
}