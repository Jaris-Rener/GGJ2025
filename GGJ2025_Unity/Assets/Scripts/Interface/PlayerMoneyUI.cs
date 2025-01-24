using System;
using TMPro;
using UnityEngine;

public class PlayerMoneyUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _moneyLabel;
    private void Start()
    {
        PlayerAssetManager.Instance.OnMoneyChanged += UpdateMoney;
    }

    private void OnDestroy()
    {
        PlayerAssetManager.Instance.OnMoneyChanged -= UpdateMoney;
    }

    private void UpdateMoney(float currentMoney)
    {
        _moneyLabel.text = $"${currentMoney:N0}K";
    }
}