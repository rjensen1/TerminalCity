using SadRogue.Primitives;
using TerminalCity.Domain;
using TerminalCity.Parsers;

namespace TerminalCity.Generation;

/// <summary>
/// Generates initial map terrain based on scenario parameters
/// </summary>
public static class MapGenerator
{
    /// <summary>
    /// Generate a map from a scenario definition
    /// </summary>
    public static void GenerateFromScenario(GameState gameState, Scenario scenario)
    {
        var random = new Random();

        // Parse map size (e.g., "100x100")
        var sizeParts = scenario.MapSize.Split('x');
        int width = int.Parse(sizeParts[0]);
        int height = int.Parse(sizeParts[1]);

        // Step 1: Fill with base terrain based on scenario
        FillBaseTerrain(gameState, scenario, random);

        // Step 2: Generate road grid if specified
        if (scenario.TerrainFeatures.Contains("existing_road_grid"))
        {
            GenerateRoadGrid(gameState, scenario);
        }

        // Step 2.5: Generate plots (bounded by roads)
        GeneratePlots(gameState, random);

        // Step 3: Add farms based on percentage
        if (scenario.InitialFarmPercent > 0)
        {
            PlaceFarms(gameState, scenario, random);
        }

        // Step 3.5: Add field boundaries between plots
        AddFieldBoundaries(gameState, random);

        // Step 3.6: Add farmsteads (house/barn clusters) to some plots
        AddFarmsteads(gameState, random);

        // Step 4: Add trees/vegetation
        if (scenario.InitialTreesPercent > 0)
        {
            PlaceTrees(gameState, scenario, random);
        }
    }

    private static void FillBaseTerrain(GameState gameState, Scenario scenario, Random random)
    {
        // Fill entire map with base terrain (grass for farmland scenarios)
        for (int x = 0; x < gameState.MapWidth; x++)
        {
            for (int y = 0; y < gameState.MapHeight; y++)
            {
                gameState.Tiles[x, y] = new Tile(TileType.Grass, null);
            }
        }
    }

    private static void GenerateRoadGrid(GameState gameState, Scenario scenario)
    {
        int spacing = scenario.RoadGridSpacing;
        int centerX = gameState.MapWidth / 2;
        int centerY = gameState.MapHeight / 2;

        // Main vertical road through center (if specified)
        if (scenario.HasMainVerticalRoad)
        {
            for (int y = 0; y < gameState.MapHeight; y++)
            {
                gameState.Tiles[centerX, y] = new Tile(TileType.DirtRoad, null);
            }
        }

        // Main horizontal road through center (if specified)
        if (scenario.HasMainHorizontalRoad)
        {
            for (int x = 0; x < gameState.MapWidth; x++)
            {
                gameState.Tiles[x, centerY] = new Tile(TileType.DirtRoad, null);
            }
        }

        // Additional roads spaced out from center (sparse grid pattern)
        if (scenario.RoadGridPattern == "sparse_grid")
        {
            // Vertical roads to the left and right
            for (int x = centerX - spacing; x >= 0; x -= spacing)
            {
                for (int y = 0; y < gameState.MapHeight; y++)
                {
                    gameState.Tiles[x, y] = new Tile(TileType.DirtRoad, null);
                }
            }
            for (int x = centerX + spacing; x < gameState.MapWidth; x += spacing)
            {
                for (int y = 0; y < gameState.MapHeight; y++)
                {
                    gameState.Tiles[x, y] = new Tile(TileType.DirtRoad, null);
                }
            }

            // Horizontal roads above and below
            for (int y = centerY - spacing; y >= 0; y -= spacing)
            {
                for (int x = 0; x < gameState.MapWidth; x++)
                {
                    gameState.Tiles[x, y] = new Tile(TileType.DirtRoad, null);
                }
            }
            for (int y = centerY + spacing; y < gameState.MapHeight; y += spacing)
            {
                for (int x = 0; x < gameState.MapWidth; x++)
                {
                    gameState.Tiles[x, y] = new Tile(TileType.DirtRoad, null);
                }
            }
        }
    }

