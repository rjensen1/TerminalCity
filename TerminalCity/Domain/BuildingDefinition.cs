using SadRogue.Primitives;

namespace TerminalCity.Domain;

/// <summary>
/// Defines a building type with zoom-level appearance patterns
/// </summary>
public class BuildingDefinition
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public string Type { get; set; } = "";
    public string Description { get; set; } = "";
    public int Width { get; set; }
    public int Height { get; set; }
    public int Cost { get; set; }
    public int Maintenance { get; set; }
    public Color Color { get; set; }
    public Color BackgroundColor { get; set; }
    public bool UnlockedByDefault { get; set; }
    public int EraAvailable { get; set; }
    public int EraObsolete { get; set; }

    // Zoom-level patterns
    public ZoomPattern? Pattern25ft { get; set; }
    public ZoomPattern? Pattern50ft { get; set; }
    public ZoomPattern? Pattern100ft { get; set; }
    public ZoomPattern? Pattern200ft { get; set; }
    public ZoomPattern? Pattern400ft { get; set; }
}

/// <summary>
/// Represents a visual pattern at a specific zoom level
/// </summary>
public class ZoomPattern
{
    /// <summary>
    /// The pattern text - can be multi-line for larger buildings (e.g., "⌂╗\n└╝")
    /// or single character for 1x1 buildings (e.g., "▐")
    /// </summary>
    public string Pattern { get; set; } = "";

    /// <summary>
    /// Whether this building is important enough to show at this zoom level
    /// when sampling/prioritizing structures at far zooms
    /// </summary>
    public bool Important { get; set; }

    /// <summary>
    /// Gets the character at a specific position within the pattern (0-indexed)
    /// </summary>
    public char GetCharAt(int x, int y)
    {
        var lines = Pattern.Split('\n');
        if (y < 0 || y >= lines.Length) return ' ';
        if (x < 0 || x >= lines[y].Length) return ' ';
        return lines[y][x];
    }

    /// <summary>
    /// Gets the width of the pattern (number of columns)
    /// </summary>
    public int GetWidth()
    {
        var lines = Pattern.Split('\n');
        return lines.Length > 0 ? lines.Max(l => l.Length) : 0;
    }

    /// <summary>
    /// Gets the height of the pattern (number of rows)
    /// </summary>
    public int GetHeight()
    {
        return Pattern.Split('\n').Length;
    }
}
