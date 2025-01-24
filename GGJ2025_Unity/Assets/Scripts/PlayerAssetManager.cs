using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAssetManager : Singleton<PlayerAssetManager>
{
    public float money = 1000.0f;
    public float taxRate = 0.3f;
    public List<BuildingListing> Properties = new();

    public void AddBuilding(BuildingListing listing)
    {
        Properties.Add(listing);
    }
    
    private void OnEnable()
    {
        GlobalStepManager.OnStep += TaxStep;
    }

    private void OnDisable()
    {
        GlobalStepManager.OnStep -= TaxStep;
    }

    private void TaxStep()
    {
        Debug.Log(money = TaxPlayer());
        Debug.Log(TaxPlayer());
    }

    public float TaxPlayer()
    {
        if (money >= 0)
        {
            // Decrease positive money by 30%
            return money * (1 - taxRate);
        }
        else
        {
            // Increase debt (negative money) by 30%
            return money * (1 + taxRate);
        }
    }
}
