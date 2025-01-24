using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class BuildingManager : Singleton<BuildingManager>
{
    [SerializeField] private int _initialListingCount = 3;
    [SerializeField] private float _minListingDelay = 3;
    [SerializeField] private float _maxListingDelay = 10;
    [SerializeField] private int _minNewListings = 1;
    [SerializeField] private int _maxNewListings = 2;
    [SerializeField] private float _minListingTime = 6;
    [SerializeField] private float _maxListingTime = 12;
    
    private readonly Dictionary<Location, List<BuildingListing>> _listings = new();

    private void Start()
    {
        CreateInitialListings();
        StartCoroutine(GenerateListings());
    }

    private void Update()
    {
        foreach (var (location, list) in _listings)
        {
            for (var i = list.Count - 1; i >= 0; i--)
            {
                var listing = list[i];
                var curTime = Time.time - listing.CreatedTime;
                if (curTime > listing.Lifetime)
                    RemoveListing(location, listing);
            }
        }
    }

    private IEnumerator GenerateListings()
    {
        while (true)
        {
            var delay = Random.Range(_minListingDelay, _maxListingDelay);
            yield return new WaitForSeconds(delay);

            var newListings = Random.Range(_minNewListings, _maxNewListings);
            for (var i = 0; i < newListings; i++)
            {
                var lifetime = Random.Range(_minListingTime, _maxListingTime);
                AddListing(Util.GenerateRandomBuilding(lifetime), Util.GetRandomLocation());
            }
        }
    }

    private void CreateInitialListings()
    {
        for (int i = 0; i < _initialListingCount; i++)
        {
            var lifetime = Random.Range(_minListingTime, _maxListingTime);
            AddListing(Util.GenerateRandomBuilding(lifetime), Util.GetRandomLocation());
        }
    }

    public IEnumerable<(BuildingListing, Location)> GetAllListings()
    {
        foreach (var entry in _listings)
        foreach (var listing in entry.Value)
            yield return (listing, entry.Key);
    }

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
    
    private void RemoveListing(Location location, BuildingListing listing)
    {
        if (_listings.TryGetValue(location, out var listings))
            listings?.Remove(listing);
    }
}