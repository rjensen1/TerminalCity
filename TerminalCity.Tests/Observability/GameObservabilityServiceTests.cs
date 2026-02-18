using System.Text.Json;
using TerminalCity.Domain;
using TerminalCity.Observability;
using Xunit;

namespace TerminalCity.Tests.Observability;

public class GameObservabilityServiceTests : IDisposable
{
    private readonly string _tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

    public GameObservabilityServiceTests()
    {
        Directory.CreateDirectory(_tempDir);
    }

    // --- SerializeState: structure ---

    [Fact]
    public void WhenSerializeState_ShouldIncludeModeAsString()
    {
        var service = new GameObservabilityService(_tempDir);
        var state = new GameState(5, 5) { CurrentMode = GameMode.Playing };

        var json = service.SerializeState(state);
        var doc = JsonDocument.Parse(json);

        Assert.Equal("Playing", doc.RootElement.GetProperty("mode").GetString());
    }

    [Fact]
    public void WhenSerializeState_ShouldIncludeMoneyAndPopulation()
    {
        var service = new GameObservabilityService(_tempDir);
        var state = new GameState(5, 5) { Money = 42000, Population = 123 };

        var json = service.SerializeState(state);
        var doc = JsonDocument.Parse(json);

        Assert.Equal(42000, doc.RootElement.GetProperty("money").GetInt32());
        Assert.Equal(123, doc.RootElement.GetProperty("population").GetInt32());
    }

    [Fact]
    public void WhenSerializeState_ShouldIncludeZoomAndGameSpeed()
    {
        var service = new GameObservabilityService(_tempDir);
        var state = new GameState(5, 5) { ZoomLevel = -2, GameSpeed = 3 };

        var json = service.SerializeState(state);
        var doc = JsonDocument.Parse(json);

        Assert.Equal(-2, doc.RootElement.GetProperty("zoomLevel").GetInt32());
        Assert.Equal(3, doc.RootElement.GetProperty("gameSpeed").GetInt32());
    }

    [Fact]
    public void WhenSerializeState_ShouldIncludeCameraPosition()
    {
        var service = new GameObservabilityService(_tempDir);
        var state = new GameState(20, 20);

        var json = service.SerializeState(state);
        var doc = JsonDocument.Parse(json);

        var cam = doc.RootElement.GetProperty("cameraPosition");
        Assert.Equal(state.CameraPosition.X, cam.GetProperty("x").GetInt32());
        Assert.Equal(state.CameraPosition.Y, cam.GetProperty("y").GetInt32());
    }

    [Fact]
    public void WhenSerializeState_ShouldIncludeVisualTimeOfDayAsString()
    {
        var service = new GameObservabilityService(_tempDir);
        var state = new GameState(5, 5) { VisualTimeOfDay = TimeOfDay.Dusk };

        var json = service.SerializeState(state);
        var doc = JsonDocument.Parse(json);

        Assert.Equal("Dusk", doc.RootElement.GetProperty("visualTimeOfDay").GetString());
    }

    [Fact]
    public void WhenSerializeState_ShouldIncludeWeatherObject()
    {
        var service = new GameObservabilityService(_tempDir);
        var state = new GameState(5, 5);
        state.CurrentWeather.TemperatureF = 75;
        state.CurrentWeather.HumidityPercent = 80;

        var json = service.SerializeState(state);
        var doc = JsonDocument.Parse(json);

        var weather = doc.RootElement.GetProperty("weather");
        Assert.Equal(75, weather.GetProperty("temperatureF").GetInt32());
        Assert.Equal(80, weather.GetProperty("humidityPercent").GetInt32());
        Assert.True(weather.TryGetProperty("condition", out _));
        Assert.True(weather.TryGetProperty("windSpeedMph", out _));
        Assert.True(weather.TryGetProperty("windDirection", out _));
    }

    // --- SerializeState: tiles ---

    [Fact]
    public void WhenSerializeState_ShouldIncludeTilesArray()
    {
        var service = new GameObservabilityService(_tempDir);
        var state = new GameState(3, 3);

        var json = service.SerializeState(state);
        var doc = JsonDocument.Parse(json);

        var tiles = doc.RootElement.GetProperty("tiles");
        Assert.Equal(JsonValueKind.Array, tiles.ValueKind);
        Assert.Equal(9, tiles.GetArrayLength()); // 3x3 = 9 tiles
    }

    [Fact]
    public void WhenSerializeState_TilesShouldIncludeXYAndType()
    {
        var service = new GameObservabilityService(_tempDir);
        var state = new GameState(2, 2);
        state.Tiles[1, 0] = new Tile(TileType.Farm, null, null, "wheat");

        var json = service.SerializeState(state);
        var doc = JsonDocument.Parse(json);

        var tiles = doc.RootElement.GetProperty("tiles").EnumerateArray().ToList();
        var farmTile = tiles.First(t =>
            t.GetProperty("x").GetInt32() == 1 &&
            t.GetProperty("y").GetInt32() == 0);

        Assert.Equal("Farm", farmTile.GetProperty("type").GetString());
        Assert.Equal("wheat", farmTile.GetProperty("cropType").GetString());
    }

