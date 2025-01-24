using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BuildingManager : MonoBehaviour
{
    private readonly List<BuildingListing> _listings = new();
    
    [ContextMenu("Test")]
    private void Test()
    {
        for (int i = 0; i < 10; i++)
        {
            var house = GenerateRandomHouse();
            Debug.Log(house);
            _listings.Add(house);
        }
    }

    private BuildingListing GenerateRandomHouse()
    {
        var house = new BuildingListing();
        house.Cost = Random.Range(100, 1000);
        var buildingTypes = Enum.GetNames(typeof(BuildingType));
        house.BuildingType = (BuildingType)Random.Range(0, buildingTypes.Length);
        return house;
    }
}

public enum BuildingType
{
    Apartment = 0,
    House = 1,
    Mansion = 2
}

public class BuildingListing
{
    public BuildingType BuildingType;
    public int Cost;

    public override string ToString()
    {
        return $"{BuildingType} @ ${Cost}K";
    }
}