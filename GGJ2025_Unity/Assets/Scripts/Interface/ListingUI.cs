using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ListingUI : MonoBehaviour
{
    public bool Active { get; private set; }
    public BuildingListing Listing { get; private set; }

    [SerializeField] private CanvasGroup _canvasGroup;
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
    
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _showClip;
    [SerializeField] private AudioClip _hideClip;

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
        if (Listing == null)
            return;
        
        SetCost(Listing.CurrentCost);

        if (_projectionIcon == null)
            return;

        var nextMarketForce = MarketForceManager.Instance.GetMarketDirection(Listing.Location);
        _projectionIcon.sprite = _marketSprites.Get(nextMarketForce);
        _projectionIcon.color = _marketColours.Get(nextMarketForce);
    }

    public void Setup(BuildingListing listing, bool playAudio)
    {
        if (Listing == listing)
        {
            Toggle(playAudio);
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

        Show(playAudio);
    }

    private void SetCost(float cost)
    {
        _cost.text = $"${cost}K";
    }

    public void Toggle(bool playAudio)
    {
        if (Active)
            Hide(playAudio);
        else
            Show(playAudio);
    }

    public void Show(bool playAudio = true)
    {
        Active = true;
        _canvasGroup.alpha = 1;
        if (playAudio)
            _audioSource.PlayOneShot(_showClip);
    }

    public void Hide(bool playAudio = true)
    {
        Active = false;
        _canvasGroup.alpha = 0;
        if (playAudio)
            _audioSource.PlayOneShot(_hideClip);
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