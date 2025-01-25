using System;
using System.Collections.Generic;
using UnityEngine;

public class ListingsAudio : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private List<AudioClip> _clips = new();
    [SerializeField] private float _minDelay = 3;
    
    private float _lastAudioTime;
    
    private void Start()
    {
        BuildingManager.Instance.OnListingCreated += OnListingCreated;
    }

    private void OnDestroy()
    {
        BuildingManager.Instance.OnListingCreated -= OnListingCreated;
    }

    private void OnListingCreated(BuildingListing listing)
    {
        if (Time.time - _lastAudioTime < _minDelay)
            return;
        
        _lastAudioTime = Time.time;
        var clip = _clips.GetRandom();
        _audioSource.PlayOneShot(clip);
    }
}