using SadConsole;
using SadConsole.Configuration;
using SadConsole.Input;
using SadRogue.Primitives;
using TerminalCity.Domain;
using TerminalCity.Generation;
using TerminalCity.Parsers;
using TerminalCity.Rendering;
using TerminalCity.UI;

Settings.WindowTitle = "TerminalCity - ASCII City Builder";

// Game state
GameState? gameState = null;
ScreenSurface? mainConsole = null;
Scenario? currentScenario = null;
string? statusMessage = null;
DateTime? statusMessageTime = null;
bool fontTestRandomMode = true; // Toggle between random and organized display
TimeSpan timeAccumulator = TimeSpan.Zero; // Accumulator for throttling updates
TimeSpan updateInterval = TimeSpan.FromSeconds(1.0); // Update once per second

// Configure and start SadConsole
Builder
    .GetBuilder()
    .SetWindowSizeInCells(120, 40)
    .ConfigureFonts(true)
    .UseDefaultConsole()
    .OnStart(Startup)
    .Run();

void Startup(object? sender, GameHost host)
{
    // Initialize game state in title screen mode
    gameState = new GameState();

    // Load scenario
    var scenarioPath = Path.Combine("definitions", "scenarios", "bedroom_community.txt");
    currentScenario = ScenarioParser.LoadFromFile(scenarioPath);

    // Create main console
    mainConsole = new ScreenSurface(120, 40);
    mainConsole.UseKeyboard = true;
    mainConsole.UseMouse = false;
    mainConsole.IsFocused = true;

    // Set up input handler
    mainConsole.SadComponents.Add(new InputHandler(OnKeyPressed));

    // Set up update handler for game loop
    mainConsole.SadComponents.Add(new UpdateComponent(OnUpdate));

    // Set as active screen
    Game.Instance.Screen = mainConsole;

    // Render initial screen
    RenderTitleScreen();
}

void OnUpdate(IScreenObject console, TimeSpan delta)
{
    if (gameState == null) return;

    // Only update when playing
    if (gameState.CurrentMode == GameMode.Playing)
    {
        // Accumulate time and only update at fixed intervals
        timeAccumulator += delta;

        if (timeAccumulator >= updateInterval)
        {
            timeAccumulator -= updateInterval;
            gameState.AdvanceTime();
            Render();
        }
    }
}

void OnKeyPressed(IScreenObject console, Keyboard keyboard)
{
    if (gameState == null) return;

    // Handle dialog input first (dialogs are modal and block other input)
    if (gameState.CurrentDialog != null)
    {
        foreach (var option in gameState.CurrentDialog.Options)
        {
            // Check if the key for this option was pressed
            var key = Enum.TryParse<Keys>(option.Key, true, out var parsedKey) ? parsedKey : Keys.None;
            if (key != Keys.None && keyboard.IsKeyPressed(key))
            {
                // Handle the dialog response
                HandleDialogResponse(option.Key);
                return;
            }
        }
        return; // Block all other input while dialog is open
    }

    // Font test mode
    if (gameState.CurrentMode == GameMode.FontTest)
    {
        if (keyboard.IsKeyPressed(Keys.R))
        {
            // Regenerate random tiles
            RenderFontTest();
        }
        else if (keyboard.IsKeyPressed(Keys.M))
        {
            // Toggle between random and organized mode
            fontTestRandomMode = !fontTestRandomMode;
            RenderFontTest();
        }
        else if (keyboard.IsKeyPressed(Keys.T))
        {
            // Go to title screen
            gameState.CurrentMode = GameMode.TitleScreen;
            RenderTitleScreen();
        }
        else if (keyboard.IsKeyPressed(Keys.Escape))
        {
            Game.Instance.MonoGameInstance.Exit();
        }
        return;
    }

    // Title screen
    if (gameState.CurrentMode == GameMode.TitleScreen)
    {
        if (keyboard.IsKeyPressed(Keys.Enter) || keyboard.IsKeyPressed(Keys.Space))
        {
            // Show scenario dialog instead of going directly to game
            if (currentScenario != null)
            {
                ShowScenarioDialog();
            }
            else
            {
                // Fallback if scenario not loaded
                gameState.CurrentMode = GameMode.Playing;
                Render();
            }
        }
        else if (keyboard.IsKeyPressed(Keys.F))
        {
            // Enter font test mode
            gameState.CurrentMode = GameMode.FontTest;
            RenderFontTest();
        }
        else if (keyboard.IsKeyPressed(Keys.Escape))
        {
            Game.Instance.MonoGameInstance.Exit();
        }
        return;
    }

    // Playing mode
    if (gameState.CurrentMode == GameMode.Playing)
    {
        // Camera movement
        if (keyboard.IsKeyPressed(Keys.Up) || keyboard.IsKeyPressed(Keys.W))
        {
            gameState.MoveCamera(new Point(0, -1));
        }
        else if (keyboard.IsKeyPressed(Keys.Down) || keyboard.IsKeyPressed(Keys.S))
        {
            gameState.MoveCamera(new Point(0, 1));
        }
        else if (keyboard.IsKeyPressed(Keys.Left) || keyboard.IsKeyPressed(Keys.A))
        {
            gameState.MoveCamera(new Point(-1, 0));
        }
        else if (keyboard.IsKeyPressed(Keys.Right) || keyboard.IsKeyPressed(Keys.D))
        {
            gameState.MoveCamera(new Point(1, 0));
        }
        // Zoom controls
        else if (keyboard.IsKeyPressed(Keys.OemOpenBrackets)) // [
        {
            if (gameState.ZoomLevel > -2)
            {
                gameState.ZoomLevel--;
                statusMessage = $"Zoom: {gameState.GetZoomLevelName()} (1 tile = {gameState.GetTileScale()}ft)";
                statusMessageTime = DateTime.Now;
            }
        }
        else if (keyboard.IsKeyPressed(Keys.OemCloseBrackets)) // ]
        {
            if (gameState.ZoomLevel < 2)
            {
                gameState.ZoomLevel++;
                statusMessage = $"Zoom: {gameState.GetZoomLevelName()} (1 tile = {gameState.GetTileScale()}ft)";
                statusMessageTime = DateTime.Now;
            }
        }
        // Speed controls
        else if (keyboard.IsKeyPressed(Keys.OemPlus) || keyboard.IsKeyPressed(Keys.Add)) // + key
        {
            if (gameState.GameSpeed < 4)
            {
                gameState.GameSpeed++;
                statusMessage = gameState.GameSpeed == 0 ? "PAUSED" : $"Speed: {gameState.GameSpeed}";
                statusMessageTime = DateTime.Now;
            }
        }
        else if (keyboard.IsKeyPressed(Keys.OemMinus) || keyboard.IsKeyPressed(Keys.Subtract)) // - key
        {
            if (gameState.GameSpeed > 0)
            {
                gameState.GameSpeed--;
                statusMessage = gameState.GameSpeed == 0 ? "PAUSED" : $"Speed: {gameState.GameSpeed}";
                statusMessageTime = DateTime.Now;
            }
        }
        // Escape to show exit confirmation
        else if (keyboard.IsKeyPressed(Keys.Escape))
        {
            gameState.CurrentDialog = Dialog.CreateYesNo(
                "Return to Title Screen?",
                "Press Y to confirm, N to cancel"
            );
            Render(); // Re-render with dialog overlay
            return;
        }

        Render();
    }
}

