using UnityEngine;

public class StaminaPowerUp : MonoBehaviour
{
    [SerializeField] private float extraTimeAmount = 2.0f; // Add 2 seconds to sprint

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {


            // 1. Find the system
            PlayerStamina stamina = other.GetComponent<PlayerStamina>();

            if (stamina != null)
            {
                // 2. Apply upgrade
                stamina.UpgradeStamina(extraTimeAmount);

                // 3. Feedback & Destroy
                // AudioSource.PlayClipAtPoint(pickupSound, transform.position); 
                Destroy(gameObject);
            }
        }
    }
}