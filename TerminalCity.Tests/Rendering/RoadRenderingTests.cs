using SadRogue.Primitives;
using TerminalCity.Domain;
using TerminalCity.Rendering;
using Xunit;

namespace TerminalCity.Tests.Rendering;

/// <summary>
/// Tests for road rendering at different zoom levels
/// </summary>
public class RoadRenderingTests
{
    [Theory]
    [InlineData(-2, 196, "SandyBrown", "Green")]  // 400ft: ─
    [InlineData(-1, 205, "SandyBrown", "Green")]  // 200ft: ═
    [InlineData(0, 177, "SandyBrown", "Green")]   // 100ft: ░
    [InlineData(1, 178, "SandyBrown", "Green")]   // 50ft: ▒
    [InlineData(2, 219, "SandyBrown", "DarkGray")]   // 25ft: █
    public void DirtRoad_AtDifferentZoomLevels_UsesCorrectGlyphAndColors(
        int zoomLevel, int expectedGlyphCode, string expectedForegroundName, string expectedBackgroundName)
    {
        // Arrange
        var roadType = TileType.DirtRoad;
        var expectedGlyph = (char)expectedGlyphCode;
        var expectedForeground = GetColorByName(expectedForegroundName);
        var expectedBackground = GetColorByName(expectedBackgroundName);

        // Act
        var (glyph, foreground, background) = RoadRenderer.GetRoadAppearance(roadType, zoomLevel);

        // Assert
        Assert.Equal(expectedGlyph, glyph);
        Assert.Equal(expectedForeground, foreground);
        Assert.Equal(expectedBackground, background);
    }

    [Theory]
    [InlineData(-2, 196, "DarkGray", "Green")]    // 400ft: ─
    [InlineData(-1, 205, "DarkGray", "Green")]    // 200ft: ═
    [InlineData(0, 177, "DarkGray", "Green")]     // 100ft: ░
    [InlineData(1, 178, "DarkGray", "Green")]     // 50ft: ▒
    [InlineData(2, 219, "DarkGray", "Black")]     // 25ft: █ - paved roads have black background
    public void PavedRoad_AtDifferentZoomLevels_UsesCorrectGlyphAndColors(
        int zoomLevel, int expectedGlyphCode, string expectedForegroundName, string expectedBackgroundName)
    {
        // Arrange
        var roadType = TileType.PavedRoad;
        var expectedGlyph = (char)expectedGlyphCode;
        var expectedForeground = GetColorByName(expectedForegroundName);
        var expectedBackground = GetColorByName(expectedBackgroundName);

        // Act
        var (glyph, foreground, background) = RoadRenderer.GetRoadAppearance(roadType, zoomLevel);

        // Assert
        Assert.Equal(expectedGlyph, glyph);
        Assert.Equal(expectedForeground, foreground);
        Assert.Equal(expectedBackground, background);
    }

    [Fact]
    public void ThreeByThreeGrid_WithRoadInMiddle_RendersCorrectlyAtZoom0()
    {
        // Arrange - Create a 3x3 grid with a horizontal road in the middle
        var gameState = new GameState(3, 3);

        // Set up grid: grass everywhere except middle row which is dirt road
        for (int x = 0; x < 3; x++)
        {
            gameState.Tiles[x, 0] = new Tile(TileType.Grass, null);
            gameState.Tiles[x, 1] = new Tile(TileType.DirtRoad, null);
            gameState.Tiles[x, 2] = new Tile(TileType.Grass, null);
        }

        gameState.ZoomLevel = 0; // 100ft zoom

        // Act & Assert - Check each tile's appearance
        for (int x = 0; x < 3; x++)
        {
            // Top row: grass (simple assertion - grass doesn't use renderer)
            var topTile = gameState.Tiles[x, 0];
            Assert.Equal(TileType.Grass, topTile.Type);

            // Middle row: dirt road
            var (glyph, fg, bg) = RoadRenderer.GetRoadAppearance(TileType.DirtRoad, gameState.ZoomLevel);
            Assert.Equal((char)177, glyph);  // Should be ░ (light shade) at zoom 0
            Assert.Equal(Color.SandyBrown, fg);
            Assert.Equal(Color.Green, bg);

            // Bottom row: grass (simple assertion - grass doesn't use renderer)
            var bottomTile = gameState.Tiles[x, 2];
            Assert.Equal(TileType.Grass, bottomTile.Type);
        }
    }

    private Color GetColorByName(string name)
    {
        return name switch
        {
            "SandyBrown" => Color.SandyBrown,
            "DarkGray" => Color.DarkGray,
            "Pink" => Color.Pink,
            "Green" => Color.Green,
            "Black" => Color.Black,
            _ => Color.White
        };
    }
}
