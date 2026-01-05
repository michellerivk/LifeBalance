using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Music")]
    [SerializeField] private AudioSource _titleMusic;
    [SerializeField] private AudioSource _bg;

    [Header("SFX")]
    [SerializeField] private AudioSource[] _sfx;

    [Header("Settings")]
    [SerializeField] private bool isMusicMuted = false;
    [SerializeField] private bool isSFXMuted = false;

    [Header("Sprites")]
    [SerializeField] private Sprite _musicMuted;
    [SerializeField] private Sprite _musicNotMuted;
    [SerializeField] private Sprite _sfxMuted;
    [SerializeField] private Sprite _sfxNotMuted;

    //[SerializeField] private Sprite _currentMusicSprite;
    //[SerializeField] private Sprite _currentSfxSprite;



    private AudioSource _currentMusic;

    private void Awake()
    {
        // Singleton
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        ApplySfxMute();
        ApplyMusicMute();
    }

    private void OnEnable()
    {
        SceneManager.activeSceneChanged += HandleActiveSceneChanged;
    }

    private void OnDisable()
    {
        SceneManager.activeSceneChanged -= HandleActiveSceneChanged;
    }

    private void Start()
    {
        // Make sure initial scene gets the correct music
        SetMusicForScene(SceneManager.GetActiveScene().name);
    }

    private void HandleActiveSceneChanged(Scene oldScene, Scene newScene)
    {
        SetMusicForScene(newScene.name);
    } 

    private void SetMusicForScene(string sceneName)
    {
        AudioSource next = null;

        if (sceneName == "MainMenuScene")
            next = _titleMusic;
        else if ((sceneName == "LevelScene") || (sceneName == "HardMode") || (sceneName == "EasyMode"))
            next = _bg;


        SwitchMusic(next);
    }

    private void SwitchMusic(AudioSource next)
    {
        // Prevent overlap
        StopAllMusic();

        _currentMusic = next;

        // Keep mute state consistent across all music sources
        ApplyMusicMute();

        // If muted, do not start playback
        if (isMusicMuted || _currentMusic == null)
            return;

        _currentMusic.Play();
    }

    private void StopAllMusic()
    {
        if (_titleMusic != null) _titleMusic.Stop();
        if (_bg != null) _bg.Stop();
    }

    private void ApplyMusicMute()
    {
        if (_titleMusic != null) _titleMusic.mute = isMusicMuted;
        if (_bg != null) _bg.mute = isMusicMuted;
    }

    private void ApplySfxMute()
    {
        if (_sfx == null) return;

        foreach (var sfx in _sfx)
            if (sfx != null) sfx.mute = isSFXMuted;
    }

    public void ToggleMuteMusic(Image music)
    {
        isMusicMuted = !isMusicMuted;

        ApplyMusicMute();

        if (isMusicMuted)
        {
            // Stop to not hear multiple tracks
            StopAllMusic();
            SetIcon(music, _musicMuted, 80f, 80f);
        }
        else
        {
            if (!_currentMusic.isPlaying)
            {
                _currentMusic.Play();
            }

            SetIcon(music, _musicNotMuted, 80f, 60f);
        }

        Debug.Log("Music Muted: " + isMusicMuted);
    }

    public void ToggleMuteSFX(Image sfx)
    {
        isSFXMuted = !isSFXMuted;
        ApplySfxMute();

        if (isSFXMuted)
        {
            SetIcon(sfx, _sfxMuted, 80f, 80f);
        }
        else
        {
            SetIcon(sfx, _sfxNotMuted, 60f, 80f);
        }

        Debug.Log("SFX Muted: " + isSFXMuted);
    }

    public void PlaySFX(int sfxToPlay)
    {
        if (_sfx == null || sfxToPlay < 0 || sfxToPlay >= _sfx.Length) return;
        var src = _sfx[sfxToPlay];
        if (src == null) return;

        src.Stop();
        src.Play();
    }

    public void PlaySFXPitchAdjusted(int sfxToPlay)
    {
        if (_sfx == null || sfxToPlay < 0 || sfxToPlay >= _sfx.Length) return;
        var src = _sfx[sfxToPlay];
        if (src == null) return;

        src.pitch = Random.Range(0.8f, 1.2f);
        PlaySFX(sfxToPlay);
    }

    public void PlayLowerSFXVolume(int sfxToPlay, float newVol)
    {
        if (_sfx == null || sfxToPlay < 0 || sfxToPlay >= _sfx.Length) 
            return;

        var src = _sfx[sfxToPlay];
        if (src == null) 
            return;

        src.volume = newVol;
        PlaySFX(sfxToPlay);
    }

    public void PlayButtonSound()
    {
        if (_sfx == null || _sfx.Length == 0 || _sfx[0] == null) return;

        _sfx[0].volume = 0.3f;
        PlaySFXPitchAdjusted(0);
    }

    public void PlayTitleMusic() => SwitchMusic(_titleMusic);
    public void PlayBackgroundMusic() => SwitchMusic(_bg);
    
    public void SetFirstIcon(Image musicImage, Image sfxImage)
    {
        if (isMusicMuted) SetIcon(musicImage, _musicMuted, 80f, 80f);
        else SetIcon(musicImage, _musicNotMuted, 80f, 60f);

        if (isSFXMuted) SetIcon(sfxImage, _sfxMuted, 80f, 80f);
        else SetIcon(sfxImage, _sfxNotMuted, 60f, 80f);
    }
    private void SetIcon(Image img, Sprite sprite, float width, float height)
    {
        if (img == null) return;

        img.sprite = sprite;

        RectTransform rt = img.rectTransform;
        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
    }


}
