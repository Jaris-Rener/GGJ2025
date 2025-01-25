using System;
using UnityEngine;
using UnityEngine.UI;

public class MarketTimerUI : MonoBehaviour
{
    [SerializeField] private Image _fill;
    
    private void Update()
    {
        _fill.fillAmount = 1 - Mathf.InverseLerp(
            GlobalStepManager.Instance.LastStepTime,
            GlobalStepManager.Instance.NextStepTime,
            Time.time);
    }
}