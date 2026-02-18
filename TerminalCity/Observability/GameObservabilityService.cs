using System.Collections.Concurrent;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using SadConsole;
using TerminalCity.Domain;

namespace TerminalCity.Observability;

/// <summary>
/// Exposes game state to external observers (agents, tests).
///
/// Phase 1 — File dumps: writes screen_dump.txt and game_state.json after every Render().
/// Phase 2 — REST API: embedded HTTP server on port 5200 serves the same data and accepts queued commands.
///
/// Output directory: TC_DUMPS_DIR env var, or the outputDir constructor arg, or "dumps".
/// </summary>
public class GameObservabilityService
{
    private readonly string _outputDir;
    private readonly ConcurrentQueue<GameCommand> _commandQueue = new();
    private volatile string _stateSnapshot = "{}";
    private volatile string _screenSnapshot = "";

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters = { new JsonStringEnumConverter() }
    };

    public GameObservabilityService(string? outputDir = null)
    {
        _outputDir = Environment.GetEnvironmentVariable("TC_DUMPS_DIR")
            ?? outputDir
            ?? "dumps";
        Directory.CreateDirectory(_outputDir);
    }

    /// <summary>
    /// Called after every Render(). Writes screen_dump.txt and game_state.json.
    /// </summary>
    public void UpdateDumps(ScreenSurface console, GameState state)
    {
        _stateSnapshot = SerializeState(state);
        _screenSnapshot = DumpScreen(console);

        File.WriteAllText(Path.Combine(_outputDir, "game_state.json"), _stateSnapshot);
        File.WriteAllText(Path.Combine(_outputDir, "screen_dump.txt"), _screenSnapshot);
    }

    /// <summary>
    /// Called each game tick. Returns and removes all queued commands.
    /// </summary>
    public IEnumerable<GameCommand> DrainCommands()
    {
        while (_commandQueue.TryDequeue(out var cmd))
            yield return cmd;
    }

    /// <summary>
    /// Enqueues a command. Called by the HTTP handler; also usable directly in tests.
    /// </summary>
    public void EnqueueCommand(GameCommand cmd) => _commandQueue.Enqueue(cmd);

    /// <summary>
    /// Starts the background HTTP server (Phase 2).
    /// </summary>
    public void StartHttpServer(int port = 5200)
    {
        var thread = new Thread(() => RunHttpServer(port)) { IsBackground = true, Name = "ObservabilityHttp" };
        thread.Start();
    }

    /// <summary>
    /// Serializes game state to JSON. Public so tests can verify the output format.
    /// </summary>
    public string SerializeState(GameState state)
    {
        var tiles = new List<TileDto>();
        for (int y = 0; y < state.MapHeight; y++)
        {
            for (int x = 0; x < state.MapWidth; x++)
            {
                var tile = state.Tiles[x, y];
                tiles.Add(new TileDto(
                    x, y,
                    tile.Type.ToString(),
                    tile.CropType,
                    tile.BuildingOffset.HasValue
                        ? new OffsetDto(tile.BuildingOffset.Value.x, tile.BuildingOffset.Value.y)
                        : null
                ));
            }
        }

        var dto = new GameStateDto(
            Mode: state.CurrentMode.ToString(),
            Money: state.Money,
            Population: state.Population,
            Date: state.CurrentDate,
            GameSpeed: state.GameSpeed,
            ZoomLevel: state.ZoomLevel,
            CameraPosition: new OffsetDto(state.CameraPosition.X, state.CameraPosition.Y),
            VisualTimeOfDay: state.VisualTimeOfDay.ToString(),
            Weather: new WeatherDto(
                Condition: state.CurrentWeather.Condition.ToString(),
                TemperatureF: state.CurrentWeather.TemperatureF,
                WindSpeedMph: state.CurrentWeather.WindSpeedMph,
                WindDirection: state.CurrentWeather.WindDirection.ToString(),
                HumidityPercent: state.CurrentWeather.HumidityPercent
            ),
            Tiles: tiles
        );

        return JsonSerializer.Serialize(dto, JsonOptions);
    }

    private static string DumpScreen(ScreenSurface console)
    {
        var sb = new System.Text.StringBuilder();
        for (int y = 0; y < console.Height; y++)
        {
            for (int x = 0; x < console.Width; x++)
            {
                var glyph = console.GetGlyph(x, y);
                sb.Append((char)glyph);
            }
            sb.Append('\n');
        }
        return sb.ToString();
    }

    private void RunHttpServer(int port)
    {
        var listener = new HttpListener();
        listener.Prefixes.Add($"http://localhost:{port}/");
        try
        {
            listener.Start();
            while (listener.IsListening)
            {
                var context = listener.GetContext();
                try { ProcessRequest(context); }
                catch (Exception ex) { LogHttpError(ex.ToString()); }
            }
        }
        catch (Exception ex)
        {
            LogHttpError($"HTTP server failed to start or crashed: {ex}");
        }
    }

    private void ProcessRequest(HttpListenerContext context)
    {
        var request = context.Request;
        var response = context.Response;
        try
        {
            var path = request.Url?.AbsolutePath;

            if (request.HttpMethod == "GET" && path == "/state")
            {
                Write(response, 200, "application/json", _stateSnapshot);
            }
            else if (request.HttpMethod == "GET" && path == "/screen")
            {
                Write(response, 200, "text/plain; charset=utf-8", _screenSnapshot);
            }
            else if (request.HttpMethod == "POST" && path == "/command")
            {
                using var reader = new StreamReader(request.InputStream);
                var body = reader.ReadToEnd();
                var cmd = JsonSerializer.Deserialize<GameCommand>(body,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (cmd == null || string.IsNullOrEmpty(cmd.Key))
                {
                    response.StatusCode = 400;
                }
                else
                {
                    EnqueueCommand(cmd);
                    response.StatusCode = 202;
                }
            }
            else
            {
                response.StatusCode = 404;
            }
        }
        finally
        {
            response.OutputStream.Close();
        }
    }

    private static void Write(HttpListenerResponse response, int status, string contentType, string body)
    {
        var bytes = System.Text.Encoding.UTF8.GetBytes(body);
        response.StatusCode = status;
        response.ContentType = contentType;
        response.ContentLength64 = bytes.Length;
        response.OutputStream.Write(bytes);
    }

    private void LogHttpError(string message)
    {
        try { File.AppendAllText(Path.Combine(_outputDir, "http_errors.log"), $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {message}{Environment.NewLine}"); }
        catch { }
    }

    // DTOs for JSON serialization

    private record GameStateDto(
        string Mode,
        int Money,
        int Population,
        DateTime Date,
        int GameSpeed,
        int ZoomLevel,
        OffsetDto CameraPosition,
        string VisualTimeOfDay,
        WeatherDto Weather,
        List<TileDto> Tiles
    );

    private record WeatherDto(
        string Condition,
        int TemperatureF,
        int WindSpeedMph,
        string WindDirection,
        int HumidityPercent
    );

    private record TileDto(
        int X,
        int Y,
        string Type,
        string? CropType,
        OffsetDto? BuildingOffset
    );

    private record OffsetDto(int X, int Y);
}
