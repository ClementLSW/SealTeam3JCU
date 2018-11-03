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
    public enum Sound { MAINBGM, TITLEBGM, DEATH, START, GUNSHOT, PICKUP, IMPACT};

    public static SFXManager instance;
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;

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
        }
        sfxSource.loop = loop;
        sfxSource.clip = clipToUse;
        sfxSource.Play();
    }
}
