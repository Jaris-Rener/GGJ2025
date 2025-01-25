using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ListingUI : MonoBehaviour
{
    public BuildingListing Listing { get; private set; }

    [SerializeField] private Image _timerImage;
    
    [SerializeField] private LocationSpriteLookup _locationIcons;
    [SerializeField] private BuildingTypeSpriteLookup _buildingIcons;
    
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _cost;
    [SerializeField] private Image _locationIcon;
    [SerializeField] private Image _buildingTypeIcon;
    
    public void Setup(BuildingListing listing)
    {
        if (Listing == listing)
        {
            Toggle();
            return;
        }

        Listing = listing;
        
        _name.text = listing.Name;
        _cost.text = $"${listing.Cost}K";

        _locationIcon.sprite = _locationIcons.Get(listing.Location);
        _buildingTypeIcon.sprite = _buildingIcons.Get(listing.BuildingType);

        Show();
    }

    public void Toggle()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void OnListingRemoved()
    {
        Hide();
    }

    private void Update()
    {
        if (Listing == null)
            return;

        if (Listing.Lifetime < 0)
        {
            _timerImage.fillAmount = 0;
            return;
        }
        
        var curTime = Time.time - Listing.CreatedTime;
        var fill = 1 - (curTime / Listing.Lifetime);
        _timerImage.fillAmount = fill;
    }
}