using UnityEngine;
using System; // Required for Actions/Events

public static class LocalizationManager
{
    // 0 = English, 1 = Spanish
    public static int CurrentLanguage = 0;

    // The Event: Listeners subscribe to this
    public static event Action OnLanguageChanged;

    public static void SetLanguage(int index)
    {
        CurrentLanguage = index;

        // Save the choice so it remembers next time you play
        PlayerPrefs.SetInt("SelectedLanguage", index);
        PlayerPrefs.Save();

        // Notify all text objects to update immediately
        OnLanguageChanged?.Invoke();

        Debug.Log("Language switched to: " + (index == 0 ? "English" : "Spanish"));
    }

    public static void LoadLanguage()
    {
        // Load saved language (Default to 0/English if not found)
        CurrentLanguage = PlayerPrefs.GetInt("SelectedLanguage", 0);

        // Notify everyone so the UI starts in the correct language
        OnLanguageChanged?.Invoke();
    }
}