using TMPro;
using UnityEngine;

/// <summary>
/// The HUD's face: score, timer, lives, and the win and game-over screens.
/// Lives on the HUD prefab, so a fix in one place reaches all five levels.
/// The LevelController (in each scene) tells it what to show.
/// </summary>
public class Hud : MonoBehaviour
{
    public static Hud Instance { get; private set; }

    [Header("Top bar")]
    [SerializeField] GameObject scorePanel;
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text timerText;
    [SerializeField] TMP_Text livesText;

    [Header("End screens")]
    [SerializeField] GameObject winPanel;
    [SerializeField] GameObject losePanel;
    [SerializeField] TMP_Text winInfoText;
    [SerializeField] TMP_Text loseReasonText;
    [SerializeField] GameObject newBestBadge;

    [Header("Text colours (measured on the dark panel)")]
    [SerializeField] Color normalColor = new Color32(0xE9, 0xF7, 0xF6, 0xFF);   // 13.48 : 1
    [SerializeField] Color warnColor   = new Color32(0xF5, 0x99, 0xFF, 0xFF);   //  7.68 : 1

    void Awake()
    {
        Instance = this;
        if (winPanel != null) winPanel.SetActive(false);
        if (losePanel != null) losePanel.SetActive(false);
    }

    void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    // ── Called by the LevelController ───────────────────────
    public void SetScoreVisible(bool visible)
    {
        if (scorePanel != null) scorePanel.SetActive(visible);
    }

    public void SetScore(int score, int target) => scoreText.text = $"Coins {score} / {target}";

    public void SetLives(int lives) => livesText.text = $"Lives {lives}";

    public void SetTimer(string text, bool warn)
    {
        timerText.text = text;
        timerText.color = warn ? warnColor : normalColor;
    }

    public void ShowWin(float time, bool newBest)
    {
        winPanel.SetActive(true);                // SelectOnOpen selects "Next level"
        winInfoText.text = $"Time {time:0.0} s";
        if (newBestBadge != null) newBestBadge.SetActive(newBest);
    }

    public void ShowLose(string reason)
    {
        losePanel.SetActive(true);               // SelectOnOpen selects "Restart"
        loseReasonText.text = reason;
    }

    // ── Buttons on the win and game-over screens ────────────
    public void NextLevel() => LevelLoader.Instance.LoadNext();
    public void Restart()   => LevelLoader.Instance.Reload();
    public void MainMenu()  => LevelLoader.Instance.LoadMenu();
}
