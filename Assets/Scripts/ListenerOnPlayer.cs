using UnityEngine;

/// <summary>
/// For a top-down or far follow camera: the ears belong at the player, not several metres up.
/// Put this and ONE AudioListener on an empty object, and remove the AudioListener from the camera.
/// </summary>
[RequireComponent(typeof(AudioListener))]
public class ListenerOnPlayer : MonoBehaviour
{
    Transform player;

    void Start()
    {
        GameObject p = GameObject.FindWithTag("Player");
        if (p != null) player = p.transform;
    }

    void LateUpdate()
    {
        if (player != null) transform.position = player.position;
    }
}