    [Fact]
    public void WhenSerializeState_TileWithBuildingOffset_ShouldIncludeOffset()
    {
        var service = new GameObservabilityService(_tempDir);
        var state = new GameState(3, 3);
        state.Tiles[2, 1] = new Tile(TileType.Grass, null, null, "tiny_farmhouse", (1, 0));

        var json = service.SerializeState(state);
        var doc = JsonDocument.Parse(json);

        var tiles = doc.RootElement.GetProperty("tiles").EnumerateArray().ToList();
        var offsetTile = tiles.First(t =>
            t.GetProperty("x").GetInt32() == 2 &&
            t.GetProperty("y").GetInt32() == 1);

        var offset = offsetTile.GetProperty("buildingOffset");
        Assert.Equal(1, offset.GetProperty("x").GetInt32());
        Assert.Equal(0, offset.GetProperty("y").GetInt32());
    }

    [Fact]
    public void WhenSerializeState_TileWithNoBuildingOffset_OffsetShouldBeNull()
    {
        var service = new GameObservabilityService(_tempDir);
        var state = new GameState(2, 2);

        var json = service.SerializeState(state);
        var doc = JsonDocument.Parse(json);

        var firstTile = doc.RootElement.GetProperty("tiles").EnumerateArray().First();
        Assert.Equal(JsonValueKind.Null, firstTile.GetProperty("buildingOffset").ValueKind);
    }

    // --- Command queue ---

    [Fact]
    public void WhenDrainCommands_WithNoCommands_ShouldReturnEmpty()
    {
        var service = new GameObservabilityService(_tempDir);

        var commands = service.DrainCommands().ToList();

        Assert.Empty(commands);
    }

    [Fact]
    public void WhenEnqueueCommand_ThenDrainCommands_ShouldReturnCommand()
    {
        var service = new GameObservabilityService(_tempDir);
        service.EnqueueCommand(new GameCommand("Right"));

        var commands = service.DrainCommands().ToList();

        Assert.Single(commands);
        Assert.Equal("Right", commands[0].Key);
    }

    [Fact]
    public void WhenDrainCommands_ShouldClearQueue()
    {
        var service = new GameObservabilityService(_tempDir);
        service.EnqueueCommand(new GameCommand("Up"));
        service.DrainCommands().ToList(); // first drain

        var second = service.DrainCommands().ToList();

        Assert.Empty(second);
    }

    [Fact]
    public void WhenEnqueueMultipleCommands_DrainShouldReturnAllInOrder()
    {
        var service = new GameObservabilityService(_tempDir);
        service.EnqueueCommand(new GameCommand("Up"));
        service.EnqueueCommand(new GameCommand("Right"));
        service.EnqueueCommand(new GameCommand("T"));

        var commands = service.DrainCommands().ToList();

        Assert.Equal(3, commands.Count);
        Assert.Equal("Up", commands[0].Key);
        Assert.Equal("Right", commands[1].Key);
        Assert.Equal("T", commands[2].Key);
    }

    // --- Output directory config ---

    [Fact]
    public void WhenConstructedWithOutputDir_ShouldCreateDirectory()
    {
        var customDir = Path.Combine(_tempDir, "custom_dumps");
        Assert.False(Directory.Exists(customDir));

        _ = new GameObservabilityService(customDir);

        Assert.True(Directory.Exists(customDir));
    }

    public void Dispose()
    {
        if (Directory.Exists(_tempDir))
            Directory.Delete(_tempDir, recursive: true);
    }
}

/// <summary>
/// Integration tests for the embedded HTTP server.
/// Each test starts a service on a dedicated port to avoid cross-test interference.
/// </summary>
public class GameObservabilityServiceHttpTests : IDisposable
{
    private readonly string _tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
    private readonly HttpClient _http = new();

    public GameObservabilityServiceHttpTests()
    {
        Directory.CreateDirectory(_tempDir);
    }

    [Fact]
    public async Task WhenPostCommand_WithUnknownKey_ShouldReturn400()
    {
        var service = new GameObservabilityService(_tempDir);
        service.StartHttpServer(15210);
        await Task.Delay(150); // wait for HttpListener thread to start

        var body = new StringContent("{\"key\":\"UnknownKey123\"}", System.Text.Encoding.UTF8, "application/json");
        var response = await _http.PostAsync("http://localhost:15210/command", body);

        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task WhenPostCommand_WithKnownKey_ShouldReturn202()
    {
        var service = new GameObservabilityService(_tempDir);
        service.StartHttpServer(15211);
        await Task.Delay(150);

        var body = new StringContent("{\"key\":\"Right\"}", System.Text.Encoding.UTF8, "application/json");
        var response = await _http.PostAsync("http://localhost:15211/command", body);

        Assert.Equal(System.Net.HttpStatusCode.Accepted, response.StatusCode);
    }

    public void Dispose()
    {
        _http.Dispose();
        if (Directory.Exists(_tempDir))
            Directory.Delete(_tempDir, recursive: true);
    }
}
