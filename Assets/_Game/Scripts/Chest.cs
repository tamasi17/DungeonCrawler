using UnityEngine;

public class Chest : MonoBehaviour
{
    // Escribe un nombre ÚNICO en el inspector para cada cofre (ej: "Level1_Chest_Hidden")
    [SerializeField] private string chestID;

    private void Start()
    {
        // Generar ID automático si se te olvida ponerlo (usando su posición)
        if (string.IsNullOrEmpty(chestID))
        {
            chestID = transform.position.ToString();
        }

        // PREGUNTA: ¿Ya me abrieron antes?
        if (GameSession.Instance != null)
        {
            if (GameSession.Instance.HasCollected(chestID))
            {
                // Si ya me abrieron, desaparezco antes de que el jugador me vea
                Destroy(gameObject);
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Interact();
        }
    }

    void Interact()
    {

        if (GameSession.Instance != null)
        {
            GameSession.Instance.chests++; // Sumar al total
            GameSession.Instance.AddCollectedItem(chestID); // Recordar ESTE cofre específico
            Debug.Log("Open Chest!");
        }

        // Sonido, partículas...
        Destroy(gameObject);
    }
}