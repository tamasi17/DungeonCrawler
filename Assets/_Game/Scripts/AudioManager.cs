using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioMixer mainMixer;
    public AudioSource musicSource;
    private string currentSongName = "";

    [Header("Clips de Música")]
    public AudioClip introMusic;
    public AudioClip gameplayMusic;
    public AudioClip finalMusic;

    // --- VARIABLES DE ESTADO PARA EL TOGGLE ---
    private float lastMusicVol = 0.75f;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            // Nos aseguramos de que el AudioSource no empiece a lo loco
            if (musicSource != null) musicSource.playOnAwake = false;
        }
        else
        {
            // SI ENTRA AQUÍ, ES UN DUPLICADO. 
            // Lo silenciamos inmediatamente y lo destruimos.
            AudioSource duplicateSource = GetComponent<AudioSource>();
            if (duplicateSource != null) duplicateSource.Stop();

            Destroy(gameObject);
            return; // Salimos para no ejecutar nada más
        }
    }

    // --- MÉTODOS QUE PIDE EL AUDIOUICONTROLLER ---
    public void SetLastMusicVol(float val) => lastMusicVol = val;
    public float GetLastMusicVol() => lastMusicVol;

    // --- LÓGICA DE REPRODUCCIÓN ---

    public void PlayMusic(AudioClip clip)
    {
        if (clip == null) return;

        // Comparamos el nombre del clip nuevo con el que ya está sonando
        if (clip.name == currentSongName)
        {
            // Si es el mismo, salimos de la función sin tocar nada
            return;
        }

        // Si es diferente, actualizamos el nombre y reproducimos
        currentSongName = clip.name;
        musicSource.clip = clip;
        musicSource.Play();
    }

    // --- LÓGICA DE VOLUMEN ---
    public void UpdateMixerVolume(string parameter, float sliderValue)
    {
        // Asegúrate de que el valor nunca sea 0 absoluto para el logaritmo
        float clampedValue = Mathf.Clamp(sliderValue, 0.0001f, 1f);
        float dbValue = Mathf.Log10(clampedValue) * 20;

        // Log para depurar: si este mensaje sale en consola, el script está funcionando
        Debug.Log($"Intentando cambiar {parameter} a {dbValue} dB");

        bool result = mainMixer.SetFloat(parameter, dbValue);

        if (!result)
        {
            Debug.LogError($"Error: No se encontró el parámetro '{parameter}' en el Mixer.");
        }

        PlayerPrefs.SetFloat(parameter, sliderValue);
    }
}