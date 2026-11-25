using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A button per level, built from SaveData: locked until unlocked, with its best time if it has one.
/// Every button loads through the LevelLoader. Drag the five buttons and their labels in, in order.
/// </summary>
public class LevelSelect : MonoBehaviour
{
    [SerializeField] Button[] levelButtons;    // 5, Level 1 first
    [SerializeField] TMP_Text[] levelLabels;   // 5, same order

    void Start()
    {
        for (int i = 0; i < levelButtons.Length; i++)
        {
            int level = i + 1;                 // build index 1..5
            levelButtons[i].onClick.AddListener(() => LevelLoader.Instance.Load(level));
        }
    }

    void OnEnable() => Refresh();

    void Refresh()
    {
        for (int i = 0; i < levelButtons.Length; i++)
        {
            int level = i + 1;
            bool open = SaveData.IsUnlocked(level);
            levelButtons[i].interactable = open;

            if (!open) levelLabels[i].text = $"Level {level}\nLocked";
            else if (SaveData.HasBest(level)) levelLabels[i].text = $"Level {level}\nBest {SaveData.BestTime(level):0.0} s";
            else levelLabels[i].text = $"Level {level}\nNot won yet";
        }
    }

    /// <summary>Hook to a "Reset progress" button (handy for testing a fresh game).</summary>
    public void ResetProgress()
    {
        SaveData.ResetProgress();
        Refresh();
    }
}
