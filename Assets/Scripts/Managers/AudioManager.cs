using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private Sound[] sfxSounds;
    [SerializeField] private GameObject audioParent;
    private AudioSource[] sfxSource;

    private static bool soundMuted = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        AudioSourceAdd();
    }
    public void AudioSourceAdd()
    {
        for (int i = 0; i < sfxSounds.Length; i++)
        {
            sfxSounds[i].source = audioParent.AddComponent<AudioSource>();
            sfxSounds[i].source.clip = sfxSounds[i].clip;
            sfxSounds[i].source.playOnAwake = false;
            sfxSounds[i].source.loop = sfxSounds[i].loop;
            sfxSounds[i].source.volume = (soundMuted) ? 0 : 1;
        }
    }
    public void PlaySfx(string name)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound not found!");
        }
        else
        {
            if (Array.Exists(sfxSounds, element => element.name == name))
                Array.Find(sfxSounds, sound => sound.name == name).source.Play();
        }
    }
    public void StopAudioSources()
    {
        for (int i = 0; i < sfxSounds.Length; i++)
        {
            sfxSounds[i].source.Stop();
            sfxSounds[i].source.volume=0;
        }
    }
    public bool GetAudioVolume()
    {
        return (sfxSounds[0].source.volume == 1) ? true : false;
    }
}



[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    public AudioSource source;
    public bool loop;
}
