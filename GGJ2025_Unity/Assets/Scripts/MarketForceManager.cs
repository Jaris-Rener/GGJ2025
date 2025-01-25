using System;
using UnityEngine;

public class MarketForceManager : Singleton<MarketForceManager>
{
    public event Action OnMarketUpdated;
    
    public int beachPriceLevel = 0;
    public int suburbPriceLevel = 0;
    public int cityPriceLevel = 0;

    private int priceLevelDie = 5;

    private void OnEnable()
    {
        GlobalStepManager.OnStep += UpdateMarket;
    }

    private void OnDisable()
    {
        GlobalStepManager.OnStep -= UpdateMarket;
    }

    private void UpdateMarket()
    {
        // Roll a 5-sided die for each category
        int beachRoll = RollDie();
        int suburbRoll = RollDie();
        int cityRoll = RollDie();

        // Apply rolls to price levels, clamping the result between -2 and 2 for each category
        beachPriceLevel = Mathf.Clamp(beachPriceLevel + beachRoll, -2, 2);
        suburbPriceLevel = Mathf.Clamp(suburbPriceLevel + suburbRoll, -2, 2);
        cityPriceLevel = Mathf.Clamp(cityPriceLevel + cityRoll, -2, 2);

        OnMarketUpdated?.Invoke();
        
        // Log the results for debugging
        Debug.Log($"Market Update: Beach Level = {beachPriceLevel}, Suburb Level = {suburbPriceLevel}, City Level = {cityPriceLevel}");
    }

    private int RollDie()
    {
        // Calculate the range dynamically based on priceLevelDie
        int min = -priceLevelDie / 2;
        int max = priceLevelDie / 2 + 1;

        // Roll the die within the range
        return UnityEngine.Random.Range(min, max);
    }

    public int GetCurrentMarketForce(Location location)
    {
        return location switch
        {
            Location.Beach => beachPriceLevel,
            Location.City => cityPriceLevel,
            Location.Suburbs => suburbPriceLevel,
            _ => throw new ArgumentOutOfRangeException(nameof(location), location, null)
        };
    }

    public int GetMarketDirection(Location location)
    {
        return GetNextMarketForce(location) - GetCurrentMarketForce(location);
    }

    private int GetNextMarketForce(Location location)
    {
        return 0;
    }

    public float GetMultiplier(Location location)
    {
        return IntToFloatMapper.GetMultiplier(GetCurrentMarketForce(location));
    }
}
