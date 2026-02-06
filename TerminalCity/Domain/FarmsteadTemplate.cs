namespace TerminalCity.Domain;

/// <summary>
/// Template for a farmstead plot with buildings and yard areas
/// </summary>
public class FarmsteadTemplate
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int Width { get; set; }
    public int Height { get; set; }
    public string Description { get; set; } = string.Empty;
    public List<string> MapRows { get; set; } = new();
    public Dictionary<char, string> Legend { get; set; } = new();

    /// <summary>
    /// Get what tile type should be at a given position in the farmstead
    /// </summary>
    public string? GetTileTypeAt(int x, int y)
    {
        if (y < 0 || y >= MapRows.Count) return null;
        if (x < 0 || x >= MapRows[y].Length) return null;

        char symbol = MapRows[y][x];
        return Legend.TryGetValue(symbol, out var tileType) ? tileType : null;
    }
}
