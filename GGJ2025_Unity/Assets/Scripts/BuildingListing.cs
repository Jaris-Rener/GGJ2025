using System;

public class BuildingListing
{
    public Location Location;
    public string Name;
    public BuildingType BuildingType;
    public int Cost;
    public float Lifetime;
    public float CreatedTime;

    public override string ToString()
    {
        return $"{BuildingType} @ {Location} @ ${Cost}K";
    }

    public Action OnRemoved;
}