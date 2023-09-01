using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class SoundManager : MonoBehaviour
{
    public static SoundManager _instance;

    [SerializeField] private AudioSource _musicSource, _effectsSource;

    [Header("======Basic Sounds======")] 
    public AudioClip mouseDownSound;
    public AudioClip uiMouseDownSound;
    public AudioClip homePageBackSound;
    public AudioClip gamePlayBackSound;
    public AudioClip secGamePlayBackSound;


    
    
    // Start is called before the first frame update
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {

    }



    public void PlayEffectSound(AudioClip clip)
    {
        _effectsSource.PlayOneShot(clip);
    }
    

    public void PlayMusicSound(AudioClip clip,bool isLoop,float volume = 1f)
    {
        _musicSource.Stop();
        _musicSource.clip = clip;
        _musicSource.loop = isLoop;
        _musicSource.volume = volume;
        _musicSource.Play();
    }

    public void ChangeMasterVolume(float value)
    {
        AudioListener.volume = value; 
    }
}
