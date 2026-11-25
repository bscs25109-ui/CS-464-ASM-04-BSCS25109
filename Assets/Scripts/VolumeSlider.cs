using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

/// <summary>
/// One per slider. Put this script on the CANVAS (always active), not on the hidden pause panel:
/// a script on an inactive object never runs Start, so the saved volume would not be applied.
/// The slider itself can live inside the pause panel; drag it into the Slider field.
/// </summary>
public class VolumeSlider : MonoBehaviour
{
    [SerializeField] AudioMixer mixer;
    [SerializeField] string parameter = "MusicVolume";   // the exposed parameter AND the PlayerPrefs key
    [SerializeField] Slider slider;

    void Start()
    {
        slider.minValue = 0.0001f;
        slider.maxValue = 1f;

        float saved = Mathf.Clamp(SaveData.GetVolume(parameter), 0.0001f, 1f);
        slider.SetValueWithoutNotify(saved);
        Apply(saved);

        slider.onValueChanged.AddListener(OnChanged);
    }

    void OnDestroy()
    {
        if (slider != null) slider.onValueChanged.RemoveListener(OnChanged);
    }

    void OnChanged(float value)
    {
        Apply(value);
        SaveData.SetVolume(parameter, value);
    }

    void Apply(float value) => mixer.SetFloat(parameter, Mathf.Log10(value) * 20f);   // 1 -> 0 dB, 0.5 -> -6 dB, 0.0001 -> -80 dB
}
