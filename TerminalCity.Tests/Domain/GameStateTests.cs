using SadRogue.Primitives;
using TerminalCity.Domain;
using Xunit;

namespace TerminalCity.Tests.Domain;

public class GameStateTests
{
    [Fact]
    public void GameState_InitializesWithDefaultValues()
    {
        // Act
        var gameState = new GameState(100, 100);

        // Assert
        Assert.Equal(100, gameState.MapWidth);
        Assert.Equal(100, gameState.MapHeight);
        Assert.Equal(10000, gameState.Money);
        Assert.Equal(0, gameState.Population);
        Assert.Equal(GameMode.TitleScreen, gameState.CurrentMode);
    }

    [Fact]
    public void GameState_InitializesAllTilesAsGrass()
    {
        // Act
        var gameState = new GameState(10, 10);

        // Assert
        for (int x = 0; x < 10; x++)
        {
            for (int y = 0; y < 10; y++)
            {
                Assert.Equal(TileType.Grass, gameState.Tiles[x, y].Type);
            }
        }
    }

    [Fact]
    public void MoveCamera_ClampsToMapBounds()
    {
        // Arrange
        var gameState = new GameState(100, 100);
        gameState.CameraPosition = new Point(5, 5);

        // Act - try to move off the left edge
        gameState.MoveCamera(new Point(-10, 0));

        // Assert - should clamp to 0
        Assert.Equal(0, gameState.CameraPosition.X);
        Assert.Equal(5, gameState.CameraPosition.Y);
    }

    [Fact]
    public void PlaceBuilding_RequiresSufficientFunds()
    {
        // Arrange
        var gameState = new GameState(100, 100);
        gameState.Money = 100;
        var expensiveBuilding = new Building(
            "Skyscraper",
            BuildingType.Commercial,
            5, 5,
            10000, // Cost more than available
            100,
            'S',
            Color.Blue
        );

        // Act
        bool placed = gameState.PlaceBuilding(expensiveBuilding, new Point(10, 10));

        // Assert
        Assert.False(placed);
        Assert.Equal(100, gameState.Money); // Money unchanged
    }

    [Fact]
    public void PlaceBuilding_DeductsCostWhenPlaced()
    {
        // Arrange
        var gameState = new GameState(100, 100);
        gameState.Money = 10000;
        var building = new Building(
            "House",
            BuildingType.Residential,
            2, 2,
            500,
            10,
            'H',
            Color.Green
        );

        // Act
        bool placed = gameState.PlaceBuilding(building, new Point(10, 10));

        // Assert
        Assert.True(placed);
        Assert.Equal(9500, gameState.Money);
        Assert.Single(gameState.Buildings);
    }

    [Fact]
    public void PlaceBuilding_UpdatesTiles()
    {
        // Arrange
        var gameState = new GameState(100, 100);
        var building = new Building(
            "House",
            BuildingType.Residential,
            2, 2,
            500,
            10,
            'H',
            Color.Green
        );

        // Act
        gameState.PlaceBuilding(building, new Point(10, 10));

        // Assert
        for (int x = 10; x < 12; x++)
        {
            for (int y = 10; y < 12; y++)
            {
                Assert.Equal(TileType.Building, gameState.Tiles[x, y].Type);
                Assert.Equal(building, gameState.Tiles[x, y].Building);
            }
        }
    }
}
