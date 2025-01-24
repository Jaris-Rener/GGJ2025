using System;
using System.Collections.Generic;
using UnityEngine;

public class ListingsUI : MonoBehaviour
{
    [SerializeField] private Transform _notificationsRoot;
    
    [SerializeField] private ListingNotificationUI _notificationUIPrefab;
    [SerializeField] private ListingUI _listingPopup;

    private readonly List<ListingUI> _instances = new();
    
    private void Start()
    {
        BuildingManager.Instance.OnListingCreated += AddListing;
    }

    private void OnDestroy()
    {
        BuildingManager.Instance.OnListingCreated -= AddListing;
    }

    private void AddListing(BuildingListing listing, Location location)
    {
        var instance = Instantiate(_notificationUIPrefab, _notificationsRoot);
        instance.Setup(listing, location);
        instance.OnSelected += OnListingSelected;
        _instances.Add(instance);
        listing.OnExpired += () =>
        {
            _instances.Remove(instance);
            Destroy(instance.gameObject);
        };
    }

    private void OnListingSelected(BuildingListing listing, Location location)
    {
        _listingPopup.Setup(listing, location);
    }
}