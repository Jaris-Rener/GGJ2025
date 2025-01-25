using System;
using TMPro;
using UnityEngine;

public class PlayerMoneyUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _moneyLabel;
    [SerializeField] private TextMeshProUGUI _taxLabel;
    
    private void Start()
    {
        PlayerAssetManager.Instance.OnMoneyChanged += UpdateMoney;
        MarketForceManager.Instance.OnMarketUpdated += OnMarketUpdated;
        
        OnMarketUpdated();
        UpdateMoney(PlayerAssetManager.Instance.money);
    }

    private void OnDestroy()
    {
        PlayerAssetManager.Instance.OnMoneyChanged -= UpdateMoney;
        MarketForceManager.Instance.OnMarketUpdated -= OnMarketUpdated;
    }

    private void OnMarketUpdated()
    {
        var taxAmount = PlayerAssetManager.Instance.money - PlayerAssetManager.Instance.TaxPlayer();
        _taxLabel.text = $"-${taxAmount:N0}K";
    }

    private void UpdateMoney(float currentMoney)
    {
        _moneyLabel.text = $"${currentMoney:N0}K";
    }
}