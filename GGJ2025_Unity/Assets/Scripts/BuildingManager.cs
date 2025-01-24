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
            var listing = Util.GenerateRandomBuilding();
            var location = Util.GetRandomLocation();
            AddListing(listing, location);
            Debug.Log($"{location} - {listing}");
        }
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
}