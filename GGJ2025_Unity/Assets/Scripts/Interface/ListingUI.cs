using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ListingUI : MonoBehaviour
{
    public BuildingListing Listing { get; private set; }

    [SerializeField] private Image _timerImage;
    
    [SerializeField] private LocationSpriteLookup _locationIcons;
    [SerializeField] private BuildingTypeSpriteLookup _buildingIcons;
    [SerializeField] private IntToSpriteLookup _marketSprites;
    [SerializeField] private IntToColourLookup _marketColours;
    
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _cost;
    [SerializeField] private TextMeshProUGUI _buyCost;
    [SerializeField] private Image _locationIcon;
    [SerializeField] private Image _buildingTypeIcon;
    [SerializeField] private Image _projectionIcon;

    protected virtual void Start()
    {
        MarketForceManager.Instance.OnMarketUpdated += UpdateCost;
    }

    protected virtual void OnDestroy()
    {
        MarketForceManager.Instance.OnMarketUpdated -= UpdateCost;
    }

    private void UpdateCost()
    {
        SetCost(Listing.CurrentCost);
        
        if (_projectionIcon == null)
            return;

        var nextMarketForce = MarketForceManager.Instance.GetMarketDirection(Listing.Location);
        _projectionIcon.sprite = _marketSprites.Get(nextMarketForce);
        _projectionIcon.color = _marketColours.Get(nextMarketForce);
    }

    public void Setup(BuildingListing listing)
    {
        if (Listing == listing)
        {
            Toggle();
            return;
        }

        Listing = listing;
        
        _name.text = listing.Name;
        UpdateCost();

        if (listing.BuyCost > 0)
        {
            if (_buyCost != null)
                _buyCost.text = $"Bought at: ${listing.BuyCost}K";
        }
        else
        {
            if (_buyCost != null)
                _buyCost.text = $"Median price: ${listing.BaseCost}K";
        }

        _locationIcon.sprite = _locationIcons.Get(listing.Location);
        _buildingTypeIcon.sprite = _buildingIcons.Get(listing.BuildingType);

        Show();
    }

    private void SetCost(float cost)
    {
        _cost.text = $"${cost}K";
    }

    public void Toggle()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void OnListingRemoved()
    {
        Hide();
    }

    private void Update()
    {
        if (Listing == null)
            return;

        if (Listing.Lifetime < 0)
        {
            _timerImage.fillAmount = 0;
            return;
        }
        
        var curTime = Time.time - Listing.CreatedTime;
        var fill = 1 - (curTime / Listing.Lifetime);
        _timerImage.fillAmount = fill;
    }
}