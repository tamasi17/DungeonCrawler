using UnityEngine;

public class SpeedPowerUp : MonoBehaviour
{
    [Header("Settings")]
    public float speedIncrease = 5.0f;
    public float freezeDuration = 1.0f;// How much faster? (e.g., from 8 to 10)

    [Header("Visuals")]
    public GameObject pickupEffect; // Optional: Particle system to spawn

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerAnimator controller = other.GetComponent<PlayerAnimator>();

            if (controller != null)
            {
                // 1. Apply upgrade
                controller.UpgradeSpeed(speedIncrease);

                controller.LockMovement(freezeDuration);

                // 2. VISUALS FIX: Use 'other.transform.position' (The Player)
                if (pickupEffect != null)
                {
                    // Optional: Add a tiny offset (-0.5f on Y) so it spawns exactly at the feet
                    Vector3 feetPos = other.transform.position + new Vector3(0, -0.8f, 0);

                    // Instantiate at the PLAYER'S feet
                    Instantiate(pickupEffect, feetPos, Quaternion.identity);
                }

                // 3. Destroy powerup
                Destroy(gameObject);
            }
        }
    }
}