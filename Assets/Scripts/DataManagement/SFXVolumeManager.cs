using UnityEngine;
using UnityEngine.UI;

public class SFXVolumeManager : MonoBehaviour
{
    [Header("SFX Settings")]
    [SerializeField] private Slider sfxSlider;

    private const string SFXVolumeKey = "SFXVolume";

    public static float Volume { get; private set; } = 1f;

    private void Awake()
    {
        LoadSFXVolume();
    }

    private void LoadSFXVolume()
    {
        Volume = PlayerPrefs.GetFloat(SFXVolumeKey, 1f);

        if (sfxSlider == null)
            return;

        sfxSlider.minValue = 0f;
        sfxSlider.maxValue = 1f;
        sfxSlider.wholeNumbers = false;
        sfxSlider.value = Volume;

        sfxSlider.onValueChanged.RemoveListener(SetSFXVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    public void SetSFXVolume(float volume)
    {
        Volume = volume;

        PlayerPrefs.SetFloat(SFXVolumeKey, volume);
        PlayerPrefs.Save();
    }
}