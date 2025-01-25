using System;

public class BuildingListing
{
    public float CurrentCost => BaseCost * MarketForceManager.Instance.GetMultiplier(Location);
    public Location Location;
    public string Name;
    public BuildingType BuildingType;
    public float BaseCost;
    public float Lifetime;
    public float CreatedTime;

    public override string ToString()
    {
        return $"{BuildingType} @ {Location} @ ${CurrentCost}K";
    }

    public Action OnRemoved;
}