using UnityEngine;

/// <summary>
/// Esc pauses and resumes. Resume() comes before EVERY load, because Time.timeScale,
/// AudioListener.pause and the static IsPaused all survive a scene load.
/// </summary>
public class PauseMenu : MonoBehaviour
{
    public static bool IsPaused { get; private set; }

    [SerializeField] GameObject panel;    // has SelectOnOpen with Resume as the first button

    void Awake()
    {
        IsPaused = false;                 // a static survives a load: start every scene unpaused
        if (panel != null) panel.SetActive(false);
    }

    void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Escape)) return;

        if (IsPaused) Resume();
        else if (CanPause()) Pause();
    }

    static bool CanPause() => LevelController.Instance == null || !LevelController.Instance.IsOver;

    public void Pause()
    {
        IsPaused = true;
        Time.timeScale = 0f;
        AudioListener.pause = true;       // silences the 3D world sounds; music and menu clicks ignore it
        panel.SetActive(true);
    }

    public void Resume()
    {
        IsPaused = false;
        Time.timeScale = 1f;
        AudioListener.pause = false;
        panel.SetActive(false);
    }

    public void Restart()
    {
        Resume();
        LevelLoader.Instance.Reload();
    }

    public void QuitToMenu()
    {
        Resume();
        LevelLoader.Instance.LoadMenu();
    }
}
