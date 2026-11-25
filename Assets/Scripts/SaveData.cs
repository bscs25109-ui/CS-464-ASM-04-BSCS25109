using UnityEngine;

/// <summary>
/// The ONLY script that spells a PlayerPrefs key.
/// Every read has a default, so a fresh machine (or Clear All PlayerPrefs) just works.
/// </summary>
public static class SaveData
{
    public const int FirstLevel = 1;
    public const int LastLevel = 5;

    const string UnlockedKey = "unlocked";

    static string BestKey(int level) => "best" + level;

    // ── Unlocks ──────────────────────────────────────────────
    /// <summary>Highest unlocked level (default 1).</summary>
    public static int Unlocked => PlayerPrefs.GetInt(UnlockedKey, FirstLevel);

    public static bool IsUnlocked(int level) => level <= Unlocked;

    public static void Unlock(int level)
    {
        level = Mathf.Clamp(level, FirstLevel, LastLevel);
        if (level <= Unlocked) return;
        PlayerPrefs.SetInt(UnlockedKey, level);
        PlayerPrefs.Save();
    }

    // ── Best times ───────────────────────────────────────────
    public static bool HasBest(int level) => PlayerPrefs.HasKey(BestKey(level));

    /// <summary>Best time in seconds, or -1 if the level has never been won.</summary>
    public static float BestTime(int level) =>
        HasBest(level) ? PlayerPrefs.GetFloat(BestKey(level), 0f) : -1f;

    /// <summary>Saves the time if it is the first or a new best. Returns true when it was saved.</summary>
    public static bool Record(int level, float time)
    {
        if (level < FirstLevel || level > LastLevel) return false;

        if (HasBest(level) && time >= PlayerPrefs.GetFloat(BestKey(level), float.MaxValue))
            return false;

        PlayerPrefs.SetFloat(BestKey(level), time);
        PlayerPrefs.Save();
        return true;
    }

    // ── Volumes (key = the mixer's exposed parameter name) ───
    public static float GetVolume(string key) => PlayerPrefs.GetFloat(key, 1f);

    public static void SetVolume(string key, float linear)
    {
        PlayerPrefs.SetFloat(key, linear);
        PlayerPrefs.Save();
    }

    // ── Reset (level progress only; volumes stay) ────────────
    public static void ResetProgress()
    {
        PlayerPrefs.DeleteKey(UnlockedKey);
        for (int level = FirstLevel; level <= LastLevel; level++)
            PlayerPrefs.DeleteKey(BestKey(level));
        PlayerPrefs.Save();
    }
}
