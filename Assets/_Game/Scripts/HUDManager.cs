using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class HUDManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private CanvasGroup hudGroup; // Drag HUD_Panel here!
    [SerializeField] private TextMeshProUGUI chestText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI deathText;

    [Header("Settings")]
    [SerializeField] private float displayDuration = 3f;

    // Internal State
    private float showTimer = 0f;
    private int lastChests = -1;
    private int lastDeaths = -1;

    private void Start()
    {
        // Initialize with current values so it doesn't pop up instantly on load
        if (GameSession.Instance != null)
        {
            lastChests = GameSession.Instance.chests;
            lastDeaths = GameSession.Instance.deaths;
        }
    }

    private void Update()
    {
        // 1. Hide in Main Menu
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            hudGroup.alpha = 0;
            return;
        }

        if (GameSession.Instance == null) return;

        // --- TRIGGER LOGIC ---

        // Check for Input ('I' Key)
        if (Input.GetKeyDown(KeyCode.I))
        {
            ShowHUD();
        }

        // Check for Data Changes (Chests)
        if (GameSession.Instance.chests != lastChests)
        {
            lastChests = GameSession.Instance.chests;
            ShowHUD();
        }

        // Check for Data Changes (Deaths)
        if (GameSession.Instance.deaths != lastDeaths)
        {
            lastDeaths = GameSession.Instance.deaths;
            ShowHUD();
        }

        // --- VISIBILITY LOGIC ---

        if (showTimer > 0)
        {
            // Count down
            showTimer -= Time.deltaTime;

            // Fade In (Optional polish: smooth lerp)
            hudGroup.alpha = 1;
        }
        else
        {
            // Fade Out
            // (You can change this to use Mathf.Lerp for a slow fade)
            hudGroup.alpha = 0;
        }

        // --- UPDATE TEXT (Always update so it's ready when shown) ---
        chestText.text = " " + GameSession.Instance.chests;
        deathText.text = " " + GameSession.Instance.deaths;

        float t = GameSession.Instance.timePlayed;
        string minutes = Mathf.Floor(t / 60).ToString("00");
        string seconds = (t % 60).ToString("00");
        timeText.text = $"{minutes}:{seconds}";
    }

    private void ShowHUD()
    {
        showTimer = displayDuration;
    }
}