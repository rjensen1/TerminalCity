using SadConsole;
using SadConsole.Configuration;
using SadConsole.Input;
using SadRogue.Primitives;
using TerminalCity.Domain;
using TerminalCity.Generation;
using TerminalCity.Observability;
using TerminalCity.Parsers;
using TerminalCity.Rendering;
using TerminalCity.UI;

Settings.WindowTitle = "TerminalCity - ASCII City Builder";

// Game state
GameState? gameState = null;
ScreenSurface? mainConsole = null;
GameObservabilityService? observability = null;
Scenario? currentScenario = null;
List<BuildingDefinition> buildingDefinitions = new();
List<StructureDefinition> structureDefinitions = new();
List<CropDefinition> cropDefinitions = new();
List<BorderDefinition> borderDefinitions = new();
string? statusMessage = null;
DateTime? statusMessageTime = null;
bool fontTestRandomMode = true; // Toggle between random and organized display
TimeSpan timeAccumulator = TimeSpan.Zero; // Accumulator for throttling updates
TimeSpan updateInterval = TimeSpan.FromSeconds(1.0); // Update once per second
bool needsRender = true; // Dirty flag - only render when something changed

// Configure and start SadConsole
Builder
    .GetBuilder()
    .SetWindowSizeInCells(120, 40)
    .ConfigureFonts((fontConfig, gameHost) => {
        // Use IBM Extended font (8x16 with 70+ additional glyphs beyond CP437)
        fontConfig.UseCustomFont("fonts\\IBM_ext.font");

        // Load alternative fonts (can switch via Game.Instance.Fonts if needed)
        fontConfig.AddExtraFonts("fonts\\Cheepicus12.font");
    })
    .UseDefaultConsole()
    .OnStart(Startup)
    .Run();

void Startup(object? sender, GameHost host)
{
    // Initialize game state in title screen mode
    gameState = new GameState();

    // Observability: file dumps + REST API (port 5200)
    observability = new GameObservabilityService();
    observability.StartHttpServer(port: 5200);

    // Load scenario
    var scenarioPath = Path.Combine("definitions", "scenarios", "scenarios_test_tiny.txt");
    currentScenario = ScenarioParser.LoadFromFile(scenarioPath);

    // Load building and structure definitions
    var residentialPath = Path.Combine("definitions", "buildings", "buildings_residential.txt");
    var agriculturePath = Path.Combine("definitions", "buildings", "buildings_agriculture.txt");
    var structuresPath = Path.Combine("definitions", "outbuildings", "outbuildings_structures.txt");

    buildingDefinitions.AddRange(BuildingParser.LoadFromFile(residentialPath));
    buildingDefinitions.AddRange(BuildingParser.LoadFromFile(agriculturePath));
    structureDefinitions.AddRange(StructureParser.LoadFromFile(structuresPath));

    // Load crop definitions
    var cropsPath = Path.Combine("definitions", "crops", "crops.txt");
    cropDefinitions.AddRange(CropParser.LoadFromFile(cropsPath));

    // Load border definitions
    var bordersPath = Path.Combine("definitions", "borders", "border_definitions.txt");
    borderDefinitions.AddRange(BorderParser.LoadFromFile(bordersPath));
    System.Console.WriteLine($"BORDER DEBUG: Loaded {borderDefinitions.Count} border definitions");

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
        // Drain any commands injected via REST API
        foreach (var cmd in observability?.DrainCommands() ?? [])
            ApplyObservabilityCommand(cmd);

        // Accumulate time and only update at fixed intervals
        timeAccumulator += delta;

        if (timeAccumulator >= updateInterval)
        {
            timeAccumulator -= updateInterval;
            gameState.AdvanceTime();
            needsRender = true; // Time changed, need to re-render
        }

        // Only render when something changed (dirty flag pattern)
        if (needsRender)
        {
            Render();
            needsRender = false;
        }
    }
}

