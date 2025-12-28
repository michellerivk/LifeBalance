using UnityEngine;

public class MuteAudio : MonoBehaviour
{
    public void ToggleMusic()
    {
        if (AudioManager.instance != null)
            AudioManager.instance.ToggleMuteMusic();
        else
            Debug.LogWarning("AudioManager.instance is null. Make sure AudioManager is created before this scene.");
    }

    public void ToggleSFX()
    {
        if (AudioManager.instance != null)
            AudioManager.instance.ToggleMuteSFX();
    }

    public void PlayButtonSound()
    {
        AudioManager.instance.PlayButtonSound();
    }
}
