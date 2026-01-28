using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private bool isActivated = false;
    private Animator anim; // Reference to the Animator

    private void Awake()
    {
        anim = GetComponent<Animator>(); // Get the Animator component
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isActivated && other.CompareTag("Player"))
        {
            ActivateCheckpoint();
        }
    }

    private void ActivateCheckpoint()
    {
        isActivated = true;

        // Save Position
        CheckpointManager.SetCheckpoint(transform.position);

        // Play Animation
        if (anim != null)
        {
            anim.SetBool("IsActive", true); // Switches to the Waving state
        }
    }
}