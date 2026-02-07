using SadRogue.Primitives;
using TerminalCity.Domain;
using Xunit;

namespace TerminalCity.Tests.Rendering;

public class FarmsteadRenderingTests
{
    [Fact]
    public void YardTile_RendersAsGrass()
    {
        // Arrange: Create a yard tile (Grass type with "yard" crop type)
        var yardTile = new Tile(TileType.Grass, null, null, "yard");
        var gameState = new GameState(10, 10);

        // Act: Get the appearance using the same logic as Program.cs
        var (glyph, foreground, background) = GetTileAppearance(yardTile, gameState);

        // Assert: Yard should render as a period with green colors
        Assert.Equal('.', glyph);
        Assert.Equal(Color.Green, foreground);
        Assert.Equal(Color.DarkGreen, background);
    }

    [Fact]
    public void FarmhouseTile_RendersAsHouse()
    {
        // Arrange: Create a farmhouse tile
        var farmhouseTile = new Tile(TileType.Grass, null, null, "farmhouse");
        var gameState = new GameState(10, 10);

        // Act
        var (glyph, foreground, background) = GetTileAppearance(farmhouseTile, gameState);

        // Assert: Farmhouse should render as ⌂ (char 127) with white on dark khaki
        Assert.Equal((char)127, glyph);
        Assert.Equal(Color.White, foreground);
        Assert.Equal(Color.DarkKhaki, background);
    }

    [Fact]
    public void BarnTile_RendersAsRedBarn()
    {
        // Arrange: Create a barn tile
        var barnTile = new Tile(TileType.Grass, null, null, "barn");
        var gameState = new GameState(10, 10);

        // Act
        var (glyph, foreground, background) = GetTileAppearance(barnTile, gameState);

        // Assert: Barn should render as ⌂ (char 127) with red on peru
        Assert.Equal((char)127, glyph);
        Assert.Equal(Color.Red, foreground);
        Assert.Equal(Color.Peru, background);
    }

    [Fact]
    public void FarmTile_RendersAsFarm_NotGrass()
    {
        // Arrange: Create a farm tile for comparison
        var farmTile = new Tile(TileType.Farm, null, null, "fallow_plowed");
        var gameState = new GameState(10, 10);

        // Act
        var (glyph, foreground, background) = GetTileAppearance(farmTile, gameState);

        // Assert: Farm should render as char 240 (≡), NOT as a period
        Assert.Equal((char)240, glyph);
        Assert.NotEqual('.', glyph); // Should NOT be a period
        Assert.NotEqual(Color.Green, foreground); // Should NOT be green grass
    }

    // Duplicate the rendering logic from Program.cs for testing
    private (char glyph, Color foreground, Color background) GetTileAppearance(Tile tile, GameState gameState)
    {
        if (tile.Type == TileType.Farm && tile.CropType != null && gameState != null)
        {
            return GetFarmAppearance(tile.CropType, gameState.GetCurrentSeason());
        }

        // Handle field boundaries (stored as Trees type with special crop type)
        if (tile.Type == TileType.Trees && tile.CropType != null)
        {
            return GetBoundaryAppearance(tile.CropType);
        }

        // Handle farmstead structures (stored as Grass type with structure name in CropType)
        if (tile.Type == TileType.Grass && tile.CropType != null)
        {
            return GetFarmsteadStructureAppearance(tile.CropType);
        }

        return tile.Type switch
        {
            TileType.Grass => ('.', Color.Green, Color.DarkGreen),
            TileType.Farm => ((char)240, Color.SaddleBrown, Color.DarkKhaki),
            _ => ('.', Color.White, Color.Black)
        };
    }

    private (char glyph, Color foreground, Color background) GetFarmAppearance(string cropType, Season season)
    {
        if (cropType == "fallow_plowed")
        {
            return season switch
            {
                Season.Spring => ((char)240, Color.SaddleBrown, Color.Peru),
                Season.Summer => ((char)240, Color.Brown, Color.Tan),
                Season.Fall => ((char)240, Color.DarkGoldenrod, Color.Peru),
                Season.Winter => ((char)240, Color.DarkSlateGray, Color.SlateGray),
                _ => ((char)240, Color.SaddleBrown, Color.Peru)
            };
        }
        return ((char)240, Color.SaddleBrown, Color.DarkKhaki);
    }

    private (char glyph, Color foreground, Color background) GetBoundaryAppearance(string boundaryType)
    {
        return ('.', Color.YellowGreen, Color.DarkOliveGreen);
    }

    private (char glyph, Color foreground, Color background) GetFarmsteadStructureAppearance(string structureType)
    {
        return structureType switch
        {
            "farmhouse" => ((char)127, Color.White, Color.DarkKhaki),
            "barn" => ((char)127, Color.Red, Color.Peru),
            "silo" => ((char)186, Color.Silver, Color.SaddleBrown),
            "shed" => ((char)254, Color.Brown, Color.SaddleBrown),
            "well" => ((char)9, Color.Gray, Color.DarkGray),
            "yard" => ('.', Color.Green, Color.DarkGreen),
            "driveway" => ((char)176, Color.Tan, Color.SaddleBrown),
            _ => ('.', Color.Green, Color.DarkGreen)
        };
    }

