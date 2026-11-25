using UnityEngine;

/// <summary>
/// One player for every sound effect (Lecture 09, slide 11).
/// Any script plays a sound with SfxPlayer.Instance.Play(clip), or a shortcut below.
/// Four AudioSources on the SFX mixer group, used in turn, so a changed pitch never bends a sound still playing.
/// </summary>
public class SfxPlayer : MonoBehaviour
{
    public static SfxPlayer Instance { get; private set; }

    [SerializeField] AudioSource[] sources;           // four, Output = SFX group
    [SerializeField] float pitchSemitones = 2f;       // +/- 2 semitones
    [SerializeField, Range(0f, 0.5f)] float volumeJitter = 0.1f;

    [Header("The game's sounds")]
    public AudioClip pickup;
    public AudioClip hit;
    public AudioClip button;
    public AudioClip win;
    public AudioClip lose;

    int next;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }   // the singleton check
        Instance = this;
        DontDestroyOnLoad(gameObject);

        foreach (AudioSource s in sources)
        {
            s.playOnAwake = false;
            s.loop = false;
            s.ignoreListenerPause = true;   // menu clicks are heard while the game is paused
        }
    }

    void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    public void Play(AudioClip clip, float volume = 1f)
    {
        if (clip == null || sources == null || sources.Length == 0) return;

        AudioSource s = sources[next];
        next = (next + 1) % sources.Length;

        s.pitch = Mathf.Pow(2f, Random.Range(-pitchSemitones, pitchSemitones) / 12f);
        s.PlayOneShot(clip, volume * Random.Range(1f - volumeJitter, 1f));
    }

    // Shortcuts (safe if a scene has no SfxPlayer yet)
    public static void PlayPickup() { if (Instance != null) Instance.Play(Instance.pickup); }
    public static void PlayHit()    { if (Instance != null) Instance.Play(Instance.hit); }
    public static void PlayButton() { if (Instance != null) Instance.Play(Instance.button); }
    public static void PlayWin()    { if (Instance != null) Instance.Play(Instance.win); }
    public static void PlayLose()   { if (Instance != null) Instance.Play(Instance.lose); }
}
