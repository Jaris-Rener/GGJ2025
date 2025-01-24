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
        GlobalStepManager.OnStep += PlayerTax;
    }

    private void OnDisable()
    {
        GlobalStepManager.OnStep -= PlayerTax;
    }

    private void PlayerTax()
    {
        if (money >= 0)
        {
            // Decrease positive money by 30%
            money *= 1 - taxRate;
        }
        else
        {
            // Increase debt (negative money) by 30%
            money *= 1 + taxRate;
        }

        Debug.Log(money);
    }
}
