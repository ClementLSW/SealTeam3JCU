using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{
    private float targetTime;
    private float timeStarted;

    public void SetTimer(float duration)
    {
        timeStarted = Time.time;
        targetTime = Time.time + duration;
    }

    public bool TimeIsUp => Time.time >= targetTime;
    public float TimeLeft => targetTime - Time.time;
    public float PrecTimeLeft => Mathf.Abs(targetTime - Time.time) / timeStarted;
}
