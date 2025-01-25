using System;
using UnityEngine;

public class MarketUpdateAudio : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _marketClip;
    private void Start()
    {
        MarketForceManager.Instance.OnMarketUpdated += OnMarketUpdate;
    }
    private void OnDestroy()
    {
        MarketForceManager.Instance.OnMarketUpdated -= OnMarketUpdate;
    }

    private void OnMarketUpdate()
    {
        _audioSource.PlayOneShot(_marketClip);
    }
}