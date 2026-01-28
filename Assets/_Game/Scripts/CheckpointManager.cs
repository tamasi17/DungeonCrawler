using UnityEngine;
using UnityEngine.SceneManagement; // Needed to get scene name

public static class CheckpointManager
{
    public static Vector3 lastCheckPointPos;
    public static string lastSceneName; // New: Remember which level this was in
    public static bool hasCheckpoint = false;

    public static void SetCheckpoint(Vector3 position)
    {
        lastCheckPointPos = position;
        lastSceneName = SceneManager.GetActiveScene().name; // Save "Level1"
        hasCheckpoint = true;

        Debug.Log($"Checkpoint Saved: {lastSceneName} at {position}");
    }
}