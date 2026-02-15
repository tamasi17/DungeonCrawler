using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SettingsMenu : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider musicSlider, sfxSlider;
    // [SerializeField] private Toggle musicToggle, sfxToggle; // Optional if you want mute buttons

    [Header("Resolution")]
    [SerializeField] private TMP_Dropdown resDropdown;

    [Header("Language")]
    [SerializeField] private TMP_Dropdown langDropdown;
    [SerializeField] private GameObject musicLabel_EN;
    [SerializeField] private GameObject musicLabel_ES;
    [SerializeField] private GameObject sfxLabel_EN;
    [SerializeField] private GameObject sfxLabel_ES;

    [Header("Navigation")]
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button saveButton;
    [SerializeField] private Button loadButton;
    [SerializeField] private Button deleteSaveButton;

    private void OnEnable()
    {

        // 2. Refresh Resolution (optional, but good practice)
        if (resDropdown != null) resDropdown.SetValueWithoutNotify(GetCurrentResolutionIndex());
        resDropdown.RefreshShownValue();

        // 3. THE FIX: Refresh Buttons based on the CURRENT scene
        SetupButtons();
    }

    private void Start()
    {
        // 1. Setup Language
        LocalizationManager.LoadLanguage();
        langDropdown.SetValueWithoutNotify(LocalizationManager.CurrentLanguage);

        // 2. Setup Resolution
        SetupResolutions();

        // 3. Initialize Audio Sliders (So they don't snap to 0 when menu opens)
        // We assume the slider goes from 0.001 to 1
        float currentMusicVol;
        audioMixer.GetFloat("MusicVol", out currentMusicVol);
        musicSlider.value = Mathf.Pow(10, currentMusicVol / 20); // Convert dB back to linear

        float currentSFXVol;
        audioMixer.GetFloat("SFXVol", out currentSFXVol);
        sfxSlider.value = Mathf.Pow(10, currentSFXVol / 20);

        // 4. Setup Buttons based on Scene
        SetupButtons();
    }

    private void SetupButtons()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        bool isMainMenu = (currentScene == "MainMenu");

        if (mainMenuButton) mainMenuButton.interactable = !isMainMenu;
        if (saveButton) saveButton.interactable = !isMainMenu; // Can't save in menu

        bool hasSave = SaveSystem.SaveFileExists();
        if (loadButton) loadButton.interactable = hasSave;
        if (deleteSaveButton) deleteSaveButton.interactable = hasSave;
    }




    // --- AUDIO LOGIC ---
    public void SetMusicVolume(float volume)
    {
        // Fix: Check for 0 BEFORE math to avoid errors
        if (volume <= 0.001f)
        {
            audioMixer.SetFloat("MusicVol", -80);
        }
        else
        {
            float db = Mathf.Log10(volume) * 20;
            audioMixer.SetFloat("MusicVol", db);
        }
    }

    public void SetSFXVolume(float volume)
    {
        if (volume <= 0.001f)
        {
            audioMixer.SetFloat("SFXVol", -80);
        }
        else
        {
            float db = Mathf.Log10(volume) * 20;
            audioMixer.SetFloat("SFXVol", db);
        }
    }

    // --- RESOLUTION LOGIC ---
    private void SetupResolutions()
    {
        resDropdown.ClearOptions();
        List<string> options = new List<string> { "1920x1080", "4k", "2880x1800"};
        resDropdown.AddOptions(options);
    }

    public void SetResolution(int index)
    {
        switch (index)
        {
            case 0: Screen.SetResolution(1920, 1080, true); break;
            case 1: Screen.SetResolution(3840, 2160, true); break;
            case 2: Screen.SetResolution(2880, 1800, true); break;
        }

        Debug.Log($"Game Window is now: {Screen.width}x{Screen.height}");
    }

    private int GetCurrentResolutionIndex()
    {
        // Check width to decide which index matches
        // 0 = 1920x1080
        // 1 = 3840x2160 (4k)
        // 2 = 2880x1800

        int width = Screen.width;
        int height = Screen.height;

        // Strict check (Width AND Height must match)
        if (width == 1920 && height == 1080) return 0;
        if (width == 3840 && height == 2160) return 1;
        if (width == 2880 && height == 1800) return 2;

        // Fallback: If the window is some weird custom size, default to 1080p (0)
        return 0;
    }

    // --- LANGUAGE LOGIC ---
    public void SetLanguage(int index)
    {
        LocalizationManager.SetLanguage(index);
    }

    // --- SAVE / LOAD / EXIT ---
    public void SaveGame()
    {
        // 1. Create Data Container
        GameData data = new GameData();
        data.levelToLoad = SceneManager.GetActiveScene().name;

        // 2. Pull Stats from the "Brain" (GameSession)
        if (GameSession.Instance != null)
        {
            data.chests = GameSession.Instance.chests; 
            data.deaths = GameSession.Instance.deaths;
            data.timePlayed = GameSession.Instance.timePlayed;
            data.collectedItemsID = new List<string>(GameSession.Instance.collectedItems);
        }

        PlayerAnimator player = FindFirstObjectByType<PlayerAnimator>();

        if (player != null)
        {
            data.currentSprintSpeed = player.sprintSpeed;
        }
        else
        {
            
            data.currentSprintSpeed = 10f; 
        }

        // 3. Write to File
        SaveSystem.SaveGame(data);
        Debug.Log("Game Saved!");
    }

    public void LoadGame()
    {
        GameData data = SaveSystem.LoadGame();
        if (data != null)
        {
       
            if (GameSession.Instance != null)
            {
                GameSession.Instance.LoadDataFromSave(data);
            }

            SceneManager.LoadScene(data.levelToLoad);
        }
    }

public void DeleteGame()
{
        SaveSystem.DeleteSave();

        // Actualizar botones
        if (loadButton != null) loadButton.interactable = false;
        if (deleteSaveButton != null) deleteSaveButton.interactable = false;

        if (GameSession.Instance != null)
        {
            GameSession.Instance.ResetStats();
        }

        // ARREGLO 3: Si estás jugando, VETE al menú principal
        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            GoToMainMenu();
        }
    }

public void GoToMainMenu()
    {
        Time.timeScale = 1f; // IMPORTANT: Unpause time before leaving!
        SceneManager.LoadScene("MainMenu"); // Make sure your menu scene is named exactly this
    }

    public void CloseSettings()
    {
        // 1. Check if a PauseManager exists (Are we in a level?)
        // (Note: Use FindObjectOfType for older Unity, FindFirstObjectByType for Unity 2023+)
        PauseManager pm = Object.FindFirstObjectByType<PauseManager>();

        if (pm != null)
        {
            // CASE A: We are In-Game
            // Let the PauseManager handle the resume (resetting time, locking cursor, etc.)
            pm.ResumeGame();
        }
        else
        {
            // CASE B: We are in the Main Menu
            // Just hide this settings panel. No time manipulation needed.
            gameObject.SetActive(false);
        }
    }
}