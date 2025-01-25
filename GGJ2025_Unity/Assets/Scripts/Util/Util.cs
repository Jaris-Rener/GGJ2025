using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public static class Util
{
    public static void ClearObjects<T>(this List<T> list) where T : Object
    {
        for (var i = list.Count - 1; i >= 0; i--)
        {
            var item = list[i];
            Object.Destroy(item);
        }
        
        list.Clear();
    }

    public static T GetRandom<T>(this List<T> list)
    {
        return list[Random.Range(0, list.Count)];
    }

    private static float GetCost(BuildingType type)
    {
        return type switch
        {
            BuildingType.Apartment => 500,
            BuildingType.House => 1000,
            BuildingType.Mansion => 2000,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }

    public static BuildingListing GenerateListingFromBuilding(Building building, float lifetime)
    {
        var listing = new BuildingListing();
        listing.Name = building.Name;
        listing.BuildingType = building.Type;
        listing.Location = building.Location;
        listing.BaseCost = GetCost(listing.BuildingType);
        listing.Lifetime = lifetime;
        listing.Building = building;

        return listing;
    }
    
    public static void Shuffle<T>(this IList<T> ts) {
        var count = ts.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i) {
            var r = Random.Range(i, count);
            (ts[i], ts[r]) = (ts[r], ts[i]);
        }
    }
}