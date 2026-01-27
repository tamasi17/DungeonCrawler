using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Button loadGameButton;

    private void Start()
    {
        // Check if we have a saved game. 
        // We haven't built the Save System yet, so this checks a temporary key.
        // If "PlayerLevel" exists, we assume a save exists.
        bool hasSave = PlayerPrefs.HasKey("PlayerLevel");

        // Disable the Continue button if no save exists
        loadGameButton.interactable = hasSave;
    }

    public void OnNewGameClicked()
    {
        // Optional: Clear old saves here using PlayerPrefs.DeleteAll();
        // Load the actual gameplay scene (Index 1)
        SceneManager.LoadScene(1);
    }

    public void OnContinueClicked()
    {
        // Load the game scene without deleting data
        SceneManager.LoadScene(1);
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