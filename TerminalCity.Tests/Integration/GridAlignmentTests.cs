using SadRogue.Primitives;
using TerminalCity.Domain;
using TerminalCity.Parsers;
using Xunit;

namespace TerminalCity.Tests.Integration;

/// <summary>
/// Tests for grid alignment issues when rendering multi-tile buildings at zoomed-out levels.
/// Verifies that buildings render consistently regardless of camera position/sampling offset.
/// </summary>
public class GridAlignmentTests
{
    private readonly List<BuildingDefinition> _buildingDefs;
    private readonly List<StructureDefinition> _structureDefs;
    private readonly FarmsteadTemplate _tinyFarmhouse;

    public GridAlignmentTests()
    {
        var residentialPath = Path.Combine("definitions", "buildings", "buildings_residential.txt");
        var structuresPath = Path.Combine("definitions", "outbuildings", "outbuildings_structures.txt");
        var farmsteadPath = Path.Combine("definitions", "plots", "plots_farmsteads.txt");

        _buildingDefs = BuildingParser.LoadFromFile(residentialPath);
        _structureDefs = StructureParser.LoadFromFile(structuresPath);
        _tinyFarmhouse = FarmsteadParser.LoadFromFile(farmsteadPath)!;
    }

    [Theory]
    [InlineData(0, 0)] // Origin at even grid alignment
    [InlineData(1, 0)] // Origin offset +1 in X
    [InlineData(0, 1)] // Origin offset +1 in Y
    [InlineData(1, 1)] // Origin offset +1 in both
    public void TinyFarmhouse_At50ftZoom_RendersAsSingleTile_RegardlessOfCameraOffset(int offsetX, int offsetY)
    {
        // Arrange: Create a large enough map to test offsets
        var gameState = new GameState(20, 20);
        gameState.ZoomLevel = 1; // 50ft zoom (skipFactor = 2)

        // Fill with grass
        for (int x = 0; x < 20; x++)
        {
            for (int y = 0; y < 20; y++)
            {
                gameState.Tiles[x, y] = new Tile(TileType.Grass, null, null, null);
            }
        }

        // Place farmhouse at position that includes the test offset
        // For a 2x2 building at 50ft zoom, we need to test different alignments
        int farmhouseX = 8 + offsetX;
        int farmhouseY = 8 + offsetY;
        PlaceFarmsteadPlot(gameState, _tinyFarmhouse, farmhouseX, farmhouseY);

        // Act: Simulate sampling as RenderZoomedOut does
        // At 50ft zoom, skipFactor = 2, so we sample every 2nd tile
        var uniqueBuildingOrigins = new HashSet<(int, int)>();

        for (int renderY = 0; renderY < 10; renderY++)
        {
            for (int renderX = 0; renderX < 10; renderX++)
            {
                int tileX = renderX * 2;
                int tileY = renderY * 2;

                if (tileX >= 20 || tileY >= 20)
                    continue;

                // Simulate the important structure search
                var (foundTile, originX, originY) = FindImportantStructureInSampleArea(gameState, tileX, tileY, 2);

                if (foundTile != null && foundTile.CropType == "tiny_farmhouse")
                {
                    // Record the building's origin position
                    uniqueBuildingOrigins.Add((originX, originY));
                }
            }
        }

        // Assert: Should find exactly ONE unique farmhouse origin, regardless of offset
        Assert.Single(uniqueBuildingOrigins);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 0)]
    [InlineData(2, 0)]
    [InlineData(3, 0)]
    [InlineData(0, 1)]
    [InlineData(0, 2)]
    [InlineData(0, 3)]
    [InlineData(1, 1)]
    [InlineData(2, 2)]
    [InlineData(3, 3)]
    public void TinyFarmhouse_At100ftZoom_RendersAsSingleTile_RegardlessOfCameraOffset(int offsetX, int offsetY)
    {
        // Arrange: Create a large map for 100ft zoom testing
        var gameState = new GameState(40, 40);
        gameState.ZoomLevel = 0; // 100ft zoom (skipFactor = 4)

        // Fill with grass
        for (int x = 0; x < 40; x++)
        {
            for (int y = 0; y < 40; y++)
            {
                gameState.Tiles[x, y] = new Tile(TileType.Grass, null, null, null);
            }
        }

        // Place farmhouse with various offsets relative to the 4x4 sampling grid
        int farmhouseX = 16 + offsetX;
        int farmhouseY = 16 + offsetY;
        PlaceFarmsteadPlot(gameState, _tinyFarmhouse, farmhouseX, farmhouseY);

        // Act: Simulate sampling with 4x4 grid
        var uniqueBuildingOrigins = new HashSet<(int, int)>();

        for (int renderY = 0; renderY < 10; renderY++)
        {
            for (int renderX = 0; renderX < 10; renderX++)
            {
                int tileX = renderX * 4;
                int tileY = renderY * 4;

                if (tileX >= 40 || tileY >= 40)
                    continue;

                var (foundTile, originX, originY) = FindImportantStructureInSampleArea(gameState, tileX, tileY, 4);

                if (foundTile != null && foundTile.CropType == "tiny_farmhouse")
                {
                    uniqueBuildingOrigins.Add((originX, originY));
                }
            }
        }

        // Assert: Should find exactly ONE unique farmhouse origin, regardless of grid offset
        Assert.Single(uniqueBuildingOrigins);
    }

    [Fact]
    public void MultipleFarmhouses_At50ftZoom_EachRendersOnce()
    {
        // Arrange: Place multiple farmhouses with various spacings
        var gameState = new GameState(30, 30);
        gameState.ZoomLevel = 1; // 50ft zoom

        for (int x = 0; x < 30; x++)
        {
            for (int y = 0; y < 30; y++)
            {
                gameState.Tiles[x, y] = new Tile(TileType.Grass, null, null, null);
            }
        }

        // Place 3 farmhouses at different positions
        PlaceFarmsteadPlot(gameState, _tinyFarmhouse, 5, 5);   // First farmhouse
        PlaceFarmsteadPlot(gameState, _tinyFarmhouse, 12, 5);  // Second farmhouse
        PlaceFarmsteadPlot(gameState, _tinyFarmhouse, 5, 12);  // Third farmhouse

        // Act: Sample the entire area
        var uniqueBuildingOrigins = new HashSet<(int, int)>();

        for (int renderY = 0; renderY < 15; renderY++)
        {
            for (int renderX = 0; renderX < 15; renderX++)
            {
                int tileX = renderX * 2;
                int tileY = renderY * 2;

                if (tileX >= 30 || tileY >= 30)
                    continue;

                var (foundTile, originX, originY) = FindImportantStructureInSampleArea(gameState, tileX, tileY, 2);

                if (foundTile != null && foundTile.CropType == "tiny_farmhouse")
                {
                    uniqueBuildingOrigins.Add((originX, originY));
                }
            }
        }

        // Assert: Should find exactly 3 unique farmhouse origins (one for each placed)
        Assert.Equal(3, uniqueBuildingOrigins.Count);
    }

    [Fact]
    public void AdjacentFarmhouses_At50ftZoom_BothRenderOnce()
    {
        // Arrange: Place two farmhouses right next to each other
        var gameState = new GameState(20, 20);
        gameState.ZoomLevel = 1; // 50ft zoom

        for (int x = 0; x < 20; x++)
        {
            for (int y = 0; y < 20; y++)
            {
                gameState.Tiles[x, y] = new Tile(TileType.Grass, null, null, null);
            }
        }

        // Place adjacent farmhouses (3x3 plots side by side)
        PlaceFarmsteadPlot(gameState, _tinyFarmhouse, 5, 5);   // First
        PlaceFarmsteadPlot(gameState, _tinyFarmhouse, 8, 5);   // Adjacent (3 tiles apart)

        // Act: Sample the area
        var uniqueBuildingOrigins = new HashSet<(int, int)>();

        for (int renderY = 0; renderY < 10; renderY++)
        {
            for (int renderX = 0; renderX < 10; renderX++)
            {
                int tileX = renderX * 2;
                int tileY = renderY * 2;

                if (tileX >= 20 || tileY >= 20)
                    continue;

                var (foundTile, originX, originY) = FindImportantStructureInSampleArea(gameState, tileX, tileY, 2);

                if (foundTile != null && foundTile.CropType == "tiny_farmhouse")
                {
                    uniqueBuildingOrigins.Add((originX, originY));
                }
            }
        }

        // Assert: Should find exactly 2 unique farmhouse origins
        Assert.Equal(2, uniqueBuildingOrigins.Count);
    }

    // Helper: Simulates the important structure search from RenderZoomedOut
    // Returns the found tile and its origin coordinates
    private (Tile? tile, int originX, int originY) FindImportantStructureInSampleArea(GameState gameState, int startX, int startY, int skipFactor)
    {
        Tile? importantStructure = null;
        int structurePriority = 0;
        int bestOriginX = -1;
        int bestOriginY = -1;
        HashSet<(int, int)> processedOrigins = new();

        for (int dy = 0; dy < skipFactor; dy++)
        {
            for (int dx = 0; dx < skipFactor; dx++)
            {
                int checkX = startX + dx;
                int checkY = startY + dy;

                if (checkX >= gameState.MapWidth || checkY >= gameState.MapHeight)
                    continue;

                var checkTile = gameState.Tiles[checkX, checkY];

                if (checkTile.Type == TileType.Grass && checkTile.CropType != null)
                {
                    Tile tileToConsider;
                    int originX, originY;

                    // If multi-tile building, find its origin
                    if (checkTile.BuildingOffset.HasValue)
                    {
                        originX = checkX - checkTile.BuildingOffset.Value.x;
                        originY = checkY - checkTile.BuildingOffset.Value.y;

                        // Skip if already processed this building
                        if (processedOrigins.Contains((originX, originY)))
                            continue;

                        processedOrigins.Add((originX, originY));
                        tileToConsider = gameState.Tiles[originX, originY];
                    }
                    else
                    {
                        originX = checkX;
                        originY = checkY;
                        tileToConsider = checkTile;
                    }

                    int priority = GetStructurePriority(tileToConsider.CropType);
                    if (priority > structurePriority)
                    {
                        importantStructure = tileToConsider;
                        structurePriority = priority;
                        bestOriginX = originX;
                        bestOriginY = originY;
                    }
                }
            }
        }

        return (importantStructure, bestOriginX, bestOriginY);
    }

    // Helper: Get structure priority (mimics Program.cs logic)
    private int GetStructurePriority(string? cropType)
    {
        if (cropType == null) return 0;

        return cropType.ToLower() switch
        {
            "tiny_farmhouse" => 100,
            "barn" => 90,
            "shed" => 50,
            "yard" => 10,
            _ => 0
        };
    }

    // Helper: Place a farmstead plot (same as FarmsteadPlacementTests)
    private void PlaceFarmsteadPlot(GameState gameState, FarmsteadTemplate template, int startX, int startY)
    {
        Dictionary<string, (int minX, int minY)> buildingOrigins = new();

        // First pass: find top-left corner of each multi-tile building
        for (int y = 0; y < template.Height; y++)
        {
            for (int x = 0; x < template.Width; x++)
            {
                var tileType = template.GetTileTypeAt(x, y);
                if (tileType == null) continue;

                var type = tileType.ToLower();
                if (type == "tiny_farmhouse" || type == "barn")
                {
                    if (!buildingOrigins.ContainsKey(type))
                    {
                        buildingOrigins[type] = (x, y);
                    }
                    else
                    {
                        var origin = buildingOrigins[type];
                        buildingOrigins[type] = (Math.Min(origin.minX, x), Math.Min(origin.minY, y));
                    }
                }
            }
        }

        // Second pass: place tiles
        for (int y = 0; y < template.Height; y++)
        {
            for (int x = 0; x < template.Width; x++)
            {
                int worldX = startX + x;
                int worldY = startY + y;

                if (worldX < 0 || worldX >= gameState.MapWidth || worldY < 0 || worldY >= gameState.MapHeight)
                    continue;

                var tileType = template.GetTileTypeAt(x, y);
                if (tileType == null) continue;

                switch (tileType.ToLower())
                {
                    case "yard":
                        gameState.Tiles[worldX, worldY] = new Tile(TileType.Grass, null, null, "yard");
                        break;
                    case "tiny_farmhouse":
                        var houseOrigin = buildingOrigins["tiny_farmhouse"];
                        var houseOffset = (x - houseOrigin.minX, y - houseOrigin.minY);
                        gameState.Tiles[worldX, worldY] = new Tile(TileType.Grass, null, null, "tiny_farmhouse", houseOffset);
                        break;
                    case "shed":
                        gameState.Tiles[worldX, worldY] = new Tile(TileType.Grass, null, null, "shed");
                        break;
                }
            }
        }
    }
}
