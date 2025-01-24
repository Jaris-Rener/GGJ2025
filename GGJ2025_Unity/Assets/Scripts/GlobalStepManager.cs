using System;
using System.Collections;
using UnityEngine;

public class GlobalStepManager : Singleton<GlobalStepManager>
{
    // The event that other classes can subscribe to
    public static event Action OnStep;

    // Interval in seconds between each step
    [SerializeField]
    private float stepInterval = 1.0f;
    
    private void Start()
    {
        // Start the coroutine to trigger steps
        StartCoroutine(StepCoroutine());
    }

    private IEnumerator StepCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(stepInterval);
            TriggerStep();
        }
    }

    private void TriggerStep()
    {
        // Invoke the event
        OnStep?.Invoke();
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
