using UnityEngine;
using UnityEngine.UI;

public class AudioUIController : MonoBehaviour
{
    public Slider musicSlider, sfxSlider;
    public Toggle musicToggle, sfxToggle;

    private float lastMusicVol = 0.75f;

    [Header("Feedback de Sonido")]
    public AudioSource sfxPreviewSource; // Un AudioSource que solo use el canal SFX
    public AudioClip sfxTestClip;        // El ruidito de prueba

    private float lastSoundTime;
    private float soundCooldown = 0.03f; // Para que no sature el oído

    public void SetLastMusicVol(float val)
    {
        lastMusicVol = val;
    }

    public float GetLastMusicVol()
    {
        return lastMusicVol;
    }

    void Start()
    {
        // Recuperar valores guardados o usar 0.75 como default
        float savedMusic = PlayerPrefs.GetFloat("MusicVol", 0.75f);
        float savedSFX = PlayerPrefs.GetFloat("SFXVol", 0.75f);

        // Sincronizar Sliders
        musicSlider.value = savedMusic;
        sfxSlider.value = savedSFX;

        // Aplicar al Mixer inmediatamente al arrancar
        AudioManager.instance.UpdateMixerVolume("MusicVol", savedMusic);
        AudioManager.instance.UpdateMixerVolume("SFXVol", savedSFX);
    }

    public void OnMusicSliderChanged(float value)
    {
        AudioManager.instance.UpdateMixerVolume("MusicVol", value);
        if (value > 0.0001f) AudioManager.instance.SetLastMusicVol(value);
    }

    public void OnSFXSliderChanged(float value)
    {
        AudioManager.instance.UpdateMixerVolume("SFXVol", value);

        // Feedback auditivo: solo suena si ha pasado un pequeño tiempo
        if (Time.unscaledTime - lastSoundTime > soundCooldown)
        {
            if (sfxPreviewSource != null && sfxTestClip != null)
            {
                sfxPreviewSource.PlayOneShot(sfxTestClip);
                lastSoundTime = Time.unscaledTime;
            }
        }
    }

    public void OnMusicToggleChanged(bool isOn)
    {
        if (isOn)
        {
            // Restaurar al valor que estaba antes de silenciar
            float val = AudioManager.instance.GetLastMusicVol();
            musicSlider.value = val;
            AudioManager.instance.UpdateMixerVolume("MusicVol", val);
        }
        else
        {
            // Silencio total pero recordamos la posición del slider
            musicSlider.value = 0;
            AudioManager.instance.UpdateMixerVolume("MusicVol", 0);
        }
    }

    public void OnSFXToggleChanged(bool isOn)
    {
        if (isOn)
        {
            // Restauramos el volumen de SFX (puedes usar un valor fijo o el del slider)
            float val = sfxSlider.value;
            if (val <= 0.0001f) val = 0.75f; // Valor de rescate si el slider estaba a cero

            sfxSlider.value = val;
            AudioManager.instance.UpdateMixerVolume("SFXVol", val);
        }
        else
        {
            // Silencio total de efectos
            sfxSlider.value = 0;
            AudioManager.instance.UpdateMixerVolume("SFXVol", 0);
        }
    }
}