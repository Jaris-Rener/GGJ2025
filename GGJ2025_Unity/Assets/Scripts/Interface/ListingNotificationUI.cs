using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ListingNotificationUI : ListingUI, IPointerClickHandler
{
    public event Action<BuildingListing> OnSelected;

    public void OnPointerClick(PointerEventData eventData)
    {
        OnSelected?.Invoke(Listing);
    }
}