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
        BuildingManager.Instance.Test(); // Temp generate some random listing

        _instances.ClearObjects();
        foreach ((BuildingListing listing, Location location) in BuildingManager.Instance.GetAllListings())
        {
            var instance = Instantiate(_notificationUIPrefab, _notificationsRoot);
            instance.Setup(listing, location);
            instance.OnSelected += OnListingSelected;
        }
    }

    private void OnListingSelected(BuildingListing listing, Location location)
    {
        _listingPopup.Setup(listing, location);
    }
}