using UnityEngine;

public class Building : MonoBehaviour
{
    public string Name => _name;
    public BuildingType Type => _type;
    public Location Location => _location;

    [SerializeField] private BuildingType _type;
    [SerializeField] private Location _location;
    [SerializeField] private string _name;

    [SerializeField] private GameObject[] mansionOptions;
    [SerializeField] private GameObject[] apartmentOptions;
    [SerializeField] private GameObject[] houseOptions;

    private void Start()
    {
        /*
        // Determine which prefab options to use based on the building type
        GameObject[] prefabOptions = null;

        switch (_type)
        {
            case BuildingType.Mansion:
                prefabOptions = mansionOptions;
                break;
            case BuildingType.Apartment:
                prefabOptions = apartmentOptions;
                break;
            case BuildingType.House:
                prefabOptions = houseOptions;
                break;
            default:
                Debug.LogWarning("Unhandled building type: " + _type);
                break;
        }

        // Spawn a prefab if options are available
        if (prefabOptions != null && prefabOptions.Length > 0)
        {
            int randomIndex = Random.Range(0, prefabOptions.Length);
            GameObject selectedPrefab = prefabOptions[randomIndex];

            // Instantiate the selected prefab and set it as a child
            GameObject instance = Instantiate(selectedPrefab, transform.position, Quaternion.identity);
            instance.transform.SetParent(transform);
        }
        else
        {
            Debug.LogWarning($"No prefab options set for the building type {_type}!");
        }
        */
    }
}