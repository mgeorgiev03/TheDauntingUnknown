using Unity.Audio;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public float sceneMusicVolume; 
    public Sound sceneMusic;
    public Sound[] sounds;

    void Awake()
    {
        foreach (Sound s in sounds) 
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.pitch = s.pitch;
            s.source.volume = s.volume;
        }
    }
    void Start()
    {
        sceneMusic.source = gameObject.AddComponent<AudioSource>();
        sceneMusic.source.clip = sceneMusic.clip;

        sceneMusic.source.pitch = 1;

        sceneMusic.source.loop = true;

        sceneMusic.source.Play();
    }
    void Update()
    {
        sceneMusic.source.volume = sceneMusicVolume;
       /* if (FindObjectOfType<PauseMenu>().GameIsPaused)
        {
            sceneMusic.source.Pause();
        }
        else
        {
            sceneMusic.source.UnPause();
        }*/
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if(SceneManager.GetActiveScene().name != "MenuScene")
        {
            if (!FindObjectOfType<PauseMenu>().GameIsPaused)
            {
                s.source.PlayOneShot(s.source.clip);
            }
        }
      
    }
}
