using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float interactRange = 1.5f;
    [SerializeField] private LayerMask interactLayer;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryInteract();
        }
    }

    private void TryInteract()
    {
        // 1. Create a small circle around the player to check for objects
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, interactRange, interactLayer);

        foreach (Collider2D obj in hitColliders)
        {
            // 2. Check if the object has the "IInteractable" rulebook
            IInteractable interactable = obj.GetComponent<IInteractable>();

            if (interactable != null)
            {
                // 3. Trigger the interaction and stop looking
                interactable.Interact();
                return;
            }
        }
    }

    // Draw a helper circle in the Editor so we can see the range
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }
}