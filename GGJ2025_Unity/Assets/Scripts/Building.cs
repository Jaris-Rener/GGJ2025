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
        // Find the first child transform and store its properties
        Transform visualizationTransform = transform.childCount > 0 ? transform.GetChild(0) : null;

        if (visualizationTransform == null)
        {
            Debug.LogWarning("No visualization mesh found for this building!");
            return;
        }

        Vector3 spawnPosition = visualizationTransform.position;
        Quaternion spawnRotation = visualizationTransform.rotation;

        // Destroy the visualization mesh
        Destroy(visualizationTransform.gameObject);

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

            // Instantiate the selected prefab at the previous visualization transform
            GameObject instance = Instantiate(selectedPrefab, spawnPosition, spawnRotation);
            instance.transform.SetParent(transform);
        }
        else
        {
            Debug.LogWarning($"No prefab options set for the building type {_type}!");
        }
    }
}