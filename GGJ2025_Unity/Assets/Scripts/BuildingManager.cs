using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class BuildingManager : Singleton<BuildingManager>
{
    public event Action<BuildingListing> OnListingCreated;
    public event Action<BuildingListing> OnListingRemoved;

    [SerializeField] private List<string> _listingNamePool = new();
    
    [SerializeField] private int _initialListingCount = 3;
    [SerializeField] private float _minListingDelay = 3;
    [SerializeField] private float _maxListingDelay = 10;
    [SerializeField] private int _minNewListings = 1;
    [SerializeField] private int _maxNewListings = 2;
    [SerializeField] private float _minListingTime = 6;
    [SerializeField] private float _maxListingTime = 12;
    public IEnumerable<BuildingListing> AllListings => _listings;
    private readonly List<BuildingListing> _listings = new();

    private void Start()
    {
        CreateInitialListings();
        StartCoroutine(GenerateListings());
    }

    private void Update()
    {
        for (var i = _listings.Count - 1; i >= 0; i--)
        {
            var listing = _listings[i];
            var curTime = Time.time - listing.CreatedTime;
            if (curTime > listing.Lifetime)
            {
                RemoveListing(listing);
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
    
    public IEnumerable<BuildingListing> GetListings(Location location)
        => _listings.Where(x => x.Location == location);

    public void AddListing(BuildingListing listing, Location location)
    {
        listing.Name = _listingNamePool.GetRandom();
        listing.Location = location;
        _listings.Add(listing);
        OnListingCreated?.Invoke(listing);
        Debug.Log($"New listing {listing}");
    }

    public void RemoveListing(BuildingListing listing)
    {
        _listings?.Remove(listing);
        OnListingRemoved?.Invoke(listing);
        Debug.Log($"Listing removed {listing}");
    }
}