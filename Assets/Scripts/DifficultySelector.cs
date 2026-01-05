using UnityEngine;
using UnityEngine.UI;

public enum Difficulty { Easy, Normal, Hard }

public class DifficultySelector : MonoBehaviour
{
    [Header("Difficulties")]
    [SerializeField] private Toggle easy;
    [SerializeField] private Toggle normal;
    [SerializeField] private Toggle hard;

    [Header("Images")]
    [SerializeField] private Image _easyImage;
    [SerializeField] private Image _normalImage;
    [SerializeField] private Image _hardImage;

    [Header("Colors")]
    private readonly Color offColor = Color.white;
    private readonly Color easyOn = new Color(252f / 255f, 199f / 255f, 239f / 255f, 1f); // #FCC7EF
    private readonly Color normalOn = new Color(181f / 255f, 226f / 255f, 165f / 255f, 1f); // #B5E2A5
    private readonly Color hardOn = new Color(226f / 255f, 112f / 255f, 114f / 255f, 1f); // #E27072

    public Difficulty SelectedDifficulty { get; private set; } = Difficulty.Normal;

    private void Start()
    {
        Debug.Log("Saved difficulty int = " + PlayerPrefs.GetInt("difficulty", -1));

        int savedInt = PlayerPrefs.GetInt("difficulty", (int)Difficulty.Normal);
        Difficulty saved = (Difficulty)savedInt;

        ApplyDifficultyToToggles(saved);
        ApplyDifficultyVisuals(saved);
        SelectedDifficulty = saved;

        easy.onValueChanged.AddListener(isOn => { if (isOn) SetDifficulty(Difficulty.Easy); });
        normal.onValueChanged.AddListener(isOn => { if (isOn) SetDifficulty(Difficulty.Normal); });
        hard.onValueChanged.AddListener(isOn => { if (isOn) SetDifficulty(Difficulty.Hard); });

    }

    private void ApplyDifficultyToToggles(Difficulty difficulty)
    {
        easy.SetIsOnWithoutNotify(difficulty == Difficulty.Easy);
        normal.SetIsOnWithoutNotify(difficulty == Difficulty.Normal);
        hard.SetIsOnWithoutNotify(difficulty == Difficulty.Hard);

        // If somehow none are on, force Normal
        if (!easy.isOn && !normal.isOn && !hard.isOn)
        {
            normal.SetIsOnWithoutNotify(true);
            SelectedDifficulty = Difficulty.Normal;

            PlayerPrefs.SetInt("difficulty", (int)Difficulty.Normal);
            PlayerPrefs.Save();
        }
    }
    private void SetDifficulty(Difficulty difficulty)
    {
        SelectedDifficulty = difficulty;

        ApplyDifficultyVisuals(difficulty);

        // Save the chosen difficulty for the next game sessions
        PlayerPrefs.SetInt("difficulty", (int)difficulty);
        PlayerPrefs.Save();

        Debug.Log($"Difficulty set to: {difficulty}");
    }
    private void ApplyDifficultyVisuals(Difficulty difficulty)
    {
        // Reset all colors
        _easyImage.color = offColor;
        _normalImage.color = offColor;
        _hardImage.color = offColor;

        switch (difficulty)
        {
            case Difficulty.Easy:
                _easyImage.color = easyOn;
                break;
            case Difficulty.Normal:
                _normalImage.color = normalOn;
                break;
            case Difficulty.Hard:
                _hardImage.color = hardOn;
                break;
        }
    }
}
