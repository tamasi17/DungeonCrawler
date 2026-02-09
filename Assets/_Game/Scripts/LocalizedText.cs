using UnityEngine;
using TMPro;

public class LocalizedText : MonoBehaviour
{
    [TextArea] public string englishText; // Type in Inspector
    [TextArea] public string spanishText; // Type in Inspector

    private TextMeshProUGUI textComponent;

    private void Awake()
    {
        textComponent = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        // 1. Update text immediately when scene starts
        UpdateText();

        // 2. Subscribe to the event (Listen for changes)
        LocalizationManager.OnLanguageChanged += UpdateText;
    }

    private void OnDestroy()
    {
        // 3. Unsubscribe when object is destroyed (prevents errors)
        LocalizationManager.OnLanguageChanged -= UpdateText;
    }

    private void UpdateText()
    {
        if (textComponent == null) return;

        if (LocalizationManager.CurrentLanguage == 0)
        {
            textComponent.text = englishText;
        }
        else
        {
            textComponent.text = spanishText;
        }
    }
}