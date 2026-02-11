using SadRogue.Primitives;
using TerminalCity.Domain;
using TerminalCity.Parsers;
using Xunit;

namespace TerminalCity.Tests.Rendering;

/// <summary>
/// Tests for 50ft zoom rendering logic.
/// At 50ft zoom, each screen character should represent 2x2 world tiles (skipFactor = 2).
/// A 2x2 farmhouse should only appear ONCE on screen, not multiple times.
/// </summary>
public class FiftyFtRenderingTests
{
    [Fact]
    public void TinyFarmhouse_At50ftZoom_AppearsOnlyOnce_WhenCenteredInViewport()
    {
        // Arrange - Create a 10x10 map with a 2x2 farmhouse at position (4, 4)
        var gameState = new GameState(10, 10);
        gameState.ZoomLevel = 1; // 50ft zoom
        gameState.CameraPosition = new Point(5, 5); // Center camera on farmhouse

        // Place a 2x2 farmhouse at (4, 4)
        PlaceFarmhouse(gameState, 4, 4);

        // Act - Render to a mock screen and count how many farmhouse symbols appear
        var renderedScreen = RenderToMockScreen(gameState, 10, 10);

        // Assert - Should see exactly ONE farmhouse symbol
        int farmhouseCount = CountGlyph(renderedScreen, '⌂');
        Assert.Equal(1, farmhouseCount);
    }

    [Fact]
    public void TinyFarmhouse_At50ftZoom_AppearsOnlyOnce_WhenAtEdgeOfSamplingWindow()
    {
        // Arrange - Position the farmhouse so it straddles sampling boundaries
        var gameState = new GameState(10, 10);
        gameState.ZoomLevel = 1; // 50ft zoom, skipFactor = 2
        gameState.CameraPosition = new Point(5, 5);

        // Place farmhouse at (3, 3) - at an odd position to test sampling edge cases
        PlaceFarmhouse(gameState, 3, 3);

        // Act
        var renderedScreen = RenderToMockScreen(gameState, 10, 10);

        // Assert - Should see exactly ONE farmhouse symbol
        int farmhouseCount = CountGlyph(renderedScreen, '⌂');
        Assert.Equal(1, farmhouseCount);
    }

    [Fact]
    public void TinyFarmhouse_At50ftZoom_AppearsOnlyOnce_WhenCameraOffsetByOne()
    {
        // Arrange - Test with camera at different offset to trigger different modulo positions
        var gameState = new GameState(10, 10);
        gameState.ZoomLevel = 1; // 50ft zoom
        gameState.CameraPosition = new Point(4, 4); // Camera offset differently

        PlaceFarmhouse(gameState, 3, 3);

        // Act
        var renderedScreen = RenderToMockScreen(gameState, 10, 10);

        // Assert
        int farmhouseCount = CountGlyph(renderedScreen, '⌂');
        Assert.Equal(1, farmhouseCount);
    }

    [Fact]
    public void TwoFarmhouses_At50ftZoom_BothAppearOnce()
    {
        // Arrange - Two farmhouses spaced apart
        var gameState = new GameState(20, 20);
        gameState.ZoomLevel = 1; // 50ft zoom
        gameState.CameraPosition = new Point(10, 10);

        // Place two farmhouses
        PlaceFarmhouse(gameState, 5, 5);
        PlaceFarmhouse(gameState, 14, 14);

        // Act
        var renderedScreen = RenderToMockScreen(gameState, 20, 20);

        // Assert - Should see exactly TWO farmhouse symbols
        int farmhouseCount = CountGlyph(renderedScreen, '⌂');
        Assert.Equal(2, farmhouseCount);
    }

    [Fact]
    public void GetStructurePriority_Farmhouse_ReturnsHigherPriorityThanYard()
    {
        // This tests the priority logic used to decide which structure to show
        // when multiple structures are in the sampling window
        int farmhousePriority = GetStructurePriority("tiny_farmhouse");
        int yardPriority = GetStructurePriority("yard");

        Assert.True(farmhousePriority > yardPriority,
            "Farmhouse should have higher priority than yard");
    }

