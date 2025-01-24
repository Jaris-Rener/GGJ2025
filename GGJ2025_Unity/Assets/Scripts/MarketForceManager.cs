using System;
using UnityEngine;

public class MarketForceManager : Singleton<MarketForceManager>
{
    private int beachPriceLevel = 0;
    private int suburbPriceLevel = 0;
    private int cityPriceLevel = 0;

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

        // Log the results for debugging
        Debug.Log($"Market Update: Beach Level = {beachPriceLevel}, Suburb Level = {suburbPriceLevel}, City Level = {cityPriceLevel}");
    }

    private int RollDie()
    {
        // Roll a 5-sided die (-2, -1, 0, 1, 2)
        return UnityEngine.Random.Range(-2, 3); // Random.Range is inclusive of the lower bound and exclusive of the upper bound
    }
}
