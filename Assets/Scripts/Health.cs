using System;
using UnityEngine;

/// <summary>
/// Lecture 07's Health, now with C# events (Lecture 09).
/// Listeners (the bar, the sound, the sparks, the flash, the LevelController) subscribe in OnEnable
/// and unsubscribe in OnDisable.
/// </summary>
public class Health : MonoBehaviour
{
    [SerializeField] int max = 3;
    [SerializeField] float iFrames = 1f;

    public bool shielded;

    int current;
    float invulnerableUntil;

    public int Current => current;
    public int Max => max;
    public string LastReason { get; private set; } = "You were hit!";

    public event Action<int, int> Damaged;    // (current, max)
    public event Action<int, int> Restored;   // (current, max)
    public event Action Died;

    void Awake() => current = max;

    /// <summary>The player's Health (tag "Player"), or null.</summary>
    public static Health FindPlayer()
    {
        GameObject p = GameObject.FindWithTag("Player");
        return p != null ? p.GetComponent<Health>() : null;
    }

    public bool TakeDamage(int amount, string reason = "You were hit!")
    {
        // Already dead, or still invulnerable: ignore the hit
        if (current <= 0 || Time.time < invulnerableUntil) return false;

        invulnerableUntil = Time.time + iFrames;

        // Shield absorbs one hit then disappears
        if (shielded) { shielded = false; return false; }

        Apply(amount, reason);
        return true;
    }

    /// <summary>Instant death (levels 4 and 5): ignores the shield and the invulnerability.</summary>
    public void Kill(string reason)
    {
        if (current <= 0) return;
        Apply(current, reason);
    }

    void Apply(int amount, string reason)
    {
        LastReason = reason;
        current = Mathf.Max(0, current - amount);

        Damaged?.Invoke(current, max);          // ?. : null when nobody listens
        if (current <= 0) Died?.Invoke();
    }

    public void Restore(float protection)
    {
        current = max;
        invulnerableUntil = Time.time + protection;
        Restored?.Invoke(current, max);
    }
}
