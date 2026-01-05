using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private Image musicIcon;
    [SerializeField] private Image sfxIcon;

    private void Start()
    {
        if (AudioManager.instance != null)
            AudioManager.instance.SetFirstIcon(musicIcon, sfxIcon);
    }
}
