using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Clips")]
    [SerializeField] Sound[] sounds;

    public static AudioManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;  // Singleton instance
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);  // Destroy duplicate instances
        }

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
        PlaySFX("MenuMusic");
    }

    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;

        // Stop duplicate
        if (s.source.isPlaying)
        {
            s.source.Stop();
        }

        if (s.source.loop)
        {
            s.source.loop = true;
        }
        // Compute effective volume
        float categoryVolume = GetCategoryVolume(s.category);
        s.source.volume = (s.volume * categoryVolume * (SettingsManager.Instance.MasterVolume / 100) / 100);

        s.source.Play();
    }

    public void StopSFX(string name)
    {
        Sound sound = Array.Find(sounds, s => s.name == name);
        if (sound != null && sound.source.isPlaying)
        {
            sound.source.Stop();
        }
    }

    private float GetCategoryVolume(SoundCategory category)
    {
        switch (category)
        {
            case SoundCategory.Music:
                return SettingsManager.Instance.MusicVolume;
            case SoundCategory.Effects:
                return SettingsManager.Instance.EffectsVolume;
            case SoundCategory.Menu:
                return SettingsManager.Instance.MenuVolume;
            default:
                return 1f;
        }
    }

    public void RefreshAllVolumes()
    {
        foreach (Sound s in sounds)
        {
            float categoryVolume = GetCategoryVolume(s.category);
            s.source.volume = s.volume * categoryVolume * SettingsManager.Instance.MasterVolume;
        }
    }
}