void ShowScenarioDialog()
{
    if (gameState == null || currentScenario == null) return;

    var dialog = new Dialog(currentScenario.Name);

    // Add description lines (word wrap for readability)
    var description = currentScenario.GetFormattedDescription();
    var words = description.Split(' ');
    var currentLine = "";

    foreach (var word in words)
    {
        if ((currentLine + " " + word).Length > 60)
        {
            dialog.AddLine(currentLine.Trim());
            currentLine = word;
        }
        else
        {
            currentLine += (currentLine.Length > 0 ? " " : "") + word;
        }
    }
    if (currentLine.Length > 0)
        dialog.AddLine(currentLine.Trim());

    dialog.AddLine(""); // Empty line

    // Add key info
    dialog.AddLine($"Year: {currentScenario.StartYear}");
    dialog.AddLine($"Starting Budget: ${currentScenario.StartMoney:N0}");

    if (currentScenario.ParentCityName != null)
    {
        dialog.AddLine($"Near: {currentScenario.ParentCityName} ({currentScenario.ParentCityDistance})");
    }

    dialog.AddLine(""); // Empty line
    dialog.AddLine("Ready to begin?");
    dialog.AddLine(""); // Empty line

    // Add options
    dialog.AddOption("Enter", "Start Game", Color.Green);
    dialog.AddOption("Escape", "Back to Title", Color.Gray);

    // Set width for better display
    dialog.Width = 70;

    gameState.CurrentDialog = dialog;
    RenderTitleScreen(); // Re-render title screen with dialog overlay
}

void HandleDialogResponse(string optionKey)
{
    if (gameState == null || gameState.CurrentDialog == null) return;

    // Handle scenario dialog
    if (currentScenario != null && gameState.CurrentDialog.Title == currentScenario.Name)
    {
        if (optionKey.ToUpper() == "ENTER")
        {
            // Start game with scenario conditions
            gameState.CurrentDialog = null;
            gameState.Money = currentScenario.StartMoney;
            gameState.Population = currentScenario.StartPopulation;
            gameState.CurrentDate = new DateTime(currentScenario.StartYear, 1, 1);

            // Generate initial map from scenario
            MapGenerator.GenerateFromScenario(gameState, currentScenario);

            // Set camera position based on scenario
            if (currentScenario.CameraStartPosition == "center")
            {
                gameState.CameraPosition = new Point(gameState.MapWidth / 2, gameState.MapHeight / 2);
            }
            else if (currentScenario.CameraStartPosition.Contains(','))
            {
                var coords = currentScenario.CameraStartPosition.Split(',');
                if (coords.Length == 2 &&
                    int.TryParse(coords[0].Trim(), out var x) &&
                    int.TryParse(coords[1].Trim(), out var y))
                {
                    gameState.CameraPosition = new Point(x, y);
                }
            }

            gameState.CurrentMode = GameMode.Playing;
            Render();
        }
        else if (optionKey.ToUpper() == "ESCAPE")
        {
            // Back to title screen
            gameState.CurrentDialog = null;
            RenderTitleScreen();
        }
        return;
    }

    // Handle exit confirmation dialog
    if (gameState.CurrentDialog.Title == "Return to Title Screen?")
    {
        if (optionKey.ToUpper() == "Y")
        {
            gameState.CurrentDialog = null;
            gameState.CurrentMode = GameMode.TitleScreen;
            RenderTitleScreen();
        }
        else if (optionKey.ToUpper() == "N")
        {
            gameState.CurrentDialog = null;
            Render();
        }
    }
}

