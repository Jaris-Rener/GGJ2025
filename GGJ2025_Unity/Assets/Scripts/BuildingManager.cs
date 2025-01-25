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

    [SerializeField] private AnimationCurve _speedMultiplier;
    [SerializeField] private int _minListings = 2;
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

    private void Start()
    {
        CreateInitialListings();
        StartCoroutine(GenerateListings());
    }

    public BuildingListing GetListing()
    {
        if (_listingPool.Count <= 0)
        {
            _soldBuildings.Shuffle();
            foreach (var building in _soldBuildings)
            {
                _listingPool.Push(building);
            }
            
            _soldBuildings.Clear();
        }
        
        var listing = _listingPool.Pop();
        listing.CreatedTime = Time.time;
        return listing;
    }

    private void Update()
    {
        for (var i = _listings.Count - 1; i >= 0; i--)
        {
            var listing = _listings[i];
            var curTime = Time.time - listing.CreatedTime;
            if (curTime > listing.Lifetime)
            {
                RemoveListing(listing, true);
            }
        }
    }

    private IEnumerator GenerateListings()
    {
        while (!GlobalStepManager.endTriggered)
        {
            var delay = Random.Range(_minListingDelay, _maxListingDelay);
            delay *= _speedMultiplier.Evaluate(Time.time - GlobalStepManager.Instance.StartTime);
            yield return new WaitForSeconds(delay);

            var newListings = Random.Range(_minNewListings, _maxNewListings);
            for (var i = 0; i < newListings; i++)
                AddListing(GetListing());
        }
    }

    private void CreateInitialListings()
    {
        for (int i = 0; i < _initialListingCount; i++)
        {
            AddListing(GetListing());
        }
    }

    public void AddListing(BuildingListing listing)
    {
        listing.CreatedTime = Time.time;
        
        _listings.Add(listing);
        OnListingCreated?.Invoke(listing);
        Debug.Log($"New listing {listing}");
    }

    public void RemoveListing(BuildingListing listing, bool returnToPool = false)
    {
        _listings.Remove(listing);
        OnListingRemoved?.Invoke(listing);
        Debug.Log($"Listing removed {listing}");
        
        if (returnToPool)
            Return(listing);

        // Stop generating listings when end is triggered
        if (GlobalStepManager.endTriggered) return;
        if (_listings.Count < _minListings)
        {
            for (int i = 0; i < _minListings - _listings.Count; i++)
            {
                AddListing(GetListing());
            }
        }
    }

    private readonly List<BuildingListing> _soldBuildings = new();
    public void Return(BuildingListing listing)
    {
        _soldBuildings.Add(listing);
    }
}