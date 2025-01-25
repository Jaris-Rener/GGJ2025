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

    [SerializeField] private List<Building> _buildings = new();
    private Stack<BuildingListing> _listingPool;
    
    public override void Awake()
    {
        base.Awake();

        var listings = new List<BuildingListing>();
        foreach (var building in _buildings)
        {
            var lifetime = Random.Range(_minListingTime, _maxListingTime);
            var listing = Util.GenerateListingFromBuilding(building, lifetime);
            listings.Add(listing);
        }

        listings.Shuffle();
        _listingPool = new Stack<BuildingListing>(listings);
    }

    public BuildingListing GetListing()
    {
        var listing = _listingPool.Pop();
        return listing;
    }

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
                AddListing(GetListing());
            }
        }
    }

    private void CreateInitialListings()
    {
        for (int i = 0; i < _initialListingCount; i++)
        {
            var lifetime = Random.Range(_minListingTime, _maxListingTime);
            AddListing(GetListing());
        }
    }
    
    public IEnumerable<BuildingListing> GetListings(Location location)
        => _listings.Where(x => x.Location == location);

    public void AddListing(BuildingListing listing)
    {
        listing.Name = _listingNamePool.GetRandom();
        listing.CreatedTime = Time.time;
        
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