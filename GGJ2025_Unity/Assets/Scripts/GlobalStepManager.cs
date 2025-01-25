using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalStepManager : Singleton<GlobalStepManager>
{
    // The event that other classes can subscribe to
    public static event Action OnStep;
    public static event Action OnEndStep;
    public AudioSource _audioSource;
    public AudioClip _TimerClip;
    [SerializeField] private List<AudioClip> _WinGameClips;
    [SerializeField] private List<AudioClip> _LoseGameClips;

    // Interval in seconds between each step
    [SerializeField]
    private float stepInterval = 10.0f;

    [SerializeField]
    private int stepCount = 15;

    private int currentStepCount = 0;

    bool endTriggered = false;
    
    public float LastStepTime { get; private set; }
    public float NextStepTime { get; private set; }
    public float StartTime { get; private set; }

    private void Start()
    {
        // Start the coroutine to trigger steps
        StartCoroutine(StepCoroutine());
    }

    private IEnumerator StepCoroutine()
    {
        StartTime = Time.time;
        while (endTriggered == false)
        {
            LastStepTime = Time.time;
            NextStepTime = Time.time + stepInterval;
            yield return new WaitForSeconds(stepInterval);
            if (currentStepCount == stepCount)
            {
                TriggerEndStep();
                endTriggered = true;
                Debug.Log("THE END");
            }
            else
            {
                currentStepCount++;
                TriggerStep();
            }
        }
    }


    private void TriggerStep()
    {
        OnStep?.Invoke();
        _audioSource.PlayOneShot(_TimerClip);
    }

    private void TriggerEndStep()
    {
        OnEndStep?.Invoke();
        if (PlayerAssetManager.Instance.money > PlayerAssetManager.Instance.startingMoney) 
        {
            var winclip = _WinGameClips.GetRandom();
            _audioSource.PlayOneShot(winclip);
        }
        else
        {
            var loseclip = _LoseGameClips.GetRandom();
            _audioSource.PlayOneShot(loseclip);
        }
    }

    public void SetStepInterval(float interval)
    {
        if (interval > 0)
        {
            stepInterval = interval;
        }
        else
        {
            Debug.LogWarning("Step interval must be greater than 0.");
        }
    }
}