    [Fact]
    public void FarmsteadAtFarZoom_200ft_ShowsFarmhouseNotYard()
    {
        // Arrange: Create a small area with farmstead structures
        // Simulating a 2x2 sampling area at 200ft zoom (skipFactor = 2)
        var gameState = new GameState(10, 10);

        // Place a tiny farmstead: yard at (0,0), farmhouse at (1,0), yard at (0,1), shed at (1,1)
        gameState.Tiles[0, 0] = new Tile(TileType.Grass, null, null, "yard");
        gameState.Tiles[1, 0] = new Tile(TileType.Grass, null, null, "farmhouse");
        gameState.Tiles[0, 1] = new Tile(TileType.Grass, null, null, "yard");
        gameState.Tiles[1, 1] = new Tile(TileType.Grass, null, null, "shed");

        // Act: Simulate sampling at (0,0) with skipFactor=2 (200ft zoom)
        // This should search the 2x2 area and find the most important structure
        var mostImportantTile = FindMostImportantStructure(gameState, 0, 0, 2);

        // Assert: Should find the farmhouse, not the yard or shed
        Assert.NotNull(mostImportantTile);
        Assert.Equal("farmhouse", mostImportantTile.CropType);
    }

    [Fact]
    public void FarmsteadAtFarZoom_400ft_ShowsFarmhouseNotSilo()
    {
        // Arrange: Create a 4x4 area with various structures at 400ft zoom (skipFactor = 4)
        var gameState = new GameState(10, 10);

        // Place structures in a 4x4 grid
        gameState.Tiles[0, 0] = new Tile(TileType.Grass, null, null, "yard");
        gameState.Tiles[1, 0] = new Tile(TileType.Grass, null, null, "yard");
        gameState.Tiles[2, 0] = new Tile(TileType.Grass, null, null, "silo");
        gameState.Tiles[3, 0] = new Tile(TileType.Grass, null, null, "yard");

        gameState.Tiles[0, 1] = new Tile(TileType.Grass, null, null, "driveway");
        gameState.Tiles[1, 1] = new Tile(TileType.Grass, null, null, "farmhouse");
        gameState.Tiles[2, 1] = new Tile(TileType.Grass, null, null, "farmhouse");
        gameState.Tiles[3, 1] = new Tile(TileType.Grass, null, null, "yard");

        gameState.Tiles[0, 2] = new Tile(TileType.Grass, null, null, "yard");
        gameState.Tiles[1, 2] = new Tile(TileType.Grass, null, null, "well");
        gameState.Tiles[2, 2] = new Tile(TileType.Grass, null, null, "shed");
        gameState.Tiles[3, 2] = new Tile(TileType.Grass, null, null, "yard");

        gameState.Tiles[0, 3] = new Tile(TileType.Grass, null, null, "yard");
        gameState.Tiles[1, 3] = new Tile(TileType.Grass, null, null, "yard");
        gameState.Tiles[2, 3] = new Tile(TileType.Grass, null, null, "yard");
        gameState.Tiles[3, 3] = new Tile(TileType.Grass, null, null, "yard");

        // Act: Simulate sampling at (0,0) with skipFactor=4 (400ft zoom)
        var mostImportantTile = FindMostImportantStructure(gameState, 0, 0, 4);

        // Assert: Should find the farmhouse, not silo, well, shed, driveway, or yard
        Assert.NotNull(mostImportantTile);
        Assert.Equal("farmhouse", mostImportantTile.CropType);
    }

    [Fact]
    public void FarmsteadAtFarZoom_BarnVisibleWhenNoFarmhouse()
    {
        // Arrange: Create an area with barn but no farmhouse
        var gameState = new GameState(10, 10);

        gameState.Tiles[0, 0] = new Tile(TileType.Grass, null, null, "yard");
        gameState.Tiles[1, 0] = new Tile(TileType.Grass, null, null, "barn");
        gameState.Tiles[0, 1] = new Tile(TileType.Grass, null, null, "shed");
        gameState.Tiles[1, 1] = new Tile(TileType.Grass, null, null, "silo");

        // Act: Simulate 200ft zoom sampling
        var mostImportantTile = FindMostImportantStructure(gameState, 0, 0, 2);

        // Assert: Should find the barn (second most important after farmhouse)
        Assert.NotNull(mostImportantTile);
        Assert.Equal("barn", mostImportantTile.CropType);
    }

    // Helper method that simulates the important structure search logic
    private Tile? FindMostImportantStructure(GameState gameState, int worldX, int worldY, int skipFactor)
    {
        Tile? mostImportantTile = null;
        int highestPriority = 0;

        for (int dy = 0; dy < skipFactor; dy++)
        {
            for (int dx = 0; dx < skipFactor; dx++)
            {
                int checkX = worldX + dx;
                int checkY = worldY + dy;

                if (checkX < gameState.MapWidth && checkY < gameState.MapHeight)
                {
                    var tile = gameState.Tiles[checkX, checkY];

                    if (tile.Type == TileType.Grass && tile.CropType != null)
                    {
                        int priority = GetStructurePriority(tile.CropType);

                        if (priority > highestPriority)
                        {
                            mostImportantTile = tile;
                            highestPriority = priority;
                        }
                    }
                }
            }
        }

        return mostImportantTile;
    }

    // Duplicate the priority logic from Program.cs
    // TODO: When priorities move to definition files, load them from there instead
    private int GetStructurePriority(string? cropType)
    {
        return cropType switch
        {
            "farmhouse" => 5,
            "barn" => 4,
            "shed" => 3,
            "well" => 0,
            "silo" => 0,
            "driveway" => 0,
            "yard" => 0,
            _ => 0
        };
    }
}
