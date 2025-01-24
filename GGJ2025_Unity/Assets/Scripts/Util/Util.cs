using System;
using System.Collections.Generic;
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

    public static BuildingListing GenerateRandomBuilding()
    {
        var building = new BuildingListing();
        building.Cost = Random.Range(100, 1000);
        var buildingTypes = Enum.GetNames(typeof(BuildingType));
        building.BuildingType = (BuildingType)Random.Range(0, buildingTypes.Length);
        
        var randomNumber = Random.Range(0, 100);
        building.Name = $"[PLACEHOLDER NAME {randomNumber}]";
        
        return building;
    }
}