namespace TerminalCity.Domain;

/// <summary>
/// Represents a single tile in the city map
/// </summary>
public class Tile
{
    public TileType Type { get; set; }
    public Building? Building { get; set; }
    public Zone? Zone { get; set; }

    public Tile(TileType type, Building? building = null, Zone? zone = null)
    {
        Type = type;
        Building = building;
        Zone = zone;
    }
}

public enum TileType
{
    Grass,
    Road,
    Building,
    Water,
    Zone
}

public enum Zone
{
    Residential,
    Commercial,
    Industrial
}