void OnKeyPressed(IScreenObject console, Keyboard keyboard)
{
    if (gameState == null) return;

    // Any keyboard input requires a re-render
    needsRender = true;

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
            ApplyGameInput("Up");
        else if (keyboard.IsKeyPressed(Keys.Down) || keyboard.IsKeyPressed(Keys.S))
            ApplyGameInput("Down");
        else if (keyboard.IsKeyPressed(Keys.Left) || keyboard.IsKeyPressed(Keys.A))
            ApplyGameInput("Left");
        else if (keyboard.IsKeyPressed(Keys.Right) || keyboard.IsKeyPressed(Keys.D))
            ApplyGameInput("Right");
        // Zoom controls
        else if (keyboard.IsKeyPressed(Keys.OemOpenBrackets)) // [
        {
            var prev = gameState.ZoomLevel;
            ApplyGameInput("OemOpenBrackets");
            if (gameState.ZoomLevel != prev)
            {
                statusMessage = $"Zoom: {gameState.GetZoomLevelName()} (1 tile = {gameState.GetTileScale()}ft)";
                statusMessageTime = DateTime.Now;
            }
        }
        else if (keyboard.IsKeyPressed(Keys.OemCloseBrackets)) // ]
        {
            var prev = gameState.ZoomLevel;
            ApplyGameInput("OemCloseBrackets");
            if (gameState.ZoomLevel != prev)
            {
                statusMessage = $"Zoom: {gameState.GetZoomLevelName()} (1 tile = {gameState.GetTileScale()}ft)";
                statusMessageTime = DateTime.Now;
            }
        }
        // Speed controls
        else if (keyboard.IsKeyPressed(Keys.OemPlus) || keyboard.IsKeyPressed(Keys.Add)) // + key
        {
            var prev = gameState.GameSpeed;
            ApplyGameInput("OemPlus");
            if (gameState.GameSpeed != prev)
            {
                statusMessage = gameState.GameSpeed == 0 ? "PAUSED" : $"Speed: {gameState.GameSpeed}";
                statusMessageTime = DateTime.Now;
            }
        }
        else if (keyboard.IsKeyPressed(Keys.OemMinus) || keyboard.IsKeyPressed(Keys.Subtract)) // - key
        {
            var prev = gameState.GameSpeed;
            ApplyGameInput("OemMinus");
            if (gameState.GameSpeed != prev)
            {
                statusMessage = gameState.GameSpeed == 0 ? "PAUSED" : $"Speed: {gameState.GameSpeed}";
                statusMessageTime = DateTime.Now;
            }
        }
        // Time of day cycling (T key)
        else if (keyboard.IsKeyPressed(Keys.T))
        {
            ApplyGameInput("T");
            statusMessage = $"Time of Day: {gameState.GetTimeOfDayName()}";
            statusMessageTime = DateTime.Now;
        }
        // Weather controls
        else if (keyboard.IsKeyPressed(Keys.Q))
        {
            gameState.CurrentWeather.CycleWeatherCondition();
            statusMessage = $"Weather: {gameState.CurrentWeather.GetConditionName()}";
            statusMessageTime = DateTime.Now;
        }
        else if (keyboard.IsKeyPressed(Keys.PageUp))
        {
            gameState.CurrentWeather.TemperatureF += 10;
            statusMessage = $"Temperature: {gameState.CurrentWeather.TemperatureF}°F";
            statusMessageTime = DateTime.Now;
        }
        else if (keyboard.IsKeyPressed(Keys.PageDown))
        {
            gameState.CurrentWeather.TemperatureF -= 10;
            statusMessage = $"Temperature: {gameState.CurrentWeather.TemperatureF}°F";
            statusMessageTime = DateTime.Now;
        }
        else if (keyboard.IsKeyPressed(Keys.Home))
        {
            gameState.CurrentWeather.WindSpeedMph += 10;
            gameState.CurrentWeather.WindSpeedMph = Math.Min(gameState.CurrentWeather.WindSpeedMph, 150); // Cap at 150 mph
            statusMessage = $"Wind: {gameState.CurrentWeather.GetWindDescription()}";
            statusMessageTime = DateTime.Now;
        }
        else if (keyboard.IsKeyPressed(Keys.End))
        {
            gameState.CurrentWeather.WindSpeedMph -= 10;
            gameState.CurrentWeather.WindSpeedMph = Math.Max(gameState.CurrentWeather.WindSpeedMph, 0); // Min 0 mph
            statusMessage = $"Wind: {gameState.CurrentWeather.GetWindDescription()}";
            statusMessageTime = DateTime.Now;
        }
        else if (keyboard.IsKeyPressed(Keys.Insert))
        {
            gameState.CurrentWeather.CycleWindDirection();
            statusMessage = $"Wind Direction: {gameState.CurrentWeather.WindDirection}";
            statusMessageTime = DateTime.Now;
        }
        else if (keyboard.IsKeyPressed(Keys.Delete))
        {
            gameState.CurrentWeather.HumidityPercent += 10;
            if (gameState.CurrentWeather.HumidityPercent > 100)
                gameState.CurrentWeather.HumidityPercent = 0;
            statusMessage = $"Humidity: {gameState.CurrentWeather.HumidityPercent}%";
            statusMessageTime = DateTime.Now;
        }
        // Atmospheric controls
        else if (keyboard.IsKeyPressed(Keys.E))
        {
            gameState.CurrentWeather.BarometricPressure += 0.1;
            gameState.CurrentWeather.BarometricPressure = Math.Min(gameState.CurrentWeather.BarometricPressure, 31.5);
            statusMessage = $"Pressure: {gameState.CurrentWeather.BarometricPressure:F2} inHg";
            statusMessageTime = DateTime.Now;
        }
        else if (keyboard.IsKeyPressed(Keys.R))
        {
            gameState.CurrentWeather.BarometricPressure -= 0.1;
            gameState.CurrentWeather.BarometricPressure = Math.Max(gameState.CurrentWeather.BarometricPressure, 28.0);
            statusMessage = $"Pressure: {gameState.CurrentWeather.BarometricPressure:F2} inHg";
            statusMessageTime = DateTime.Now;
        }
        else if (keyboard.IsKeyPressed(Keys.F))
        {
            gameState.CurrentWeather.VisibilityMiles += 1.0;
            gameState.CurrentWeather.VisibilityMiles = Math.Min(gameState.CurrentWeather.VisibilityMiles, 20.0);
            statusMessage = $"Visibility: {gameState.CurrentWeather.VisibilityMiles:F1} mi";
            statusMessageTime = DateTime.Now;
        }
        else if (keyboard.IsKeyPressed(Keys.G))
        {
            gameState.CurrentWeather.VisibilityMiles -= 1.0;
            gameState.CurrentWeather.VisibilityMiles = Math.Max(gameState.CurrentWeather.VisibilityMiles, 0.0);
            statusMessage = $"Visibility: {gameState.CurrentWeather.VisibilityMiles:F1} mi";
            statusMessageTime = DateTime.Now;
        }
        else if (keyboard.IsKeyPressed(Keys.V))
        {
            gameState.CurrentWeather.CycleFireDanger();
            statusMessage = $"Fire Danger: {gameState.CurrentWeather.GetFireDangerName()}";
            statusMessageTime = DateTime.Now;
        }
        // X key to dump screen contents to file
        else if (keyboard.IsKeyPressed(Keys.X))
        {
            DumpScreenToFile();
            statusMessage = "Screen dumped to screen_dump.txt";
            statusMessageTime = DateTime.Now;
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

void DumpScreenToFile()
{
    if (mainConsole == null || gameState == null) return;

    var sb = new System.Text.StringBuilder();
    int viewportHeight = mainConsole.Height - 3; // Same as in Render()

    sb.AppendLine($"=== Screen Dump ===");
    sb.AppendLine($"Zoom Level: {gameState.ZoomLevel} ({gameState.GetZoomLevelName()})");
    sb.AppendLine($"Render Scale: {gameState.GetRenderScale()}");
    sb.AppendLine($"Camera: ({gameState.CameraPosition.X}, {gameState.CameraPosition.Y})");
    sb.AppendLine($"Map Size: {gameState.MapWidth}x{gameState.MapHeight}");
    sb.AppendLine();

    // Dump the main viewport (excluding UI)
    for (int y = 0; y < viewportHeight; y++)
    {
        for (int x = 0; x < mainConsole.Width; x++)
        {
            var glyph = mainConsole.GetGlyph(x, y);
            sb.Append((char)glyph);
        }
        sb.AppendLine();
    }

    sb.AppendLine();
    sb.AppendLine("=== UI Area ===");
    for (int y = viewportHeight; y < mainConsole.Height; y++)
    {
        for (int x = 0; x < mainConsole.Width; x++)
        {
            var glyph = mainConsole.GetGlyph(x, y);
            sb.Append((char)glyph);
        }
        sb.AppendLine();
    }

    File.WriteAllText("screen_dump.txt", sb.ToString());
}

/// <summary>
/// Applies a single named game input action. Shared by keyboard handler and REST command path.
/// Key names match .NET's Keys enum and the KnownKeys set in GameObservabilityService.
/// </summary>
void ApplyGameInput(string key)
{
    if (gameState == null) return;

    switch (key)
    {
        case "Up":
        case "W":    gameState.MoveCamera(new Point(0, -1)); break;
        case "Down":
        case "S":    gameState.MoveCamera(new Point(0, 1));  break;
        case "Left":
        case "A":    gameState.MoveCamera(new Point(-1, 0)); break;
        case "Right":
        case "D":    gameState.MoveCamera(new Point(1, 0));  break;

        case "OemOpenBrackets":
            if (gameState.ZoomLevel > -2) gameState.ZoomLevel--;
            break;
        case "OemCloseBrackets":
            if (gameState.ZoomLevel < 2) gameState.ZoomLevel++;
            break;

        case "OemPlus":
        case "Add":
            if (gameState.GameSpeed < 4) gameState.GameSpeed++;
            break;
        case "OemMinus":
        case "Subtract":
            if (gameState.GameSpeed > 0) gameState.GameSpeed--;
            break;

        case "T":
            gameState.CycleTimeOfDay();
            break;
    }
}

void ApplyObservabilityCommand(GameCommand cmd)
{
    ApplyGameInput(cmd.Key);
    needsRender = true;
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
            // Parse map size from scenario
            var sizeParts = currentScenario.MapSize.Split('x');
            int width = int.Parse(sizeParts[0]);
            int height = int.Parse(sizeParts[1]);

            // Create new game state with correct map size
            gameState = new GameState(width, height);
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

    // Write observability dumps after every render
    observability?.UpdateDumps(mainConsole, gameState);
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

                // Check if this position is on a plot border
                var borderAppearance = GetBorderAppearance(worldX, worldY);
                if (borderAppearance.HasValue)
                {
                    (glyph, foreground, background) = borderAppearance.Value;
                }
                // For roads at scale=1, detect direction and use appropriate glyph
                else if (tile.Type == TileType.DirtRoad || tile.Type == TileType.PavedRoad)
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
                        {
                            // Apply time of day lighting
                            var litFg = LightingEffects.ApplyTimeOfDayLighting(foreground, gameState.VisualTimeOfDay);
                            var litBg = LightingEffects.ApplyTimeOfDayLighting(background, gameState.VisualTimeOfDay);
                            mainConsole.Print(screenX, screenY, glyph.ToString(), litFg, litBg);
                        }
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
                    var litFg = LightingEffects.ApplyTimeOfDayLighting(boundaryFg, gameState.VisualTimeOfDay);
                    var litBg = LightingEffects.ApplyTimeOfDayLighting(boundaryBg, gameState.VisualTimeOfDay);
                    mainConsole.Print(screenX, screenY, boundaryGlyph.ToString(), litFg, litBg);
                }
                else
                {
                    var litFg = LightingEffects.ApplyTimeOfDayLighting(cropFg, gameState.VisualTimeOfDay);
                    var litBg = LightingEffects.ApplyTimeOfDayLighting(cropBg, gameState.VisualTimeOfDay);
                    mainConsole.Print(screenX, screenY, cropGlyph.ToString(), litFg, litBg);
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
            {
                var litFg = LightingEffects.ApplyTimeOfDayLighting(naturalGround, gameState.VisualTimeOfDay);
                var litBg = LightingEffects.ApplyTimeOfDayLighting(naturalGroundDark, gameState.VisualTimeOfDay);
                mainConsole.Print(screenX, screenY, ".", litFg, litBg);
            }
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
            {
                var litFg = LightingEffects.ApplyTimeOfDayLighting(vForeground, gameState.VisualTimeOfDay);
                var litBg = LightingEffects.ApplyTimeOfDayLighting(vBackground, gameState.VisualTimeOfDay);
                mainConsole.Print(screenX, screenY, vRoadGlyph.ToString(), litFg, litBg);
            }
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
            {
                var litFg = LightingEffects.ApplyTimeOfDayLighting(hForeground, gameState.VisualTimeOfDay);
                var litBg = LightingEffects.ApplyTimeOfDayLighting(hBackground, gameState.VisualTimeOfDay);
                mainConsole.Print(screenX, screenY, hRoadGlyph.ToString(), litFg, litBg);
            }
        }
    }

    // Draw intersection marker ONLY at the exact center point
    if (isIntersection)
    {
        int screenX = dataX * scale + midX;
        int screenY = dataY * scale + midY;
        if (screenX < viewportWidth && screenY < viewportHeight)
        {
            var litFg = LightingEffects.ApplyTimeOfDayLighting(hForeground, gameState.VisualTimeOfDay);
            var litBg = LightingEffects.ApplyTimeOfDayLighting(hBackground, gameState.VisualTimeOfDay);
            mainConsole.Print(screenX, screenY, intersectionChar.ToString(), litFg, litBg);
        }
    }

    // If no neighbors (isolated road tile), use horizontal road character
    if (!hasNorth && !hasSouth && !hasWest && !hasEast)
    {
        int screenX = dataX * scale + midX;
        int screenY = dataY * scale + midY;
        if (screenX < viewportWidth && screenY < viewportHeight)
        {
            var litFg = LightingEffects.ApplyTimeOfDayLighting(hForeground, gameState.VisualTimeOfDay);
            var litBg = LightingEffects.ApplyTimeOfDayLighting(hBackground, gameState.VisualTimeOfDay);
            mainConsole.Print(screenX, screenY, hRoadGlyph.ToString(), litFg, litBg);
        }
    }
}

