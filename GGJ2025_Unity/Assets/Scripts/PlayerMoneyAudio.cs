using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoneyAudio : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private List<AudioClip> _moneyclips = new();
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
        // Maybe want to add some protection here to make it not pick the same clip twice in a row? unsure how to do that - Chrispy
        var clip = _moneyclips.GetRandom();
        _audioSource.PlayOneShot(clip);
    }
}