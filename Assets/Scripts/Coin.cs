using UnityEngine;

/// <summary>
/// One coin for every level. Pickup: score, sound on the SfxPlayer (it outlives the coin),
/// a particle burst that destroys itself, then the coin goes.
/// </summary>
public class Coin : MonoBehaviour
{
    [SerializeField] protected int worth = 1;

    protected virtual void Update() => transform.Rotate(0f, 90f * Time.deltaTime, 0f);

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        LevelController level = LevelController.Instance;
        if (level != null)
        {
            if (level.IsOver) return;
            level.AddScore(worth);
        }

        SfxPlayer.PlayPickup();
        Fx.Pickup(transform.position);
        Destroy(gameObject);
    }
}
