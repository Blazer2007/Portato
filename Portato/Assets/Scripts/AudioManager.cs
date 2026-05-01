using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioMixer _mixer;
    [SerializeField] Slider _sliderMenu;
    [SerializeField] Slider _sliderPause;

    void Start()
    {
        float saved = PlayerPrefs.GetFloat("MasterVolume", 0.75f);
        SetupSlider(_sliderMenu, saved);
        SetupSlider(_sliderPause, saved);
        ApplyVolume(saved);
    }

    void SetupSlider(Slider slider, float value)
    {
        if (slider == null) return;
        slider.value = value;
        slider.onValueChanged.AddListener(OnSliderChanged);
    }

    void OnSliderChanged(float value)
    {
        ApplyVolume(value);
        if (_sliderMenu != null) _sliderMenu.value = value;
        if (_sliderPause != null) _sliderPause.value = value;
        PlayerPrefs.SetFloat("MasterVolume", value);
    }

    void ApplyVolume(float value)
    {
        // AudioMixer usa decibéis — converte de 0-1 para -80db a 0db
        float db = value > 0.001f ? Mathf.Log10(value) * 20f : -80f;
        _mixer.SetFloat("MasterVolume", db);
    }
}
