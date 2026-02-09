using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerRespawn : MonoBehaviour
{
    private void Start()
    {
        // Get the name of the room we are currently standing in
        string currentScene = SceneManager.GetActiveScene().name;

        // CHECK: Do we have a save? AND Is it for this specific room?
        if (CheckpointManager.hasCheckpoint && CheckpointManager.lastSceneName == currentScene)
        {
            Health playerHealth = GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.isDead = false;
            }

            GetComponent<PlayerAnimator>().enabled = true;
            transform.position = CheckpointManager.lastCheckPointPos;
            Debug.Log("Restoring Player to Checkpoint.");
        }
        else
        {
            Debug.Log("No valid checkpoint for this level. Starting at default spawn.");
        }
    }
}