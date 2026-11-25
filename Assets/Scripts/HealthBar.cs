using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Two Images filled from the left: the fill, and a paler lag bar behind it.
/// The lag bar waits 0.4 s, then drains at 0.8 (of the bar) per second.
/// Driven by Set(current, max).
/// </summary>
public class HealthBar : MonoBehaviour
{
    [SerializeField] Image fill;     // Image Type: Filled, Horizontal, Origin Left
    [SerializeField] Image lag;      // behind the fill, paler
    [SerializeField] float lagDelay = 0.4f;
    [SerializeField] float lagSpeed = 0.8f;

    float target = 1f;
    float wait;

    public void Set(int current, int max)
    {
        target = max > 0 ? Mathf.Clamp01((float)current / max) : 0f;
        fill.fillAmount = target;

        if (target > lag.fillAmount) lag.fillAmount = target;   // healing: the lag bar jumps up with it
        wait = lagDelay;
    }

    void Update()
    {
        if (lag.fillAmount <= target) return;

        if (wait > 0f) { wait -= Time.unscaledDeltaTime; return; }

        lag.fillAmount = Mathf.MoveTowards(lag.fillAmount, target, lagSpeed * Time.unscaledDeltaTime);
    }
}
