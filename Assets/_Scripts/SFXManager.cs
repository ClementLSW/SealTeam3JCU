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
    private AudioSource aSource;

    private void Start()
    {
        aSource = GetComponent<AudioSource>();
    }

    public void PlaySound(Sound sound, bool loop)
    {
        AudioClip clipToUse = mainbgm;
        switch (sound)
        {
            case Sound.MAINBGM:
                clipToUse = mainbgm;
                break;

            case Sound.TITLEBGM:
                clipToUse = titlebgm;
                break;

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
        aSource.loop = loop;
        aSource.clip = clipToUse;
        aSource.Play();
    }
}