    private static void GeneratePlots(GameState gameState, Random random)
    {
        // Find rectangular sections bounded by roads
        // We'll scan the map to find road intersections and create plots between them

        List<int> verticalRoads = new();
        List<int> horizontalRoads = new();

        // Find vertical roads (roads that span vertically)
        for (int x = 0; x < gameState.MapWidth; x++)
        {
            bool isVerticalRoad = true;
            for (int y = 0; y < gameState.MapHeight; y++)
            {
                if (gameState.Tiles[x, y].Type != TileType.DirtRoad &&
                    gameState.Tiles[x, y].Type != TileType.PavedRoad)
                {
                    isVerticalRoad = false;
                    break;
                }
            }
            if (isVerticalRoad)
                verticalRoads.Add(x);
        }

        // Find horizontal roads (roads that span horizontally)
        for (int y = 0; y < gameState.MapHeight; y++)
        {
            bool isHorizontalRoad = true;
            for (int x = 0; x < gameState.MapWidth; x++)
            {
                if (gameState.Tiles[x, y].Type != TileType.DirtRoad &&
                    gameState.Tiles[x, y].Type != TileType.PavedRoad)
                {
                    isHorizontalRoad = false;
                    break;
                }
            }
            if (isHorizontalRoad)
                horizontalRoads.Add(y);
        }

        // Add map edges as boundaries
        verticalRoads.Insert(0, -1);
        verticalRoads.Add(gameState.MapWidth);
        horizontalRoads.Insert(0, -1);
        horizontalRoads.Add(gameState.MapHeight);

        // Create plots for each rectangular section between roads
        int plotId = 0;
        for (int i = 0; i < verticalRoads.Count - 1; i++)
        {
            for (int j = 0; j < horizontalRoads.Count - 1; j++)
            {
                int x1 = verticalRoads[i] + 1;
                int y1 = horizontalRoads[j] + 1;
                int x2 = verticalRoads[i + 1];
                int y2 = horizontalRoads[j + 1];

                int width = x2 - x1;
                int height = y2 - y1;

                if (width > 0 && height > 0)
                {
                    // Decide whether to split this section into smaller plots
                    var plots = CreatePlotsForSection(x1, y1, width, height, plotId, random);
                    gameState.Plots.AddRange(plots);
                    plotId += plots.Count;
                }
            }
        }
    }

    private static List<Plot> CreatePlotsForSection(int startX, int startY, int width, int height, int baseId, Random random)
    {
        var plots = new List<Plot>();

        // Determine if we should split this section
        int area = width * height;

        // If section is small (< 100 tiles), keep as one plot
        if (area < 100)
        {
            var cropType = random.NextDouble() < 0.7 ? "fallow_plowed" : "fallow_unplowed";
            plots.Add(new Plot(
                $"plot_{baseId}",
                new Rectangle(startX, startY, width, height),
                PlotType.Farmland,
                cropType
            ));
            return plots;
        }

        // For larger sections, randomly decide to split or not
        bool shouldSplit = random.NextDouble() < 0.6; // 60% chance to split

        if (!shouldSplit)
        {
            // Keep as single large plot
            var cropType = random.NextDouble() < 0.7 ? "fallow_plowed" : "fallow_unplowed";
            plots.Add(new Plot(
                $"plot_{baseId}",
                new Rectangle(startX, startY, width, height),
                PlotType.Farmland,
                cropType
            ));
        }
        else
        {
            // Split into 2-4 smaller plots
            // For simplicity, split vertically or horizontally once or twice
            if (width > height && width > 10)
            {
                // Split vertically
                int split1 = width / 2 + random.Next(-3, 4);
                plots.Add(new Plot(
                    $"plot_{baseId}",
                    new Rectangle(startX, startY, split1, height),
                    PlotType.Farmland,
                    random.NextDouble() < 0.7 ? "fallow_plowed" : "fallow_unplowed"
                ));
                plots.Add(new Plot(
                    $"plot_{baseId + 1}",
                    new Rectangle(startX + split1, startY, width - split1, height),
                    PlotType.Farmland,
                    random.NextDouble() < 0.7 ? "fallow_plowed" : "fallow_unplowed"
                ));
            }
            else if (height > width && height > 10)
            {
                // Split horizontally
                int split1 = height / 2 + random.Next(-3, 4);
                plots.Add(new Plot(
                    $"plot_{baseId}",
                    new Rectangle(startX, startY, width, split1),
                    PlotType.Farmland,
                    random.NextDouble() < 0.7 ? "fallow_plowed" : "fallow_unplowed"
                ));
                plots.Add(new Plot(
                    $"plot_{baseId + 1}",
                    new Rectangle(startX, startY + split1, width, height - split1),
                    PlotType.Farmland,
                    random.NextDouble() < 0.7 ? "fallow_plowed" : "fallow_unplowed"
                ));
            }
            else
            {
                // Keep as single plot if dimensions don't allow good splitting
                var cropType = random.NextDouble() < 0.7 ? "fallow_plowed" : "fallow_unplowed";
                plots.Add(new Plot(
                    $"plot_{baseId}",
                    new Rectangle(startX, startY, width, height),
                    PlotType.Farmland,
                    cropType
                ));
            }
        }

        return plots;
    }

