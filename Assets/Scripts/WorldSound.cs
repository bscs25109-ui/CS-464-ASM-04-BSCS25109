using UnityEngine;

/// <summary>
/// A sound with a place in the level (Lecture 10). Put on the object that makes the sound
/// (a hazard, a goal). It sets the AudioSource's 3D settings from the Inspector and draws
/// the Min and Max Distance spheres in the Scene view so you can SEE your choice.
/// Rolloff: Logarithmic = -6 dB per doubling of distance, never fully silent until Max Distance.
///          Linear      = falls in a straight line, silent exactly at Max Distance.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class WorldSound : MonoBehaviour
{
    [SerializeField] AudioClip clip;
    [SerializeField] AudioRolloffMode rolloff = AudioRolloffMode.Logarithmic;
    [Tooltip("The sound's size: full volume inside this distance.")]
    [SerializeField] float minDistance = 1.5f;
    [Tooltip("Where the fade stops.")]
    [SerializeField] float maxDistance = 12f;

    void Awake()
    {
        AudioSource s = GetComponent<AudioSource>();
        if (clip != null) s.clip = clip;
        s.loop = true;
        s.playOnAwake = false;
        s.spatialBlend = 1f;                // fully 3D
        s.rolloffMode = rolloff;
        s.minDistance = minDistance;
        s.maxDistance = maxDistance;
        s.dopplerLevel = 0f;                // no pitch shifting as the player moves
        s.Play();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, minDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, maxDistance);
    }
}
