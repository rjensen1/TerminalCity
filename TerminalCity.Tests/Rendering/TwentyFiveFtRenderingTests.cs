using SadRogue.Primitives;
using TerminalCity.Domain;
using Xunit;

namespace TerminalCity.Tests.Rendering;

/// <summary>
/// Classical unit tests for 25ft zoom rendering logic.
/// These tests ensure the multi-tile building pattern rendering works correctly
/// and won't regress when fixing issues at other zoom levels.
/// </summary>
public class TwentyFiveFtRenderingTests
{
    #region ZoomPattern.GetCharAt Tests

    [Fact]
    public void ZoomPattern_GetCharAt_SingleLinePattern_ReturnsCorrectCharacter()
    {
        // Arrange
        var pattern = new ZoomPattern { Pattern = "⌂" };

        // Act
        char result = pattern.GetCharAt(0, 0);

        // Assert
        Assert.Equal('⌂', result);
    }

    [Fact]
    public void ZoomPattern_GetCharAt_MultiLinePattern_TopLeft_ReturnsCorrectCharacter()
    {
        // Arrange - 2x2 farmhouse pattern
        var pattern = new ZoomPattern { Pattern = "⌂╗\n└╝" };

        // Act
        char result = pattern.GetCharAt(0, 0);

        // Assert
        Assert.Equal('⌂', result);
    }

    [Fact]
    public void ZoomPattern_GetCharAt_MultiLinePattern_TopRight_ReturnsCorrectCharacter()
    {
        // Arrange - 2x2 farmhouse pattern
        var pattern = new ZoomPattern { Pattern = "⌂╗\n└╝" };

        // Act
        char result = pattern.GetCharAt(1, 0);

        // Assert
        Assert.Equal('╗', result);
    }

    [Fact]
    public void ZoomPattern_GetCharAt_MultiLinePattern_BottomLeft_ReturnsCorrectCharacter()
    {
        // Arrange - 2x2 farmhouse pattern
        var pattern = new ZoomPattern { Pattern = "⌂╗\n└╝" };

        // Act
        char result = pattern.GetCharAt(0, 1);

        // Assert
        Assert.Equal('└', result);
    }

    [Fact]
    public void ZoomPattern_GetCharAt_MultiLinePattern_BottomRight_ReturnsCorrectCharacter()
    {
        // Arrange - 2x2 farmhouse pattern
        var pattern = new ZoomPattern { Pattern = "⌂╗\n└╝" };

        // Act
        char result = pattern.GetCharAt(1, 1);

        // Assert
        Assert.Equal('╝', result);
    }

    [Fact]
    public void ZoomPattern_GetCharAt_OutOfBoundsNegativeX_ReturnsSpace()
    {
        // Arrange
        var pattern = new ZoomPattern { Pattern = "⌂╗\n└╝" };

        // Act
        char result = pattern.GetCharAt(-1, 0);

        // Assert
        Assert.Equal(' ', result);
    }

    [Fact]
    public void ZoomPattern_GetCharAt_OutOfBoundsNegativeY_ReturnsSpace()
    {
        // Arrange
        var pattern = new ZoomPattern { Pattern = "⌂╗\n└╝" };

        // Act
        char result = pattern.GetCharAt(0, -1);

        // Assert
        Assert.Equal(' ', result);
    }

    [Fact]
    public void ZoomPattern_GetCharAt_OutOfBoundsPositiveX_ReturnsSpace()
    {
        // Arrange
        var pattern = new ZoomPattern { Pattern = "⌂╗\n└╝" };

        // Act
        char result = pattern.GetCharAt(2, 0);

        // Assert
        Assert.Equal(' ', result);
    }

    [Fact]
    public void ZoomPattern_GetCharAt_OutOfBoundsPositiveY_ReturnsSpace()
    {
        // Arrange
        var pattern = new ZoomPattern { Pattern = "⌂╗\n└╝" };

        // Act
        char result = pattern.GetCharAt(0, 2);

        // Assert
        Assert.Equal(' ', result);
    }

