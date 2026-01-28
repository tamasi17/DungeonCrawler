using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private Color activeColor = Color.green;
    private bool isActivated = false;
    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Only trigger if we haven't already, and it's the player
        if (!isActivated && other.CompareTag("Player"))
        {
            ActivateCheckpoint();
        }
    }

    private void ActivateCheckpoint()
    {
        isActivated = true;

        // Save the EXACT position of this object to the Manager
        CheckpointManager.SetCheckpoint(transform.position);

        // Visual Feedback (Turn Green)
        if (sr != null) sr.color = activeColor;
    }
}