void RenderTitleScreen()
{
    if (mainConsole == null) return;

    mainConsole.Clear();

    // ASCII art title
    int startY = 5;
    var title = new[]
    {
        "████████╗███████╗██████╗ ███╗   ███╗██╗███╗   ██╗ █████╗ ██╗",
        "╚══██╔══╝██╔════╝██╔══██╗████╗ ████║██║████╗  ██║██╔══██╗██║",
        "   ██║   █████╗  ██████╔╝██╔████╔██║██║██╔██╗ ██║███████║██║",
        "   ██║   ██╔══╝  ██╔══██╗██║╚██╔╝██║██║██║╚██╗██║██╔══██║██║",
        "   ██║   ███████╗██║  ██║██║ ╚═╝ ██║██║██║ ╚████║██║  ██║███████╗",
        "   ╚═╝   ╚══════╝╚═╝  ╚═╝╚═╝     ╚═╝╚═╝╚═╝  ╚═══╝╚═╝  ╚═╝╚══════╝",
        "",
        "                      ██████╗██╗████████╗██╗   ██╗",
        "                     ██╔════╝██║╚══██╔══╝╚██╗ ██╔╝",
        "                     ██║     ██║   ██║    ╚████╔╝",
        "                     ██║     ██║   ██║     ╚██╔╝",
        "                     ╚██████╗██║   ██║      ██║",
        "                      ╚═════╝╚═╝   ╚═╝      ╚═╝"
    };

    var titleColor = Color.Cyan;
    for (int i = 0; i < title.Length; i++)
    {
        int x = (mainConsole.Width - title[i].Length) / 2;
        mainConsole.Print(x, startY + i, title[i], titleColor);
    }

    // Subtitle
    mainConsole.Print(mainConsole.Width / 2 - 15, startY + title.Length + 2, "A Terminal-Based City Builder", Color.Yellow);

    // Instructions
    var instructions = new[]
    {
        "",
        "Press ENTER or SPACE to start",
        "Press F for Font Test",
        "Press ESC to quit",
        "",
        "",
        "Controls:",
        "  Arrow Keys or WASD - Move camera",
        "  [ ] - Zoom in/out",
        "  ESC - Return to title screen",
        "",
        "Coming soon:",
        "  - Place buildings and roads",
        "  - Zone residential, commercial, industrial areas",
        "  - Manage city budget and population",
        "  - Watch your city grow!"
    };

    int instructY = startY + title.Length + 4;
    for (int i = 0; i < instructions.Length; i++)
    {
        int x = (mainConsole.Width - instructions[i].Length) / 2;
        var color = i < 3 ? Color.White : Color.Gray;
        mainConsole.Print(x, instructY + i, instructions[i], color);
    }

    // Render modal dialog overlay if one is active
    if (gameState?.CurrentDialog != null)
    {
        gameState.CurrentDialog.Render(mainConsole);
    }
}

void RenderFontTest()
{
    if (mainConsole == null) return;

    mainConsole.Clear();

    var random = new Random();

    if (fontTestRandomMode)
    {
        // RANDOM MODE: Fill the entire screen with random characters and colors
        for (int y = 0; y < mainConsole.Height - 5; y++)
        {
            for (int x = 0; x < mainConsole.Width; x++)
            {
                // Random character from extended ASCII range
                int charCode = random.Next(33, 256);
                char randomChar = (char)charCode;

                // Random colors
                var foreground = new Color(random.Next(256), random.Next(256), random.Next(256));
                var background = new Color(random.Next(100), random.Next(100), random.Next(100)); // Darker backgrounds

                mainConsole.Print(x, y, randomChar.ToString(), foreground, background);
            }
        }
    }
    else
    {
        // ORGANIZED MODE: Show character ranges in a grid with labels
        int y = 0;
        int charsPerRow = 32;

        // Show ASCII ranges in organized rows
        var ranges = new[]
        {
            (32, 63, "ASCII 32-63: Symbols & Numbers"),
            (64, 95, "ASCII 64-95: Uppercase & Symbols"),
            (96, 127, "ASCII 96-127: Lowercase & Symbols"),
            (128, 159, "Extended ASCII 128-159"),
            (160, 191, "Extended ASCII 160-191"),
            (192, 223, "Extended ASCII 192-223"),
            (224, 255, "Extended ASCII 224-255"),
            (0x2500, 0x251F, "Box Drawing 2500-251F"),
            (0x2520, 0x253F, "Box Drawing 2520-253F"),
            (0x2540, 0x255F, "Box Drawing 2540-255F"),
            (0x2560, 0x257F, "Box Drawing 2560-257F")
        };

        foreach (var (start, end, label) in ranges)
        {
            if (y >= mainConsole.Height - 6) break;

            // Print label
            mainConsole.Print(0, y, label, Color.Yellow);
            y++;

            // Print characters in this range
            int x = 0;
            for (int charCode = start; charCode <= end && charCode < start + charsPerRow; charCode++)
            {
                if (x >= mainConsole.Width) break;

                char ch = (char)charCode;
                var foreground = Color.White;
                var background = Color.Black;

                // Show character code below
                if (y + 1 < mainConsole.Height - 6)
                {
                    mainConsole.Print(x * 3, y + 1, charCode.ToString("X2"), Color.Gray, Color.Black);
                }

                mainConsole.Print(x * 3, y, ch.ToString(), foreground, background);
                x++;
            }
            y += 3; // Space for character code + gap
        }
    }

    // Instructions at bottom
    int instructY = mainConsole.Height - 4;
    mainConsole.DrawLine(new Point(0, instructY - 1), new Point(mainConsole.Width - 1, instructY - 1), '─', Color.Gray);

    var modeText = fontTestRandomMode ? "RANDOM" : "ORGANIZED";
    mainConsole.Print(2, instructY, $"FONT TEST MODE - {modeText} - Extended ASCII Character Display", Color.Yellow);
    mainConsole.Print(2, instructY + 1, "R: Regenerate  |  M: Toggle Mode  |  T: Title Screen  |  ESC: Quit", Color.White);
    mainConsole.Print(2, instructY + 2, fontTestRandomMode
        ? "Displaying random chars 33-255 with random colors"
        : "Displaying organized character ranges with hex codes", Color.Gray);
}

