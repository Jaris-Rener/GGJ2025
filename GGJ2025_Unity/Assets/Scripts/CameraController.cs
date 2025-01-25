using System;
using DG.Tweening;
using UnityEngine;

public class CameraController : Singleton<CameraController>
{
    [SerializeField] private float _zoomFov;
    [SerializeField] private Camera _camera;
    [SerializeField] private FullListingUI _buyListingUI;
    private Vector3 _startPos;
    private float _startFov;

    public override void Awake()
    {
        base.Awake();
        _startPos = transform.position;
        _startFov = _camera.fieldOfView;
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
        transform.DOKill();
        transform.DOMove(_startPos, 3f);
        _camera.DOKill();
        _camera.DOOrthoSize(_startFov, 3f);
    }

    private void FocusBuilding(BuildingListing listing)
    {
        transform.DOKill();
        transform.DOMove(listing.Building.transform.position, 0.5f);
        _camera.DOKill();
        _camera.DOOrthoSize(_zoomFov, 0.5f);
    }
}