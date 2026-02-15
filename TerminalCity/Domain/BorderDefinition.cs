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

    // Optional per-zoom-level background colors (null = use default BackgroundColor)
    public Color? BackgroundColor25ft { get; set; }
    public Color? BackgroundColor50ft { get; set; }
    public Color? BackgroundColor100ft { get; set; }
    public Color? BackgroundColor200ft { get; set; }
    public Color? BackgroundColor400ft { get; set; }

    // Importance flags - should this border always be visible at this zoom level?
    public bool Important25ft { get; set; } = false;
    public bool Important50ft { get; set; } = false;
    public bool Important100ft { get; set; } = false;
    public bool Important200ft { get; set; } = false;
    public bool Important400ft { get; set; } = false;

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

    /// <summary>
    /// Get background color for this zoom level
    /// Returns null if "neighbor" color should be used
    /// </summary>
    public Color? GetBackgroundColorForZoom(int zoomLevel)
    {
        var zoomBackground = zoomLevel switch
        {
            2 => BackgroundColor25ft,   // 25ft
            1 => BackgroundColor50ft,   // 50ft
            0 => BackgroundColor100ft,  // 100ft
            -1 => BackgroundColor200ft, // 200ft
            -2 => BackgroundColor400ft, // 400ft
            _ => null
        };

        // If zoom-specific background is set, use it
        // Otherwise, return null to indicate "use neighbor"
        return zoomBackground ?? (BackgroundColor == Color.Transparent ? null : BackgroundColor);
    }

    /// <summary>
    /// Check if this border is marked important at the specified zoom level
    /// Important borders are always visible even at far zoom
    /// </summary>
    public bool IsImportantAtZoom(int zoomLevel)
    {
        return zoomLevel switch
        {
            2 => Important25ft,   // 25ft
            1 => Important50ft,   // 50ft
            0 => Important100ft,  // 100ft
            -1 => Important200ft, // 200ft
            -2 => Important400ft, // 400ft
            _ => false
        };
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
