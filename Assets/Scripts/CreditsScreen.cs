using UnityEngine;

public class CreditsScreen : MonoBehaviour
{
    void Awake()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) Back();
    }

    /// <summary>LoadNext() after Credits wraps to scene 0, the menu.</summary>
    public void Back() => LevelLoader.Instance.LoadNext();
}
