using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class TimeUpdateScript : MonoBehaviour
{
    public GameObject textRef;

    private float timeRemaining;

    public float timeRemainingFranticAudioTrigger = 30f;

    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _franticAudio;

    bool flipFlop = false;


    void Start()
    {
        // Initialize timeRemaining using the step manager values
        if (GlobalStepManager.Instance != null)
        {
            timeRemaining = GlobalStepManager.Instance.stepCount * GlobalStepManager.Instance.stepInterval;
            timeRemaining += GlobalStepManager.Instance.startDelayTime + GlobalStepManager.Instance.stepInterval + 1f;
        }
        else
        {
            Debug.LogError("GlobalStepManager instance is null!");
        }
    }

    void Update()
    {
        if (timeRemaining > 0)
        {

            if (!flipFlop && timeRemaining < timeRemainingFranticAudioTrigger) 
            {
                flipFlop = true;
                _audioSource.PlayOneShot(_franticAudio);
            }

            timeRemaining -= Time.deltaTime;

            // Prevent it from going negative
            if (timeRemaining < 0)
            {
                timeRemaining = 0;
            }

            // Calculate minutes and seconds
            int minutes = Mathf.FloorToInt(timeRemaining / 60f);
            int seconds = Mathf.FloorToInt(timeRemaining % 60f);

            if (textRef != null)
            {
                TextMeshProUGUI textMeshProRef = textRef.GetComponent<TextMeshProUGUI>();
                if (textMeshProRef != null)
                {
                    textMeshProRef.SetText($"{minutes:D2}:{seconds:D2}");
                }
                else
                {
                    Debug.LogError("TextMeshProUGUI component not found on textRef!");
                }
            }
        }
    }
}