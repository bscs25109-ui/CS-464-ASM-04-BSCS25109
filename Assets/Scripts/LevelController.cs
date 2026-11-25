using System.Collections;
using UnityEngine;

public enum LevelMode { CollectCoins, ReachExit, CollectThenExit, Survive }

/// <summary>
/// Replaces GameManager and Level2Manager..Level5Manager. One per level scene.
/// Same rules as your Lab 06 levels, set in the Inspector:
///   Level 1  CollectCoins     target 5   timeLimit 0   lives 3  (Health max 3)
///   Level 2  ReachExit        target 0   timeLimit 60  lives 1  (Health max 5)
///   Level 3  CollectCoins     target 10  timeLimit 45  lives 1  (Health max 5)
///   Level 4  Survive          target 0   timeLimit 45  lives 1  (Health max 1)
///   Level 5  CollectThenExit  target 5   timeLimit 60  lives 1  (Health max 1)
/// </summary>
public class LevelController : MonoBehaviour
{
    public static LevelController Instance { get; private set; }

    [Header("Rules")]
    [SerializeField] LevelMode mode = LevelMode.CollectCoins;
    [SerializeField] int targetScore = 5;
    [Tooltip("0 = no limit: the HUD counts up instead of down.")]
    [SerializeField] float timeLimit = 0f;
    [SerializeField] int lives = 3;

    [Header("In this scene")]
    [SerializeField] Health player;
    [SerializeField] Transform respawnPoint;      // optional: defaults to where the player starts
    [SerializeField] float respawnDelay = 0.6f;
    [SerializeField] float spawnProtection = 1.5f;

    int score;
    int livesLeft;
    float elapsed;
    bool over;
    int lastShown = -1;
    Vector3 startPosition;
    Hud hud;

    public bool IsOver => over;
    bool ExitOpen => mode == LevelMode.ReachExit ||
                     (mode == LevelMode.CollectThenExit && score >= targetScore);

    void Awake()
    {
        Instance = this;
        Time.timeScale = 1f;
    }

    void OnEnable()
    {
        if (player != null) player.Died += OnPlayerDied;
    }

    void OnDisable()
    {
        if (player != null) player.Died -= OnPlayerDied;
    }

    void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    void Start()
    {
        hud = Hud.Instance;
        livesLeft = lives;
        startPosition = respawnPoint != null ? respawnPoint.position : player.transform.position;

        if (hud == null) { Debug.LogError("No HUD prefab in this scene."); return; }

        hud.SetScoreVisible(mode == LevelMode.CollectCoins || mode == LevelMode.CollectThenExit);
        hud.SetScore(score, targetScore);
        hud.SetLives(livesLeft);
        UpdateTimer();
    }

    void Update()
    {
        if (over || PauseMenu.IsPaused) return;

        elapsed += Time.deltaTime;

        if (timeLimit > 0f && elapsed >= timeLimit)
        {
            if (mode == LevelMode.Survive) Win();
            else Lose("Time ran out!");
            return;
        }

        UpdateTimer();
    }

    void UpdateTimer()
    {
        if (hud == null) return;

        bool countdown = timeLimit > 0f;
        float left = timeLimit - elapsed;
        int shown = countdown ? Mathf.CeilToInt(left) : Mathf.FloorToInt(elapsed * 10f);
        if (shown == lastShown) return;
        lastShown = shown;

        string text;
        if (!countdown) text = $"Time {elapsed:0.0}";
        else if (mode == LevelMode.Survive) text = $"Survive {shown}";
        else if (mode == LevelMode.CollectThenExit && ExitOpen) text = $"GO TO EXIT {shown}";
        else text = $"Time {shown}";

        hud.SetTimer(text, countdown && left <= 10f && mode != LevelMode.Survive);
    }

    // ── Called by coins ─────────────────────────────────────
    public void AddScore(int amount)
    {
        if (over) return;
        score += amount;
        if (hud != null) hud.SetScore(score, targetScore);

        if (mode == LevelMode.CollectCoins && score >= targetScore) Win();
        if (mode == LevelMode.CollectThenExit && score >= targetScore) { lastShown = -1; UpdateTimer(); }
    }

    // ── Called by the exit zones ────────────────────────────
    public void ReachExit()
    {
        if (over || !ExitOpen) return;
        Win();
    }

    // ── Death, lives, respawn ───────────────────────────────
    void OnPlayerDied()
    {
        if (over) return;
        livesLeft--;
        if (hud != null) hud.SetLives(Mathf.Max(livesLeft, 0));

        if (livesLeft <= 0) Lose(player.LastReason);
        else StartCoroutine(Respawn());
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnDelay);
        Rigidbody body = player.GetComponent<Rigidbody>();
        body.position = startPosition;
        body.linearVelocity = Vector3.zero;       // Unity 6 (body.velocity on 2022)
        player.Restore(spawnProtection);
    }

    // ── Win / Lose ──────────────────────────────────────────
    public void Win()
    {
        if (over) return;
        over = true;

        int level = gameObject.scene.buildIndex;
        bool best = SaveData.Record(level, elapsed);
        SaveData.Unlock(level + 1);

        SfxPlayer.PlayWin();
        hud.ShowWin(elapsed, best);
        Time.timeScale = 0f;
    }

    public void Lose(string reason)
    {
        if (over) return;
        over = true;

        SfxPlayer.PlayLose();
        hud.ShowLose(reason);
        Time.timeScale = 0f;
    }
}
