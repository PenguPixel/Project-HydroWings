using System.Collections;
using UnityEngine;

public class BossAudio : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource introOutroAudioSource;
    [SerializeField] private AudioSource loopAudioSource;

    [Header("Boss Music")]
    [SerializeField] private AudioClip introClip;
    [SerializeField] private AudioClip loopClip;
    [SerializeField] private AudioClip outroClip;

    [Header("Settings")]
    [Tooltip("Grundlautstärke der Bossmusik. Wird mit dem Music-Slider multipliziert.")]
    [SerializeField, Range(0f, 1f)] private float volume = 0.3f;

    private const string MusicVolumeKey = "MusicVolume";

    private Coroutine musicCoroutine;
    private bool musicStarted;
    private bool outroStarted;

    public void PlayBossMusic()
    {
        if (musicStarted || outroStarted)
        {
            return;
        }

        if (introOutroAudioSource == null || loopAudioSource == null)
        {
            Debug.LogWarning(
                "BossAudio: Beide AudioSources müssen zugewiesen werden."
            );

            return;
        }

        musicStarted = true;

        StopAllAudio();

        PrepareAudioSource(introOutroAudioSource);
        PrepareAudioSource(loopAudioSource);

        musicCoroutine = StartCoroutine(
            BossMusicRoutine()
        );
    }

    private IEnumerator BossMusicRoutine()
    {
        if (introClip != null)
        {
            ApplyMusicVolume(introOutroAudioSource);

            introOutroAudioSource.clip = introClip;
            introOutroAudioSource.loop = false;
            introOutroAudioSource.Play();

            while (
                introOutroAudioSource.isPlaying &&
                !outroStarted
            )
            {
                yield return null;
            }
        }

        if (outroStarted)
        {
            yield break;
        }

        if (loopClip != null)
        {
            ApplyMusicVolume(loopAudioSource);

            loopAudioSource.clip = loopClip;
            loopAudioSource.loop = true;
            loopAudioSource.Play();
        }
        else
        {
            Debug.LogWarning(
                "BossAudio: Kein Loop-Clip zugewiesen."
            );
        }

        musicCoroutine = null;
    }

    public void PlayOutro()
    {
        if (outroStarted)
        {
            return;
        }

        outroStarted = true;

        if (musicCoroutine != null)
        {
            StopCoroutine(musicCoroutine);
            musicCoroutine = null;
        }

        StopAllAudio();

        if (introOutroAudioSource == null)
        {
            Debug.LogWarning(
                "BossAudio: Intro/Outro AudioSource wurde nicht zugewiesen."
            );

            return;
        }

        PrepareAudioSource(introOutroAudioSource);
        ApplyMusicVolume(introOutroAudioSource);

        if (outroClip != null)
        {
            introOutroAudioSource.clip = outroClip;
            introOutroAudioSource.loop = false;
            introOutroAudioSource.Play();
        }
        else
        {
            Debug.LogWarning(
                "BossAudio: Kein Outro-Clip zugewiesen."
            );
        }
    }

    private void PrepareAudioSource(AudioSource audioSource)
    {
        if (audioSource == null)
        {
            return;
        }

        audioSource.playOnAwake = false;
        audioSource.loop = false;
        audioSource.spatialBlend = 0f;

        ApplyMusicVolume(audioSource);
    }

    private void ApplyMusicVolume(AudioSource audioSource)
    {
        if (audioSource == null)
        {
            return;
        }

        float savedMusicVolume =
            PlayerPrefs.GetFloat(MusicVolumeKey, 1f);

        audioSource.volume =
            volume * savedMusicVolume;
    }

    private void StopAllAudio()
    {
        if (introOutroAudioSource != null)
        {
            introOutroAudioSource.Stop();
        }

        if (loopAudioSource != null)
        {
            loopAudioSource.Stop();
        }
    }
}