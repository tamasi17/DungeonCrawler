using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    private void Start()
    {
        // Check if we have a saved checkpoint in memory
        if (CheckpointManager.hasCheckpoint)
        {
            // Teleport player instantly
            transform.position = CheckpointManager.lastCheckPointPos;
            Debug.Log("Player respawned at Checkpoint.");
        }
    }
}