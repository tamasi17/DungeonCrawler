using UnityEngine;

public class MainMenuSettingsFix : MonoBehaviour
{
    public void OpenGlobalSettings()
    {
        // 1. Buscamos el Canvas que sobrevivió
        GameObject pc = GameObject.Find("PersistentCanvas");

        if (pc != null)
        {
            
            Transform settings = pc.transform.Find("Settings");

            if (settings != null)
            {
                settings.gameObject.SetActive(true);
            }
            else
            {
                Debug.LogError("No encuentro el hijo 'SettingsPanel' en el PersistentCanvas");
            }
        }
        else
        {
            Debug.LogError("No hay PersistentCanvas en la escena.");
        }
    }
}