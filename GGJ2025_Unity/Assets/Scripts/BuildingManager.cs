using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class BuildingManager : Singleton<BuildingManager>
{
    private readonly Dictionary<Location, List<BuildingListing>> _listings = new();

    [ContextMenu("Test")]
    private void Test()
    {
        for (int i = 0; i < 10; i++)
        {
            var listing = GenerateRandomBuilding();
            var location = GetRandomLocation();
            AddListing(listing, location);
            Debug.Log($"{location} - {listing}");
        }
    }

    private Location GetRandomLocation()
    {
        var locations = Enum.GetValues(typeof(Location));
        return (Location)locations.GetValue(Random.Range(0, locations.Length));
    }

    public IEnumerable<BuildingListing> GetAllListings()
        => _listings.Values.SelectMany(entry => entry);

    public IEnumerable<BuildingListing> GetListings(Location location)
    {
        return _listings.TryGetValue(location, out var listings)
            ? listings
            : Enumerable.Empty<BuildingListing>();
    }

    public void AddListing(BuildingListing listing, Location location)
    {
        if (_listings.TryGetValue(location, out var listings))
        {
            listings ??= new();
            listings.Add(listing);
        }
        else
        {
            listings = new List<BuildingListing> { listing };
            _listings.Add(location, listings);
        }
    }
    
    private BuildingListing GenerateRandomBuilding()
    {
        var house = new BuildingListing();
        house.Cost = Random.Range(100, 1000);
        var buildingTypes = Enum.GetNames(typeof(BuildingType));
        house.BuildingType = (BuildingType)Random.Range(0, buildingTypes.Length);
        return house;
    }
}