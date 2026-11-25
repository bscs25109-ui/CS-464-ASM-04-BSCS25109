using UnityEngine;

/// <summary>Listener 1: the health bar follows the player's Health.</summary>
public class HealthBarListener : MonoBehaviour
{
    [SerializeField] HealthBar bar;
    Health health;

    void OnEnable()
    {
        health = Health.FindPlayer();
        if (health == null) return;
        health.Damaged += OnChanged;
        health.Restored += OnChanged;
    }

    void Start()
    {
        if (health != null) bar.Set(health.Current, health.Max);   // Health.Awake has run by now
    }

    void OnDisable()
    {
        if (health == null) return;
        health.Damaged -= OnChanged;
        health.Restored -= OnChanged;
    }

    void OnChanged(int current, int max) => bar.Set(current, max);
}
