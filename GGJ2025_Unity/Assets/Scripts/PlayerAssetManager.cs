using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAssetManager : Singleton<PlayerAssetManager>
{
    [SerializeField] private int _maxProperties = 10;
    
    public event Action<float> OnMoneyChanged;
    public event Action<BuildingListing> OnPropertyAdded;
    public event Action<BuildingListing> OnPropertyRemoved;
    
    public float money = 1000.0f;
    public float taxRate = 0.3f;
    public float minimumTaxAmount = 50.0f;
    public List<BuildingListing> Properties = new();

    private void Start()
    {
        OnMoneyChanged?.Invoke(money);
    }

    public bool Buy(BuildingListing listing)
    {
        if (Properties.Count >= _maxProperties)
        {
            Debug.Log("Too many properties owned");
            return false;
        }
        
        // if (money < listing.CurrentCost)
        // {
        //     Debug.Log($"Cannot afford {listing}");
        //     return false;
        // }

        listing.Lifetime = -1;
        listing.BuyCost = listing.CurrentCost;
        money -= listing.CurrentCost;
        OnMoneyChanged?.Invoke(money);
        Properties.Add(listing);
        OnPropertyAdded?.Invoke(listing);
        return true;
    }

    public bool Sell(BuildingListing listing)
    {
        if (!Properties.Remove(listing))
            return false;
        
        money += listing.CurrentCost;
        OnMoneyChanged?.Invoke(money);
        OnPropertyRemoved?.Invoke(listing);
        return true;
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
        money = TaxPlayer();
        OnMoneyChanged?.Invoke(money);
        Debug.Log(money);

        // Preview tax for next TaxStep
        Debug.Log(TaxPlayer());
    }

    public float TaxPlayer()
    {
        float taxedAmount;

        if (money >= 0)
        {
            // Decrease positive money by 30%
            taxedAmount = money * (1 - taxRate);
        }
        else
        {
            // Increase debt (negative money) by 30%
            taxedAmount = money * (1 + taxRate);
        }

        // Ensure taxed amount is at least the minimum tax amount
        if (money - taxedAmount < minimumTaxAmount)
        {
            taxedAmount = money - minimumTaxAmount;
        }

        return taxedAmount;
    }
}