    private static void PlaceFarms(GameState gameState, Scenario scenario, Random random)
    {
        // Fill all available (non-road) tiles with farms based on plot assignments
        for (int x = 0; x < gameState.MapWidth; x++)
        {
            for (int y = 0; y < gameState.MapHeight; y++)
            {
                var tile = gameState.Tiles[x, y];

                // Skip roads - only convert grass to farmland
                if (tile.Type == TileType.Grass)
                {
                    // Find which plot this tile belongs to
                    var plot = gameState.Plots.FirstOrDefault(p => p.Contains(x, y));

                    if (plot != null && plot.Type == PlotType.Farmland)
                    {
                        // Use the plot's crop type
                        gameState.Tiles[x, y] = new Tile(TileType.Farm, null, null, plot.CropType);
                    }
                    else
                    {
                        // Fallback: assign random crop if no plot found
                        string cropType = random.NextDouble() < 0.7 ? "fallow_plowed" : "fallow_unplowed";
                        gameState.Tiles[x, y] = new Tile(TileType.Farm, null, null, cropType);
                    }
                }
            }
        }
    }

    private static void AddFieldBoundaries(GameState gameState, Random random)
    {
        foreach (var plot in gameState.Plots)
        {
            if (plot.Type != PlotType.Farmland) continue;

            // Check for adjacent plots and add boundaries (50% chance for each edge)

            // North boundary (top edge of plot)
            if (HasAdjacentPlot(gameState, plot, 0, -1) && random.NextDouble() > 0.5)
            {
                AddBoundary(gameState, plot, "north", random);
            }

            // East boundary (right edge of plot)
            if (HasAdjacentPlot(gameState, plot, 1, 0) && random.NextDouble() > 0.5)
            {
                AddBoundary(gameState, plot, "east", random);
            }
        }
    }

    private static bool HasAdjacentPlot(GameState gameState, Plot plot, int dx, int dy)
    {
        // Check if there's another plot adjacent in the given direction
        int checkX = plot.Bounds.X + (dx > 0 ? plot.Bounds.Width : dx);
        int checkY = plot.Bounds.Y + (dy > 0 ? plot.Bounds.Height : dy);

        // Out of bounds
        if (checkX < 0 || checkX >= gameState.MapWidth || checkY < 0 || checkY >= gameState.MapHeight)
            return false;

        // Check if it's a road (plots don't extend into roads)
        var tile = gameState.Tiles[checkX, checkY];
        if (tile.Type == TileType.DirtRoad || tile.Type == TileType.PavedRoad)
            return false;

        // Check if there's another plot there
        var adjacentPlot = gameState.Plots.FirstOrDefault(p => p.Contains(checkX, checkY) && p != plot);
        return adjacentPlot != null;
    }

    private static void AddBoundary(GameState gameState, Plot plot, string edge, Random random)
    {
        // Choose a random boundary type and store edge direction
        int typeChoice = random.Next(5);
        string boundaryType = typeChoice switch
        {
            0 => "tall_grass",
            1 => "tree_line",
            2 => "stone_wall",
            3 => "fence",
            _ => "hedgerow"
        };

        // Add edge direction to boundary type (e.g., "fence_north" or "fence_east")
        string boundaryKey = $"{boundaryType}_{edge}";

        if (edge == "north")
        {
            // Replace top row of plot
            int y = plot.Bounds.Y;
            for (int x = plot.Bounds.X; x < plot.Bounds.X + plot.Bounds.Width; x++)
            {
                if (x >= 0 && x < gameState.MapWidth && y >= 0 && y < gameState.MapHeight)
                {
                    gameState.Tiles[x, y] = new Tile(TileType.Trees, null, null, boundaryKey);
                }
            }
        }
        else if (edge == "east")
        {
            // Replace right column of plot
            int x = plot.Bounds.X + plot.Bounds.Width - 1;
            for (int y = plot.Bounds.Y; y < plot.Bounds.Y + plot.Bounds.Height; y++)
            {
                if (x >= 0 && x < gameState.MapWidth && y >= 0 && y < gameState.MapHeight)
                {
                    gameState.Tiles[x, y] = new Tile(TileType.Trees, null, null, boundaryKey);
                }
            }
        }
    }

