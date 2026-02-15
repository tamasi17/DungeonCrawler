using UnityEngine;
using UnityEngine.Events; // Needed for UnityEvents

public class Health : MonoBehaviour, IDamageable
{
    [Header("Settings")]
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;
    public bool isDead = false;

    [Header("Events")]
    // This allows us to drag-and-drop functions in the Inspector (like playing a sound or particle)
    public UnityEvent OnTakeDamage;
    public UnityEvent OnDie;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log($"{gameObject.name} took {amount} damage! HP: {currentHealth}");

        // Trigger the "Ouch" event (flashing red, sound, etc.)
        OnTakeDamage?.Invoke();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        // 1. Disable controls/animation so they stop moving
        GetComponent<PlayerAnimator>().enabled = false;

        // 2. Tell GameSession "I died"
        // (GameSession will increment the counter)
        GameSession.Instance.AddDeath();

        Debug.Log($"{gameObject.name} has died.");
        OnDie?.Invoke();

        
        if (GameSession.Instance.deaths < GameSession.Instance.maxDeaths)
        {
            Debug.Log("Death Count: " + GameSession.Instance.deaths + ". Reloading Level...");
            StartCoroutine(ReloadLevelRoutine());
        }
        else
        {
            Debug.Log("Death Limit Reached! Waiting for GameSession to switch scenes...");
        }
    }

    // Coroutine to wait a moment before resetting (so we see the death animation)
    private System.Collections.IEnumerator ReloadLevelRoutine()
    {
        yield return new WaitForSeconds(1.0f); // Wait 1 second

        // Reload the current scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }


}