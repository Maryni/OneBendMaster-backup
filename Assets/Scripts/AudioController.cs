using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AudioController : MonoBehaviour
{
    [SerializeField] private List<AudioSource> soundSources = new List<AudioSource>(); //for every sound
    [SerializeField] private List<AudioSource> musicSources = new List<AudioSource>(); //for background music

    private bool mutedAudio;
    private bool mutedMusic;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        
    }

    public void ChangeSoundState()
    {
        if (!mutedAudio)
        {
            foreach (var item  in soundSources)
            {
                item.mute = true;
            }
        }
        else
        {
            foreach (var item  in soundSources)
            {
                item.mute = false;
            }
        }
        mutedAudio = !mutedAudio;
    }

    public void ChangeMusicState()
    {
        if (!mutedMusic)
        {
            foreach (var item  in musicSources)
            {
                item.mute = true;
            }
        }
        else
        {
            foreach (var item  in musicSources)
            {
                item.mute = false;
            }
        }
        mutedMusic = !mutedMusic;
    }
}
