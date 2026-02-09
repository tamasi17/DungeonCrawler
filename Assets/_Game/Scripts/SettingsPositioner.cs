using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsPositioner : MonoBehaviour
{
    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        // Whenever this menu opens, check which scene we are in
        if (SceneManager.GetActiveScene().buildIndex == 0) // Main Menu
        {
            MoveToSide();
        }
        else // In Game (Level 1, Level 2...)
        {
            MoveToCenter();
        }
    }

    private void MoveToSide()
    {
        // 1. Reset Position/Rotation just in case
        rectTransform.localRotation = Quaternion.identity;
        rectTransform.localScale = Vector3.one;

        // 2. Set Anchors to RIGHT side (Middle-Right)
        // Min(1, 0.5) and Max(1, 0.5) means "Stick to the right edge"
        rectTransform.anchorMin = new Vector2(1, -0.5f);
        rectTransform.anchorMax = new Vector2(1, -0.5f);
        rectTransform.pivot = new Vector2(1, 0.5f); // Pivot on the right edge

        // 3. Set Position (X = -50 means 50 pixels from the right edge)
        rectTransform.anchoredPosition = new Vector2(-50, 0);

        // Debug.Log("Settings moved to SIDE");
    }

    private void MoveToCenter()
    {
        // 1. Set Anchors to CENTER
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);

        // 2. Center it
        rectTransform.anchoredPosition = Vector2.zero;

        // Debug.Log("Settings moved to CENTER");
    }
}