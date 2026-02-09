using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    // Singleton: Easy access from anywhere (GameSession.Instance)
    public static GameSession Instance;

    [Header("Live Stats")]
    public int chests = 0;
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
        chests += amount;
    }

    public void AddDeath()
    {
        deaths++;
    }


    // Call this when you Load Game to restore saved stats
    public void LoadStats(int loadedChests, int loadedDeaths, float loadedTime)
    {
        chests = loadedChests;
        deaths = loadedDeaths;
        timePlayed = loadedTime;
    }

    public void ResetStats()
    {
        chests = 0;
        deaths = 0;
        timePlayed = 0f;

        Debug.Log("Game Session stats reset to zero.");
    }
}