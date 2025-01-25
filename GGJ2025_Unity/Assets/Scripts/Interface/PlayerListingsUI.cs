using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerListingsUI : MonoBehaviour
{
    [SerializeField] private Transform _listingsRoot;
    [SerializeField] private ListingNotificationUI _listingPrefab;
    [SerializeField] private FullListingUI _fullListingUI;

    private readonly List<ListingUI> _instances = new();
    
    public BuildingListing Selected { get; set; }
    
    private void Start()
    {
        PlayerAssetManager.Instance.OnPropertyAdded += AddProperty;
        PlayerAssetManager.Instance.OnPropertyRemoved += RemoveProperty;
    }

    private void OnDestroy()
    {
        PlayerAssetManager.Instance.OnPropertyAdded -= AddProperty;
        PlayerAssetManager.Instance.OnPropertyRemoved -= RemoveProperty;
    }
    
    private void AddProperty(BuildingListing listing)
    {
        var instance = Instantiate(_listingPrefab, _listingsRoot);
        instance.Setup(listing);
        instance.OnSelected += OnSelected;
        instance.Show();
        _instances.Add(instance);
    }

    private void OnSelected(BuildingListing listing)
    {
        _fullListingUI.Setup(listing);
        Selected = listing;
    }

    private void RemoveProperty(BuildingListing listing)
    {
        var instance = _instances.Find(x => x.Listing == listing);
        if (instance == null)
            return;
        
        Destroy(instance.gameObject);
        _instances.Remove(instance);
        
        if (Selected == listing)
        {
            Selected = null;
            _fullListingUI.Hide();
        }
    }
}