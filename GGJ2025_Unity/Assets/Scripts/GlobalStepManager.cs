using System;
using System.Collections;
using UnityEngine;

public class GlobalStepManager : Singleton<GlobalStepManager>
{
    // The event that other classes can subscribe to
    public static event Action OnStep;
    public static event Action OnEndStep;

    // Interval in seconds between each step
    [SerializeField]
    private float stepInterval = 10.0f;

    [SerializeField]
    private int stepCount = 15;

    private float currentStepCount = 0;

    bool endTriggered = false;
    
    public float LastStepTime { get; private set; }
    public float NextStepTime { get; private set; }
    
    private void Start()
    {
        // Start the coroutine to trigger steps
        StartCoroutine(StepCoroutine());
    }

    private IEnumerator StepCoroutine()
    {
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
    }

    private void TriggerEndStep()
    {
        OnEndStep?.Invoke();
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
