using System;
using UnityEngine;

public class PlayerMoneyAudio : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _moneyClip;
    private void Start()
    {
        PlayerAssetManager.Instance.OnMoneyChanged += OnMoneyChanged;
    }
    private void OnDestroy()
    {
        PlayerAssetManager.Instance.OnMoneyChanged -= OnMoneyChanged;
    }

    private void OnMoneyChanged(float money)
    {
        _audioSource.PlayOneShot(_moneyClip);
    }
}