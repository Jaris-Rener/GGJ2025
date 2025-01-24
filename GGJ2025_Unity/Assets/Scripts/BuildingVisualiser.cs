using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BuildingVisualiser : Singleton<BuildingVisualiser>
{
    [SerializeField] private Building _house;
    [SerializeField] private Building _apartment;
    [SerializeField] private Building _mansion;

    private readonly List<Building> _instances = new();
    
    [ContextMenu("Update")]
    public void UpdateBuildings()
    {
        _instances.ClearObjects();
        
        foreach (var (listing, location) in BuildingManager.Instance.GetAllListings())
        {
            var prefab = GetPrefab(listing.BuildingType);
            
            // TODO: Place based on building location, for now place them in a random location
            var pos2D = Random.insideUnitCircle*15;
            var pos = new Vector3(pos2D.x, 0, pos2D.y);
            
            var instance = Instantiate(prefab, pos, Quaternion.identity);
            _instances.Add(instance);
        }
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