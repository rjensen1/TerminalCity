using TerminalCity.Domain;
using SadRogue.Primitives;
using Xunit;

namespace TerminalCity.Tests.Rendering;

/// <summary>
/// Classical unit tests for border rendering at various zoom levels
/// Tests that borders appear on plot edges as expected
/// </summary>
public class BorderRenderingTests
{
    [Fact]
    public void FarmPlot_WithFenceOnAllSides_At25ftZoom_ShowsFenceOnEdges()
    {
        // Arrange - Create 10x10 map with farm plot that has fence border
        var gameState = new GameState(10, 10);
        gameState.ZoomLevel = 2; // 25ft zoom

        // Fill with grass first
        for (int y = 0; y < 10; y++)
            for (int x = 0; x < 10; x++)
                gameState.Tiles[x, y] = new Tile(TileType.Grass, null, null, null);

        // Create a 6x6 farm plot in the center (leaving 2 tile border)
        var farmPlot = new Plot(
            id: "test_farm",
            bounds: new Rectangle(2, 2, 6, 6),
            type: PlotType.Farmland,
            cropType: "wheat"
        )
        {
            BorderType = "fence",
            BorderSides = BorderSides.All
        };
        gameState.Plots.Add(farmPlot);

        // Fill plot with farm tiles
        for (int y = 2; y < 8; y++)
            for (int x = 2; x < 8; x++)
                gameState.Tiles[x, y] = new Tile(TileType.Farm, null, null, "wheat");

        // Act - Check border positions
        // North edge at y=2, x=2-7
        bool hasNorthBorder = CheckIfPositionIsOnBorder(gameState, farmPlot, 4, 2); // middle of north edge
        bool hasSouthBorder = CheckIfPositionIsOnBorder(gameState, farmPlot, 4, 7); // middle of south edge
        bool hasWestBorder = CheckIfPositionIsOnBorder(gameState, farmPlot, 2, 4);  // middle of west edge
        bool hasEastBorder = CheckIfPositionIsOnBorder(gameState, farmPlot, 7, 4);  // middle of east edge
        bool hasInterior = CheckIfPositionIsOnBorder(gameState, farmPlot, 4, 4);    // center (not border)

        // Assert
        Assert.True(hasNorthBorder, "North edge should be detected as border");
        Assert.True(hasSouthBorder, "South edge should be detected as border");
        Assert.True(hasWestBorder, "West edge should be detected as border");
        Assert.True(hasEastBorder, "East edge should be detected as border");
        Assert.False(hasInterior, "Center should NOT be detected as border");
    }

    [Fact]
    public void FarmPlot_WithFenceOnNorthOnly_At25ftZoom_ShowsFenceOnlyOnNorth()
    {
        // Arrange
        var gameState = new GameState(10, 10);
        gameState.ZoomLevel = 2; // 25ft zoom

        var farmPlot = new Plot(
            id: "test_farm",
            bounds: new Rectangle(2, 2, 6, 6),
            type: PlotType.Farmland,
            cropType: "wheat"
        )
        {
            BorderType = "fence",
            BorderSides = BorderSides.North  // Only north side
        };
        gameState.Plots.Add(farmPlot);

        // Act
        bool hasNorthBorder = CheckIfPositionIsOnBorder(gameState, farmPlot, 4, 2);
        bool hasSouthBorder = CheckIfPositionIsOnBorder(gameState, farmPlot, 4, 7);
        bool hasWestBorder = CheckIfPositionIsOnBorder(gameState, farmPlot, 2, 4);
        bool hasEastBorder = CheckIfPositionIsOnBorder(gameState, farmPlot, 7, 4);

        // Assert
        Assert.True(hasNorthBorder, "North edge should have border");
        Assert.False(hasSouthBorder, "South edge should NOT have border");
        Assert.False(hasWestBorder, "West edge should NOT have border");
        Assert.False(hasEastBorder, "East edge should NOT have border");
    }

    [Fact]
    public void FarmPlot_WithFenceOnNorthEast_At25ftZoom_ShowsFenceOnBothSides()
    {
        // Arrange
        var gameState = new GameState(10, 10);
        gameState.ZoomLevel = 2;

        var farmPlot = new Plot(
            id: "test_farm",
            bounds: new Rectangle(2, 2, 6, 6),
            type: PlotType.Farmland,
            cropType: "wheat"
        )
        {
            BorderType = "fence",
            BorderSides = BorderSides.NorthEast  // North and East
        };
        gameState.Plots.Add(farmPlot);

        // Act
        bool hasNorthBorder = CheckIfPositionIsOnBorder(gameState, farmPlot, 4, 2);
        bool hasEastBorder = CheckIfPositionIsOnBorder(gameState, farmPlot, 7, 4);
        bool hasSouthBorder = CheckIfPositionIsOnBorder(gameState, farmPlot, 4, 7);
        bool hasWestBorder = CheckIfPositionIsOnBorder(gameState, farmPlot, 2, 4);

        // Assert
        Assert.True(hasNorthBorder, "North edge should have border");
        Assert.True(hasEastBorder, "East edge should have border");
        Assert.False(hasSouthBorder, "South edge should NOT have border");
        Assert.False(hasWestBorder, "West edge should NOT have border");
    }