    [Fact]
    public void ZoomPattern_GetCharAt_ThreeByThreePattern_AllPositions_ReturnCorrectCharacters()
    {
        // Arrange - larger pattern to test more complex layouts
        var pattern = new ZoomPattern { Pattern = "ABC\nDEF\nGHI" };

        // Act & Assert - test all 9 positions
        Assert.Equal('A', pattern.GetCharAt(0, 0));
        Assert.Equal('B', pattern.GetCharAt(1, 0));
        Assert.Equal('C', pattern.GetCharAt(2, 0));
        Assert.Equal('D', pattern.GetCharAt(0, 1));
        Assert.Equal('E', pattern.GetCharAt(1, 1));
        Assert.Equal('F', pattern.GetCharAt(2, 1));
        Assert.Equal('G', pattern.GetCharAt(0, 2));
        Assert.Equal('H', pattern.GetCharAt(1, 2));
        Assert.Equal('I', pattern.GetCharAt(2, 2));
    }

    #endregion

    #region ZoomPattern.GetHeight Tests

    [Fact]
    public void ZoomPattern_GetHeight_SingleLinePattern_ReturnsOne()
    {
        // Arrange
        var pattern = new ZoomPattern { Pattern = "⌂" };

        // Act
        int height = pattern.GetHeight();

        // Assert
        Assert.Equal(1, height);
    }

    [Fact]
    public void ZoomPattern_GetHeight_TwoLinePattern_ReturnsTwo()
    {
        // Arrange
        var pattern = new ZoomPattern { Pattern = "⌂╗\n└╝" };

        // Act
        int height = pattern.GetHeight();

        // Assert
        Assert.Equal(2, height);
    }

    [Fact]
    public void ZoomPattern_GetHeight_ThreeLinePattern_ReturnsThree()
    {
        // Arrange
        var pattern = new ZoomPattern { Pattern = "ABC\nDEF\nGHI" };

        // Act
        int height = pattern.GetHeight();

        // Assert
        Assert.Equal(3, height);
    }

    [Fact]
    public void ZoomPattern_GetHeight_EmptyPattern_ReturnsOne()
    {
        // Arrange - empty string splits into one empty line
        var pattern = new ZoomPattern { Pattern = "" };

        // Act
        int height = pattern.GetHeight();

        // Assert
        Assert.Equal(1, height);
    }

    #endregion

    #region ZoomPattern.GetWidth Tests

    [Fact]
    public void ZoomPattern_GetWidth_SingleCharPattern_ReturnsOne()
    {
        // Arrange
        var pattern = new ZoomPattern { Pattern = "⌂" };

        // Act
        int width = pattern.GetWidth();

        // Assert
        Assert.Equal(1, width);
    }

    [Fact]
    public void ZoomPattern_GetWidth_TwoCharPattern_ReturnsTwo()
    {
        // Arrange
        var pattern = new ZoomPattern { Pattern = "⌂╗\n└╝" };

        // Act
        int width = pattern.GetWidth();

        // Assert
        Assert.Equal(2, width);
    }

    [Fact]
    public void ZoomPattern_GetWidth_ThreeCharPattern_ReturnsThree()
    {
        // Arrange
        var pattern = new ZoomPattern { Pattern = "ABC\nDEF\nGHI" };

        // Act
        int width = pattern.GetWidth();

        // Assert
        Assert.Equal(3, width);
    }

    [Fact]
    public void ZoomPattern_GetWidth_JaggedPattern_ReturnsMaxWidth()
    {
        // Arrange - pattern with varying line lengths
        var pattern = new ZoomPattern { Pattern = "A\nBCD\nEF" };

        // Act
        int width = pattern.GetWidth();

        // Assert
        Assert.Equal(3, width); // Max line length is 3 (from "BCD")
    }

    [Fact]
    public void ZoomPattern_GetWidth_EmptyPattern_ReturnsZero()
    {
        // Arrange
        var pattern = new ZoomPattern { Pattern = "" };

        // Act
        int width = pattern.GetWidth();

        // Assert
        Assert.Equal(0, width);
    }

    #endregion

    #region BuildingOffset Integration Tests

