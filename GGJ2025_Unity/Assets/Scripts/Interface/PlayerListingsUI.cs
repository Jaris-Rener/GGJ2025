using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerListingsUI : MonoBehaviour
{
    [SerializeField] private Transform _listingsRoot;
    [SerializeField] private ListingNotificationUI _listingPrefab;

    private readonly List<ListingUI> _instances = new();
    
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
        _instances.Add(instance);
    }

    private void RemoveProperty(BuildingListing obj)
    {
        throw new NotImplementedException();
    }
}