using UnityEngine;
using  UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    public string name;
    
    public AudioClip clip;

    public AudioMixerGroup audioMixerGroup;

    [Range(0,1)]
    public float volume;
    [Range(0.1f, 3)]
    public float pitch;

    [HideInInspector]
    public AudioSource source;
}