void Render()
{
    if (gameState == null || mainConsole == null) return;

    mainConsole.Clear();

    // Calculate viewport
    int viewportWidth = mainConsole.Width;
    int viewportHeight = mainConsole.Height - 3; // Reserve 3 rows for UI

    double renderScale = gameState.GetRenderScale();

    if (renderScale >= 1.0)
    {
        // Zoomed in: Each data tile takes up multiple screen tiles
        RenderZoomedIn(viewportWidth, viewportHeight, (int)renderScale);
    }
    else
    {
        // Zoomed out: Skip tiles, show less detail
        RenderZoomedOut(viewportWidth, viewportHeight, renderScale);
    }

    // Render camera position indicator
    int centerX = viewportWidth / 2;
    int centerY = viewportHeight / 2;
    mainConsole.SetGlyph(centerX, centerY, '+', Color.Yellow);

    // Render UI at bottom
    RenderUI(mainConsole, viewportHeight);

    // Render modal dialog overlay if one is active
    if (gameState.CurrentDialog != null)
    {
        gameState.CurrentDialog.Render(mainConsole);
    }
}

void RenderZoomedIn(int viewportWidth, int viewportHeight, int scale)
{
    if (gameState == null || mainConsole == null) return;

    // Calculate how many data tiles we can see
    int dataTilesWide = viewportWidth / scale;
    int dataTilesHigh = viewportHeight / scale;

    int startX = gameState.CameraPosition.X - dataTilesWide / 2;
    int startY = gameState.CameraPosition.Y - dataTilesHigh / 2;

    for (int dataY = 0; dataY < dataTilesHigh; dataY++)
    {
        for (int dataX = 0; dataX < dataTilesWide; dataX++)
        {
            int worldX = startX + dataX;
            int worldY = startY + dataY;

            // Check bounds
            if (worldX < 0 || worldX >= gameState.MapWidth || worldY < 0 || worldY >= gameState.MapHeight)
            {
                // Fill this data tile's screen space with black
                for (int sy = 0; sy < scale; sy++)
                {
                    for (int sx = 0; sx < scale; sx++)
                    {
                        int screenX = dataX * scale + sx;
                        int screenY = dataY * scale + sy;
                        if (screenX < viewportWidth && screenY < viewportHeight)
                            mainConsole.Print(screenX, screenY, " ", Color.Black, Color.Black);
                    }
                }
                continue;
            }

            var tile = gameState.Tiles[worldX, worldY];

            // Roads need smart rendering based on neighbors when scale > 1
            // At scale == 1, just render as single glyph
            if ((tile.Type == TileType.DirtRoad || tile.Type == TileType.PavedRoad) && scale > 1)
            {
                RenderRoadZoomedIn(dataX, dataY, scale, worldX, worldY, viewportWidth, viewportHeight, tile.Type);
            }
            // Field boundaries need edge rendering at scale > 1
            else if (tile.Type == TileType.Trees && tile.CropType != null &&
                     (tile.CropType.EndsWith("_north") || tile.CropType.EndsWith("_east")) &&
                     scale > 1)
            {
                RenderBoundaryZoomedIn(dataX, dataY, scale, worldX, worldY, viewportWidth, viewportHeight, tile.CropType);
            }
            else
            {
                // All tiles (including roads at scale=1) fill the scaled area with their glyph
                var (glyph, foreground, background) = GetTileAppearance(tile);

                // For roads at scale=1, detect direction and use appropriate glyph
                if (tile.Type == TileType.DirtRoad || tile.Type == TileType.PavedRoad)
                {
                    bool hasNorth = worldY > 0 && IsRoadTile(gameState.Tiles[worldX, worldY - 1].Type);
                    bool hasSouth = worldY < gameState.MapHeight - 1 && IsRoadTile(gameState.Tiles[worldX, worldY + 1].Type);
                    bool hasWest = worldX > 0 && IsRoadTile(gameState.Tiles[worldX - 1, worldY].Type);
                    bool hasEast = worldX < gameState.MapWidth - 1 && IsRoadTile(gameState.Tiles[worldX + 1, worldY].Type);

                    bool isIntersection = (hasNorth || hasSouth) && (hasWest || hasEast);

                    if (isIntersection)
                    {
                        glyph = RoadRenderer.GetIntersectionChar(gameState.ZoomLevel);
                    }
                    else
                    {
                        bool isVertical = (hasNorth || hasSouth) && !(hasWest || hasEast);
                        (glyph, foreground, background) = RoadRenderer.GetRoadAppearance(tile.Type, gameState.ZoomLevel, isVertical);
                    }
                }

                for (int sy = 0; sy < scale; sy++)
                {
                    for (int sx = 0; sx < scale; sx++)
                    {
                        int screenX = dataX * scale + sx;
                        int screenY = dataY * scale + sy;
                        if (screenX < viewportWidth && screenY < viewportHeight)
                            mainConsole.Print(screenX, screenY, glyph.ToString(), foreground, background);
                    }
                }
            }
        }
    }
}

void RenderBoundaryZoomedIn(int dataX, int dataY, int scale, int worldX, int worldY, int viewportWidth, int viewportHeight, string boundaryType)
{
    if (gameState == null || mainConsole == null) return;

    // Determine if this is north or east boundary
    bool isNorth = boundaryType.EndsWith("_north");
    bool isEast = boundaryType.EndsWith("_east");

    // Get the boundary appearance
    var (boundaryGlyph, boundaryFg, boundaryBg) = GetBoundaryAppearance(boundaryType);

    // Find the plot this tile belongs to and get its crop type
    var plot = gameState.Plots.FirstOrDefault(p => p.Contains(worldX, worldY));
    var cropType = plot?.CropType ?? "fallow_plowed";

    // Get the crop appearance (what should fill the non-boundary area)
    var (cropGlyph, cropFg, cropBg) = GetFarmAppearance(cropType, gameState.GetCurrentSeason());

    // Render the scaled block
    for (int sy = 0; sy < scale; sy++)
    {
        for (int sx = 0; sx < scale; sx++)
        {
            int screenX = dataX * scale + sx;
            int screenY = dataY * scale + sy;

            if (screenX < viewportWidth && screenY < viewportHeight)
            {
                // Determine if this screen position should show boundary or crop
                bool showBoundary = false;

                if (isNorth && sy == 0)
                {
                    // North boundary: top row only
                    showBoundary = true;
                }
                else if (isEast && sx == scale - 1)
                {
                    // East boundary: rightmost column only
                    showBoundary = true;
                }

                if (showBoundary)
                {
                    mainConsole.Print(screenX, screenY, boundaryGlyph.ToString(), boundaryFg, boundaryBg);
                }
                else
                {
                    mainConsole.Print(screenX, screenY, cropGlyph.ToString(), cropFg, cropBg);
                }
            }
        }
    }
}

