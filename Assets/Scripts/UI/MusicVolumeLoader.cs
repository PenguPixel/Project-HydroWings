using UnityEngine;

public class MusicVolumeLoader : MonoBehaviour
{
    [Header("Music Sources")]
    [SerializeField] private AudioSource[] musicSources;

    private const string MusicVolumeKey = "MusicVolume";

    private void Start()
    {
        ApplySavedMusicVolume();
    }

    public void ApplySavedMusicVolume()
    {
        float savedVolume = PlayerPrefs.GetFloat(MusicVolumeKey, 1f);

        foreach (AudioSource musicSource in musicSources)
        {
            if (musicSource != null)
                musicSource.volume = savedVolume;
        }
    }
}