using UnityEngine;
using UnityEngine.Audio;

public enum SoundCategory
{
    Master,
    Music,
    Effects, 
    Menu
}

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    [Range(0f, 1f)]
    public float volume;
    [Range(0.1f, 3f)]
    public float pitch;
    public bool loop;

    public SoundCategory category = SoundCategory.Master;

    [HideInInspector]
    public AudioSource source;
}
