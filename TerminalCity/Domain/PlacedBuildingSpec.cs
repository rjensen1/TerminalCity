namespace TerminalCity.Domain;

/// <summary>
/// Specifies a building to be pre-placed during scenario initialization.
/// Used in the [placed_buildings] section of scenario .txt files.
/// </summary>
public class PlacedBuildingSpec
{
    /// <summary>
    /// Building type identifier — must match a key recognized by PlaceSpecifiedBuildings()
    /// in MapGenerator. Currently supported: "school", "cemetery".
    /// </summary>
    public string BuildingType { get; set; } = "";

    /// <summary>
    /// Placement hint for the building. Recognized values:
    ///   "near_main_intersection" — near the center of the map (road intersection)
    ///   "edge_north"  — near the northern edge, random x position
    ///   "edge_south"  — near the southern edge, random x position
    ///   "edge_east"   — near the eastern edge,  random y position
    ///   "edge_west"   — near the western edge,  random y position
    ///   "x,y"         — exact tile coordinates (e.g. "12,8")
    /// </summary>
    public string Placement { get; set; } = "";
}
