using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("The exact name of the scene to load next")]
    [SerializeField] private string nextSceneName = "Level2";

    // 1. Declare the variable so the script recognizes "fader"
    private SceneFader fader;

    private void Start()
    {
        // 2. Find the Fader automatically when the level starts
        // Note: Using FindFirstObjectByType is the modern Unity standard
        fader = FindFirstObjectByType<SceneFader>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            LoadNextLevel();
        }
    }

    private void LoadNextLevel()
    {
        // --- STEP 1: SAVE THE GAME ---
        // We save the *next* level name so if the player quits, 
        // they resume at the start of the new level.
        GameData data = new GameData();
        data.levelToLoad = nextSceneName;

        // Pull current stats from the Session
        data.chests = GameSession.Instance.chests;
        data.deaths = GameSession.Instance.deaths;
        data.timePlayed = GameSession.Instance.timePlayed;

        SaveSystem.SaveGame(data);
        Debug.Log("Game Saved. Transitioning to: " + nextSceneName);

        // --- STEP 2: TRANSITION ---
        // If we found the fader, use the smooth transition.
        if (fader != null)
        {
            fader.FadeToLevel(nextSceneName);
        }
        else
        {
            // Fallback: If no fader exists, just load the scene instantly
            SceneManager.LoadScene(nextSceneName);
        }
    }
}