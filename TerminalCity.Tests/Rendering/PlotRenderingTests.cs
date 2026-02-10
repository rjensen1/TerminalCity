using SadRogue.Primitives;
using TerminalCity.Domain;
using TerminalCity.Parsers;
using Xunit;

namespace TerminalCity.Tests.Rendering;

public class PlotRenderingTests
{
    private readonly List<BuildingDefinition> _buildingDefs;
    private readonly List<StructureDefinition> _structureDefs;
    private readonly FarmsteadTemplate _tinyFarmhouse;

    public PlotRenderingTests()
    {
        // Load real definitions
        var residentialPath = Path.Combine("definitions", "buildings", "buildings_residential.txt");
        var structuresPath = Path.Combine("definitions", "outbuildings", "outbuildings_structures.txt");
        var farmsteadPath = Path.Combine("definitions", "plots", "plots_farmsteads.txt");

        _buildingDefs = BuildingParser.LoadFromFile(residentialPath);
        _structureDefs = StructureParser.LoadFromFile(structuresPath);
        _tinyFarmhouse = FarmsteadParser.LoadFromFile(farmsteadPath)!;
    }

    [Fact]
    public void TinyFarmhousePlot_LoadsCorrectly()
    {
        // Assert
        Assert.NotNull(_tinyFarmhouse);
        Assert.Equal("tiny_farmhouse", _tinyFarmhouse.Id);
        Assert.Equal(3, _tinyFarmhouse.Width);
        Assert.Equal(3, _tinyFarmhouse.Height);
        Assert.Equal(3, _tinyFarmhouse.MapRows.Count);
    }

    [Fact]
    public void TinyFarmhousePlot_At25ftZoom_AllTilesRenderCorrectly()
    {
        // Arrange
        var gameState = new GameState(10, 10);
        gameState.ZoomLevel = 2; // 25ft zoom
        PlacePlot(gameState, _tinyFarmhouse, 0, 0);

        // Act & Assert - Verify each tile in the 3x3 plot
        // Row 0: ... (yard, yard, yard)
        AssertTileAppearance(gameState, 0, 0, '.', Color.Green, Color.DarkGreen, "yard");
        AssertTileAppearance(gameState, 1, 0, '.', Color.Green, Color.DarkGreen, "yard");
        AssertTileAppearance(gameState, 2, 0, '.', Color.Green, Color.DarkGreen, "yard");

        // Row 1: HHS (tiny_farmhouse 2x2 pattern starts here, shed)
        // Pattern row 0: ⌂╗
        AssertTileAppearance(gameState, 0, 1, '⌂', Color.White, Color.DarkKhaki, "tiny_farmhouse"); // top-left
        AssertTileAppearance(gameState, 1, 1, '╗', Color.White, Color.DarkKhaki, "tiny_farmhouse"); // top-right
        AssertTileAppearance(gameState, 2, 1, '▐', Color.Brown, Color.SaddleBrown, "shed");

        // Row 2: HH. (tiny_farmhouse continued, yard)
        // Pattern row 1: └╝
        AssertTileAppearance(gameState, 0, 2, '└', Color.White, Color.DarkKhaki, "tiny_farmhouse"); // bottom-left
        AssertTileAppearance(gameState, 1, 2, '╝', Color.White, Color.DarkKhaki, "tiny_farmhouse"); // bottom-right
        AssertTileAppearance(gameState, 2, 2, '.', Color.Green, Color.DarkGreen, "yard");
    }

    [Fact]
    public void TinyFarmhousePlot_At50ftZoom_ShedChangesCharacter()
    {
        // Arrange
        var gameState = new GameState(10, 10);
        gameState.ZoomLevel = 1; // 50ft zoom
        PlacePlot(gameState, _tinyFarmhouse, 0, 0);

        // Act & Assert
        // At 50ft, shed should show as ║ instead of ▐
        var shedAppearance = GetTileAppearance(gameState.Tiles[2, 1], gameState);
        Assert.Equal('║', shedAppearance.glyph);
        Assert.Equal(Color.Brown, shedAppearance.foreground);

        // Farmhouse should still show as ⌂
        var farmhouseAppearance = GetTileAppearance(gameState.Tiles[0, 1], gameState);
        Assert.Equal('⌂', farmhouseAppearance.glyph);
        Assert.Equal(Color.White, farmhouseAppearance.foreground);
    }

    [Fact]
    public void TinyFarmhousePlot_At100ftZoom_OnlyFarmhouseVisible()
    {
        // Arrange
        var gameState = new GameState(10, 10);
        gameState.ZoomLevel = 0; // 100ft zoom
        PlacePlot(gameState, _tinyFarmhouse, 0, 0);

        // Act & Assert
        // Farmhouse should still render
        var farmhouseAppearance = GetTileAppearance(gameState.Tiles[0, 1], gameState);
        Assert.Equal('⌂', farmhouseAppearance.glyph);

        // Shed has no pattern at 100ft, so it returns null or falls back
        // The actual game would skip rendering or show as grass
        var shedPattern = GetStructurePattern(gameState.Tiles[2, 1].CropType, gameState);
        Assert.Null(shedPattern);
    }

