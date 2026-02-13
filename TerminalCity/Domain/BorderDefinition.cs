using SadRogue.Primitives;

namespace TerminalCity.Domain;

/// <summary>
/// Defines visual appearance of plot borders at various zoom levels
/// Examples: fences, hedges, tree lines, ditches, stone walls
/// </summary>
public class BorderDefinition
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";

    // Visual patterns at different zoom levels with directional support
    public BorderPatternSet? Pattern25ft { get; set; }
    public BorderPatternSet? Pattern50ft { get; set; }
    public BorderPatternSet? Pattern100ft { get; set; }
    public BorderPatternSet? Pattern200ft { get; set; }
    public BorderPatternSet? Pattern400ft { get; set; }

    public Color Color { get; set; }
    public Color BackgroundColor { get; set; }

    /// <summary>
    /// Get the appropriate character for this border at the specified zoom level and side
    /// </summary>
    public char? GetPatternForZoom(int zoomLevel, BorderSides side)
    {
        var patternSet = zoomLevel switch
        {
            2 => Pattern25ft,   // 25ft
            1 => Pattern50ft,   // 50ft
            0 => Pattern100ft,  // 100ft
            -1 => Pattern200ft, // 200ft
            -2 => Pattern400ft, // 400ft
            _ => null
        };

        return patternSet?.GetPattern(side);
    }
}

/// <summary>
/// Pattern set allowing different characters for each border side
/// </summary>
public class BorderPatternSet
{
    public char? North { get; set; }
    public char? South { get; set; }
    public char? East { get; set; }
    public char? West { get; set; }
    public char? Default { get; set; }  // Fallback if specific side not set

    /// <summary>
    /// Get pattern for a specific side, with fallbacks
    /// </summary>
    public char? GetPattern(BorderSides side)
    {
        // Check specific side first
        if (side.HasFlag(BorderSides.North) && North.HasValue) return North;
        if (side.HasFlag(BorderSides.South) && South.HasValue) return South;
        if (side.HasFlag(BorderSides.East) && East.HasValue) return East;
        if (side.HasFlag(BorderSides.West) && West.HasValue) return West;

        // Fallback to default
        return Default;
    }
}
