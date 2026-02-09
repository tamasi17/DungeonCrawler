using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject settingsPanel;
    private bool isPaused = false;

    // Check if we are in the Main Menu (Scene Index 0)
    // If we are, we DON'T want to freeze time (so particles keep moving)
    private bool isMainMenu;

    private void Start()
    {
        // Assuming MainMenu is always Build Index 0
        isMainMenu = SceneManager.GetActiveScene().buildIndex == 0;

        if (settingsPanel != null)
            settingsPanel.SetActive(false);
    }

    void Update()
    {
        // Toggle Pause on ESC or P
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            if (isPaused) ResumeGame();
            else PauseGame();
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        settingsPanel.SetActive(true);

        // ONLY freeze time if this is the actual game, NOT the main menu
        if (!isMainMenu)
        {
            Time.timeScale = 0f; // Freezes physics and movement

        }
    }

    public void ResumeGame()
    {
        isPaused = false;
        settingsPanel.SetActive(false);

        if (!isMainMenu)
        {
            Time.timeScale = 1f; // Unfreezes
        }
    }

    // Hook this up to the "Close/Resume" button in the UI
    public void OnResumeClicked()
    {
        ResumeGame();
    }
}