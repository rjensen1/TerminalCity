using SadRogue.Primitives;
using TerminalCity.Domain;
using TerminalCity.Parsers;
using Xunit;

namespace TerminalCity.Tests.Rendering;

public class ZoomPatternRenderingTests
{
    private readonly List<BuildingDefinition> _buildingDefs;
    private readonly List<StructureDefinition> _structureDefs;

    public ZoomPatternRenderingTests()
    {
        // Load real definitions from files
        var residentialPath = Path.Combine("definitions", "buildings", "buildings_residential.txt");
        var structuresPath = Path.Combine("definitions", "outbuildings", "outbuildings_structures.txt");

        _buildingDefs = BuildingParser.LoadFromFile(residentialPath);
        _structureDefs = StructureParser.LoadFromFile(structuresPath);
    }

    [Fact]
    public void TinyFarmhouse_At25ftZoom_ShowsMultiCharacterPattern()
    {
        // Arrange
        var gameState = new GameState(10, 10);
        gameState.ZoomLevel = 2; // 25ft zoom

        // Act
        var appearance = GetStructureAppearance("tiny_farmhouse", gameState);

        // Assert
        Assert.NotNull(appearance);
        // At 25ft, should use the first character of the 2x2 pattern
        Assert.Equal('⌂', appearance.Value.glyph);
        Assert.Equal(Color.White, appearance.Value.foreground);
        Assert.Equal(Color.DarkKhaki, appearance.Value.background);
    }

    [Fact]
    public void TinyFarmhouse_At50ftZoom_ShowsSingleCharacter()
    {
        // Arrange
        var gameState = new GameState(10, 10);
        gameState.ZoomLevel = 1; // 50ft zoom

        // Act
        var appearance = GetStructureAppearance("tiny_farmhouse", gameState);

        // Assert
        Assert.NotNull(appearance);
        Assert.Equal('⌂', appearance.Value.glyph);
        Assert.Equal(Color.White, appearance.Value.foreground);
    }

    [Fact]
    public void TinyFarmhouse_At100ftZoom_ShowsSingleCharacter()
    {
        // Arrange
        var gameState = new GameState(10, 10);
        gameState.ZoomLevel = 0; // 100ft zoom (default)

        // Act
        var appearance = GetStructureAppearance("tiny_farmhouse", gameState);

        // Assert
        Assert.NotNull(appearance);
        Assert.Equal('⌂', appearance.Value.glyph);
    }

    [Fact]
    public void TinyFarmhouse_At200ftZoom_IsMarkedImportant()
    {
        // Arrange
        var building = _buildingDefs.FirstOrDefault(b => b.Id == "tiny_farmhouse");

        // Assert
        Assert.NotNull(building);
        Assert.NotNull(building.Pattern200ft);
        Assert.True(building.Pattern200ft.Important);
    }

    [Fact]
    public void TinyFarmhouse_At400ftZoom_IsNotImportant()
    {
        // Arrange
        var building = _buildingDefs.FirstOrDefault(b => b.Id == "tiny_farmhouse");

        // Assert
        Assert.NotNull(building);
        Assert.NotNull(building.Pattern400ft);
        Assert.False(building.Pattern400ft.Important);
    }

    [Fact]
    public void Shed_At25ftZoom_ShowsRightHalfBlock()
    {
        // Arrange
        var gameState = new GameState(10, 10);
        gameState.ZoomLevel = 2; // 25ft zoom

        // Act
        var appearance = GetStructureAppearance("shed", gameState);

        // Assert
        Assert.NotNull(appearance);
        Assert.Equal('▐', appearance.Value.glyph);
        Assert.Equal(Color.Brown, appearance.Value.foreground);
    }

    [Fact]
    public void Shed_At50ftZoom_ShowsDoubleVerticalLine()
    {
        // Arrange
        var gameState = new GameState(10, 10);
        gameState.ZoomLevel = 1; // 50ft zoom

        // Act
        var appearance = GetStructureAppearance("shed", gameState);

        // Assert
        Assert.NotNull(appearance);
        Assert.Equal('║', appearance.Value.glyph);
        Assert.Equal(Color.Brown, appearance.Value.foreground);
    }

    [Fact]
    public void Shed_At50ftZoom_IsNotImportant()
    {
        // Arrange
        var structure = _structureDefs.FirstOrDefault(s => s.VariantOf == "shed");

        // Assert
        Assert.NotNull(structure);
        Assert.NotNull(structure.Pattern50ft);
        Assert.False(structure.Pattern50ft.Important);
    }

    [Fact]
    public void Shed_At100ftZoom_HasNoPattern()
    {
        // Arrange
        var structure = _structureDefs.FirstOrDefault(s => s.VariantOf == "shed");

        // Assert
        Assert.NotNull(structure);
        Assert.Null(structure.Pattern100ft);
        Assert.Null(structure.Pattern200ft);
        Assert.Null(structure.Pattern400ft);
    }

    [Fact]
    public void Yard_AlwaysRendersAsGrass()
    {
        // Arrange
        var gameState = new GameState(10, 10);

        // Test at multiple zoom levels
        for (int zoom = -2; zoom <= 2; zoom++)
        {
            gameState.ZoomLevel = zoom;

            // Act
            var appearance = GetStructureAppearance("yard", gameState);

            // Assert
            Assert.NotNull(appearance);
            Assert.Equal('.', appearance.Value.glyph);
            Assert.Equal(Color.Green, appearance.Value.foreground);
            Assert.Equal(Color.DarkGreen, appearance.Value.background);
        }
    }

    // Helper method that mimics the rendering logic from Program.cs
    private (char glyph, Color foreground, Color background)? GetStructureAppearance(string structureType, GameState gameState)
    {
        // Special cases
        if (structureType == "yard")
            return ('.', Color.Green, Color.DarkGreen);
        if (structureType == "driveway")
            return ((char)176, Color.Tan, Color.SaddleBrown);

        // Try building definition
        var buildingDef = _buildingDefs.FirstOrDefault(b => b.Id == structureType);
        if (buildingDef != null)
        {
            var pattern = GetBuildingPattern(buildingDef, gameState);
            if (pattern == null)
                return null;

            char ch = pattern.Pattern.Length > 0 ? pattern.Pattern[0] : '.';
            return (ch, buildingDef.Color, buildingDef.BackgroundColor);
        }

        // Try structure definition
        var structureDef = _structureDefs.FirstOrDefault(s =>
            s.Id == structureType || s.VariantOf == structureType);
        if (structureDef != null)
        {
            var pattern = GetStructurePattern(structureDef, gameState);
            if (pattern == null)
                return null;

            char ch = pattern.Pattern.Length > 0 ? pattern.Pattern[0] : '.';
            return (ch, structureDef.Color, structureDef.BackgroundColor);
        }

        return null;
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

    private ZoomPattern? GetStructurePattern(StructureDefinition def, GameState gameState)
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