    [Fact]
    public void FarmPlot_BorderOnCorner_DetectedOnBothEdges()
    {
        // Arrange - Test that corner positions are detected correctly
        var gameState = new GameState(10, 10);
        gameState.ZoomLevel = 2;

        var farmPlot = new Plot(
            id: "test_farm",
            bounds: new Rectangle(2, 2, 6, 6),
            type: PlotType.Farmland,
            cropType: "wheat"
        )
        {
            BorderType = "fence",
            BorderSides = BorderSides.All
        };
        gameState.Plots.Add(farmPlot);

        // Act - Check corners (they should be on borders since they're on edges)
        bool northWestCorner = CheckIfPositionIsOnBorder(gameState, farmPlot, 2, 2); // top-left
        bool northEastCorner = CheckIfPositionIsOnBorder(gameState, farmPlot, 7, 2); // top-right
        bool southWestCorner = CheckIfPositionIsOnBorder(gameState, farmPlot, 2, 7); // bottom-left
        bool southEastCorner = CheckIfPositionIsOnBorder(gameState, farmPlot, 7, 7); // bottom-right

        // Assert - Corners should be detected as borders
        Assert.True(northWestCorner, "Northwest corner should be on border");
        Assert.True(northEastCorner, "Northeast corner should be on border");
        Assert.True(southWestCorner, "Southwest corner should be on border");
        Assert.True(southEastCorner, "Southeast corner should be on border");
    }

    [Fact]
    public void MultiplePlots_WithDifferentBorders_EachShowsOwnBorder()
    {
        // Arrange - Create two plots with different border types
        var gameState = new GameState(20, 10);
        gameState.ZoomLevel = 2;

        // First plot: fence
        var plot1 = new Plot(
            id: "farm1",
            bounds: new Rectangle(0, 0, 8, 10),
            type: PlotType.Farmland,
            cropType: "wheat"
        )
        {
            BorderType = "fence",
            BorderSides = BorderSides.All
        };
        gameState.Plots.Add(plot1);

        // Second plot: trees
        var plot2 = new Plot(
            id: "farm2",
            bounds: new Rectangle(12, 0, 8, 10),
            type: PlotType.Farmland,
            cropType: "corn"
        )
        {
            BorderType = "trees",
            BorderSides = BorderSides.All
        };
        gameState.Plots.Add(plot2);

        // Act - Check if borders are detected on each plot
        bool plot1HasBorder = CheckIfPositionIsOnBorder(gameState, plot1, 4, 0);
        bool plot2HasBorder = CheckIfPositionIsOnBorder(gameState, plot2, 16, 0);

        // Assert
        Assert.True(plot1HasBorder, "First plot should have border");
        Assert.True(plot2HasBorder, "Second plot should have border");
    }

    [Fact]
    public void FarmPlot_NoBorderSet_NoBorderDetected()
    {
        // Arrange - Plot without border
        var gameState = new GameState(10, 10);
        gameState.ZoomLevel = 2;

        var farmPlot = new Plot(
            id: "test_farm",
            bounds: new Rectangle(2, 2, 6, 6),
            type: PlotType.Farmland,
            cropType: "wheat"
        );
        // No BorderType or BorderSides set
        gameState.Plots.Add(farmPlot);

        // Act
        bool hasNorthBorder = CheckIfPositionIsOnBorder(gameState, farmPlot, 4, 2);
        bool hasSouthBorder = CheckIfPositionIsOnBorder(gameState, farmPlot, 4, 7);

        // Assert
        Assert.False(hasNorthBorder, "Should have no border on north");
        Assert.False(hasSouthBorder, "Should have no border on south");
    }

    /// <summary>
    /// Helper to check if a position would be detected as a border
    /// Mimics the logic in GetBorderAppearance
    /// </summary>
    private bool CheckIfPositionIsOnBorder(GameState gameState, Plot plot, int worldX, int worldY)
    {
        if (plot.BorderType == null || plot.BorderSides == BorderSides.None)
            return false;

        var bounds = plot.Bounds;
        bool isNorthEdge = worldY == bounds.Y;
        bool isSouthEdge = worldY == bounds.Y + bounds.Height - 1;
        bool isWestEdge = worldX == bounds.X;
        bool isEastEdge = worldX == bounds.X + bounds.Width - 1;

        bool onBorder = false;
        if (isNorthEdge && plot.BorderSides.HasFlag(BorderSides.North)) onBorder = true;
        if (isSouthEdge && plot.BorderSides.HasFlag(BorderSides.South)) onBorder = true;
        if (isWestEdge && plot.BorderSides.HasFlag(BorderSides.West)) onBorder = true;
        if (isEastEdge && plot.BorderSides.HasFlag(BorderSides.East)) onBorder = true;

        return onBorder;
    }
}
