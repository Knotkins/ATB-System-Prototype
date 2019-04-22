using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    static SoundManager _instance = null;

    public AudioClip lastLevelMusic = null;
    public AudioSource sfxSource;
    public AudioSource musicSource;

    void Awake()
    {
        if (instance)
            Destroy(gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }

    }

    public void playSingleSound(AudioClip clip, float volume = 1.0f)
    {
        if (sfxSource)
        {
            sfxSource.clip = clip;
            sfxSource.volume = volume;

            sfxSource.Play();
        }
    }
    public void playMusic(AudioClip clip, float volume = 1.0f)
    {
        if (musicSource && clip != lastLevelMusic)
        {
            Debug.Log("Music play is go");
            musicSource.clip = clip;
            musicSource.volume = volume;

            musicSource.Play();
            lastLevelMusic = clip;
        }
    }

    public static SoundManager instance
    {
        get { return _instance; }
        set { _instance = value; }
    }
}
