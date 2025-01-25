using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LocationInfoUI : MonoBehaviour
{
    [SerializeField] private Location Location;
    
    [SerializeField] private LocationSpriteLookup _locationSprites;
    [SerializeField] private IntToSpriteLookup _marketSprites;
    [SerializeField] private IntToColourLookup _marketColours;

    [SerializeField] private Image _locationImage;
    [SerializeField] private Image _projectionImage;
    [SerializeField] private Image _marketStrength;
    [SerializeField] private TextMeshProUGUI _valueTxt;

    private void Start()
    {
        MarketForceManager.Instance.OnMarketUpdated += OnMarketUpdated;
        OnMarketUpdated();

        _locationImage.sprite = _locationSprites.Get(Location);
    }

    private void OnDestroy()
    {
        MarketForceManager.Instance.OnMarketUpdated -= OnMarketUpdated;
    }

    private void OnMarketUpdated()
    {
        var curForce = MarketForceManager.Instance.GetCurrentMarketForce(Location);
        var marketStrength = Mathf.InverseLerp(
            MarketForceManager.Instance.minPriceLevel,
            MarketForceManager.Instance.maxPriceLevel,
            curForce);

        _marketStrength.color = _marketColours.Get(curForce);
        _marketStrength.fillAmount = Mathf.Max(0.05f, marketStrength);
        
        var force = MarketForceManager.Instance.GetMarketDirection(Location);
        _projectionImage.sprite = _marketSprites.Get(force);
        _projectionImage.color = _marketColours.Get(force);

        var value = 0;
        _valueTxt.text = "$";
    }
}