using UnityEngine;

public class BuildingPin : MonoBehaviour
{
    [SerializeField] private FullListingUI _buyListingUI;
    [SerializeField] private GameObject _pinObject;
    [SerializeField] private Vector3 _offset;
    
    private void Start()
    {
        _buyListingUI.OnShow += PinBuilding;
        _buyListingUI.OnHide += HidePin;
    }

    private void HidePin()
    {
        _pinObject.SetActive(false);
    }

    private void PinBuilding(BuildingListing obj)
    {
        _pinObject.SetActive(true);
        transform.position = obj.Building.transform.position + _offset;
    }

    private void OnDestroy()
    {
        _buyListingUI.OnShow -= PinBuilding;
        _buyListingUI.OnHide -= HidePin;
    }
}