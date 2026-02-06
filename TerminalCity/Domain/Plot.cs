using SadRogue.Primitives;

namespace TerminalCity.Domain;

/// <summary>
/// Represents a plot of land (farmland, residential lot, commercial parcel, etc.)
/// </summary>
public class Plot
{
    public string Id { get; set; }
    public Rectangle Bounds { get; set; }  // x, y, width, height
    public PlotType Type { get; set; }
    public string? CropType { get; set; }  // For farmland: "fallow_plowed", "wheat", etc.

    // Future properties:
    // public string? OwnerId { get; set; }
    // public DateTime? PlantedDate { get; set; }
    // public int? YieldEstimate { get; set; }

    public Plot(string id, Rectangle bounds, PlotType type, string? cropType = null)
    {
        Id = id;
        Bounds = bounds;
        Type = type;
        CropType = cropType;
    }

    /// <summary>
    /// Check if a point is within this plot
    /// </summary>
    public bool Contains(Point point)
    {
        return Bounds.Contains(point);
    }

    /// <summary>
    /// Check if a tile position is within this plot
    /// </summary>
    public bool Contains(int x, int y)
    {
        return Bounds.Contains(new Point(x, y));
    }
}

public enum PlotType
{
    Farmland,
    Residential,
    Commercial,
    Industrial,
    Park,
    Institutional
}
