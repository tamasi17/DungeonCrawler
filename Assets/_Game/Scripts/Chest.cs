using UnityEngine;

public class Chest : MonoBehaviour
{
   
    [SerializeField] private string chestID;
    [SerializeField] private AudioClip openSound;
    private bool isOpened = false;

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
        if (other.CompareTag("Player") && !isOpened)
        {
            Interact();
        }
    }

    void Interact()
    {
        isOpened = true;

        if (GameSession.Instance != null)
        {
            GameSession.Instance.chests++; // Sumar al total
            GameSession.Instance.AddCollectedItem(chestID); // Recordar ESTE cofre específico
            Debug.Log($"Cofre {chestID} guardado. Destruyendo objeto: {gameObject.name}");
        }

        // Usamos el Singleton (AudioManager.instance) que es más seguro que buscar por nombre
        if (AudioManager.instance != null && AudioManager.instance.musicSource != null)
        {
            AudioManager.instance.musicSource.PlayOneShot(openSound);
        }
        else
        {
            // Plan B: Si el AudioManager no aparece, usamos PlayClipAtPoint 
            // para que al menos suene y no de error
            AudioSource.PlayClipAtPoint(openSound, transform.position);
        }

        this.gameObject.SetActive(false);
        Destroy(transform.root.gameObject);
    }
}