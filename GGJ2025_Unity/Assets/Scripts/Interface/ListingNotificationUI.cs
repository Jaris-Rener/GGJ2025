using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ListingNotificationUI : ListingUI, IPointerClickHandler
{
    private BuildingListing _listing;
    private Location _listingLocation;

    // TODO: Subscribe to an event from the notification manager rather than accessing directly
    [SerializeField] private FullListingUI _listingUI;

    private void Awake()
    {
        _listing = Util.GenerateRandomBuilding();
        _listingLocation = Util.GetRandomLocation();
        
        Setup(_listing, _listingLocation);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _listingUI.Setup(_listing, _listingLocation);
    }
}