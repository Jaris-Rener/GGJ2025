using System;
using DG.Tweening;
using UnityEngine;

public class CameraController : Singleton<CameraController>
{
    [SerializeField] private FullListingUI _buyListingUI;
    private Vector3 _startPos;

    public override void Awake()
    {
        base.Awake();
        _startPos = transform.position;
    }

    private void Start()
    {
        _buyListingUI.OnShow += FocusBuilding;
        _buyListingUI.OnHide += ReturnHome;
    }

    private void OnDestroy()
    {
        _buyListingUI.OnShow -= FocusBuilding;
        _buyListingUI.OnHide -= ReturnHome;
    }

    private void ReturnHome()
    {
        // transform.DOKill();
        // transform.DOMove(_startPos, 1.0f);
    }

    private void FocusBuilding(BuildingListing listing)
    {
        transform.DOKill();
        transform.DOMove(listing.Building.transform.position, 0.5f);
    }
}