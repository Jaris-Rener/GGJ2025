public class BuildingListing
{
    public string Name;
    public BuildingType BuildingType;
    public int Cost;

    public override string ToString()
    {
        return $"{BuildingType} @ ${Cost}K";
    }
}