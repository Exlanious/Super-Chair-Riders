using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Music Clips")]
    [SerializeField] private AudioClip[] musicClips;

    [Header("SFX Clips")]
    [SerializeField] private AudioClip[] sfxClips;

    [Header("Volume Settings")]
    [Range(0f, 1f)] public float musicVolume = 1f;
    [Range(0f, 1f)] public float sfxVolume = 1f;

    private void Awake()
    {
        // Singleton pattern: make this globally accessible
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        musicSource.loop = true;
        musicSource.playOnAwake = false;
        sfxSource.playOnAwake = false;
    }

    /// Play a music track by index.
    public void PlayMusic(int index, bool loop = true)
    {
        if (index < 0 || index >= musicClips.Length)
        {
            Debug.LogWarning("[SFXManager] Invalid music index.");
            return;
        }

        musicSource.clip = musicClips[index];
        musicSource.loop = loop;
        musicSource.volume = musicVolume;
        musicSource.Play();
    }


    /// Stop the current music.

    public void StopMusic()
    {
        musicSource.Stop();
    }


    /// Play a sound effect by index.

    public void PlaySFX(int index, float pitch = 1f)
    {
        if (index < 0 || index >= sfxClips.Length)
        {
            Debug.LogWarning("[SFXManager] Invalid SFX index.");
            return;
        }

        sfxSource.pitch = pitch;
        sfxSource.PlayOneShot(sfxClips[index], sfxVolume);
    }


    /// Play a sound effect by its name.

    public void PlaySFXByName(string name, float pitch = 1f)
    {
        foreach (var clip in sfxClips)
        {
            if (clip != null && clip.name == name)
            {
                sfxSource.pitch = pitch;
                sfxSource.PlayOneShot(clip, sfxVolume);
                return;
            }
        }

        Debug.LogWarning("[SFXManager] SFX not found: {name}");
    }


    /// Set the background music volume.

    public void SetMusicVolume(float vol)
    {
        musicVolume = vol;
        musicSource.volume = vol;
    }


    /// Set the sound effects volume.

    public void SetSFXVolume(float vol)
    {
        sfxVolume = vol;
    }
}
