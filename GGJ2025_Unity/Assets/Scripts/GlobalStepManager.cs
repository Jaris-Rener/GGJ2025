using EasyRoads3Dv3;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalStepManager : Singleton<GlobalStepManager>
{
    // The event that other classes can subscribe to
    public event Action OnBeginStep;
    public event Action OnStep;
    public event Action OnEndStep;
    public AudioSource _audioSource;
    public AudioClip _TimerClip;
    [SerializeField] private List<AudioClip> _WinGameClips;
    [SerializeField] private List<AudioClip> _LoseGameClips;

    private GraphHandler GraphHandlerPrefab;

    [SerializeField]
    private Vector3 spawnPosition = Vector3.zero;

    // Interval in seconds between each step
    [SerializeField]
    public float stepInterval = 10.0f;

    [SerializeField]
    public int stepCount = 15;

    [SerializeField]
    public float startDelayTime = 1f;

    public int currentStepCount = 0;

    public static bool gameStarted = false;
    public static bool endTriggered = false;

    public float LastStepTime { get; private set; }
    public float NextStepTime { get; private set; }
    public float StartTime { get; private set; }

    private void Start()
    {
        StartCoroutine(DelayedStart());

        // Find and cache the GraphHandler
        GraphHandlerPrefab = FindObjectOfType<GraphHandler>();

        if (GraphHandlerPrefab == null)
        {
            Debug.LogError("GraphHandler not found in the scene.");
            return;
        }
    }

    private IEnumerator StepMarket()
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
            }
            else
            {
                currentStepCount++;
                TriggerStep();
            }
        }
    }

    private void Update()
    {
        var debugSpeedUp = Input.GetKey(KeyCode.Backslash);
        Time.timeScale = debugSpeedUp ? 20 : 1;
    }

    private void TriggerBeginStep()
    {
        Debug.Log("Begin Market");
        gameStarted = true;
        OnBeginStep?.Invoke();
        StartCoroutine(StepMarket());
    }

    private void TriggerStep()
    {
        OnStep?.Invoke();
        _audioSource.PlayOneShot(_TimerClip);
    }

    private void TriggerEndStep()
    {
        OnEndStep?.Invoke();

        GraphHandlerPrefab.gameObject.SetActive(true);
        StartCoroutine(DelayedExecution());
    }

    private IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(startDelayTime);
        TriggerBeginStep();
        GraphHandlerPrefab.gameObject.SetActive(false);
    }

    private IEnumerator DelayedExecution()
    {
        // Play the appropriate audio clip
        if (PlayerAssetManager.Instance.money > PlayerAssetManager.Instance.startingMoney)
        {
            var winClip = _WinGameClips.GetRandom();
            _audioSource.PlayOneShot(winClip);
        }
        else
        {
            var loseClip = _LoseGameClips.GetRandom();
            _audioSource.PlayOneShot(loseClip);
        }

        yield break;
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