    [Fact]
    public void TinyFarmhouse_At25ftZoom_TopLeftTile_ShowsTopLeftCharacter()
    {
        // Arrange
        var definition = new BuildingDefinition
        {
            Id = "tiny_farmhouse",
            Color = Color.White,
            BackgroundColor = Color.DarkKhaki,
            Pattern25ft = new ZoomPattern { Pattern = "⌂╗\n└╝" }
        };

        var tile = new Tile(TileType.Grass, null, null, "tiny_farmhouse", buildingOffset: (0, 0));

        // Act
        var appearance = GetBuildingAppearance(definition, tile, zoomLevel: 2);

        // Assert
        Assert.Equal('⌂', appearance.glyph);
        Assert.Equal(Color.White, appearance.foreground);
        Assert.Equal(Color.DarkKhaki, appearance.background);
    }

    [Fact]
    public void TinyFarmhouse_At25ftZoom_TopRightTile_ShowsTopRightCharacter()
    {
        // Arrange
        var definition = new BuildingDefinition
        {
            Id = "tiny_farmhouse",
            Color = Color.White,
            BackgroundColor = Color.DarkKhaki,
            Pattern25ft = new ZoomPattern { Pattern = "⌂╗\n└╝" }
        };

        var tile = new Tile(TileType.Grass, null, null, "tiny_farmhouse", buildingOffset: (1, 0));

        // Act
        var appearance = GetBuildingAppearance(definition, tile, zoomLevel: 2);

        // Assert
        Assert.Equal('╗', appearance.glyph);
        Assert.Equal(Color.White, appearance.foreground);
        Assert.Equal(Color.DarkKhaki, appearance.background);
    }

    [Fact]
    public void TinyFarmhouse_At25ftZoom_BottomLeftTile_ShowsBottomLeftCharacter()
    {
        // Arrange
        var definition = new BuildingDefinition
        {
            Id = "tiny_farmhouse",
            Color = Color.White,
            BackgroundColor = Color.DarkKhaki,
            Pattern25ft = new ZoomPattern { Pattern = "⌂╗\n└╝" }
        };

        var tile = new Tile(TileType.Grass, null, null, "tiny_farmhouse", buildingOffset: (0, 1));

        // Act
        var appearance = GetBuildingAppearance(definition, tile, zoomLevel: 2);

        // Assert
        Assert.Equal('└', appearance.glyph);
        Assert.Equal(Color.White, appearance.foreground);
        Assert.Equal(Color.DarkKhaki, appearance.background);
    }

    [Fact]
    public void TinyFarmhouse_At25ftZoom_BottomRightTile_ShowsBottomRightCharacter()
    {
        // Arrange
        var definition = new BuildingDefinition
        {
            Id = "tiny_farmhouse",
            Color = Color.White,
            BackgroundColor = Color.DarkKhaki,
            Pattern25ft = new ZoomPattern { Pattern = "⌂╗\n└╝" }
        };

        var tile = new Tile(TileType.Grass, null, null, "tiny_farmhouse", buildingOffset: (1, 1));

        // Act
        var appearance = GetBuildingAppearance(definition, tile, zoomLevel: 2);

        // Assert
        Assert.Equal('╝', appearance.glyph);
        Assert.Equal(Color.White, appearance.foreground);
        Assert.Equal(Color.DarkKhaki, appearance.background);
    }

    [Fact]
    public void TinyFarmhouse_At50ftZoom_UsesFirstCharacter_IgnoresOffset()
    {
        // Arrange
        var definition = new BuildingDefinition
        {
            Id = "tiny_farmhouse",
            Color = Color.White,
            BackgroundColor = Color.DarkKhaki,
            Pattern50ft = new ZoomPattern { Pattern = "⌂" }
        };

        // Note: BuildingOffset is present but should be ignored at non-25ft zoom
        var tile = new Tile(TileType.Grass, null, null, "tiny_farmhouse", buildingOffset: (1, 1));

        // Act
        var appearance = GetBuildingAppearance(definition, tile, zoomLevel: 1);

        // Assert
        Assert.Equal('⌂', appearance.glyph); // Should use first char, not offset char
    }

