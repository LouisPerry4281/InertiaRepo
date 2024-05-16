using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Rendering;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public Sound[] musicSounds;
    public Sound[] sfxSounds;
    public AudioSource musicSource;
    public AudioSource sfxSource;

    List<AudioSource> audioSources = new List<AudioSource>();

    private void Awake()
    {
        //Singleton!
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        //Play Music Here AudioManager.instance.PlaySFX()/PlayMusic()
    }

    public void PlayMusic(string name, float volume, float pitch, bool loop)
    {
        //Get the requested sound from the array
        Sound s = Array.Find(musicSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Sound Not Found");
        }

        AudioSource audioSourceInstance = gameObject.AddComponent<AudioSource>();

        //Play the clip grabbed
        audioSourceInstance.volume = volume;
        audioSourceInstance.pitch = pitch;
        audioSourceInstance.loop = loop;
        audioSourceInstance.PlayOneShot(s.clip);
    }

    public void PlaySFX(string name, float volume, float pitch)
    {
        //Get the requested sound from the array
        Sound s = Array.Find(sfxSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Sound Not Found");
            return;
        }

        AudioSource audioSourceInstance = gameObject.AddComponent<AudioSource>();

        //Play the clip grabbed
        audioSourceInstance.volume = volume;
        audioSourceInstance.pitch = pitch;
        audioSourceInstance.PlayOneShot(s.clip);
      
    }
}
