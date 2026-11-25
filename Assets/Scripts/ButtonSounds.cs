using UnityEngine;
using UnityEngine.UI;

/// <summary>Put on a Canvas: every Button under it (even in hidden panels) plays the click sound.</summary>
public class ButtonSounds : MonoBehaviour
{
    void Start()
    {
        foreach (Button b in GetComponentsInChildren<Button>(true))
            b.onClick.AddListener(SfxPlayer.PlayButton);
    }
}
