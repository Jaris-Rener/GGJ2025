using EasyRoads3Dv3;
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

    private GraphHandler GraphHandlerPrefab;

    [SerializeField]
    private Vector3 spawnPosition = Vector3.zero;

    // Interval in seconds between each step
    [SerializeField]
    private float stepInterval = 10.0f;

    [SerializeField]
    private int stepCount = 15;

    private int currentStepCount = 0;

    static public bool endTriggered = false;
    
    public float LastStepTime { get; private set; }
    public float NextStepTime { get; private set; }
    public float StartTime { get; private set; }

    private void Start()
    {
        // Start the coroutine to trigger steps
        StartCoroutine(StepCoroutine());

        // Find and cache the GraphHandler
        GraphHandlerPrefab = FindObjectOfType<GraphHandler>();

        if (GraphHandlerPrefab == null)
        {
            Debug.LogError("GraphHandler not found in the scene.");
            return;
        }
        GraphHandlerPrefab.gameObject.SetActive(false);
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

        GraphHandlerPrefab.gameObject.SetActive(true);

        // Create the graph
        if (GraphHandlerPrefab != null)
        {
            float moneyMin = 9999999999.0f;
            float moneyMax = -9999999999.0f;
            for (int i = 0; i < PlayerAssetManager.moneyChanged.Count; i++)
            {
                Debug.Log("CreatePoint");
                GraphHandlerPrefab.CreatePoint(new Vector2(i, PlayerAssetManager.moneyChanged[i]));
                if (PlayerAssetManager.moneyChanged[i] > moneyMax)
                    moneyMax = PlayerAssetManager.moneyChanged[i];

                if (PlayerAssetManager.moneyChanged[i] < moneyMin)
                    moneyMin = PlayerAssetManager.moneyChanged[i];
            }
            GraphHandlerPrefab.SetCornerValues(new Vector2(0f, moneyMin), new Vector2(PlayerAssetManager.moneyChanged.Count, moneyMax));
            GraphHandlerPrefab.UpdateGraph();
            Debug.Log("End prefab spawned.");
        }
        else
        {
            Debug.LogWarning("End prefab is not assigned.");
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
