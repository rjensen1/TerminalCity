namespace TerminalCity.Domain;

/// <summary>
/// Flags enum for specifying which sides of a plot have borders
/// </summary>
[Flags]
public enum BorderSides
{
    None = 0,
    North = 1,
    South = 2,
    East = 4,
    West = 8,

    // Common two-side combinations
    NorthEast = North | East,
    NorthWest = North | West,
    SouthEast = South | East,
    SouthWest = South | West,
    NorthSouth = North | South,
    EastWest = East | West,

    All = North | South | East | West
}
