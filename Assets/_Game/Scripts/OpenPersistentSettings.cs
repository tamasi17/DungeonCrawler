using UnityEngine;

public class OpenPersistentSettings : MonoBehaviour
{
    public void OpenSettings()
    {
        // 1. Buscamos el objeto que NO se destruye. 
        // Cámbialo por el nombre exacto de tu objeto (ej: "PersistentUI" o "Canvas")
        GameObject persistentCanvas = GameObject.Find("PersistentCanvas");

        if (persistentCanvas != null)
        {
            // 2. Buscamos el panel de ajustes dentro de sus hijos
            // Si tu panel se llama "SettingsPanel", búscalo así:
            Transform settings = persistentCanvas.transform.Find("Settings");

            if (settings != null)
            {
                settings.gameObject.SetActive(true);
                Debug.Log("Abriendo ajustes del PersistentCanvas");
            }
            else
            {
                Debug.LogError("No se encontró el hijo 'Settings' dentro del PersistentCanvas");
            }
        }
        else
        {
            Debug.LogError("No se encontró el PersistentCanvas en esta escena.");
        }
    }
}