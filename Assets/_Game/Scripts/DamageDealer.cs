using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [SerializeField] private int damageAmount = 20;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object we hit has the IDamageable rulebook
        IDamageable target = other.GetComponent<IDamageable>();

        if (target != null)
        {
            target.TakeDamage(damageAmount);
        }
    }
}