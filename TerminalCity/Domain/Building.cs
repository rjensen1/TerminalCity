using SadRogue.Primitives;

namespace TerminalCity.Domain;

/// <summary>
/// Represents a building in the city
/// </summary>
public class Building
{
    public string Name { get; set; }
    public BuildingType Type { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public int Cost { get; set; }
    public int MaintenanceCost { get; set; }
    public Point Position { get; set; }
    public char DisplayChar { get; set; }
    public Color Color { get; set; }

    public Building(string name, BuildingType type, int width, int height, int cost, int maintenance, char displayChar, Color color)
    {
        Name = name;
        Type = type;
        Width = width;
        Height = height;
        Cost = cost;
        MaintenanceCost = maintenance;
        DisplayChar = displayChar;
        Color = color;
    }
}

public enum BuildingType
{
    Residential,
    Commercial,
    Industrial,
    Service,
    Infrastructure
}
