using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ListingUI : MonoBehaviour
{
    [SerializeField] private LocationSpriteLookup _locationIcons;
    [SerializeField] private BuildingTypeSpriteLookup _buildingIcons;
    
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _cost;
    [SerializeField] private Image _locationIcon;
    [SerializeField] private Image _buildingTypeIcon;
    
    public void Setup(BuildingListing listing, Location location)
    {
        _name.text = listing.Name;
        _cost.text = $"${listing.Cost}K";

        _locationIcon.sprite = _locationIcons.Get(location);
        _buildingTypeIcon.sprite = _buildingIcons.Get(listing.BuildingType);
    }
}