    [Fact]
    public void TinyFarmhousePlot_HasCorrectLegendMapping()
    {
        // Arrange & Assert
        Assert.Equal("yard", _tinyFarmhouse.Legend['.']);
        Assert.Equal("tiny_farmhouse", _tinyFarmhouse.Legend['H']);
        Assert.Equal("shed", _tinyFarmhouse.Legend['S']);
    }

    [Fact]
    public void TinyFarmhousePlot_MapMatchesExpectedPattern()
    {
        // Assert - Verify the visual map matches what we expect
        Assert.Equal("...", _tinyFarmhouse.MapRows[0]);
        Assert.Equal("HHS", _tinyFarmhouse.MapRows[1]);
        Assert.Equal("HH.", _tinyFarmhouse.MapRows[2]);
    }

    [Fact]
    public void TinyFarmhousePlot_FarmhouseOccupiesFourTiles()
    {
        // Arrange
        var gameState = new GameState(10, 10);
        PlacePlot(gameState, _tinyFarmhouse, 0, 0);

        // Act - Count farmhouse tiles
        int farmhouseCount = 0;
        for (int y = 0; y < _tinyFarmhouse.Height; y++)
        {
            for (int x = 0; x < _tinyFarmhouse.Width; x++)
            {
                if (gameState.Tiles[x, y].CropType == "tiny_farmhouse")
                {
                    farmhouseCount++;
                }
            }
        }

        // Assert - tiny_farmhouse should occupy 4 tiles (2x2 building)
        Assert.Equal(4, farmhouseCount);
    }

    // Helper: Place a farmstead plot on the game map
    private void PlacePlot(GameState gameState, FarmsteadTemplate template, int startX, int startY)
    {
        // Track the first occurrence of multi-tile buildings to calculate offsets
        Dictionary<string, (int minX, int minY)> buildingOrigins = new();

        // First pass: find the top-left corner of each multi-tile building
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

        // Second pass: place tiles with building offsets
        for (int y = 0; y < template.Height; y++)
        {
            for (int x = 0; x < template.Width; x++)
            {
                int worldX = startX + x;
                int worldY = startY + y;

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

    // Helper: Assert a tile renders with expected appearance
    private void AssertTileAppearance(GameState gameState, int x, int y, char expectedGlyph,
        Color expectedForeground, Color expectedBackground, string expectedCropType)
    {
        var tile = gameState.Tiles[x, y];
        Assert.Equal(expectedCropType, tile.CropType);

        var appearance = GetTileAppearance(tile, gameState);
        Assert.Equal(expectedGlyph, appearance.glyph);
        Assert.Equal(expectedForeground, appearance.foreground);
        Assert.Equal(expectedBackground, appearance.background);
    }

    // Helper: Get tile appearance (mimics Program.cs logic)
    private (char glyph, Color foreground, Color background) GetTileAppearance(Tile tile, GameState gameState)
    {
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
        if (structureType == "driveway")
            return ((char)176, Color.Tan, Color.SaddleBrown);

        // Try building
        var buildingDef = _buildingDefs.FirstOrDefault(b => b.Id == structureType);
        if (buildingDef != null)
        {
            var pattern = GetBuildingPattern(buildingDef, gameState);
            if (pattern == null)
                return ('.', buildingDef.Color, buildingDef.BackgroundColor);

            // At 25ft zoom with multi-tile buildings, use the building offset
            if (gameState.ZoomLevel == 2 && tile.BuildingOffset.HasValue && pattern.GetHeight() > 1)
            {
                var offset = tile.BuildingOffset.Value;
                char ch = pattern.GetCharAt(offset.x, offset.y);
                return (ch, buildingDef.Color, buildingDef.BackgroundColor);
            }

            // For single-character patterns or other zoom levels
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

    private ZoomPattern? GetStructurePattern(string? structureType, GameState gameState)
    {
        if (structureType == null) return null;

        var structureDef = _structureDefs.FirstOrDefault(s =>
            s.Id == structureType || s.VariantOf == structureType);

        if (structureDef == null) return null;

        return gameState.ZoomLevel switch
        {
            2 => structureDef.Pattern25ft,
            1 => structureDef.Pattern50ft,
            0 => structureDef.Pattern100ft,
            -1 => structureDef.Pattern200ft,
            -2 => structureDef.Pattern400ft,
            _ => structureDef.Pattern100ft
        };
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
