using UnityEngine;

public class WinZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Tell GameSession we won!
            if (GameSession.Instance != null)
            {
                GameSession.Instance.EndLevel(true); // true = Victory
            }
        }
    }
}