bool IsRoadTile(TileType type)
{
    return type == TileType.DirtRoad || type == TileType.PavedRoad;
}

int GetStructurePriority(string? cropType)
{
    // Determines which structures are important to show at far zoom
    // Uses the importance flags from building/structure definition files
    // Higher number = more important (1 for important, 0 for not important)

    if (cropType == null) return 0;

    // Special cases that aren't buildings/structures
    if (cropType == "yard" || cropType == "driveway") return 0;

    // Try to find building definition
    var buildingDef = buildingDefinitions.FirstOrDefault(b => b.Id == cropType);
    if (buildingDef != null)
    {
        var pattern = GetBuildingPattern(buildingDef);
        return pattern?.Important == true ? 1 : 0;
    }

    // Try to find structure definition
    var structureDef = structureDefinitions.FirstOrDefault(s =>
        s.Id == cropType || s.VariantOf == cropType);
    if (structureDef != null)
    {
        var pattern = GetStructurePattern(structureDef);
        return pattern?.Important == true ? 1 : 0;
    }

    return 0;
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

    // Track which building origins have been rendered globally across all screen positions
    // This prevents multi-tile buildings from appearing multiple times when they span sampling windows
    HashSet<(int, int)> globalProcessedOrigins = new();

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

            // At zoomed-out levels, ensure important farmstead structures are always visible
            // This applies whenever we're sampling (skipFactor > 1)
            if (skipFactor > 1)
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
                                Tile tileToConsider;
                                int originX, originY;

                                // If this is part of a multi-tile building, find its origin
                                if (checkTile.BuildingOffset.HasValue)
                                {
                                    originX = checkX - checkTile.BuildingOffset.Value.x;
                                    originY = checkY - checkTile.BuildingOffset.Value.y;

                                    // Skip if we've already rendered this building globally
                                    if (globalProcessedOrigins.Contains((originX, originY)))
                                        continue;

                                    // Mark this origin as processed globally
                                    globalProcessedOrigins.Add((originX, originY));

                                    // Get the origin tile
                                    if (originX >= 0 && originY >= 0 && originX < gameState.MapWidth && originY < gameState.MapHeight)
                                    {
                                        tileToConsider = gameState.Tiles[originX, originY];
                                    }
                                    else
                                    {
                                        continue; // Origin is out of bounds
                                    }
                                }
                                else
                                {
                                    // Single-tile structure, use as-is
                                    tileToConsider = checkTile;
                                    originX = checkX;
                                    originY = checkY;
                                }

                                int priority = GetStructurePriority(tileToConsider.CropType);

                                // Keep the most important structure found
                                if (priority > structurePriority)
                                {
                                    importantStructure = tileToConsider;
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
                else
                {
                    // Check if the sampled tile itself is part of an already-rendered building
                    // If so, replace with grass to avoid duplicate rendering
                    if (tile.Type == TileType.Grass && tile.CropType != null && tile.BuildingOffset.HasValue)
                    {
                        int tileOriginX = worldX - tile.BuildingOffset.Value.x;
                        int tileOriginY = worldY - tile.BuildingOffset.Value.y;

                        if (globalProcessedOrigins.Contains((tileOriginX, tileOriginY)))
                        {
                            // This tile is part of a building already rendered elsewhere - show grass instead
                            tile = new Tile(TileType.Grass, null, null, "yard");
                        }
                    }
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

            // Check if this position is on a plot border
            var borderAppearance = GetBorderAppearance(worldX, worldY);
            if (borderAppearance.HasValue)
            {
                (glyph, foreground, background) = borderAppearance.Value;
            }
            // Check for intersections and direction on roads
            else if (tile.Type == TileType.DirtRoad || tile.Type == TileType.PavedRoad)
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

            // Apply time of day lighting
            var litFg = LightingEffects.ApplyTimeOfDayLighting(foreground, gameState.VisualTimeOfDay);
            var litBg = LightingEffects.ApplyTimeOfDayLighting(background, gameState.VisualTimeOfDay);
            mainConsole.Print(screenX, screenY, glyph.ToString(), litFg, litBg);
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
        return GetFarmsteadStructureAppearance(tile);
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
    // Look up crop definition from loaded crops
    var cropDef = cropDefinitions.FirstOrDefault(c => c.Id == cropType);
    if (cropDef != null)
    {
        // Get pattern based on current zoom level
        var pattern = GetCropPattern(cropDef);
        if (pattern != null && pattern.Pattern.Length > 0)
        {
            char ch = pattern.Pattern[0];
            return (ch, cropDef.Color, cropDef.BackgroundColor);
        }

        // Fallback if no pattern
        return ('.', cropDef.Color, cropDef.BackgroundColor);
    }

    // Fallback for unknown crop types
    return ((char)240, Color.SaddleBrown, Color.DarkKhaki);
}

ZoomPattern? GetCropPattern(CropDefinition def)
{
    if (gameState == null) return null;

    return gameState.ZoomLevel switch
    {
        2 => def.Pattern25ft,   // 25ft zoom
        1 => def.Pattern50ft,   // 50ft zoom
        0 => def.Pattern100ft,  // 100ft zoom (default)
        -1 => def.Pattern200ft, // 200ft zoom
        -2 => def.Pattern400ft, // 400ft zoom
        _ => def.Pattern100ft   // Default to 100ft
    };
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

(char glyph, Color foreground, Color background) GetFarmsteadStructureAppearance(Tile tile)
{
    var structureType = tile.CropType;
    if (structureType == null)
        return ('.', Color.Green, Color.DarkGreen);

    // Special cases that aren't buildings/structures
    if (structureType == "yard")
        return ('.', Color.Green, Color.DarkGreen);
    if (structureType == "driveway")
        return ((char)176, Color.Tan, Color.SaddleBrown);
    if (structureType == "farm_field")
        return ((char)178, Color.Yellow, Color.DarkGoldenrod); // ▒ - farm field pattern

    // Try to find building definition (check by id)
    var buildingDef = buildingDefinitions.FirstOrDefault(b => b.Id == structureType);
    if (buildingDef != null)
    {
        return GetBuildingAppearance(buildingDef, tile);
    }

    // Try to find structure definition (check by id or variant_of)
    var structureDef = structureDefinitions.FirstOrDefault(s =>
        s.Id == structureType || s.VariantOf == structureType);
    if (structureDef != null)
    {
        return GetStructureAppearance(structureDef);
    }

    // Fallback to grass
    return ('.', Color.Green, Color.DarkGreen);
}

(char glyph, Color foreground, Color background) GetBuildingAppearance(BuildingDefinition def, Tile tile)
{
    var pattern = GetBuildingPattern(def);
    if (pattern == null)
        return ('.', def.Color, def.BackgroundColor);

    // At 25ft zoom with multi-tile buildings, use the building offset to look up the correct character
    if (gameState?.ZoomLevel == 2 && tile.BuildingOffset.HasValue && pattern.GetHeight() > 1)
    {
        var offset = tile.BuildingOffset.Value;
        char ch = pattern.GetCharAt(offset.x, offset.y);
        return (ch, def.Color, def.BackgroundColor);
    }

    // For single-character patterns or other zoom levels, use the first character
    char defaultCh = pattern.Pattern.Length > 0 ? pattern.Pattern[0] : '.';
    return (defaultCh, def.Color, def.BackgroundColor);
}

(char glyph, Color foreground, Color background) GetStructureAppearance(StructureDefinition def)
{
    var pattern = GetStructurePattern(def);
    if (pattern == null)
        return ('.', def.Color, def.BackgroundColor);

    // Structures are typically 1x1, so just use the pattern directly
    char ch = pattern.Pattern.Length > 0 ? pattern.Pattern[0] : '.';
    return (ch, def.Color, def.BackgroundColor);
}

/// <summary>
/// Check if a world position is on a plot border and return border appearance if applicable
/// Returns null if position is not on a border
/// </summary>
(char glyph, Color foreground, Color background)? GetBorderAppearance(int worldX, int worldY)
{
    if (gameState == null) return null;

    // Debug: Check plot finding (only log corners to reduce spam)
    if ((worldX == 0 || worldX == 29) && (worldY == 0 || worldY == 29))
    {
        System.Console.WriteLine($"BORDER DEBUG: Total plots in gameState: {gameState.Plots.Count}");
        foreach (var p in gameState.Plots)
        {
            System.Console.WriteLine($"  Plot '{p.Id}' - BorderType:{p.BorderType}, BorderSides:{p.BorderSides}, Bounds:{p.Bounds}");
        }
        var debugPlot = gameState.Plots.FirstOrDefault(p => p.Contains(worldX, worldY));
        System.Console.WriteLine($"BORDER DEBUG: Corner ({worldX},{worldY}) - Plot found: {debugPlot != null}, Plot ID: {debugPlot?.Id}, BorderType: {debugPlot?.BorderType}, BorderSides: {debugPlot?.BorderSides}");
    }

    // Find which plot this position belongs to
    // Prefer plots WITH borders if multiple plots overlap
    var plot = gameState.Plots
        .Where(p => p.Contains(worldX, worldY))
        .OrderByDescending(p => p.BorderSides != BorderSides.None ? 1 : 0)
        .ThenByDescending(p => !string.IsNullOrEmpty(p.BorderType) ? 1 : 0)
        .FirstOrDefault();

    if (plot == null || plot.BorderType == null || plot.BorderSides == BorderSides.None)
        return null;

    // Find border definition
    var borderDef = borderDefinitions.FirstOrDefault(b => b.Id == plot.BorderType);
    if (borderDef == null)
    {
        System.Console.WriteLine($"BORDER DEBUG: Border type '{plot.BorderType}' not found in definitions!");
        return null;
    }

    // Check if position is on the edge of the plot
    var bounds = plot.Bounds;
    bool isNorthEdge = worldY == bounds.Y;
    bool isSouthEdge = worldY == bounds.Y + bounds.Height - 1;
    bool isWestEdge = worldX == bounds.X;
    bool isEastEdge = worldX == bounds.X + bounds.Width - 1;

    // Debug corners
    if ((worldX == 0 || worldX == 29) && (worldY == 0 || worldY == 29))
    {
        System.Console.WriteLine($"BORDER DEBUG: Corner ({worldX},{worldY}) - North:{isNorthEdge} South:{isSouthEdge} West:{isWestEdge} East:{isEastEdge}");
        System.Console.WriteLine($"BORDER DEBUG: Bounds: {bounds}");
    }

    // Determine which specific border side this position is on
    BorderSides currentSide = BorderSides.None;
    if (isNorthEdge && plot.BorderSides.HasFlag(BorderSides.North)) currentSide = BorderSides.North;
    else if (isSouthEdge && plot.BorderSides.HasFlag(BorderSides.South)) currentSide = BorderSides.South;
    else if (isWestEdge && plot.BorderSides.HasFlag(BorderSides.West)) currentSide = BorderSides.West;
    else if (isEastEdge && plot.BorderSides.HasFlag(BorderSides.East)) currentSide = BorderSides.East;

    if (currentSide == BorderSides.None)
    {
        if ((worldX == 0 || worldX == 29) && (worldY == 0 || worldY == 29))
            System.Console.WriteLine($"BORDER DEBUG: Corner ({worldX},{worldY}) - currentSide=None");
        return null;
    }

    // Get the appropriate pattern for current zoom level and side
    var pattern = borderDef.GetPatternForZoom(gameState.ZoomLevel, currentSide);
    if (pattern == null)
    {
        // Only log this once per frame to avoid spam
        if (worldX == bounds.X && worldY == bounds.Y)
            System.Console.WriteLine($"BORDER DEBUG: Border '{borderDef.Name}' has no pattern at zoom {gameState.ZoomLevel}");
        return null; // Border invisible at this zoom
    }

    // Log first border render (top-left corner) to confirm it's working
    if (worldX == bounds.X && worldY == bounds.Y)
        System.Console.WriteLine($"BORDER DEBUG: Rendering border '{borderDef.Name}' character '{pattern.Value}' at ({worldX},{worldY}) zoom={gameState.ZoomLevel}");

    // Log corners at all zooms
    if ((worldX == 0 || worldX == 29) && (worldY == 0 || worldY == 29))
        System.Console.WriteLine($"BORDER DEBUG RENDER: ({worldX},{worldY}) zoom={gameState.ZoomLevel} border='{borderDef.Name}' char='{pattern.Value}'");

    // Get zoom-specific background color, or use tile's background if "neighbor"
    var borderBackground = borderDef.GetBackgroundColorForZoom(gameState.ZoomLevel);

    // If background is null (meaning "neighbor"), use the underlying tile's background
    if (borderBackground == null)
    {
        var tile = gameState.Tiles[worldX, worldY];
        var (_, _, tileBackground) = GetTileAppearance(tile);
        borderBackground = tileBackground;
    }

    return (pattern.Value, borderDef.Color, borderBackground.Value);
}

ZoomPattern? GetBuildingPattern(BuildingDefinition def)
{
    if (gameState == null) return null;

    return gameState.ZoomLevel switch
    {
        2 => def.Pattern25ft,   // 25ft zoom
        1 => def.Pattern50ft,   // 50ft zoom
        0 => def.Pattern100ft,  // 100ft zoom (default)
        -1 => def.Pattern200ft, // 200ft zoom
        -2 => def.Pattern400ft, // 400ft zoom
        _ => def.Pattern100ft   // Default to 100ft
    };
}

ZoomPattern? GetStructurePattern(StructureDefinition def)
{
    if (gameState == null) return null;

    return gameState.ZoomLevel switch
    {
        2 => def.Pattern25ft,   // 25ft zoom
        1 => def.Pattern50ft,   // 50ft zoom
        0 => def.Pattern100ft,  // 100ft zoom (default)
        -1 => def.Pattern200ft, // 200ft zoom
        -2 => def.Pattern400ft, // 400ft zoom
        _ => def.Pattern100ft   // Default to 100ft
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

    // Time of day display (third line top right)
    var timeText = $"Time: {gameState.GetTimeOfDayName()} (Press T)";
    int timeX = console.Width - timeText.Length - 1;
    // Color changes based on time of day for visual feedback
    var timeColor = gameState.VisualTimeOfDay switch
    {
        TimeOfDay.Dawn => Color.Orange,
        TimeOfDay.Morning => Color.Yellow,
        TimeOfDay.Midday => Color.White,
        TimeOfDay.Afternoon => Color.LightYellow,
        TimeOfDay.Dusk => Color.OrangeRed,
        TimeOfDay.Evening => Color.Purple,
        TimeOfDay.Night => Color.DarkBlue,
        _ => Color.White
    };
    console.Print(timeX, 2, timeText, timeColor, Color.Black);

    // Weather display (left side, top)
    var weatherText = $"Weather: {gameState.CurrentWeather.GetConditionName()} (Q)";
    console.Print(1, 0, weatherText, Color.LightBlue, Color.Black);

    var tempText = $"Temp: {gameState.CurrentWeather.TemperatureF}°F (PgUp/PgDn)";
    console.Print(1, 1, tempText, Color.Orange, Color.Black);

    var windText = $"Wind: {gameState.CurrentWeather.GetWindDescription()} (Home/End,Ins)";
    console.Print(1, 2, windText, Color.Cyan, Color.Black);

    var humidityText = $"Humidity: {gameState.CurrentWeather.HumidityPercent}% (Del)";
    console.Print(1, 3, humidityText, Color.LightGreen, Color.Black);

    // Atmospheric display (left side, continued)
    var pressureText = $"Pressure: {gameState.CurrentWeather.BarometricPressure:F2} inHg (E/R)";
    console.Print(1, 4, pressureText, Color.Violet, Color.Black);

    var visibilityText = $"Visibility: {gameState.CurrentWeather.VisibilityMiles:F1} mi (F/G)";
    console.Print(1, 5, visibilityText, Color.LightGray, Color.Black);

    var fireDangerText = $"Fire Danger: {gameState.CurrentWeather.GetFireDangerName()} (V)";
    console.Print(1, 6, fireDangerText, gameState.CurrentWeather.GetFireDangerColor(), Color.Black);

    // Celestial display (left side, continued)
    var sunriseText = $"Sunrise: {Weather.GetSunriseTime(gameState.CurrentDate)}";
    var sunsetText = $"Sunset: {Weather.GetSunsetTime(gameState.CurrentDate)}";
    var sunTimesText = $"{sunriseText} | {sunsetText}";
    console.Print(1, 7, sunTimesText, Color.Yellow, Color.Black);

    var moonPhase = Weather.GetMoonPhase(gameState.CurrentDate);
    var moonText = $"Moon: {Weather.GetMoonPhaseName(moonPhase)}";
    console.Print(1, 8, moonText, Color.LightCyan, Color.Black);

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