    [Fact]
    public void RenderScale_At50ftZoom_ReturnsHalf()
    {
        // Arrange
        var gameState = new GameState(10, 10);
        gameState.ZoomLevel = 1; // 50ft zoom

        // Act
        double renderScale = gameState.GetRenderScale();

        // Assert
        Assert.Equal(0.5, renderScale);
    }

    [Fact]
    public void SkipFactor_At50ftZoom_ShouldBeTwo()
    {
        // Arrange
        var gameState = new GameState(10, 10);
        gameState.ZoomLevel = 1; // 50ft zoom
        double renderScale = gameState.GetRenderScale(); // 0.5

        // Act
        int skipFactor = (int)(1.0 / renderScale);

        // Assert
        Assert.Equal(2, skipFactor);
    }

    [Fact]
    public void VisualizeBug_ShowWhereMultipleFarmhousesAppear()
    {
        // This test visualizes WHERE the duplicate farmhouses appear
        // A 2x2 farmhouse at (4,4)-(5,5) should appear once, but appears 4 times

        var gameState = new GameState(10, 10);
        gameState.ZoomLevel = 1; // 50ft zoom
        gameState.CameraPosition = new Point(5, 5);

        PlaceFarmhouse(gameState, 4, 4);

        var renderedScreen = RenderToMockScreen(gameState, 10, 10);

        // Print the screen to understand the issue
        var output = new System.Text.StringBuilder();
        output.AppendLine("\nRendered screen (should show 1 farmhouse, actually shows multiple):");
        for (int y = 0; y < renderedScreen.GetLength(1); y++)
        {
            for (int x = 0; x < renderedScreen.GetLength(0); x++)
            {
                output.Append(renderedScreen[x, y]);
            }
            output.AppendLine();
        }

        // This will fail and show the visualization in the test output
        int farmhouseCount = CountGlyph(renderedScreen, '⌂');
        Assert.Equal(1, farmhouseCount); // Will fail, showing the output
    }

    #region Helper Methods

    /// <summary>
    /// Places a 2x2 tiny_farmhouse at the specified position
    /// </summary>
    private void PlaceFarmhouse(GameState gameState, int x, int y)
    {
        // Farmhouse pattern at 25ft:
        // ⌂╗
        // └╝

        // Place the 4 tiles with proper offsets
        gameState.Tiles[x, y] = new Tile(TileType.Grass, null, null, "tiny_farmhouse", (0, 0));
        gameState.Tiles[x + 1, y] = new Tile(TileType.Grass, null, null, "tiny_farmhouse", (1, 0));
        gameState.Tiles[x, y + 1] = new Tile(TileType.Grass, null, null, "tiny_farmhouse", (0, 1));
        gameState.Tiles[x + 1, y + 1] = new Tile(TileType.Grass, null, null, "tiny_farmhouse", (1, 1));
    }

