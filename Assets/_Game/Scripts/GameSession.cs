using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameSession : MonoBehaviour
{
    public static GameSession Instance;

    [Header("Game Settings")]
    public int maxDeaths = 5;
    public string endSceneName = "End";

    [Header("Live Stats")]
    public int chests = 0;
    public int deaths = 0;
    public float timePlayed = 0f;

    [Header("Session Info")]
    public int totalChestsInLevel = 0;

    [Header("Power Ups")]
    public float extraSpeed = 0f;
    public bool hasSpeedUpgrade = false;

    // State Flags
    public bool isTimerRunning = false;
    public bool playerWon = false; // True = Victory, False = Game Over

    // Tracks opened chests so they don't respawn
    public HashSet<string> collectedItems = new HashSet<string>();

    private void Awake()
    {
        // Singleton Pattern
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        // Only count time if the level is actually running
        if (isTimerRunning)
        {
            timePlayed += Time.deltaTime;
        }
    }

    // ==========================================
    //            PART 1: LEVEL LOGIC 
    // ==========================================

    public void StartLevel()
    {
        isTimerRunning = true;
        playerWon = false;

        // AUTO-COUNT CHESTS: Find how many chests exist when level starts
        // (Make sure your Chest script is actually named "Chest")
        Chest[] allChests = Object.FindObjectsByType<Chest>(FindObjectsSortMode.None);
        totalChestsInLevel = allChests.Length;

        Debug.Log("Game Started. Total Chests to find: " + totalChestsInLevel);
    }

    public void EndLevel(bool didWin)
    {
        isTimerRunning = false; // Stop the clock
        playerWon = didWin;     // Remember result

        // Load the End Screen immediately
        SceneManager.LoadScene(endSceneName);
    }

    public void AddDeath()
    {
        deaths++;
        Debug.Log("Deaths: " + deaths);

        // Check for Game Over (5 Deaths)
        if (deaths >= maxDeaths)
        {
            deaths = maxDeaths;

            Debug.Log("Max deaths reached! Game Over.");
            EndLevel(false); // False = You Lost
        }
    }

    // ==========================================
    //           PART 2: DATA & SAVES
    // ==========================================

    public void AddChest(int amount)
    {
        chests += amount;
    }

    // Called by Chests to see if they should be open or closed
    public bool HasCollected(string id)
    {
        return collectedItems.Contains(id);
    }

    // Called when you open a chest
    public void AddCollectedItem(string id)
    {
        if (!collectedItems.Contains(id))
        {
            collectedItems.Add(id);
            Debug.Log("Added item to memory: " + id);
        }
    }

    // Called by MainMenu or Settings to load the save file
    public void LoadDataFromSave(GameData data)
    {
        // 1. Load the stats
        chests = data.chests;
        deaths = data.deaths;
        timePlayed = data.timePlayed;

        // 2. Load the list of opened chests
        collectedItems.Clear();
        if (data.collectedItemsID != null)
        {
            foreach (string id in data.collectedItemsID)
            {
                collectedItems.Add(id);
            }
        }

        // 3. Load Player Upgrades (Speed)
        // Note: Using FindFirstObjectByType as requested
        PlayerAnimator player = FindFirstObjectByType<PlayerAnimator>();
        if (player != null && data.currentSprintSpeed > 0)
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
        playerWon = false;
        isTimerRunning = false;
        collectedItems.Clear();

        Debug.Log("Stats reset.");
    }
}