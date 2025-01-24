using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class VisualiserLocation
{
    public Building CurrentBuilding { get; set; }
    
    public Transform Root;
    public List<BuildingType> AllowedTypes = new();
}

public class BuildingVisualiser : Singleton<BuildingVisualiser>
{
    [SerializeField] private Building _house;
    [SerializeField] private Building _apartment;
    [SerializeField] private Building _mansion;

    [SerializeField] private List<VisualiserLocation> _cityLocations = new();
    [SerializeField] private List<VisualiserLocation> _suburbanLocations = new();
    [SerializeField] private List<VisualiserLocation> _beachLocations = new();

    private readonly List<Building> _instances = new();
    
    [ContextMenu("Update")]
    public void UpdateBuildings()
    {
        _instances.ClearObjects();
        
        foreach (var listing in BuildingManager.Instance.AllListings)
        {
            var prefab = GetPrefab(listing.BuildingType);

            var locationsList = GetLocationList(listing.Location);
            var spawnLocation = locationsList
                .Where(x => x.CurrentBuilding == null)
                .Where(x => x.AllowedTypes.Contains(listing.BuildingType))
                .ToList()
                .GetRandom();

            if (spawnLocation == null)
            {
                Debug.LogWarning($"Could not find a valid spawn location for {listing}");
                continue;
            }

            var pos = spawnLocation.Root.position;
            var rot = spawnLocation.Root.rotation;
            var instance = Instantiate(prefab, pos, rot);
            spawnLocation.CurrentBuilding = instance;
            
            _instances.Add(instance);
        }
    }

    private List<VisualiserLocation> GetLocationList(Location location)
    {
        return location switch
        {
            Location.Beach => _beachLocations,
            Location.City => _cityLocations,
            Location.Suburbs => _suburbanLocations,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private Building GetPrefab(BuildingType type)
    {
        return type switch
        {
            BuildingType.Apartment => _apartment,
            BuildingType.House => _house,
            BuildingType.Mansion => _mansion,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }
}