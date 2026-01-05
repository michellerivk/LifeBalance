using UnityEngine;
using UnityEngine.UI;

public class MuteAudio : MonoBehaviour
{
    public void ToggleMusic(Image music)
    {
        if (AudioManager.instance != null)
            AudioManager.instance.ToggleMuteMusic(music);
        else
            Debug.LogWarning("AudioManager.instance is null. Make sure AudioManager is created before this scene.");
    }

    public void ToggleSFX(Image sfx)
    {
        if (AudioManager.instance != null)
            AudioManager.instance.ToggleMuteSFX(sfx);
    }

    public void PlayButtonSound()
    {
        AudioManager.instance.PlayButtonSound();
    }
    public void PlayStartSound()
    {
        AudioManager.instance.PlayLowerSFXVolume(3, 0.3f);
    }
}
