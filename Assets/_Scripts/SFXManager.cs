using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    [SerializeField] private AudioClip sound1;
    public enum Sound { SOUND1 };

    public static SFXManager instance;
    private AudioSource aSource;

    private void Start()
    {
        aSource = GetComponent<AudioSource>();
    }

    public void PlaySound(Sound sound, bool loop)
    {
        AudioClip clipToUse = sound1;
        switch (sound)
        {
            case Sound.SOUND1:
                clipToUse = sound1;
                break;
        }
        aSource.loop = loop;
        aSource.Play();
    }
}
