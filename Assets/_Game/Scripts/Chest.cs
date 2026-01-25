using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    // TODO: Add chest opening animation, sound effects, and item spawning


    public void Interact()
    {
        Debug.Log("Open Chest!");

        // Visual feedback: Disable the sprite to "collect" it (temporary test)
        gameObject.SetActive(false);
    }
}