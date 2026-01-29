using UnityEngine;
using TMPro;

public class LanguageText : MonoBehaviour
{
    [TextArea] public string englishText;
    [TextArea] public string spanishText;
    private TextMeshProUGUI textLabel;

    void Start()
    {
        textLabel = GetComponent<TextMeshProUGUI>();
        UpdateLanguage();
    }

    // Call this from SettingsMenu.SetLanguage() if you want instant updates
    public void UpdateLanguage()
    {
        string lang = PlayerPrefs.GetString("Language", "English");
        textLabel.text = (lang == "Spanish") ? spanishText : englishText;
    }
}