using SadRogue.Primitives;
using TerminalCity.UI;

namespace TerminalCity.Domain;

/// <summary>
/// Main game state for TerminalCity
/// </summary>
public class GameState
{
    public int MapWidth { get; }
    public int MapHeight { get; }

    // Game state
    public GameMode CurrentMode { get; set; } = GameMode.TitleScreen;
    public Point CameraPosition { get; set; }
    public Dialog? CurrentDialog { get; set; } = null; // Modal dialog overlay

    // Visual time of day (cosmetic only, not tied to game time)
    public TimeOfDay VisualTimeOfDay { get; set; } = TimeOfDay.Midday;

    // Weather state (cosmetic only for now)
    public Weather CurrentWeather { get; set; } = new Weather();

    // Zoom/Scale system
    public int ZoomLevel { get; set; } = 0; // 0 = default, negative = zoomed out, positive = zoomed in

    /// <summary>
    /// Get the real-world size (in feet) that one tile represents at current zoom
    /// </summary>
    public int GetTileScale()
    {
        return ZoomLevel switch
        {
            -2 => 400,  // Very zoomed out
            -1 => 200,  // Zoomed out
            0 => 100,   // Default
            1 => 50,    // Zoomed in
            2 => 25,    // Very zoomed in
            _ => 100
        };
    }

    /// <summary>
    /// Get zoom level name for display
    /// </summary>
    public string GetZoomLevelName()
    {
        return ZoomLevel switch
        {
            -2 => "Very Far",
            -1 => "Far",
            0 => "Normal",
            1 => "Close",
            2 => "Very Close",
            _ => "Normal"
        };
    }

    /// <summary>
    /// Get the render scale multiplier for current zoom
    /// NOTE: Tile array is stored at 25ft granularity (most detailed level).
    /// This calculates how many tiles to consolidate per screen character.
    /// </summary>
    public double GetRenderScale()
    {
        // Tile storage granularity is always 25ft
        const int storageGranularity = 25;

        // Get viewing scale (feet per screen character)
        int viewingScale = GetTileScale();

        // Calculate sampling ratio: viewingScale / storageGranularity
        // E.g., at 100ft zoom: 100/25 = 4 tiles per screen char → scale = 1/4 = 0.25
        return (double)storageGranularity / viewingScale;
    }

    // City data
    public Tile[,] Tiles { get; }
    public List<Building> Buildings { get; set; } = new();
    public List<Plot> Plots { get; set; } = new();

    // Resources
    public int Money { get; set; } = 10000;
    public int Population { get; set; } = 0;

    // Time and Speed
    public DateTime CurrentDate { get; set; } = new DateTime(2050, 1, 1, 1, 0, 0); // Start at 1 AM
    public int GameSpeed { get; set; } = 0; // 0 = paused, 1-4 = speed levels
    private int _tickCounter = 0;

    /// <summary>
    /// Advance time based on current game speed
    /// </summary>
    public void AdvanceTime()
    {
        if (GameSpeed == 0) return; // Paused

        _tickCounter++;

        // Determine time advancement based on speed
        // Speed 1: 3 ticks per day (8 hours per tick)
        // Speed 2: 1 tick per day (24 hours per tick)
        // Speed 3: 3 days per tick
        // Speed 4: 7 days per tick
        var advancement = GameSpeed switch
        {
            1 => (_tickCounter >= 3, 0, 8, 0),   // Every 3 ticks, advance 8 hours
            2 => (_tickCounter >= 1, 1, 0, 0),   // Every tick, advance 1 day
            3 => (_tickCounter >= 1, 3, 0, 0),   // Every tick, advance 3 days
            4 => (_tickCounter >= 1, 7, 0, 0),   // Every tick, advance 7 days
            _ => (false, 0, 0, 0)
        };

        if (advancement.Item1) // Should advance
        {
            CurrentDate = CurrentDate.AddDays(advancement.Item2).AddHours(advancement.Item3);
            _tickCounter = 0;
        }
    }

