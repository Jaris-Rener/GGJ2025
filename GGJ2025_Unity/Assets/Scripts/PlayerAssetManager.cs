using System;
using UnityEngine;

public class PlayerAssetManager : Singleton<PlayerAssetManager>
{
    public float money = 1000.0f;
    public float taxRate = 0.3f;

    private void OnEnable()
    {
        GlobalStepManager.OnStep += PlayerTax;
    }

    private void OnDisable()
    {
        GlobalStepManager.OnStep -= PlayerTax;
    }

    private void PlayerTax()
    {
        // Calculate the reduction factor (e.g., 30% = 0.3, so we use 0.7 to keep 70%)
        float reductionFactor = 1 - taxRate;

        // Adjust the absolute value and preserve the original sign
        money = Mathf.Abs(money) * reductionFactor;

        Debug.Log(money);
    }
}