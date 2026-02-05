using SadRogue.Primitives;

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

    // City data
    public Tile[,] Tiles { get; }
    public List<Building> Buildings { get; set; } = new();

    // Resources
    public int Money { get; set; } = 10000;
    public int Population { get; set; } = 0;

    // Time
    public DateTime CurrentDate { get; set; } = new DateTime(2050, 1, 1);

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

public enum GameMode
{
    TitleScreen,
    Playing,
    Paused,
    FontTest,
    ConfirmExit
}
