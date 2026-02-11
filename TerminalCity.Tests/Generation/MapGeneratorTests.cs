using TerminalCity.Domain;
using TerminalCity.Generation;
using TerminalCity.Parsers;
using Xunit;

namespace TerminalCity.Tests.Generation;

public class MapGeneratorTests
{
    [Fact]
    public void PlaceFarmstead_ReplacesFieldTilesWithFarmsteadStructures()
    {
        // Arrange: Create a game state with farm tiles
        var gameState = new GameState(20, 20);

        // Fill area with farm tiles (simulating a field)
        for (int x = 0; x < gameState.MapWidth; x++)
        {
            for (int y = 0; y < gameState.MapHeight; y++)
            {
                gameState.Tiles[x, y] = new Tile(TileType.Farm, null, null, "fallow_plowed");
            }
        }

        // Load the tiny farmhouse template
        var farmsteadPath = Path.Combine("definitions", "plots", "plots_farmsteads.txt");
        var template = FarmsteadParser.LoadFromFile(farmsteadPath);

        Assert.NotNull(template);
        Assert.Equal("Tiny Farmhouse", template.Name);
        Assert.Equal(3, template.Width);
        Assert.Equal(3, template.Height);
        Assert.Equal(3, template.MapRows.Count);

        // Act: Place farmstead at position (5, 5)
        int startX = 5;
        int startY = 5;
        PlaceFarmsteadPublic(gameState, template, startX, startY);

        // Assert: Verify the 3x3 area has the correct tiles
        // Row 0: ... (yard, yard, yard)
        Assert.Equal(TileType.Grass, gameState.Tiles[5, 5].Type);
        Assert.Equal("yard", gameState.Tiles[5, 5].CropType);
        Assert.Equal(TileType.Grass, gameState.Tiles[6, 5].Type);
        Assert.Equal("yard", gameState.Tiles[6, 5].CropType);
        Assert.Equal(TileType.Grass, gameState.Tiles[7, 5].Type);
        Assert.Equal("yard", gameState.Tiles[7, 5].CropType);

        // Row 1: HHS (tiny_farmhouse, tiny_farmhouse, shed)
        Assert.Equal(TileType.Grass, gameState.Tiles[5, 6].Type);
        Assert.Equal("tiny_farmhouse", gameState.Tiles[5, 6].CropType);
        Assert.Equal(TileType.Grass, gameState.Tiles[6, 6].Type);
        Assert.Equal("tiny_farmhouse", gameState.Tiles[6, 6].CropType);
        Assert.Equal(TileType.Grass, gameState.Tiles[7, 6].Type);
        Assert.Equal("shed", gameState.Tiles[7, 6].CropType);

        // Row 2: HH. (tiny_farmhouse, tiny_farmhouse, yard)
        Assert.Equal(TileType.Grass, gameState.Tiles[5, 7].Type);
        Assert.Equal("tiny_farmhouse", gameState.Tiles[5, 7].CropType);
        Assert.Equal(TileType.Grass, gameState.Tiles[6, 7].Type);
        Assert.Equal("tiny_farmhouse", gameState.Tiles[6, 7].CropType);
        Assert.Equal(TileType.Grass, gameState.Tiles[7, 7].Type);
        Assert.Equal("yard", gameState.Tiles[7, 7].CropType);

        // Verify tiles outside the farmstead area are still farm tiles
        Assert.Equal(TileType.Farm, gameState.Tiles[4, 5].Type);
        Assert.Equal("fallow_plowed", gameState.Tiles[4, 5].CropType);
        Assert.Equal(TileType.Farm, gameState.Tiles[8, 5].Type);
        Assert.Equal("fallow_plowed", gameState.Tiles[8, 5].CropType);
        Assert.Equal(TileType.Farm, gameState.Tiles[5, 4].Type);
        Assert.Equal("fallow_plowed", gameState.Tiles[5, 4].CropType);
        Assert.Equal(TileType.Farm, gameState.Tiles[5, 8].Type);
        Assert.Equal("fallow_plowed", gameState.Tiles[5, 8].CropType);
    }

    // Public wrapper for the private PlaceFarmstead method for testing
    private static void PlaceFarmsteadPublic(GameState gameState, FarmsteadTemplate template, int startX, int startY)
    {
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

                // Map legend types to actual tile types
                switch (tileType.ToLower())
                {
                    case "yard":
                        gameState.Tiles[worldX, worldY] = new Tile(TileType.Grass, null, null, "yard");
                        break;
                    case "tiny_farmhouse":
                        gameState.Tiles[worldX, worldY] = new Tile(TileType.Grass, null, null, "tiny_farmhouse");
                        break;
                    case "barn":
                        gameState.Tiles[worldX, worldY] = new Tile(TileType.Grass, null, null, "barn");
                        break;
                    case "shed":
                        gameState.Tiles[worldX, worldY] = new Tile(TileType.Grass, null, null, "shed");
                        break;
                    case "silo":
                        gameState.Tiles[worldX, worldY] = new Tile(TileType.Grass, null, null, "silo");
                        break;
                    case "well":
                        gameState.Tiles[worldX, worldY] = new Tile(TileType.Grass, null, null, "well");
                        break;
                    case "driveway":
                        gameState.Tiles[worldX, worldY] = new Tile(TileType.Grass, null, null, "driveway");
                        break;
                }
            }
        }
    }
}
