using System;
using UnityEngine;
using UnityEngine.UI;

public class FullListingUI : ListingUI
{
    public event Action<BuildingListing> OnShow;
    public event Action OnHide;

    [SerializeField] private Button _buyButton;
    [SerializeField] private Button _sellButton;

    private void Awake() => Hide();

    public override void Show(bool playAudio = true)
    {
        base.Show(playAudio);
        OnShow?.Invoke(Listing);
    }

    public override void Hide(bool playAudio = true)
    {
        base.Hide(playAudio);
        OnHide?.Invoke();
    }

    protected override void Start()
    {
        base.Start();
        
        if (_buyButton != null)
            _buyButton.onClick.AddListener(BuyListing);
        
        if (_sellButton != null) 
            _sellButton.onClick.AddListener(SellListing);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        
        if (_buyButton != null) 
            _buyButton.onClick.RemoveListener(BuyListing);
        
        if (_sellButton != null) 
            _sellButton.onClick.RemoveListener(SellListing);
    }

    private void SellListing()
    {
        PlayerAssetManager.Instance.Sell(Listing);
        PlayerAssetManager.totalPropertiesSold++;

        // Increment based on location
        switch (Listing.Location)
        {
            case Location.Beach:
                PlayerAssetManager.beachPropertiesSold++;
                break;
            case Location.City:
                PlayerAssetManager.cityPropertiesSold++;
                break;
            case Location.Suburbs:
                PlayerAssetManager.subrubPropertiesSold++;
                break;
            default:
                Debug.LogWarning("Unknown location.");
                break;
        }
    }

    private void BuyListing()
    {
        var success = PlayerAssetManager.Instance.Buy(Listing);
        if (success)
        {
            BuildingManager.Instance.RemoveListing(Listing);
            PlayerAssetManager.totalPropertiesBought++;

            // Increment based on location
            switch (Listing.Location)
            {
                case Location.Beach:
                    PlayerAssetManager.beachPropertiesBought++;
                    break;
                case Location.City:
                    PlayerAssetManager.cityPropertiesBought++;
                    break;
                case Location.Suburbs:
                    PlayerAssetManager.suburbPropertiesBought++;
                    break;
                default:
                    Debug.LogWarning("Unknown location.");
                    break;
            }
        }
       
    }
}