    /// <summary>
    /// Get formatted date and time string for display
    /// </summary>
    public string GetFormattedDate()
    {
        return CurrentDate.ToString("MMM d, yyyy h:mm tt");
    }

    /// <summary>
    /// Get current season based on date (Northern Hemisphere)
    /// </summary>
    public Season GetCurrentSeason()
    {
        int month = CurrentDate.Month;
        return month switch
        {
            3 or 4 or 5 => Season.Spring,
            6 or 7 or 8 => Season.Summer,
            9 or 10 or 11 => Season.Fall,
            12 or 1 or 2 => Season.Winter,
            _ => Season.Spring
        };
    }

    /// <summary>
    /// Cycle to next time of day (for visual lighting only)
    /// </summary>
    public void CycleTimeOfDay()
    {
        VisualTimeOfDay = VisualTimeOfDay switch
        {
            TimeOfDay.Dawn => TimeOfDay.Morning,
            TimeOfDay.Morning => TimeOfDay.Midday,
            TimeOfDay.Midday => TimeOfDay.Afternoon,
            TimeOfDay.Afternoon => TimeOfDay.Dusk,
            TimeOfDay.Dusk => TimeOfDay.Evening,
            TimeOfDay.Evening => TimeOfDay.Night,
            TimeOfDay.Night => TimeOfDay.Dawn,
            _ => TimeOfDay.Midday
        };
    }

    /// <summary>
    /// Get display name for current visual time of day
    /// </summary>
    public string GetTimeOfDayName()
    {
        return VisualTimeOfDay switch
        {
            TimeOfDay.Dawn => "Dawn",
            TimeOfDay.Morning => "Morning",
            TimeOfDay.Midday => "Midday",
            TimeOfDay.Afternoon => "Afternoon",
            TimeOfDay.Dusk => "Dusk",
            TimeOfDay.Evening => "Evening",
            TimeOfDay.Night => "Night",
            _ => "Midday"
        };
    }

    public GameState(int mapWidth = 200, int mapHeight = 200)
    {
        MapWidth = mapWidth;
        MapHeight = mapHeight;
        Tiles = new Tile[mapWidth, mapHeight];
        CameraPosition = new Point(mapWidth / 2, mapHeight / 2);

        // Initialize with grass tiles
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                Tiles[x, y] = new Tile(TileType.Grass, null);
            }
        }
    }

    /// <summary>
    /// Move camera by offset
    /// </summary>
    public void MoveCamera(Point offset)
    {
        var newPos = CameraPosition + offset;

        // Clamp to map bounds
        newPos = new Point(
            Math.Clamp(newPos.X, 0, MapWidth - 1),
            Math.Clamp(newPos.Y, 0, MapHeight - 1)
        );

        CameraPosition = newPos;
    }

    /// <summary>
    /// Place a building at the specified position
    /// </summary>
    public bool PlaceBuilding(Building building, Point position)
    {
        // Check if area is clear
        for (int x = 0; x < building.Width; x++)
        {
            for (int y = 0; y < building.Height; y++)
            {
                int tileX = position.X + x;
                int tileY = position.Y + y;

                if (tileX >= MapWidth || tileY >= MapHeight)
                    return false;

                if (Tiles[tileX, tileY].Building != null)
                    return false;
            }
        }

        // Check if player can afford it
        if (Money < building.Cost)
            return false;

        // Place building
        Buildings.Add(building);
        building.Position = position;

        for (int x = 0; x < building.Width; x++)
        {
            for (int y = 0; y < building.Height; y++)
            {
                int tileX = position.X + x;
                int tileY = position.Y + y;
                Tiles[tileX, tileY] = new Tile(TileType.Building, building);
            }
        }

        Money -= building.Cost;
        return true;
    }
}

public enum Season
{
    Spring,
    Summer,
    Fall,
    Winter
}

public enum GameMode
{
    TitleScreen,
    Playing,
    Paused,
    FontTest
}
