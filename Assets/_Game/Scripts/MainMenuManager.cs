using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button continueButton;
    private string sceneToLoad;

    private void Start()
    {
        // Try to load data
        GameData data = SaveSystem.LoadGame();

        if (data != null)
        {
            // Save exists! Enable button and remember the level
            continueButton.interactable = true;
            sceneToLoad = data.levelToLoad;
        }
        else
        {
            // No save file? Gray out the button
            continueButton.interactable = false;
        }
    }

    public void OnNewGameClicked()
    {
        // 1. Delete old save (optional, but cleaner)
        SaveSystem.DeleteSave();

        // 1. Check if a session already exists from a previous game
        if (GameSession.Instance != null)
        {
            // 2. WIPE IT CLEAN! 
            // This sets deaths to 0, chests to 0, time to 0.
            GameSession.Instance.ResetStats();
        }

        // 2. Load the first level
        SceneManager.LoadScene("Level1");
    }

    public void OnContinueClicked()
    {
        GameData data = SaveSystem.LoadGame();
        if (data != null)
        {
            if (GameSession.Instance != null)
            {
                // Restore the session stats (Coins, Deaths, AND Collected Chests)
                // Make sure this name matches the one in GameSession.cs!
                GameSession.Instance.LoadDataFromSave(data);
            }

            SceneManager.LoadScene(data.levelToLoad);
        }
    }

    public void OnSettingsClicked()
    {
        Debug.Log("Open Settings Menu");
        // TODO: Settings Panel later
    }

    public void OnExitClicked()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }
}