    /// <summary>
    /// Simulates rendering and returns a 2D array of what would be displayed
    /// This mimics the RenderZoomedOut logic
    /// </summary>
    private char[,] RenderToMockScreen(GameState gameState, int viewportWidth, int viewportHeight)
    {
        var screen = new char[viewportWidth, viewportHeight];
        double scale = gameState.GetRenderScale();
        int skipFactor = (int)(1.0 / scale);

        int dataTilesWide = viewportWidth * skipFactor;
        int dataTilesHigh = viewportHeight * skipFactor;

        int startX = gameState.CameraPosition.X - dataTilesWide / 2;
        int startY = gameState.CameraPosition.Y - dataTilesHigh / 2;

        // Track which building origins have been rendered globally
        HashSet<(int, int)> globalProcessedOrigins = new();

        for (int screenY = 0; screenY < viewportHeight; screenY++)
        {
            for (int screenX = 0; screenX < viewportWidth; screenX++)
            {
                int worldX = startX + (screenX * skipFactor);
                int worldY = startY + (screenY * skipFactor);

                // Out of bounds
                if (worldX < 0 || worldX >= gameState.MapWidth ||
                    worldY < 0 || worldY >= gameState.MapHeight)
                {
                    screen[screenX, screenY] = ' ';
                    continue;
                }

                // Sample the tile
                var tile = gameState.Tiles[worldX, worldY];

                // At zoomed-out levels, search sampling area for important structures
                if (skipFactor > 1)
                {
                    Tile? importantStructure = null;
                    int structurePriority = 0;

                    for (int dy = 0; dy < skipFactor; dy++)
                    {
                        for (int dx = 0; dx < skipFactor; dx++)
                        {
                            int checkX = worldX + dx;
                            int checkY = worldY + dy;

                            if (checkX < gameState.MapWidth && checkY < gameState.MapHeight)
                            {
                                var checkTile = gameState.Tiles[checkX, checkY];

                                if (checkTile.Type == TileType.Grass && checkTile.CropType != null)
                                {
                                    Tile tileToConsider;
                                    int originX, originY;

                                    // If multi-tile building, find origin
                                    if (checkTile.BuildingOffset.HasValue)
                                    {
                                        originX = checkX - checkTile.BuildingOffset.Value.x;
                                        originY = checkY - checkTile.BuildingOffset.Value.y;

                                        // Skip if already rendered this building globally
                                        if (globalProcessedOrigins.Contains((originX, originY)))
                                            continue;

                                        globalProcessedOrigins.Add((originX, originY));

                                        if (originX >= 0 && originY >= 0 &&
                                            originX < gameState.MapWidth && originY < gameState.MapHeight)
                                        {
                                            tileToConsider = gameState.Tiles[originX, originY];
                                        }
                                        else
                                        {
                                            continue;
                                        }
                                    }
                                    else
                                    {
                                        tileToConsider = checkTile;
                                        originX = checkX;
                                        originY = checkY;
                                    }

                                    int priority = GetStructurePriority(tileToConsider.CropType);

                                    if (priority > structurePriority)
                                    {
                                        importantStructure = tileToConsider;
                                        structurePriority = priority;
                                    }
                                }
                            }
                        }
                    }

                    if (importantStructure != null)
                    {
                        tile = importantStructure;
                    }
                    else
                    {
                        // Check if the sampled tile itself is part of an already-rendered building
                        if (tile.Type == TileType.Grass && tile.CropType != null && tile.BuildingOffset.HasValue)
                        {
                            int tileOriginX = worldX - tile.BuildingOffset.Value.x;
                            int tileOriginY = worldY - tile.BuildingOffset.Value.y;

                            if (globalProcessedOrigins.Contains((tileOriginX, tileOriginY)))
                            {
                                // This tile is part of a building already rendered - show grass instead
                                tile = new Tile(TileType.Grass, null, null, "yard");
                            }
                        }
                    }
                }

                // Get the glyph
                char glyph = GetTileGlyph(tile, gameState.ZoomLevel);
                screen[screenX, screenY] = glyph;
            }
        }

        return screen;
    }

    /// <summary>
    /// Gets the display glyph for a tile at the given zoom level
    /// </summary>
    private char GetTileGlyph(Tile tile, int zoomLevel)
    {
        if (tile.Type == TileType.Grass && tile.CropType != null)
        {
            if (tile.CropType == "tiny_farmhouse")
            {
                // At 50ft zoom, farmhouse shows as ⌂
                return zoomLevel switch
                {
                    1 => '⌂', // 50ft
                    _ => '.'
                };
            }
            if (tile.CropType == "yard")
            {
                return '.';
            }
        }

        return '.'; // Default grass
    }

    /// <summary>
    /// Mimics GetStructurePriority from Program.cs
    /// </summary>
    private int GetStructurePriority(string? structureType)
    {
        if (structureType == null) return 0;

        return structureType switch
        {
            "tiny_farmhouse" => 100,
            "barn" => 90,
            "shed" => 50,
            "yard" => 10,
            "driveway" => 5,
            _ => 0
        };
    }

    /// <summary>
    /// Counts occurrences of a glyph in the rendered screen
    /// </summary>
    private int CountGlyph(char[,] screen, char glyph)
    {
        int count = 0;
        for (int y = 0; y < screen.GetLength(1); y++)
        {
            for (int x = 0; x < screen.GetLength(0); x++)
            {
                if (screen[x, y] == glyph)
                    count++;
            }
        }
        return count;
    }

    #endregion
}
