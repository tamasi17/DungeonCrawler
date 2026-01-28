using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; // Needed to restart the level

public class CrusherTrap : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float crushSpeed = 10f;    // Very fast
    [SerializeField] private float resetSpeed = 2f;     // Slow return
    [SerializeField] private float crushDistance = 3f;  // How far it moves right
    [SerializeField] private float waitTime = 1f;       // Delay between smashes

    [Header("Damage")]
    [SerializeField] private int damageAmount = 999;    // Instant Kill

    private Vector3 startPos;
    private Vector3 targetPos;

    private void Start()
    {
        startPos = transform.position;
        // Calculate where the rock ends up (Current + Right * Distance)
        targetPos = startPos + (Vector3.right * crushDistance);

        // Start the endless cycle
        StartCoroutine(CrushRoutine());
    }

    private IEnumerator CrushRoutine()
    {
        while (true) // Run forever
        {
            // 1. Smash Forward (Move towards target)
            while (Vector3.Distance(transform.position, targetPos) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPos, crushSpeed * Time.deltaTime);
                yield return null; // Wait for next frame
            }

            // 2. Wait at the end (Optional, keeps it pinned for a moment)
            yield return new WaitForSeconds(0.5f);

            // 3. Reset (Move back to start)
            while (Vector3.Distance(transform.position, startPos) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, startPos, resetSpeed * Time.deltaTime);
                yield return null;
            }

            // 4. Wait before smashing again
            yield return new WaitForSeconds(waitTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // If it hits the player
        if (other.CompareTag("Player"))
        {
            IDamageable playerHealth = other.GetComponent<IDamageable>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);

                // Temporary: Since we don't have checkpoints, restart scene after 1 second
                // (Or handle this inside your Player Death logic)
                Invoke("RestartLevel", 1f);
            }
        }
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}