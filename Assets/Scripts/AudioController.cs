using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    #region Inspector variables

    [SerializeField] private List<AudioSource> soundSources = new List<AudioSource>(); //for every sound
    [SerializeField] private List<AudioSource> musicSources = new List<AudioSource>(); //for background music

    #endregion Inspector variables

    #region private variables

    private bool mutedAudio;
    private bool mutedMusic;

    #endregion private variables

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        
    }

    #region public functions

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

    #endregion public functions
}
