using UnityEngine.Audio;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private Sound[] sounds = new Sound[0];

    private void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            //s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    private void Start()
    {
        Play("Theme");
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;
        System.Random r = new System.Random();
        s.source.clip = s.clips[r.Next(s.clips.Length)];
        s.source.Play();
    }
}

[Serializable]
public class Sound
{
    public string name;
    public AudioClip[] clips;
    [Range(0, 1)] public float volume;
    [Range(.1f, 3)]  public float pitch;
    public bool loop;
    [HideInInspector] public AudioSource source;
}
// THX Brackeys.