using UnityEngine;
using UnityEngine.UI;

public class AudioUIController : MonoBehaviour
{
    public Slider musicSlider, sfxSlider;
    public Toggle musicToggle, sfxToggle;

    private float lastMusicVol = 0.75f;
    private float lastSFXVol = 0.75f;

    [Header("Feedback de Sonido")]
    public AudioSource sfxPreviewSource; 
    public AudioClip sfxTestClip;        

    private float lastSoundTime;
    private float soundCooldown = 0.1f; // Para que no sature el oído

    private bool isUpdatingUI = false;

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
        float savedMusic = PlayerPrefs.GetFloat("MusicVol", 0.75f);
        float savedSFX = PlayerPrefs.GetFloat("SFXVol", 0.75f);
        lastMusicVol = PlayerPrefs.GetFloat("MusicVol", 0.75f);
        lastSFXVol = PlayerPrefs.GetFloat("SFXVol", 0.75f);

        // Si el volumen guardado es 0, el toggle debería empezar apagado
        isUpdatingUI = true;
        musicSlider.value = lastMusicVol;
        sfxSlider.value = lastSFXVol;
        musicToggle.isOn = lastMusicVol > 0.001f;
        sfxToggle.isOn = lastSFXVol > 0.001f;
        isUpdatingUI = false;

        // Aplicar al Mixer
        AudioManager.instance.UpdateMixerVolume("MusicVol", lastMusicVol);
        AudioManager.instance.UpdateMixerVolume("SFXVol", lastSFXVol);
    }

    public void OnMusicSliderChanged(float value)
    {
        if (isUpdatingUI) return;

        AudioManager.instance.UpdateMixerVolume("MusicVol", value);

        // Si movemos el slider manualmente, actualizamos el Toggle
        isUpdatingUI = true;
        musicToggle.isOn = value > 0.001f;
        if (value > 0.001f) lastMusicVol = value;
        isUpdatingUI = false;
    }

    public void OnSFXSliderChanged(float value)
    {
        if (isUpdatingUI) return;

        AudioManager.instance.UpdateMixerVolume("SFXVol", value);

        // Si movemos el slider, actualizamos el Toggle
        isUpdatingUI = true;
        sfxToggle.isOn = value > 0.001f;
        if (value > 0.001f) lastSFXVol = value;
        isUpdatingUI = false;

        // Feedback sonoro
        if (Time.unscaledTime - lastSoundTime > soundCooldown && value > 0.001f)
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
        if (isUpdatingUI) return;

        isUpdatingUI = true;
        if (isOn)
        {
            // Si encendemos y el valor era 0, rescatamos a 0.75
            if (lastMusicVol <= 0.001f) lastMusicVol = 0.75f;
            musicSlider.value = lastMusicVol;
        }
        else
        {
            musicSlider.value = 0;
        }
        AudioManager.instance.UpdateMixerVolume("MusicVol", musicSlider.value);
        isUpdatingUI = false;
    }

    public void OnSFXToggleChanged(bool isOn)
    {
        if (isUpdatingUI) return;

        isUpdatingUI = true;
        if (isOn)
        {
            if (lastSFXVol <= 0.001f) lastSFXVol = 0.75f;
            sfxSlider.value = lastSFXVol;
        }
        else
        {
            sfxSlider.value = 0;
        }
        AudioManager.instance.UpdateMixerVolume("SFXVol", sfxSlider.value);
        isUpdatingUI = false;
    }
}