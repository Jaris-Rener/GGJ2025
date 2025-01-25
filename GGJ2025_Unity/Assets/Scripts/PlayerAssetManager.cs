using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAssetManager : Singleton<PlayerAssetManager>
{
    [SerializeField] private int _maxProperties = 10;
    
    public event Action<float> OnMoneyChanged;
    public event Action<BuildingListing> OnPropertyAdded;
    public event Action<BuildingListing> OnPropertyRemoved;

    public float startingMoney = 5000.0f;
    public float money = 5000.0f;
    public float taxRate = 0.3f;
    public float minimumTaxAmount = 50.0f;
    public List<BuildingListing> Properties = new();

    public static int propertiesBought = 0;
    public static int propertiesSold = 0;

    public static List<float> moneyChanged = new();
    private float currentMoney = 0.0f;



    private void Start()
    {
        OnMoneyChanged?.Invoke(money);
    }

    private void Update()
    {
        if (currentMoney != money) 
        {
            Debug.Log("Money Changed");
            currentMoney = money;
            moneyChanged.Add(money);
        }
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
        BuildingManager.Instance.Return(listing);
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
