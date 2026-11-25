using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// One music player for the whole game. Put the AudioSystem prefab in EVERY scene
/// so each scene can be tested alone: the first one stays, every later copy destroys itself.
/// </summary>
public class PersistentMusic : MonoBehaviour
{
    static PersistentMusic instance;

    [SerializeField] AudioSource music;     // loops, Output = Music group
    [SerializeField] AudioMixer mixer;
    [SerializeField] string musicParam = "MusicVolume";
    [SerializeField] string sfxParam = "SfxVolume";

    void Awake()
    {
        if (instance != null) { Destroy(gameObject); return; }   // the singleton check
        instance = this;
        DontDestroyOnLoad(gameObject);

        if (music != null)
        {
            music.loop = true;
            music.ignoreListenerPause = true;   // pausing the game keeps the music (so sliders can be heard)
        }
    }

    void Start()
    {
        if (instance != this) return;

        // Mixer values only stick from Start on, and a new Play session starts at the asset's default.
        ApplyVolume(musicParam);
        ApplyVolume(sfxParam);

        if (music != null && !music.isPlaying) music.Play();
    }

    void OnDestroy()
    {
        if (instance == this) instance = null;
    }

    void ApplyVolume(string param)
    {
        if (mixer == null) return;
        float v = Mathf.Max(SaveData.GetVolume(param), 0.0001f);
        mixer.SetFloat(param, Mathf.Log10(v) * 20f);
    }
}
