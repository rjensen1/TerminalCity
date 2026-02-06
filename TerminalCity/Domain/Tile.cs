namespace TerminalCity.Domain;

/// <summary>
/// Represents a single tile in the city map
/// </summary>
public class Tile
{
    public TileType Type { get; set; }
    public Building? Building { get; set; }
    public Zone? Zone { get; set; }
    public string? CropType { get; set; } // For farm tiles: "fallow_plowed", "fallow_unplowed", "wheat", etc.

    public Tile(TileType type, Building? building = null, Zone? zone = null, string? cropType = null)
    {
        Type = type;
        Building = building;
        Zone = zone;
        CropType = cropType;
    }
}

public enum TileType
{
    Grass,
    DirtRoad,
    PavedRoad,
    Building,
    Water,
    Zone,
    Farm,
    Trees
}

public enum Zone
{
    Residential,
    Commercial,
    Industrial
}
