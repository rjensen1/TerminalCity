using SadRogue.Primitives;
using TerminalCity.Domain;
using TerminalCity.Parsers;
using Xunit;

namespace TerminalCity.Tests.Integration;

/// <summary>
/// Tests for placing farmstead plots within larger farm plots
/// (plot-within-a-plot scenarios)
/// </summary>
public class FarmsteadPlacementTests
{
    private readonly List<BuildingDefinition> _buildingDefs;
    private readonly List<StructureDefinition> _structureDefs;
    private readonly FarmsteadTemplate _tinyFarmhouse;

    public FarmsteadPlacementTests()
    {
        var residentialPath = Path.Combine("definitions", "buildings", "buildings_residential.txt");
        var structuresPath = Path.Combine("definitions", "outbuildings", "outbuildings_structures.txt");
        var farmsteadPath = Path.Combine("definitions", "plots", "plots_farmsteads.txt");

        _buildingDefs = BuildingParser.LoadFromFile(residentialPath);
        _structureDefs = StructureParser.LoadFromFile(structuresPath);
        _tinyFarmhouse = FarmsteadParser.LoadFromFile(farmsteadPath)!;
    }

    [Fact]
    public void TinyFarmhouse_PlacedInFarmPlot_FarmhouseRendersCorrectly()
    {
        // Arrange: Create a 10x10 farm plot
        var gameState = new GameState(10, 10);
        gameState.ZoomLevel = 2; // 25ft zoom

        // Fill entire area with farm tiles
        for (int x = 0; x < 10; x++)
        {
            for (int y = 0; y < 10; y++)
            {
                gameState.Tiles[x, y] = new Tile(TileType.Farm, null, null, "fallow_plowed");
            }
        }

        // Act: Place tiny_farmhouse plot at position (3, 3)
        PlaceFarmsteadPlot(gameState, _tinyFarmhouse, 3, 3);

        // Assert: Verify farmhouse tiles render with correct 2x2 pattern
        // Farmhouse is at plot positions (0,1), (1,1), (0,2), (1,2)
        // Which translates to world positions (3,4), (4,4), (3,5), (4,5)

        // Top row of farmhouse: ⌂╗ (chars 127, 187)
        var topLeft = GetTileAppearance(gameState.Tiles[3, 4], gameState);
        Assert.Equal((char)127, topLeft.glyph);  // ⌂ converted to extended ASCII
        Assert.Equal(Color.White, topLeft.foreground);
        Assert.Equal(Color.DarkGreen, topLeft.background);

        var topRight = GetTileAppearance(gameState.Tiles[4, 4], gameState);
        Assert.Equal((char)187, topRight.glyph);  // ╗ converted to extended ASCII
        Assert.Equal(Color.White, topRight.foreground);
        Assert.Equal(Color.DarkGreen, topRight.background);

        // Bottom row of farmhouse: └╝ (chars 192, 188)
        var bottomLeft = GetTileAppearance(gameState.Tiles[3, 5], gameState);
        Assert.Equal((char)192, bottomLeft.glyph);  // └ converted to extended ASCII
        Assert.Equal(Color.White, bottomLeft.foreground);
        Assert.Equal(Color.DarkGreen, bottomLeft.background);

        var bottomRight = GetTileAppearance(gameState.Tiles[4, 5], gameState);
        Assert.Equal((char)188, bottomRight.glyph);  // ╝ converted to extended ASCII
        Assert.Equal(Color.White, bottomRight.foreground);
        Assert.Equal(Color.DarkGreen, bottomRight.background);
    }