    [Fact]
    public void TinyFarmhouse_At25ftZoom_WithoutOffset_UsesFirstCharacter()
    {
        // Arrange
        var definition = new BuildingDefinition
        {
            Id = "tiny_farmhouse",
            Color = Color.White,
            BackgroundColor = Color.DarkKhaki,
            Pattern25ft = new ZoomPattern { Pattern = "⌂╗\n└╝" }
        };

        var tile = new Tile(TileType.Grass, null, null, "tiny_farmhouse", buildingOffset: null);

        // Act
        var appearance = GetBuildingAppearance(definition, tile, zoomLevel: 2);

        // Assert
        Assert.Equal('⌂', appearance.glyph); // Should default to first character
    }

    [Fact]
    public void TinyFarmhouse_At25ftZoom_SingleLinePattern_UsesFirstCharacter()
    {
        // Arrange - pattern height = 1, so offset logic should not apply
        var definition = new BuildingDefinition
        {
            Id = "tiny_farmhouse",
            Color = Color.White,
            BackgroundColor = Color.DarkKhaki,
            Pattern25ft = new ZoomPattern { Pattern = "⌂" }
        };

        var tile = new Tile(TileType.Grass, null, null, "tiny_farmhouse", buildingOffset: (1, 1));

        // Act
        var appearance = GetBuildingAppearance(definition, tile, zoomLevel: 2);

        // Assert
        Assert.Equal('⌂', appearance.glyph); // Single-line pattern, so offset is ignored
    }

    [Fact]
    public void Building_At25ftZoom_NullPattern_ReturnsDefaultGlyph()
    {
        // Arrange
        var definition = new BuildingDefinition
        {
            Id = "test_building",
            Color = Color.Gray,
            BackgroundColor = Color.Black,
            Pattern25ft = null
        };

        var tile = new Tile(TileType.Grass, null, null, "test_building", buildingOffset: (0, 0));

        // Act
        var appearance = GetBuildingAppearance(definition, tile, zoomLevel: 2);

        // Assert
        Assert.Equal('.', appearance.glyph);
        Assert.Equal(Color.Gray, appearance.foreground);
        Assert.Equal(Color.Black, appearance.background);
    }

    [Fact]
    public void Building_At25ftZoom_EmptyPattern_ReturnsDefaultGlyph()
    {
        // Arrange
        var definition = new BuildingDefinition
        {
            Id = "test_building",
            Color = Color.Gray,
            BackgroundColor = Color.Black,
            Pattern25ft = new ZoomPattern { Pattern = "" }
        };

        var tile = new Tile(TileType.Grass, null, null, "test_building", buildingOffset: (0, 0));

        // Act
        var appearance = GetBuildingAppearance(definition, tile, zoomLevel: 2);

        // Assert
        Assert.Equal('.', appearance.glyph);
        Assert.Equal(Color.Gray, appearance.foreground);
        Assert.Equal(Color.Black, appearance.background);
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Mimics the GetBuildingAppearance logic from Program.cs
    /// This is extracted for testability
    /// </summary>
    private (char glyph, Color foreground, Color background) GetBuildingAppearance(
        BuildingDefinition def,
        Tile tile,
        int zoomLevel)
    {
        var pattern = GetBuildingPattern(def, zoomLevel);
        if (pattern == null)
            return ('.', def.Color, def.BackgroundColor);

        // At 25ft zoom with multi-tile buildings, use the building offset to look up the correct character
        if (zoomLevel == 2 && tile.BuildingOffset.HasValue && pattern.GetHeight() > 1)
        {
            var offset = tile.BuildingOffset.Value;
            char ch = pattern.GetCharAt(offset.x, offset.y);
            return (ch, def.Color, def.BackgroundColor);
        }

        // For single-character patterns or other zoom levels, use the first character
        char defaultCh = pattern.Pattern.Length > 0 ? pattern.Pattern[0] : '.';
        return (defaultCh, def.Color, def.BackgroundColor);
    }

    private ZoomPattern? GetBuildingPattern(BuildingDefinition def, int zoomLevel)
    {
        return zoomLevel switch
        {
            2 => def.Pattern25ft,   // 25ft zoom
            1 => def.Pattern50ft,   // 50ft zoom
            0 => def.Pattern100ft,  // 100ft zoom (default)
            -1 => def.Pattern200ft, // 200ft zoom
            -2 => def.Pattern400ft, // 400ft zoom
            _ => def.Pattern100ft   // Default to 100ft
        };
    }

    #endregion
}
