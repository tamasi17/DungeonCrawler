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

        // 2. Load the first level
        SceneManager.LoadScene("Level1");
    }

    public void OnContinueClicked()
    {
        // Load the level we found in the file
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
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