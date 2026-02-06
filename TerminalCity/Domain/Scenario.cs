namespace TerminalCity.Domain;

/// <summary>
/// Represents a game scenario with starting conditions and objectives
/// </summary>
public class Scenario
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string GameType { get; set; } = "traditional";
    public int Era { get; set; } = 1955;
    public int Latitude { get; set; } = 40;
    public string ClimateZone { get; set; } = "temperate";

    // Description shown to player
    public string Description { get; set; } = string.Empty;
    public string Backstory { get; set; } = string.Empty;

    // Starting conditions
    public int StartYear { get; set; } = 1955;
    public int StartPopulation { get; set; } = 0;
    public int StartMoney { get; set; } = 10000;
    public string MapSize { get; set; } = "100x100";

    // Parent city (for bedroom community scenarios)
    public string? ParentCityName { get; set; }
    public string? ParentCityDistance { get; set; }
    public int? ParentCityPopulation { get; set; }
    public string? ParentCityDescription { get; set; }

    // Objectives
    public string? PrimaryObjective { get; set; }
    public string? SecondaryObjective { get; set; }
    public string? OptionalObjective { get; set; }

    // Challenges
    public List<string> Challenges { get; set; } = new();

    // Demand levels (0-100 scale)
    public int ResidentialDemand { get; set; } = 50;
    public int CommercialDemand { get; set; } = 50;
    public int IndustrialDemand { get; set; } = 50;
    public int FarmDemand { get; set; } = 50;

    // Terrain generation
    public string TerrainType { get; set; } = "flat_farmland";
    public List<string> TerrainFeatures { get; set; } = new();
    public int RoadGridSpacing { get; set; } = 10;
    public string RoadGridType { get; set; } = "dirt_road";
    public string RoadGridPattern { get; set; } = "grid";

    // Main roads
    public bool HasMainVerticalRoad { get; set; } = false;
    public bool HasMainHorizontalRoad { get; set; } = false;

    // Camera starting position
    public string CameraStartPosition { get; set; } = "center"; // "center" or "x,y" coordinates

    // Initial land use percentages
    public int InitialFarmPercent { get; set; } = 0;
    public int InitialRoadPercent { get; set; } = 0;
    public int InitialTreesPercent { get; set; } = 0;
    public int InitialEmptyPercent { get; set; } = 0;

    /// <summary>
    /// Get formatted description for display in dialog
    /// </summary>
    public string GetFormattedDescription()
    {
        return Description;
    }

    /// <summary>
    /// Get formatted backstory for display
    /// </summary>
    public string GetFormattedBackstory()
    {
        return Backstory;
    }
}
