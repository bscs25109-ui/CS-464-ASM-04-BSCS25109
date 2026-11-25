using UnityEngine;

public class MainMenu : MonoBehaviour
{
    void Awake()
    {
        // Statics survive a load: make sure we never arrive frozen or silent
        Time.timeScale = 1f;
        AudioListener.pause = false;
    }

    /// <summary>"Continue": the highest level you have unlocked.</summary>
    public void Play() => LevelLoader.Instance.Load(Mathf.Clamp(SaveData.Unlocked, SaveData.FirstLevel, SaveData.LastLevel));

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
