using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameSession : MonoBehaviour
{
    // Singleton: Easy access from anywhere (GameSession.Instance)
    public static GameSession Instance;

    [Header("Live Stats")]
    public int chests = 0;
    public int deaths = 0;
    public float timePlayed = 0f;
    public bool isTimerRunning = false;

    public HashSet<string> collectedItems = new HashSet<string>();

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


    public bool HasCollected(string id)
    {
        Debug.Log("Collected item: " + id);
        return collectedItems.Contains(id);
    }

    public void AddCollectedItem(string id)
    {
        if (!collectedItems.Contains(id))
        {
            collectedItems.Add(id);
            Debug.Log("Added item: " + id);
        }
    }

    public void LoadDataFromSave(GameData data)
    {
        // 1. Load the stats
        chests = data.chests; // Make sure variable names match yours (chests vs chestsCollected)
        deaths = data.deaths;
        timePlayed = data.timePlayed;

        // 2. Load the list of opened chests (Fixes the "data not recognized" error)
        collectedItems.Clear();
        if (data.collectedItemsID != null)
        {
            foreach (string id in data.collectedItemsID)
            {
                collectedItems.Add(id);
            }
        }

        PlayerAnimator player = FindFirstObjectByType<PlayerAnimator>();
        if (player != null && data.currentSprintSpeed > 0) // Check > 0 so we don't load "0" by mistake
        {
            player.sprintSpeed = data.currentSprintSpeed;
        }

        Debug.Log("Game Session loaded successfully.");
    }

    public void ResetStats()
    {
        chests = 0;
        deaths = 0;
        timePlayed = 0f;

        Debug.Log("Game Session stats reset to zero.");
    }
}