using UnityEngine;
using UnityEngine.UI;

/// <summary>Listener 4: a full-screen red flash. The Image must have Raycast Target OFF.</summary>
public class HitFlash : MonoBehaviour
{
    [SerializeField] Image flash;
    [SerializeField] float peakAlpha = 0.35f;
    [SerializeField] float fadeSpeed = 2.5f;

    Health health;
    float alpha;

    void OnEnable()
    {
        health = Health.FindPlayer();
        if (health != null) health.Damaged += OnHit;
    }

    void OnDisable()
    {
        if (health != null) health.Damaged -= OnHit;
    }

    void OnHit(int current, int max) => alpha = peakAlpha;

    void Update()
    {
        if (alpha <= 0f && flash.color.a <= 0f) return;

        alpha = Mathf.MoveTowards(alpha, 0f, fadeSpeed * Time.unscaledDeltaTime * peakAlpha);
        Color c = flash.color;
        c.a = alpha;
        flash.color = c;
    }
}
