using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro; // For Dropdown
using System.Collections.Generic;

public class SettingsMenu : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider musicSlider, sfxSlider;
    [SerializeField] private Toggle musicToggle, sfxToggle; // Added Toggles

    [Header("Resolution")]
    [SerializeField] private TMP_Dropdown resDropdown;

    [Header("Language")]
    [SerializeField] private TMP_Dropdown langDropdown; // Added Language Dropdown

    private void Start()
    {
        SetupResolutions();
        // SetupLanguage(); // Call this if you want to auto-select current language
    }

    // --- AUDIO LOGIC ---
    public void SetMusicVolume(float volume)
    {
        // Convert 0.001-1 to -80dB to 0dB
        float db = Mathf.Log10(volume) * 20;
        if (volume == 0) db = -80;
        audioMixer.SetFloat("MusicVol", db);
    }

    public void SetSFXVolume(float volume)
    {
        float db = Mathf.Log10(volume) * 20;
        if (volume == 0) db = -80;
        audioMixer.SetFloat("SFXVol", db);
    }

    public void ToggleMusic(bool isMuted)
    {
        // If Toggle is Checked (Muted) -> Volume 0. 
        // If Unchecked (Sound On) -> Restore slider value.
        SetMusicVolume(isMuted ? 0 : musicSlider.value);
    }

    public void ToggleSFX(bool isMuted)
    {
        SetSFXVolume(isMuted ? 0 : sfxSlider.value);
    }

    // --- RESOLUTION LOGIC ---
    private void SetupResolutions()
    {
        resDropdown.ClearOptions();
        List<string> options = new List<string> { "1920 x 1080", "3840 x 2160", "2880 x 1800" };
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
    }

    // --- LANGUAGE LOGIC ---
    public void SetLanguage(int index)
    {
        // Index 0 = English, Index 1 = Spanish
        string lang = index == 0 ? "English" : "Spanish";
        PlayerPrefs.SetString("Language", lang);

        // Find all language text objects and update them immediately
        // (Assuming you used the LanguageText script from before)
        LanguageText[] allTexts = FindObjectsByType<LanguageText>(FindObjectsSortMode.None);
        foreach (LanguageText txt in allTexts)
        {
            txt.UpdateLanguage();
        }
    }

    // --- EXIT / CLOSE LOGIC ---
    public void CloseSettings()
    {
        // 1. Try to find the PauseManager (Level Logic)
        PauseManager pm = FindFirstObjectByType<PauseManager>();

        if (pm != null)
        {
            // If we are in-game, let the Manager handle unpausing/closing
            pm.ResumeGame();
        }
        else
        {
            // 2. If no Manager found (Main Menu), just hide this panel
            gameObject.SetActive(false);
        }
    }

    // --- SAVE LOGIC ---
    public void SaveGame()
    {
        GameData data = new GameData();
        data.levelToLoad = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        SaveSystem.SaveGame(data);
        Debug.Log("Game Saved manually.");
    }
}