    private static void AddFarmsteads(GameState gameState, Random random)
    {
        // Load farmstead template (for now, just load the first one)
        var farmsteadPath = Path.Combine("definitions", "plots", "farmsteads.txt");
        var template = FarmsteadParser.LoadFromFile(farmsteadPath);

        if (template == null) return;

        // Find farm plots that have a road on their south edge
        foreach (var plot in gameState.Plots)
        {
            if (plot.Type != PlotType.Farmland) continue;
            if (plot.Bounds.Width < template.Width || plot.Bounds.Height < template.Height) continue;

            // Check if there's a road immediately south of this plot
            int southY = plot.Bounds.Y + plot.Bounds.Height;
            if (southY >= gameState.MapHeight) continue;

            bool hasRoadSouth = false;
            for (int x = plot.Bounds.X; x < plot.Bounds.X + plot.Bounds.Width; x++)
            {
                if (x < gameState.MapWidth)
                {
                    var tile = gameState.Tiles[x, southY];
                    if (tile.Type == TileType.DirtRoad || tile.Type == TileType.PavedRoad)
                    {
                        hasRoadSouth = true;
                        break;
                    }
                }
            }

            if (!hasRoadSouth) continue;

            // Place farmstead at the south edge of the plot (near the road)
            // Position it so the bottom of the farmstead is near the road
            int farmsteadX = plot.Bounds.X + (plot.Bounds.Width - template.Width) / 2; // Center horizontally
            int farmsteadY = plot.Bounds.Y + plot.Bounds.Height - template.Height; // Bottom of plot

            PlaceFarmstead(gameState, template, farmsteadX, farmsteadY);

            // For now, only place one farmstead total
            return;
        }
    }

    private static void PlaceFarmstead(GameState gameState, FarmsteadTemplate template, int startX, int startY)
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
                // For now, simple mapping - can be expanded later
                switch (tileType.ToLower())
                {
                    case "yard":
                        gameState.Tiles[worldX, worldY] = new Tile(TileType.Grass, null);
                        break;
                    case "farmhouse":
                        // For now, just mark as grass - we'll add building rendering later
                        gameState.Tiles[worldX, worldY] = new Tile(TileType.Grass, null, null, "farmhouse");
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

    private static void PlaceTrees(GameState gameState, Scenario scenario, Random random)
    {
        // Calculate how many tiles should have trees
        int totalTiles = gameState.MapWidth * gameState.MapHeight;
        int targetTreeTiles = (int)(totalTiles * scenario.InitialTreesPercent / 100.0);
        int placedTrees = 0;

        // Place trees in small clusters (windbreaks, property lines)
        while (placedTrees < targetTreeTiles)
        {
            int x = random.Next(gameState.MapWidth);
            int y = random.Next(gameState.MapHeight);

            var tile = gameState.Tiles[x, y];

            // Only place trees on grass or farm (not roads)
            if (tile.Type != TileType.Grass && tile.Type != TileType.Farm)
                continue;

            // Place a small cluster of trees (2-5 trees)
            int clusterSize = random.Next(2, 6);
            for (int i = 0; i < clusterSize && placedTrees < targetTreeTiles; i++)
            {
                int offsetX = random.Next(-2, 3);
                int offsetY = random.Next(-2, 3);
                int treeX = x + offsetX;
                int treeY = y + offsetY;

                // Check bounds
                if (treeX < 0 || treeX >= gameState.MapWidth ||
                    treeY < 0 || treeY >= gameState.MapHeight)
                    continue;

                var treeTile = gameState.Tiles[treeX, treeY];

                // Only place on grass or farm
                if (treeTile.Type == TileType.Grass || treeTile.Type == TileType.Farm)
                {
                    gameState.Tiles[treeX, treeY] = new Tile(TileType.Trees, null);
                    placedTrees++;
                }
            }
        }
    }
}