void RenderRoadZoomedIn(int dataX, int dataY, int scale, int worldX, int worldY, int viewportWidth, int viewportHeight, TileType roadType)
{
    if (gameState == null || mainConsole == null) return;

    // First, fill the entire scaled block with grass (road verge/shoulder)
    var naturalGround = Color.Green;
    var naturalGroundDark = Color.DarkGreen;
    for (int sy = 0; sy < scale; sy++)
    {
        for (int sx = 0; sx < scale; sx++)
        {
            int screenX = dataX * scale + sx;
            int screenY = dataY * scale + sy;
            if (screenX < viewportWidth && screenY < viewportHeight)
                mainConsole.Print(screenX, screenY, ".", naturalGround, naturalGroundDark);
        }
    }

    // Check neighbors to see which directions the road continues
    bool hasNorth = worldY > 0 && IsRoadTile(gameState.Tiles[worldX, worldY - 1].Type);
    bool hasSouth = worldY < gameState.MapHeight - 1 && IsRoadTile(gameState.Tiles[worldX, worldY + 1].Type);
    bool hasWest = worldX > 0 && IsRoadTile(gameState.Tiles[worldX - 1, worldY].Type);
    bool hasEast = worldX < gameState.MapWidth - 1 && IsRoadTile(gameState.Tiles[worldX + 1, worldY].Type);

    // Get road appearance for this zoom level (horizontal and vertical)
    var (hRoadGlyph, hForeground, hBackground) = RoadRenderer.GetRoadAppearance(roadType, gameState.ZoomLevel, isVertical: false);
    var (vRoadGlyph, vForeground, vBackground) = RoadRenderer.GetRoadAppearance(roadType, gameState.ZoomLevel, isVertical: true);

    // Get matching intersection character for this zoom level
    char intersectionChar = RoadRenderer.GetIntersectionChar(gameState.ZoomLevel);

    // Check if this is an intersection
    bool isIntersection = (hasNorth || hasSouth) && (hasWest || hasEast);

    // Draw road through the middle of the scaled block
    int midX = scale / 2;
    int midY = scale / 2;

    // Draw vertical road segments
    if (hasNorth || hasSouth)
    {
        // Vertical road - draw down the middle column
        for (int sy = 0; sy < scale; sy++)
        {
            int screenX = dataX * scale + midX;
            int screenY = dataY * scale + sy;
            if (screenX < viewportWidth && screenY < viewportHeight)
                mainConsole.Print(screenX, screenY, vRoadGlyph.ToString(), vForeground, vBackground);
        }
    }

    // Draw horizontal road segments
    if (hasWest || hasEast)
    {
        // Horizontal road - draw across the middle row
        for (int sx = 0; sx < scale; sx++)
        {
            int screenX = dataX * scale + sx;
            int screenY = dataY * scale + midY;
            if (screenX < viewportWidth && screenY < viewportHeight)
                mainConsole.Print(screenX, screenY, hRoadGlyph.ToString(), hForeground, hBackground);
        }
    }

    // Draw intersection marker ONLY at the exact center point
    if (isIntersection)
    {
        int screenX = dataX * scale + midX;
        int screenY = dataY * scale + midY;
        if (screenX < viewportWidth && screenY < viewportHeight)
            mainConsole.Print(screenX, screenY, intersectionChar.ToString(), hForeground, hBackground);
    }

    // If no neighbors (isolated road tile), use horizontal road character
    if (!hasNorth && !hasSouth && !hasWest && !hasEast)
    {
        int screenX = dataX * scale + midX;
        int screenY = dataY * scale + midY;
        if (screenX < viewportWidth && screenY < viewportHeight)
            mainConsole.Print(screenX, screenY, hRoadGlyph.ToString(), hForeground, hBackground);
    }
}

bool IsRoadTile(TileType type)
{
    return type == TileType.DirtRoad || type == TileType.PavedRoad;
}

int GetStructurePriority(string? cropType)
{
    // Determines which structures are important to show at far zoom
    // Higher number = more important
    // TODO: In the future, make this definable in the structure definition files
    // (e.g., add a "zoom_priority" or "importance" field to agriculture.txt and structures.txt)
    return cropType switch
    {
        "farmhouse" => 5,  // Most important - always show
        "barn" => 4,       // Important - always show
        "shed" => 3,       // Less important - show if no farmhouse/barn
        "well" => 0,       // Not visible at far zoom
        "silo" => 0,       // Not visible at far zoom (per user requirement)
        "driveway" => 0,   // Not visible at far zoom
        "yard" => 0,       // Not visible at far zoom
        _ => 0
    };
}

