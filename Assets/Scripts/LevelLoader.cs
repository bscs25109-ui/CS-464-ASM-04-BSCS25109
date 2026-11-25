using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// The ONLY script that loads a scene. One per scene (it is destroyed and made again on every load).
/// Scene List: 0 MainMenu, 1-5 levels, 6 Credits.
/// </summary>
public class LevelLoader : MonoBehaviour
{
    public static LevelLoader Instance { get; private set; }

    [Tooltip("Build index of Credits. LoadNext() from here wraps to 0 (the menu).")]
    [SerializeField] int creditsIndex = 6;

    [Header("Loading bar (optional but marked)")]
    [SerializeField] GameObject loadingPanel;
    [SerializeField] Image barFill;           // Image Type: Filled, Horizontal
    [SerializeField] TMP_Text percentText;

    bool loading;

    void Awake()
    {
        Instance = this;
        if (loadingPanel != null) loadingPanel.SetActive(false);
    }

    void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    // ── The five ways to change scene ───────────────────────
    public void Load(int index)
    {
        if (loading) return;
        Time.timeScale = 1f;          // statics survive a load: put the time back
        StartCoroutine(LoadRoutine(index));
    }

    public void LoadNext()
    {
        int current = SceneManager.GetActiveScene().buildIndex;
        Load(current >= creditsIndex ? 0 : current + 1);   // wrap after Credits
    }

    public void Reload() => Load(SceneManager.GetActiveScene().buildIndex);

    public void LoadMenu() => Load(0);

    // ── Load in the background behind a bar ─────────────────
    IEnumerator LoadRoutine(int index)
    {
        loading = true;
        if (loadingPanel != null) loadingPanel.SetActive(true);
        SetBar(0f);

        AsyncOperation op = SceneManager.LoadSceneAsync(index);
        op.allowSceneActivation = false;

        while (op.progress < 0.9f)
        {
            SetBar(Mathf.Clamp01(op.progress / 0.9f));
            yield return null;
        }

        SetBar(1f);                                     // the bar reaches 100 %
        yield return new WaitForSecondsRealtime(0.2f);  // let the player see it
        op.allowSceneActivation = true;
    }

    void SetBar(float t)
    {
        if (barFill != null) barFill.fillAmount = t;
        if (percentText != null) percentText.text = Mathf.RoundToInt(t * 100f) + "%";
    }
}
