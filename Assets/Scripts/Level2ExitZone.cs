using UnityEngine;

public class Level2ExitZone : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (LevelController.Instance != null) LevelController.Instance.ReachExit();
    }
}