void RenderZoomedOut(int viewportWidth, int viewportHeight, double scale)
{
    if (gameState == null || mainConsole == null) return;

    // How many data tiles to skip
    int skipFactor = (int)(1.0 / scale);

    // Calculate how many data tiles we can see
    int dataTilesWide = viewportWidth * skipFactor;
    int dataTilesHigh = viewportHeight * skipFactor;

    int startX = gameState.CameraPosition.X - dataTilesWide / 2;
    int startY = gameState.CameraPosition.Y - dataTilesHigh / 2;

    for (int screenY = 0; screenY < viewportHeight; screenY++)
    {
        for (int screenX = 0; screenX < viewportWidth; screenX++)
        {
            // Sample every Nth tile
            int worldX = startX + (screenX * skipFactor);
            int worldY = startY + (screenY * skipFactor);

            // Out of bounds
            if (worldX < 0 || worldX >= gameState.MapWidth || worldY < 0 || worldY >= gameState.MapHeight)
            {
                mainConsole.Print(screenX, screenY, " ", Color.Black, Color.Black);
                continue;
            }

            var tile = gameState.Tiles[worldX, worldY];
            int actualRoadX = worldX;  // Track actual road position for direction detection
            int actualRoadY = worldY;

            // At 200ft zoom (zoom -1), ensure dirt roads are always visible even if sampling skips them
            if (gameState.ZoomLevel == -1 && tile.Type != TileType.DirtRoad && tile.Type != TileType.PavedRoad)
            {
                // First pass: look for intersections (prioritize these)
                bool foundRoad = false;
                for (int dy = 0; dy < skipFactor && !foundRoad; dy++)
                {
                    for (int dx = 0; dx < skipFactor && !foundRoad; dx++)
                    {
                        int checkX = worldX + dx;
                        int checkY = worldY + dy;

                        if (checkX < gameState.MapWidth && checkY < gameState.MapHeight)
                        {
                            var checkTile = gameState.Tiles[checkX, checkY];
                            if (checkTile.Type == TileType.DirtRoad)
                            {
                                // Check if this road tile is an intersection
                                bool hasNorth = checkY > 0 && IsRoadTile(gameState.Tiles[checkX, checkY - 1].Type);
                                bool hasSouth = checkY < gameState.MapHeight - 1 && IsRoadTile(gameState.Tiles[checkX, checkY + 1].Type);
                                bool hasWest = checkX > 0 && IsRoadTile(gameState.Tiles[checkX - 1, checkY].Type);
                                bool hasEast = checkX < gameState.MapWidth - 1 && IsRoadTile(gameState.Tiles[checkX + 1, checkY].Type);
                                bool isIntersection = (hasNorth || hasSouth) && (hasWest || hasEast);

                                if (isIntersection)
                                {
                                    // Found an intersection - use this!
                                    tile = checkTile;
                                    actualRoadX = checkX;
                                    actualRoadY = checkY;
                                    foundRoad = true;
                                }
                            }
                        }
                    }
                }

                // Second pass: if no intersection found, take any dirt road
                if (!foundRoad)
                {
                    for (int dy = 0; dy < skipFactor && !foundRoad; dy++)
                    {
                        for (int dx = 0; dx < skipFactor && !foundRoad; dx++)
                        {
                            int checkX = worldX + dx;
                            int checkY = worldY + dy;

                            if (checkX < gameState.MapWidth && checkY < gameState.MapHeight)
                            {
                                var checkTile = gameState.Tiles[checkX, checkY];
                                if (checkTile.Type == TileType.DirtRoad)
                                {
                                    tile = checkTile;
                                    actualRoadX = checkX;
                                    actualRoadY = checkY;
                                    foundRoad = true;
                                }
                            }
                        }
                    }
                }
            }

            // At far zoom (200ft and 400ft), ensure important farmstead structures are always visible
            if (gameState.ZoomLevel <= -1)
            {
                // Search the sampling area for important structures
                Tile? importantStructure = null;
                int structurePriority = 0; // Higher = more important

                for (int dy = 0; dy < skipFactor; dy++)
                {
                    for (int dx = 0; dx < skipFactor; dx++)
                    {
                        int checkX = worldX + dx;
                        int checkY = worldY + dy;

                        if (checkX < gameState.MapWidth && checkY < gameState.MapHeight)
                        {
                            var checkTile = gameState.Tiles[checkX, checkY];

                            // Check if this is a farmstead structure (Grass with CropType)
                            if (checkTile.Type == TileType.Grass && checkTile.CropType != null)
                            {
                                int priority = GetStructurePriority(checkTile.CropType);

                                // Keep the most important structure found
                                if (priority > structurePriority)
                                {
                                    importantStructure = checkTile;
                                    structurePriority = priority;
                                }
                            }
                        }
                    }
                }

                // If we found an important structure, render it instead of the sampled tile
                if (importantStructure != null)
                {
                    tile = importantStructure;
                }
            }

            // Hide dirt roads only at 400ft+ zoom (only show important infrastructure at far zoom)
            if (tile.Type == TileType.DirtRoad && gameState.ZoomLevel <= -2)
            {
                // Try to show an adjacent non-road tile instead
                Tile? replacementTile = null;

                // Check adjacent tiles in order: right, down, left, up
                if (worldX + 1 < gameState.MapWidth && gameState.Tiles[worldX + 1, worldY].Type != TileType.DirtRoad)
                    replacementTile = gameState.Tiles[worldX + 1, worldY];
                else if (worldY + 1 < gameState.MapHeight && gameState.Tiles[worldX, worldY + 1].Type != TileType.DirtRoad)
                    replacementTile = gameState.Tiles[worldX, worldY + 1];
                else if (worldX - 1 >= 0 && gameState.Tiles[worldX - 1, worldY].Type != TileType.DirtRoad)
                    replacementTile = gameState.Tiles[worldX - 1, worldY];
                else if (worldY - 1 >= 0 && gameState.Tiles[worldX, worldY - 1].Type != TileType.DirtRoad)
                    replacementTile = gameState.Tiles[worldX, worldY - 1];

                // If still no replacement, search diagonals and further out
                if (replacementTile == null)
                {
                    // Check diagonals
                    if (worldX + 1 < gameState.MapWidth && worldY + 1 < gameState.MapHeight &&
                        gameState.Tiles[worldX + 1, worldY + 1].Type != TileType.DirtRoad)
                        replacementTile = gameState.Tiles[worldX + 1, worldY + 1];
                    else if (worldX + 1 < gameState.MapWidth && worldY - 1 >= 0 &&
                        gameState.Tiles[worldX + 1, worldY - 1].Type != TileType.DirtRoad)
                        replacementTile = gameState.Tiles[worldX + 1, worldY - 1];
                    else if (worldX - 1 >= 0 && worldY + 1 < gameState.MapHeight &&
                        gameState.Tiles[worldX - 1, worldY + 1].Type != TileType.DirtRoad)
                        replacementTile = gameState.Tiles[worldX - 1, worldY + 1];
                    else if (worldX - 1 >= 0 && worldY - 1 >= 0 &&
                        gameState.Tiles[worldX - 1, worldY - 1].Type != TileType.DirtRoad)
                        replacementTile = gameState.Tiles[worldX - 1, worldY - 1];
                    // Check 2 tiles away
                    else if (worldX + 2 < gameState.MapWidth && gameState.Tiles[worldX + 2, worldY].Type != TileType.DirtRoad)
                        replacementTile = gameState.Tiles[worldX + 2, worldY];
                    else if (worldY + 2 < gameState.MapHeight && gameState.Tiles[worldX, worldY + 2].Type != TileType.DirtRoad)
                        replacementTile = gameState.Tiles[worldX, worldY + 2];
                }

                // If we found a non-road tile, use it; otherwise default to farm appearance
                if (replacementTile != null)
                {
                    tile = replacementTile;
                }
                else
                {
                    // Can't find non-road nearby (rare) - render as farm to blend in
                    mainConsole.Print(screenX, screenY, "▒", Color.Yellow, Color.DarkGoldenrod);
                    continue;
                }
            }

            var (glyph, foreground, background) = GetTileAppearance(tile);

            // Check for intersections and direction on roads
            if (tile.Type == TileType.DirtRoad || tile.Type == TileType.PavedRoad)
            {
                // Check neighbors at the actual road position (may differ from sampled position)
                bool hasNorth = actualRoadY > 0 && IsRoadTile(gameState.Tiles[actualRoadX, actualRoadY - 1].Type);
                bool hasSouth = actualRoadY < gameState.MapHeight - 1 && IsRoadTile(gameState.Tiles[actualRoadX, actualRoadY + 1].Type);
                bool hasWest = actualRoadX > 0 && IsRoadTile(gameState.Tiles[actualRoadX - 1, actualRoadY].Type);
                bool hasEast = actualRoadX < gameState.MapWidth - 1 && IsRoadTile(gameState.Tiles[actualRoadX + 1, actualRoadY].Type);

                bool isIntersection = (hasNorth || hasSouth) && (hasWest || hasEast);

                if (isIntersection)
                {
                    glyph = RoadRenderer.GetIntersectionChar(gameState.ZoomLevel);
                }
                else
                {
                    // Determine if this is a vertical or horizontal road
                    bool isVertical = (hasNorth || hasSouth) && !(hasWest || hasEast);
                    (glyph, foreground, background) = RoadRenderer.GetRoadAppearance(tile.Type, gameState.ZoomLevel, isVertical);
                }
            }

            mainConsole.Print(screenX, screenY, glyph.ToString(), foreground, background);
        }
    }
}

