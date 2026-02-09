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
        GetComponent<PlayerAnimator>().enabled = false;
        GameSession.Instance.AddDeath();
        Debug.Log($"{gameObject.name} has died.");
        OnDie?.Invoke();
    }
}