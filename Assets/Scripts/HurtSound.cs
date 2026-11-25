using UnityEngine;

/// <summary>Listener 2: a hit sound, played on the SfxPlayer (it outlives anything destroyed this frame).</summary>
public class HurtSound : MonoBehaviour
{
    Health health;

    void OnEnable()
    {
        health = Health.FindPlayer();
        if (health != null) health.Damaged += OnHit;
    }

    void OnDisable()
    {
        if (health != null) health.Damaged -= OnHit;
    }

    void OnHit(int current, int max) => SfxPlayer.PlayHit();
}