(char glyph, Color foreground, Color background) GetTileAppearance(Tile tile)
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
        TileType.DirtRoad => RoadRenderer.GetRoadAppearance(tile.Type, gameState?.ZoomLevel ?? 0),
        TileType.PavedRoad => RoadRenderer.GetRoadAppearance(tile.Type, gameState?.ZoomLevel ?? 0),
        TileType.Farm => ((char)240, Color.SaddleBrown, Color.DarkKhaki), // Fallback if no crop type
        TileType.Trees => ('♠', Color.DarkGreen, Color.Green),
        TileType.Building => tile.Building != null
            ? (tile.Building.DisplayChar, tile.Building.Color, Color.Black)
            : ('B', Color.White, Color.Black),
        TileType.Water => ('~', Color.Blue, Color.DarkBlue),
        TileType.Zone => tile.Zone switch
        {
            Zone.Residential => ('R', Color.LightGreen, Color.Black),
            Zone.Commercial => ('C', Color.LightBlue, Color.Black),
            Zone.Industrial => ('I', Color.Yellow, Color.Black),
            _ => ('.', Color.Green, Color.DarkGreen)
        },
        _ => ('.', Color.White, Color.Black)
    };
}

(char glyph, Color foreground, Color background) GetFarmAppearance(string cropType, Season season)
{
    // Hardcoded crop appearances for now (can load from files later)
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
    else if (cropType == "fallow_unplowed")
    {
        return season switch
        {
            Season.Spring => ('.', Color.YellowGreen, Color.DarkOliveGreen),
            Season.Summer => ('.', Color.GreenYellow, Color.Olive),
            Season.Fall => ('.', Color.DarkKhaki, Color.DarkGoldenrod),
            Season.Winter => ('.', Color.Tan, Color.SaddleBrown),
            _ => ('.', Color.YellowGreen, Color.DarkOliveGreen)
        };
    }

    // Fallback
    return ((char)240, Color.SaddleBrown, Color.DarkKhaki);
}

