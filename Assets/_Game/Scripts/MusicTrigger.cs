using UnityEngine;

public class MusicTrigger : MonoBehaviour
{
    public enum SongType { Intro, Gameplay, Final }
    public SongType songToPlay;

    void Start()
    {
        if (AudioManager.instance == null) return;

        // Decidimos qué canción mandar al manager persistente
        switch (songToPlay)
        {
            case SongType.Intro:
                AudioManager.instance.PlayMusic(AudioManager.instance.introMusic);
                break;
            case SongType.Gameplay:
                AudioManager.instance.PlayMusic(AudioManager.instance.gameplayMusic);
                break;
            case SongType.Final:
                AudioManager.instance.PlayMusic(AudioManager.instance.finalMusic);
                break;
        }
    }
}