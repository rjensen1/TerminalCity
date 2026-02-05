using SadConsole;
using SadConsole.Configuration;
using SadConsole.Input;
using SadRogue.Primitives;
using TerminalCity.Domain;

Settings.WindowTitle = "TerminalCity - ASCII City Builder";

// Game state
GameState? gameState = null;
ScreenSurface? mainConsole = null;
string? statusMessage = null;
DateTime? statusMessageTime = null;

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

    // Create main console
    mainConsole = new ScreenSurface(120, 40);
    mainConsole.UseKeyboard = true;
    mainConsole.UseMouse = false;
    mainConsole.IsFocused = true;

    // Set up input handler
    mainConsole.SadComponents.Add(new InputHandler(OnKeyPressed));

    // Set as active screen
    Game.Instance.Screen = mainConsole;

    // Render initial screen
    RenderTitleScreen();
}

void OnKeyPressed(IScreenObject console, Keyboard keyboard)
{
    if (gameState == null) return;

    // Title screen
    if (gameState.CurrentMode == GameMode.TitleScreen)
    {
        if (keyboard.IsKeyPressed(Keys.Enter) || keyboard.IsKeyPressed(Keys.Space))
        {
            gameState.CurrentMode = GameMode.Playing;
            Render();
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
        // Escape to title screen
        else if (keyboard.IsKeyPressed(Keys.Escape))
        {
            gameState.CurrentMode = GameMode.TitleScreen;
            RenderTitleScreen();
            return;
        }

        Render();
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
        "Press ESC to quit",
        "",
        "",
        "Controls:",
        "  Arrow Keys or WASD - Move camera",
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
}

void Render()
{
    if (gameState == null || mainConsole == null) return;

    mainConsole.Clear();

    // Calculate viewport
    int viewportWidth = mainConsole.Width;
    int viewportHeight = mainConsole.Height - 3; // Reserve 3 rows for UI
    int startX = gameState.CameraPosition.X - viewportWidth / 2;
    int startY = gameState.CameraPosition.Y - viewportHeight / 2;

    // Render map
    for (int screenY = 0; screenY < viewportHeight; screenY++)
    {
        for (int screenX = 0; screenX < viewportWidth; screenX++)
        {
            int worldX = startX + screenX;
            int worldY = startY + screenY;

            // Out of bounds - render black
            if (worldX < 0 || worldX >= gameState.MapWidth || worldY < 0 || worldY >= gameState.MapHeight)
            {
                mainConsole.Print(screenX, screenY, " ", Color.Black, Color.Black);
                continue;
            }

            // Get tile and render
            var tile = gameState.Tiles[worldX, worldY];
            var (glyph, foreground, background) = GetTileAppearance(tile);
            mainConsole.Print(screenX, screenY, glyph.ToString(), foreground, background);
        }
    }

    // Render camera position indicator
    int centerX = viewportWidth / 2;
    int centerY = viewportHeight / 2;
    mainConsole.SetGlyph(centerX, centerY, '+', Color.Yellow);

    // Render UI at bottom
    RenderUI(mainConsole, viewportHeight);
}

(char glyph, Color foreground, Color background) GetTileAppearance(Tile tile)
{
    return tile.Type switch
    {
        TileType.Grass => ('.', Color.Green, Color.DarkGreen),
        TileType.Road => ('#', Color.Gray, Color.DarkGray),
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

void RenderUI(ScreenSurface console, int uiStartY)
{
    if (gameState == null) return;

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
