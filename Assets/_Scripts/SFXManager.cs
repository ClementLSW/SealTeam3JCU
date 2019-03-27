using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    [SerializeField] private AudioClip mainbgm;
    [SerializeField] private AudioClip titlebgm;
    [SerializeField] private AudioClip death;
    [SerializeField] private AudioClip start;
    [SerializeField] private AudioClip gunshot;
    [SerializeField] private AudioClip pickup;
    [SerializeField] private AudioClip impact;
    [SerializeField] private AudioClip gameOver;
    [SerializeField] private AudioClip rocketFire; 
    [SerializeField] private AudioClip explosion;
    public enum Sound { MAINBGM, TITLEBGM, DEATH, START, GUNSHOT, PICKUP, IMPACT, ROCKETFIRE, GAMEOVER, EXPLOSION};

    public static SFXManager instance;
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;

    private void Start()
    {
        instance = this;
        bgmSource = gameObject.AddComponent<AudioSource>();
        sfxSource = gameObject.AddComponent<AudioSource>();
    }

    public void PlayBgm(Sound sound)
    {
        switch (sound)
        {
            case Sound.MAINBGM:
                bgmSource.clip = mainbgm;
                break;

            case Sound.TITLEBGM:
                bgmSource.clip = titlebgm;
                break;
        }

        bgmSource.loop = true;
        bgmSource.Play();
    }

    public void PlaySound(Sound sound, bool loop)
    {
        AudioClip clipToUse = start;
        switch (sound)
        {
            case Sound.DEATH:
                clipToUse = death;
                break;

            case Sound.START:
                clipToUse = start;
                break;

            case Sound.GUNSHOT:
                clipToUse = gunshot;
                break;

            case Sound.PICKUP:
                clipToUse = pickup;
                break;

            case Sound.IMPACT:
                clipToUse = impact;
                break;

            case Sound.GAMEOVER:
                clipToUse = gameOver;
                break;

            case Sound.ROCKETFIRE:
                clipToUse = rocketFire;
                break; 

            case Sound.EXPLOSION:
                clipToUse = explosion;
                break;
        }
        sfxSource.loop = loop;
        sfxSource.clip = clipToUse;
        sfxSource.Play();
    }

    public void PlayStartSound()
    {
        PlaySound(Sound.START, false);
    }
}
