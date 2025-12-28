using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] private AudioSource _titleMusic;
    [SerializeField] private AudioSource[] _sfx;
    [SerializeField] private AudioSource _bg;
    [SerializeField] private bool isMusicMuted = false;
    [SerializeField] private bool isSFXMuted = false;

    public void Awake()
    {
        // Singleton data structure
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        foreach (AudioSource sfx in _sfx)
        {
            sfx.mute = isSFXMuted;
        }

        _bg.mute = isSFXMuted;
        _titleMusic.mute = isMusicMuted;
    }

    public void PlayTitle() // Play main menu music
    {
        _titleMusic.Play();
    }
    public void StartGameMusic()
    {
        StopTitle();
        PlayBG();
    }
    public void StopTitle() // Stop the title music (after hitting play)
    {
        _titleMusic.mute = true; // If stop doesnt work for some reason
    }

    public void LowerTitle() // Lower the title music (in the settings for example)
    {
        _titleMusic.volume = 0.2f;
    }

    public void IncreaseTitle() // Increase the title music (after closing the settings for example)
    {
        _titleMusic.volume = 0.6f;
    }

    public void PlaySFX(int sfxToPlay) // Dragging an item, item dropping, cracks on the floor etc...
    {
        _sfx[sfxToPlay].Stop(); // If a sound effect is already playing, stop it
        _sfx[sfxToPlay].Play();
    }

    public void PlaySFXPitchAdjusted(int sfxToPlay) // Changing the pitch on a sound that you hear over and over
    {
        _sfx[sfxToPlay].pitch = Random.Range(0.8f, 1.2f);

        PlaySFX(sfxToPlay);
    }
    public void PlayLowerSFXVolume(int sfxToPlay, float newVol) // Changing the pitch on a sound that you hear over and over
    {
        _sfx[sfxToPlay].volume = newVol;

        PlaySFX(sfxToPlay);
    }

    public void PlayBG() // Play background music after hitting start
    {
        _bg.Stop();
        _bg.Play();
    }

    public void ToggleMuteMusic()
    {
        isMusicMuted = !isMusicMuted; // Flip the boolean state

        if (SceneManager.GetActiveScene().name == "MainMenuScene")
        {
            _titleMusic.mute = isMusicMuted; // Mute/Unmute the AudioSource (_titleMusic)
            _bg.mute = isMusicMuted;
        }

        if (SceneManager.GetActiveScene().name == "LevelScene")
            _bg.mute = isMusicMuted; // Mute / Unmute the backgroundMusic
        Debug.Log("Music Muted: " + isMusicMuted);
    }

    public void ToggleMuteSFX()
    {
        isSFXMuted = !isSFXMuted; // Flip the boolean state

        foreach (var sfx in _sfx)
        {
            sfx.mute = isSFXMuted;
        }

        Debug.Log("SFX Muted: " + isSFXMuted);
    }


    public void PlayButtonSound()
    {
        _sfx[0].volume = 0.3f;
        PlaySFXPitchAdjusted(0);
    }
    public void PlayStartSound()
    {
        _sfx[3].volume = 0.3f;
        PlaySFXPitchAdjusted(3);
    }
}