(char glyph, Color foreground, Color background) GetBoundaryAppearance(string boundaryType)
{
    return boundaryType switch
    {
        // Tall grass (same for both)
        "tall_grass_north" => ('.', Color.YellowGreen, Color.DarkOliveGreen),
        "tall_grass_east" => ('.', Color.YellowGreen, Color.DarkOliveGreen),

        // Tree line (same for both)
        "tree_line_north" => ('♠', Color.DarkGreen, Color.Green),
        "tree_line_east" => ('♠', Color.DarkGreen, Color.Green),

        // Stone wall (horizontal vs vertical appearance)
        "stone_wall_north" => ((char)205, Color.Gray, Color.DarkGray), // ═ horizontal double line
        "stone_wall_east" => ((char)186, Color.Gray, Color.DarkGray),  // ║ vertical double line

        // Fence (different for north vs east)
        "fence_north" => ((char)209, Color.SaddleBrown, Color.Peru),   // ╤ horizontal fence
        "fence_east" => ('|', Color.SaddleBrown, Color.Peru),          // | vertical fence

        // Hedgerow (horizontal vs vertical density)
        "hedgerow_north" => ((char)196, Color.DarkGreen, Color.Olive), // ─ horizontal line
        "hedgerow_east" => ((char)179, Color.DarkGreen, Color.Olive),  // │ vertical line

        _ => ('.', Color.YellowGreen, Color.DarkOliveGreen)
    };
}

(char glyph, Color foreground, Color background) GetFarmsteadStructureAppearance(string structureType)
{
    // TODO: Zoom-level building patterns
    // At 25ft zoom (ZoomLevel = 2), multi-tile buildings should use pattern_25ft from building definitions
    // Need to:
    // 1. Load building definitions (parse agriculture.txt, structures.txt)
    // 2. Track which position within a multi-tile building each tile is
    // 3. Look up the character for that position from the pattern
    // 4. Handle different zoom levels (100ft uses display_char, 25ft uses pattern_25ft)
    //
    // For now, using hardcoded single-character appearances

    return structureType switch
    {
        // Main buildings from agriculture.txt
        "farmhouse" => ((char)127, Color.White, Color.DarkKhaki),      // ⌂ white house
        "barn" => ((char)127, Color.Red, Color.Peru),                  // ⌂ red barn
        "silo" => ((char)186, Color.Silver, Color.SaddleBrown),        // ║ silver silo

        // Outbuildings from structures.txt
        "shed" => ((char)254, Color.Brown, Color.SaddleBrown),         // ■ brown shed
        "well" => ((char)9, Color.Gray, Color.DarkGray),               // ○ gray well
        "chicken_coop" => ((char)254, Color.Red, Color.DarkRed),       // ▪ red coop
        "woodshed" => ((char)178, Color.SaddleBrown, Color.Peru),      // ▓ woodshed
        "outhouse" => ((char)10, Color.Gray, Color.DarkGray),          // ◙ outhouse
        "smokehouse" => ((char)177, Color.DarkGray, Color.Black),      // ▒ smokehouse

        // Yard is just grass
        "yard" => ('.', Color.Green, Color.DarkGreen),

        // Driveway is dirt/gravel
        "driveway" => ((char)176, Color.Tan, Color.SaddleBrown),       // ░ light shade

        _ => ('.', Color.Green, Color.DarkGreen) // Default to grass
    };
}

// Road appearance logic moved to RoadRenderer class for testability

void RenderUI(ScreenSurface console, int uiStartY)
{
    if (gameState == null) return;

    // Date and speed display (top right corner)
    var speedText = gameState.GameSpeed == 0 ? "PAUSED" : $"Speed: {gameState.GameSpeed}";
    var dateText = $"{gameState.GetFormattedDate()} | {speedText}";
    int dateX = console.Width - dateText.Length - 1;
    console.Print(dateX, 0, dateText, gameState.GameSpeed == 0 ? Color.Yellow : Color.LightGreen, Color.Black);

    // Zoom level display (second line top right)
    var zoomText = $"Zoom: {gameState.GetZoomLevelName()} | 1 tile = {gameState.GetTileScale()}ft";
    int zoomX = console.Width - zoomText.Length - 1;
    console.Print(zoomX, 1, zoomText, Color.Cyan, Color.Black);

    // Draw separator
    console.DrawLine(new Point(0, uiStartY), new Point(console.Width - 1, uiStartY), '─', Color.Gray);

    // Status bar
    var statusLine = $" Money: ${gameState.Money:N0}  |  Population: {gameState.Population:N0}  |  Date: {gameState.CurrentDate:MMM dd, yyyy}  |  Pos: ({gameState.CameraPosition.X}, {gameState.CameraPosition.Y})";
    console.Print(0, uiStartY + 1, statusLine, Color.White);

    // Status message
    if (statusMessage != null && statusMessageTime != null)
    {
        if ((DateTime.Now - statusMessageTime.Value).TotalSeconds < 3)
        {
            console.Print(0, uiStartY + 2, statusMessage, Color.Yellow);
        }
        else
        {
            statusMessage = null;
            statusMessageTime = null;
        }
    }
}

// Input handler component
class InputHandler : SadConsole.Components.UpdateComponent
{
    private readonly Action<IScreenObject, Keyboard> _onKeyPressed;

    public InputHandler(Action<IScreenObject, Keyboard> onKeyPressed)
    {
        _onKeyPressed = onKeyPressed;
    }

    public override void Update(IScreenObject console, TimeSpan delta)
    {
        var keyboard = GameHost.Instance.Keyboard;
        if (keyboard.HasKeysPressed)
        {
            _onKeyPressed(console, keyboard);
        }
    }
}

// Update handler component for game loop
class UpdateComponent : SadConsole.Components.UpdateComponent
{
    private readonly Action<IScreenObject, TimeSpan> _onUpdate;

    public UpdateComponent(Action<IScreenObject, TimeSpan> onUpdate)
    {
        _onUpdate = onUpdate;
    }

    public override void Update(IScreenObject console, TimeSpan delta)
    {
        _onUpdate(console, delta);
    }
}
