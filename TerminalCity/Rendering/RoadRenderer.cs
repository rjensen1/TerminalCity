using SadRogue.Primitives;
using TerminalCity.Domain;

namespace TerminalCity.Rendering;

/// <summary>
/// Handles road appearance rendering logic for different zoom levels
/// </summary>
public static class RoadRenderer
{
    /// <summary>
    /// Get the appearance (character, colors) for a road at a given zoom level
    /// Note: This returns the horizontal road character. Use GetVerticalRoadChar for vertical roads.
    /// </summary>
    public static (char glyph, Color foreground, Color background) GetRoadAppearance(TileType roadType, int zoomLevel, bool isVertical = false)
    {
        // Define natural ground color (grass verge)
        var naturalGround = Color.Green;

        // Determine colors based on road type
        Color roadColor = roadType == TileType.DirtRoad
            ? Color.SandyBrown  // Dirt color
            : Color.DarkGray;   // Paved color

        // Use CP437 characters (0-255) for roads - different for horizontal vs vertical
        if (isVertical)
        {
            return zoomLevel switch
            {
                -2 => ('|', roadColor, naturalGround),         // 400ft: | (pipe for vertical)
                -1 => ('|', roadColor, naturalGround),         // 200ft: | (thin vertical for dirt roads)
                0 => ((char)221, roadColor, naturalGround),    // 100ft: ▌ (left half block)
                1 => ((char)221, roadColor, naturalGround),    // 50ft: ▌ (left half block)
                2 => ((char)219, roadColor, roadType == TileType.PavedRoad ? Color.Black : Color.DarkGray), // 25ft: █ (full block)
                _ => ('|', roadColor, naturalGround)
            };
        }
        else
        {
            return zoomLevel switch
            {
                -2 => ('-', roadColor, naturalGround),         // 400ft: - (thin horizontal)
                -1 => ('-', roadColor, naturalGround),         // 200ft: - (thin horizontal for dirt roads)
                0 => ((char)220, roadColor, naturalGround),    // 100ft: ▄ (lower half block)
                1 => ((char)220, roadColor, naturalGround),    // 50ft: ▄ (lower half block)
                2 => ((char)219, roadColor, roadType == TileType.PavedRoad ? Color.Black : Color.DarkGray), // 25ft: █ (full block)
                _ => ((char)220, roadColor, naturalGround)
            };
        }
    }

    /// <summary>
    /// Get the intersection character for a given zoom level
    /// </summary>
    public static char GetIntersectionChar(int zoomLevel)
    {
        return zoomLevel switch
        {
            -2 => '-',         // 400ft: hyphen (shouldn't be visible for dirt roads)
            -1 => '+',         // 200ft: thin plus sign for dirt road intersections
            0 => (char)219,    // 100ft: █ (full block, same as 50ft)
            1 => (char)219,    // 50ft: █ (full block)
            2 => (char)219,    // 25ft: █ (full block)
            _ => '#'
        };
    }
}
