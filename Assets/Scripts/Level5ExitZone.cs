using UnityEngine;

/// <summary>The LevelController only opens the exit once the coins are collected (CollectThenExit).</summary>
public class Level5ExitZone : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (LevelController.Instance != null) LevelController.Instance.ReachExit();
    }
}
