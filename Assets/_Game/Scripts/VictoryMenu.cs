using TMPro; // Use UnityEngine.UI if not using TextMeshPro
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryMenu : MonoBehaviour
{
    [Header("UI Text References")]
    public TextMeshProUGUI timeText;   // Drag your "Time: 00s" text here
    public TextMeshProUGUI deathsText; // Drag your "Deaths: 0" text here
    public TextMeshProUGUI chestsText; // Drag your "Chests: 0/5" text here

    [Header("UI Objects")]
    public GameObject winObject;  // Drag the "WinText" object here
    public GameObject loseObject; // Drag the "LoseText" object here  // Optional: "Victory" or "Game Over"

    void Start()
    {

        Debug.Log("VictoryMenu: Start() called.");

        // 1. Safety Check
        if (GameSession.Instance == null)
        {
            Debug.LogWarning("No GameSession found! Stats will be empty.");
            return;
        }

        Debug.Log($"VictoryMenu: Found GameSession. Deaths={GameSession.Instance.deaths}, Time={GameSession.Instance.timePlayed}");

        // 2. Display Title (Green for Win, Red for Loss)
        if (GameSession.Instance.playerWon)
        {
            winObject.SetActive(true);
            loseObject.SetActive(false);
        }
        else
        {
            winObject.SetActive(false);
            loseObject.SetActive(true);
        }

        // 3. Display Time (e.g., "45.2s")
        if (timeText != null)
        {
            float t = GameSession.Instance.timePlayed;
            timeText.text = $"{t:F2}s";
        }

        // 4. Display Deaths (e.g., "3")
        if (deathsText != null)
        {
            deathsText.text = $"{GameSession.Instance.deaths}";
        }

        // 5. Display Chests (e.g., "2")
        if (chestsText != null)
        {
            int current = GameSession.Instance.chests;
            int total = GameSession.Instance.totalChestsInLevel;

            chestsText.text = $"{current}";
        }

    }


}