    [Fact]
    public void TinyFarmhouse_PlacedInFarmPlot_SurroundingTilesRemainFarmFields()
    {
        // Arrange: Create a 10x10 farm plot
        var gameState = new GameState(10, 10);
        gameState.ZoomLevel = 2; // 25ft zoom

        // Fill entire area with farm tiles
        for (int x = 0; x < 10; x++)
        {
            for (int y = 0; y < 10; y++)
            {
                gameState.Tiles[x, y] = new Tile(TileType.Farm, null, null, "fallow_plowed");
            }
        }

        // Act: Place tiny_farmhouse plot at position (3, 3)
        // Plot is 3x3, so it occupies (3,3) to (5,5)
        PlaceFarmsteadPlot(gameState, _tinyFarmhouse, 3, 3);

        // Assert: Verify tiles outside the 3x3 farmstead area are still farm tiles

        // Above the farmstead (row 2)
        Assert.Equal(TileType.Farm, gameState.Tiles[3, 2].Type);
        Assert.Equal("fallow_plowed", gameState.Tiles[3, 2].CropType);

        // Below the farmstead (row 6)
        Assert.Equal(TileType.Farm, gameState.Tiles[3, 6].Type);
        Assert.Equal("fallow_plowed", gameState.Tiles[3, 6].CropType);

        // Left of the farmstead (column 2)
        Assert.Equal(TileType.Farm, gameState.Tiles[2, 3].Type);
        Assert.Equal("fallow_plowed", gameState.Tiles[2, 3].CropType);

        // Right of the farmstead (column 6)
        Assert.Equal(TileType.Farm, gameState.Tiles[6, 3].Type);
        Assert.Equal("fallow_plowed", gameState.Tiles[6, 3].CropType);

        // Corners
        Assert.Equal(TileType.Farm, gameState.Tiles[2, 2].Type);
        Assert.Equal(TileType.Farm, gameState.Tiles[6, 6].Type);
    }

    [Fact]
    public void TinyFarmhouse_PlacedInFarmPlot_YardTilesRenderAsGrass()
    {
        // Arrange
        var gameState = new GameState(10, 10);
        gameState.ZoomLevel = 2;

        // Fill with farm tiles
        for (int x = 0; x < 10; x++)
        {
            for (int y = 0; y < 10; y++)
            {
                gameState.Tiles[x, y] = new Tile(TileType.Farm, null, null, "fallow_plowed");
            }
        }

        // Act: Place farmstead at (3, 3)
        PlaceFarmsteadPlot(gameState, _tinyFarmhouse, 3, 3);

        // Assert: Yard tiles (top row of plot) render as grass, not farm
        // Plot map row 0: "..." (all yards)
        var yard1 = GetTileAppearance(gameState.Tiles[3, 3], gameState);
        Assert.Equal('.', yard1.glyph);
        Assert.Equal(Color.Green, yard1.foreground);
        Assert.Equal(Color.DarkGreen, yard1.background);

        var yard2 = GetTileAppearance(gameState.Tiles[4, 3], gameState);
        Assert.Equal('.', yard2.glyph);

        var yard3 = GetTileAppearance(gameState.Tiles[5, 3], gameState);
        Assert.Equal('.', yard3.glyph);
    }

    [Fact]
    public void TinyFarmhouse_PlacedInFarmPlot_ShedRendersCorrectly()
    {
        // Arrange
        var gameState = new GameState(10, 10);
        gameState.ZoomLevel = 2;

        // Fill with farm tiles
        for (int x = 0; x < 10; x++)
        {
            for (int y = 0; y < 10; y++)
            {
                gameState.Tiles[x, y] = new Tile(TileType.Farm, null, null, "fallow_plowed");
            }
        }

        // Act: Place farmstead at (3, 3)
        PlaceFarmsteadPlot(gameState, _tinyFarmhouse, 3, 3);

        // Assert: Shed at position (5, 4) - plot position (2, 1)
        var shed = GetTileAppearance(gameState.Tiles[5, 4], gameState);
        Assert.Equal((char)222, shed.glyph); // ▐ converted to extended ASCII char 222
        Assert.Equal(Color.Brown, shed.foreground);
        Assert.Equal(Color.DarkGreen, shed.background);
    }

