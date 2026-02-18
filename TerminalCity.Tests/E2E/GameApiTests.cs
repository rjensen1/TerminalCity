using System.Net;
using Xunit.Abstractions;

namespace TerminalCity.Tests.E2E;

/// <summary>
/// E2E tests against the live TerminalCity REST API (localhost:5200).
/// All tests skip with a clear message if the game is not running.
/// Command tests additionally skip if GameMode is not Playing (load a scenario first).
/// </summary>
[Collection("GameApi")]
public class GameApiTests
{
    private readonly GameApiFixture _api;
    private readonly ITestOutputHelper _output;

    public GameApiTests(GameApiFixture api, ITestOutputHelper output)
    {
        _api = api;
        _output = output;
    }

    private void SkipIfNotReachable() =>
        Skip.If(!_api.GameReachable, "Game not reachable at localhost:5200 — start TerminalCity and try again.");

    private async Task<GameStateDto> RequirePlayingModeAsync()
    {
        var state = await _api.GetStateAsync();
        Skip.If(state.Mode != "Playing", $"Game must be in Playing mode (current: {state.Mode}) — load a scenario first.");
        return state;
    }

    // AC1: GET /state returns valid JSON with required fields present
    [SkippableFact]
    public async Task WhenGetState_ShouldReturnValidJsonWithRequiredFields()
    {
        SkipIfNotReachable();

        var state = await _api.GetStateAsync();

        Assert.NotNull(state.Mode);
        Assert.NotNull(state.CameraPosition);
        Assert.NotNull(state.Weather);
        Assert.NotNull(state.VisualTimeOfDay);
        // Money and Population are value types — always present
        _output.WriteLine($"Mode={state.Mode} Money={state.Money} Pop={state.Population} Camera=({state.CameraPosition.X},{state.CameraPosition.Y})");
    }

    // AC6: GET /screen returns non-empty ASCII content
    [SkippableFact]
    public async Task WhenGetScreen_ShouldReturnNonEmptyContent()
    {
        SkipIfNotReachable();

        var response = await _api.Client.GetAsync("/screen");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();

        Assert.NotEmpty(content);
        _output.WriteLine($"Screen dump length: {content.Length} chars");
    }

    // AC7: POST unknown key → 400
    [SkippableFact]
    public async Task WhenPostUnknownKey_ShouldReturn400()
    {
        SkipIfNotReachable();

        var response = await _api.PostCommandAsync("UnknownKey_XYZ");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    // AC2: POST Right × 1 → cameraPosition.x increases by at least 1 (or is clamped at map edge)
    [SkippableFact]
    public async Task WhenPostRightCommand_ShouldIncreaseCameraX()
    {
        SkipIfNotReachable();
        var initial = await RequirePlayingModeAsync();
        _output.WriteLine($"Initial camera: ({initial.CameraPosition.X},{initial.CameraPosition.Y})");

        var postResponse = await _api.PostCommandAsync("Right");
        Assert.Equal(HttpStatusCode.Accepted, postResponse.StatusCode);

        var updated = await _api.WaitForState(s => s.CameraPosition.X != initial.CameraPosition.X);

        Assert.True(updated.CameraPosition.X >= initial.CameraPosition.X + 1,
            $"Expected X >= {initial.CameraPosition.X + 1}, got {updated.CameraPosition.X}");
        _output.WriteLine($"Updated camera: ({updated.CameraPosition.X},{updated.CameraPosition.Y})");
    }

    // AC3: POST ] → zoomLevel increases by 1
    [SkippableFact]
    public async Task WhenPostCloseBracket_ShouldIncreaseZoomLevel()
    {
        SkipIfNotReachable();
        var initial = await RequirePlayingModeAsync();
        Skip.If(initial.ZoomLevel >= 2, $"ZoomLevel already at max (2) — can't increase further.");
        _output.WriteLine($"Initial zoomLevel: {initial.ZoomLevel}");

        var postResponse = await _api.PostCommandAsync("OemCloseBrackets");
        Assert.Equal(HttpStatusCode.Accepted, postResponse.StatusCode);

        var updated = await _api.WaitForState(s => s.ZoomLevel != initial.ZoomLevel);

        Assert.Equal(initial.ZoomLevel + 1, updated.ZoomLevel);
        _output.WriteLine($"Updated zoomLevel: {updated.ZoomLevel}");
    }

    // AC4: POST + → gameSpeed increases by 1
    [SkippableFact]
    public async Task WhenPostPlus_ShouldIncreaseGameSpeed()
    {
        SkipIfNotReachable();
        var initial = await RequirePlayingModeAsync();
        Skip.If(initial.GameSpeed >= 4, $"GameSpeed already at max (4) — can't increase further.");
        _output.WriteLine($"Initial gameSpeed: {initial.GameSpeed}");

        var postResponse = await _api.PostCommandAsync("OemPlus");
        Assert.Equal(HttpStatusCode.Accepted, postResponse.StatusCode);

        var updated = await _api.WaitForState(s => s.GameSpeed != initial.GameSpeed);

        Assert.Equal(initial.GameSpeed + 1, updated.GameSpeed);
        _output.WriteLine($"Updated gameSpeed: {updated.GameSpeed}");
    }

    // AC5: POST T → visualTimeOfDay advances to next in 7-step cycle
    [SkippableFact]
    public async Task WhenPostT_ShouldCycleVisualTimeOfDay()
    {
        SkipIfNotReachable();
        var initial = await RequirePlayingModeAsync();
        _output.WriteLine($"Initial visualTimeOfDay: {initial.VisualTimeOfDay}");

        var postResponse = await _api.PostCommandAsync("T");
        Assert.Equal(HttpStatusCode.Accepted, postResponse.StatusCode);

        var updated = await _api.WaitForState(s => s.VisualTimeOfDay != initial.VisualTimeOfDay);

        Assert.NotEqual(initial.VisualTimeOfDay, updated.VisualTimeOfDay);
        _output.WriteLine($"Updated visualTimeOfDay: {updated.VisualTimeOfDay}");
    }

    // AC8: Sequential commands → state accumulates correctly (3× Right → X += 3)
    [SkippableFact]
    public async Task WhenSequentialRightCommands_StateShouldAccumulateCorrectly()
    {
        SkipIfNotReachable();
        var initial = await RequirePlayingModeAsync();
        const int steps = 3;
        _output.WriteLine($"Initial camera X: {initial.CameraPosition.X}");

        for (int i = 0; i < steps; i++)
        {
            var r = await _api.PostCommandAsync("Right");
            Assert.Equal(HttpStatusCode.Accepted, r.StatusCode);
        }

        var updated = await _api.WaitForState(
            s => s.CameraPosition.X >= initial.CameraPosition.X + steps,
            timeout: TimeSpan.FromSeconds(3));

        Assert.True(updated.CameraPosition.X >= initial.CameraPosition.X + steps,
            $"Expected X >= {initial.CameraPosition.X + steps}, got {updated.CameraPosition.X}");
        _output.WriteLine($"Updated camera X: {updated.CameraPosition.X} (delta={updated.CameraPosition.X - initial.CameraPosition.X})");
    }
}
