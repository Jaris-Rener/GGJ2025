using System;

public class BuildingListing
{
    public float CurrentCost => BaseCost * MarketForceManager.Instance.GetMultiplier(Location);
    public Building Building { get; set; }

    public Location Location;
    public string Name;
    public BuildingType BuildingType;
    public float BuyCost;
    public float BaseCost;
    public float Lifetime;
    public float CreatedTime;

    public override string ToString()
    {
        return $"{BuildingType} @ {Location} @ ${CurrentCost}K";
    }

    public Action OnRemoved;
}