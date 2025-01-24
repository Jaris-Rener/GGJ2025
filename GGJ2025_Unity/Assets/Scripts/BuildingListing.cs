using System;

public class BuildingListing
{
    public string Name;
    public BuildingType BuildingType;
    public int Cost;
    public float Lifetime;
    public float CreatedTime;

    public override string ToString()
    {
        return $"{BuildingType} @ ${Cost}K";
    }

    public Action OnExpired;
}