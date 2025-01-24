public class BuildingListing
{
    public BuildingType BuildingType;
    public int Cost;

    public override string ToString()
    {
        return $"{BuildingType} @ ${Cost}K";
    }
}