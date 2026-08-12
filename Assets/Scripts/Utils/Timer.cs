using System;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public bool Autostart = false;
    public bool OneShot = false;
    public bool Paused = false;
    public float WaitTime = 1f;
    public float TimeLeft { get; private set; }
    public event Action OnTimeout;

    private bool isRunning = false;

    void Start()
    {
        TimeLeft = WaitTime;

        if (Autostart)
        {
            StartTimer();
        }
    }

    void Update()
    {
        if (!isRunning || Paused) return;

        TimeLeft -= Time.deltaTime;

        if (TimeLeft <= 0f)
        {
            TimeLeft = 0f;
            OnTimeout?.Invoke();

            if (OneShot)
            {
                StopTimer();
            }
            else
            {
                // Reset for repeating timer
                TimeLeft = WaitTime;
            }
        }
    }

    public void StartTimer()
    {
        TimeLeft = WaitTime;
        isRunning = true;
        Paused = false;
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public void SetPaused(bool pauseState)
    {
        Paused = pauseState;
    }
}