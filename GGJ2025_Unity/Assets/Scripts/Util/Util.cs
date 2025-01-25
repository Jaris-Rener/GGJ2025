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

    public static Location GetRandomLocation()
    {
        var locations = Enum.GetValues(typeof(Location));
        return (Location)locations.GetValue(Random.Range(0, locations.Length));
    }

    public static T GetRandom<T>(this List<T> list)
    {
        return list[Random.Range(0, list.Count)];
    }

    public static BuildingListing GenerateRandomBuilding(float lifetime)
    {
        var building = new BuildingListing();
        var buildingTypes = Enum.GetNames(typeof(BuildingType));
        building.BuildingType = (BuildingType)Random.Range(0, buildingTypes.Length);
        building.BaseCost = GetCost(building.BuildingType);
        building.Lifetime = lifetime;
        building.CreatedTime = Time.time;

        return building;
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
}