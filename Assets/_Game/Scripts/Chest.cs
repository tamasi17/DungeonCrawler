using UnityEngine;

public class Chest : MonoBehaviour
{
   
    [SerializeField] private string chestID;
    [SerializeField] private AudioClip openSound;

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

       
        if (openSound != null)
        {
            GameObject.Find("AudioManager").GetComponent<AudioSource>().PlayOneShot(openSound);
        }

        // Sonido, partículas...
        Destroy(gameObject);
    }
}