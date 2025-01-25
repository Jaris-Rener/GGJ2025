using System;
using UnityEngine;
using UnityEngine.UI;

public class FullListingUI : ListingUI
{
    [SerializeField] private Button _buyButton;
    [SerializeField] private Button _sellButton;

    private void Awake() => Hide();

    private void Start()
    {
        if (_buyButton != null)
            _buyButton.onClick.AddListener(BuyListing);
        
        if (_sellButton != null) 
            _sellButton.onClick.AddListener(SellListing);
    }

    private void OnDestroy()
    {
        if (_buyButton != null) 
            _buyButton.onClick.RemoveListener(BuyListing);
        
        if (_sellButton != null) 
            _sellButton.onClick.RemoveListener(SellListing);
    }

    private void SellListing()
    {
        PlayerAssetManager.Instance.Sell(Listing);
    }

    private void BuyListing()
    {
        var success = PlayerAssetManager.Instance.Buy(Listing);
        if (success)
            BuildingManager.Instance.RemoveListing(Listing);
    }
}