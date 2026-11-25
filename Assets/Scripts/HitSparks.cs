using UnityEngine;

/// <summary>Listener 3: one burst of sparks at the player when it is hit (Stop Action: Destroy, no loop).</summary>
public class HitSparks : MonoBehaviour
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

    void OnHit(int current, int max) => Fx.Hit(health.transform.position + Vector3.up * 0.3f);
}