    [Fact]
    public void TinyFarmhouse_PlacedInFarmPlot_CompleteLayoutVerification()
    {
        // Arrange
        var gameState = new GameState(10, 10);
        gameState.ZoomLevel = 2;

        // Fill with farm tiles
        for (int x = 0; x < 10; x++)
        {
            for (int y = 0; y < 10; y++)
            {
                gameState.Tiles[x, y] = new Tile(TileType.Farm, null, null, "fallow_plowed");
            }
        }

        // Act: Place farmstead at (3, 3)
        PlaceFarmsteadPlot(gameState, _tinyFarmhouse, 3, 3);

        // Assert: Verify the complete 3x3 layout
        // Expected layout:
        // ...  (3,3) (4,3) (5,3) - yards
        // ⌂╗▐  (3,4) (4,4) (5,4) - farmhouse + shed
        // └╝.  (3,5) (4,5) (5,5) - farmhouse + yard

        // Row 0: yards
        Assert.Equal("yard", gameState.Tiles[3, 3].CropType);
        Assert.Equal("yard", gameState.Tiles[4, 3].CropType);
        Assert.Equal("yard", gameState.Tiles[5, 3].CropType);

        // Row 1: farmhouse + shed
        Assert.Equal("tiny_farmhouse", gameState.Tiles[3, 4].CropType);
        Assert.Equal("tiny_farmhouse", gameState.Tiles[4, 4].CropType);
        Assert.Equal("shed", gameState.Tiles[5, 4].CropType);

        // Row 2: farmhouse + yard
        Assert.Equal("tiny_farmhouse", gameState.Tiles[3, 5].CropType);
        Assert.Equal("tiny_farmhouse", gameState.Tiles[4, 5].CropType);
        Assert.Equal("yard", gameState.Tiles[5, 5].CropType);

        // Verify surrounding tiles are still farm
        Assert.Equal(TileType.Farm, gameState.Tiles[2, 3].Type);
        Assert.Equal(TileType.Farm, gameState.Tiles[6, 3].Type);
        Assert.Equal(TileType.Farm, gameState.Tiles[3, 2].Type);
        Assert.Equal(TileType.Farm, gameState.Tiles[3, 6].Type);
    }

    // Helper: Place a farmstead plot (mimics MapGenerator.PlaceFarmstead)
    private void PlaceFarmsteadPlot(GameState gameState, FarmsteadTemplate template, int startX, int startY)
    {
        // Track building origins for offset calculation
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

    // Helper: Get tile appearance
    private (char glyph, Color foreground, Color background) GetTileAppearance(Tile tile, GameState gameState)
    {
        // Handle farm tiles
        if (tile.Type == TileType.Farm && tile.CropType != null)
        {
            return ((char)240, Color.SaddleBrown, Color.Peru); // fallow_plowed
        }

        // Handle farmstead structures
        if (tile.Type == TileType.Grass && tile.CropType != null)
        {
            return GetStructureAppearance(tile, gameState);
        }

        return ('.', Color.Green, Color.DarkGreen);
    }

    private (char glyph, Color foreground, Color background) GetStructureAppearance(Tile tile, GameState gameState)
    {
        var structureType = tile.CropType;
        if (structureType == null)
            return ('.', Color.Green, Color.DarkGreen);

        if (structureType == "yard")
            return ('.', Color.Green, Color.DarkGreen);

        // Try building
        var buildingDef = _buildingDefs.FirstOrDefault(b => b.Id == structureType);
        if (buildingDef != null)
        {
            var pattern = GetBuildingPattern(buildingDef, gameState);
            if (pattern == null)
                return ('.', buildingDef.Color, buildingDef.BackgroundColor);

            // At 25ft zoom with building offset, use correct character
            if (gameState.ZoomLevel == 2 && tile.BuildingOffset.HasValue && pattern.GetHeight() > 1)
            {
                var offset = tile.BuildingOffset.Value;
                char ch = pattern.GetCharAt(offset.x, offset.y);
                return (ch, buildingDef.Color, buildingDef.BackgroundColor);
            }

            char defaultCh = pattern.Pattern.Length > 0 ? pattern.Pattern[0] : '.';
            return (defaultCh, buildingDef.Color, buildingDef.BackgroundColor);
        }

        // Try structure
        var structureDef = _structureDefs.FirstOrDefault(s =>
            s.Id == structureType || s.VariantOf == structureType);
        if (structureDef != null)
        {
            var pattern = gameState.ZoomLevel switch
            {
                2 => structureDef.Pattern25ft,
                1 => structureDef.Pattern50ft,
                0 => structureDef.Pattern100ft,
                -1 => structureDef.Pattern200ft,
                -2 => structureDef.Pattern400ft,
                _ => structureDef.Pattern100ft
            };

            if (pattern == null)
                return ('.', structureDef.Color, structureDef.BackgroundColor);

            char ch = pattern.Pattern.Length > 0 ? pattern.Pattern[0] : '.';
            return (ch, structureDef.Color, structureDef.BackgroundColor);
        }

        return ('.', Color.Green, Color.DarkGreen);
    }

    private ZoomPattern? GetBuildingPattern(BuildingDefinition def, GameState gameState)
    {
        return gameState.ZoomLevel switch
        {
            2 => def.Pattern25ft,
            1 => def.Pattern50ft,
            0 => def.Pattern100ft,
            -1 => def.Pattern200ft,
            -2 => def.Pattern400ft,
            _ => def.Pattern100ft
        };
    }
}
