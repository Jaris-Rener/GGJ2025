using UnityEngine;

public class Building : MonoBehaviour
{
    public BuildingType Type => _type;
    public Location Location => _location;

    [SerializeField] private BuildingType _type;
    [SerializeField] private Location _location;
}