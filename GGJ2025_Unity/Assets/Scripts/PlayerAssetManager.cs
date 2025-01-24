using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAssetManager : Singleton<PlayerAssetManager>
{
    public int money;
    public List<BuildingListing> Properties = new();

    public void AddBuilding(BuildingListing listing)
    {
        Properties.Add(listing);
    }
}
