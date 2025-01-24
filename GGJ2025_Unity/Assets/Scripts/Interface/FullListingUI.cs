using System;
using UnityEngine;
using UnityEngine.UI;

public class FullListingUI : ListingUI
{
    [SerializeField] private Button _buyButton;

    private void Start()
    {
        _buyButton.onClick.AddListener(BuyListing);
    }

    private void OnDestroy()
    {
        _buyButton.onClick.RemoveListener(BuyListing);
    }

    private void BuyListing()
    {
        var success = PlayerAssetManager.Instance.Buy(Listing);
        if (success)
            BuildingManager.Instance.RemoveListing(Listing);
    }
}