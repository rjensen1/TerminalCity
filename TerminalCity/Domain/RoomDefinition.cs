namespace TerminalCity.Domain;

/// <summary>
/// Defines a type of room that can be placed in procedurally generated buildings
/// </summary>
public class RoomDefinition
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public string Category { get; set; } = "";
    public string Description { get; set; } = "";

    // Size constraints
    public int SizeWidthMin { get; set; }
    public int SizeWidthMax { get; set; }
    public int SizeHeightMin { get; set; }
    public int SizeHeightMax { get; set; }

    // Placement constraints (Phase 1)
    public double? MinPercentageOfBuilding { get; set; }  // e.g., 0.4 = 40%
    public double? MaxPercentageOfBuilding { get; set; }  // e.g., 0.7 = 70%
    public bool PrefersCentral { get; set; }              // Centrally located
    public bool RequiresExteriorAccess { get; set; }      // Needs exterior door
    public int MinWindows { get; set; }
    public int MinLights { get; set; }
    public int MinSwitches { get; set; }

    // Furniture and features
    public List<string> RequiredFurniture { get; set; } = new();
    public List<string> OptionalFurniture { get; set; } = new();
    public List<string> RequiredFeatures { get; set; } = new();
    public List<string> OptionalFeatures { get; set; } = new();

    public RoomDefinition()
    {
    }
}
