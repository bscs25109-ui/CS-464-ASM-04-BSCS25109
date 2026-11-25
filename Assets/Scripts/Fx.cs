using UnityEngine;

/// <summary>
/// Spawns particle prefabs from Assets/Resources/Fx/. Both prefabs use Stop Action: Destroy,
/// so they clean themselves up.
/// </summary>
public static class Fx
{
    static GameObject pickupPrefab;
    static GameObject hitPrefab;

    public static void Pickup(Vector3 position) => Spawn(ref pickupPrefab, "Fx/PickupBurst", position);
    public static void Hit(Vector3 position)    => Spawn(ref hitPrefab, "Fx/HitSparks", position);

    static void Spawn(ref GameObject cache, string path, Vector3 position)
    {
        if (cache == null) cache = Resources.Load<GameObject>(path);
        if (cache != null) Object.Instantiate(cache, position, Quaternion.identity);
    }
}
