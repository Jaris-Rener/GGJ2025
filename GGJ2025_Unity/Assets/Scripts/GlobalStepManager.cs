using EasyRoads3Dv3;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalStepManager : Singleton<GlobalStepManager>
{
    // The event that other classes can subscribe to
    public static event Action OnBeginStep;
    public static event Action OnStep;
    public static event Action OnEndStep;
    public AudioSource _audioSource;
    public AudioClip _TimerClip;
    [SerializeField] private List<AudioClip> _WinGameClips;
    [SerializeField] private List<AudioClip> _LoseGameClips;

    private GraphHandler GraphHandlerPrefab;

    [SerializeField]
    private Vector3 spawnPosition = Vector3.zero;

    // Interval in seconds between each step
    [SerializeField]
    private float stepInterval = 10.0f;

    [SerializeField]
    private int stepCount = 15;

    [SerializeField]
    private float startDelayTime = 1f;

    private int currentStepCount = 0;

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
        GraphHandlerPrefab.gameObject.SetActive(false);
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

        // Delay for 2 seconds (change duration as needed)
        yield return new WaitForSeconds(0.5f);

        // Activate the GraphHandlerPrefab and create the graph
        if (GraphHandlerPrefab != null)
        {
            GraphHandlerPrefab.gameObject.SetActive(true);

            float moneyMin = float.MaxValue;
            float moneyMax = float.MinValue;

            for (int i = 0; i < PlayerAssetManager.moneyChanged.Count; i++)
            {
                GraphHandlerPrefab.CreatePoint(new Vector2(i, PlayerAssetManager.moneyChanged[i]));
                moneyMax = Mathf.Max(moneyMax, PlayerAssetManager.moneyChanged[i]);
                moneyMin = Mathf.Min(moneyMin, PlayerAssetManager.moneyChanged[i]);
            }

            GraphHandlerPrefab.SetCornerValues(new Vector2(-1f, moneyMin), new Vector2(PlayerAssetManager.moneyChanged.Count + 1, moneyMax));
            GraphHandlerPrefab.UpdateGraph();

            Debug.Log("GraphHandlerPrefab updated after delay.");
        }
        else
        {
            Debug.LogWarning("GraphHandlerPrefab is not assigned.");
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
