using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAssetManager : Singleton<PlayerAssetManager>
{
    public event Action<float> OnMoneyChanged;
    public event Action<BuildingListing> OnPropertyAdded;
    public event Action<BuildingListing> OnPropertyRemoved;
    
    public float money = 1000.0f;
    public float taxRate = 0.3f;
    public List<BuildingListing> Properties = new();

    private void Start()
    {
        OnMoneyChanged?.Invoke(money);
    }

    public bool Buy(BuildingListing listing)
    {
        if (money < listing.Cost)
        {
            Debug.Log($"Cannot afford {listing}");
            return false;
        }

        listing.Lifetime = -1;
        money -= listing.Cost;
        OnMoneyChanged?.Invoke(money);
        Properties.Add(listing);
        OnPropertyAdded?.Invoke(listing);
        return true;
    }

    public bool Sell(BuildingListing listing)
    {
        money += listing.Cost;
        OnMoneyChanged?.Invoke(money);
        Properties.Remove(listing);
        OnPropertyRemoved?.Invoke(listing);
        return true;
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
            OnMoneyChanged?.Invoke(money);
        }
        else
        {
            // Increase debt (negative money) by 30%
            money *= 1 + taxRate;
            OnMoneyChanged?.Invoke(money);
        }

        Debug.Log(money);
    }
}
