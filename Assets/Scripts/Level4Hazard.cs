using UnityEngine;

/// <summary>Level 4: touching a hazard is instant death (the player's Health max is 1 and lives is 1).</summary>
public class Level4Hazard : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out Health health))
            health.Kill("You touched a hazard!");
    }
}
