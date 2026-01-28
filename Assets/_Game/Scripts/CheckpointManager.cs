using UnityEngine;

public static class CheckpointManager
{
    // "Static" means this variable belongs to the class, not a specific object.
    // It will survive when the scene reloads.
    public static Vector3 lastCheckPointPos;
    public static bool hasCheckpoint = false;

    public static void SetCheckpoint(Vector3 position)
    {
        lastCheckPointPos = position;
        hasCheckpoint = true;
        Debug.Log("Checkpoint Saved at: " + position);
    }
}