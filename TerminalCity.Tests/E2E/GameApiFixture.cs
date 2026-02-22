using System.Text;
using System.Text.Json;

namespace TerminalCity.Tests.E2E;

/// <summary>
/// Shared fixture for E2E tests. Checks connectivity at startup; all tests skip if unreachable.
/// </summary>
public class GameApiFixture : IAsyncLifetime
{
    private const string BaseUrl = "http://localhost:5200";

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public HttpClient Client { get; } = new() { BaseAddress = new Uri(BaseUrl) };

    /// <summary>True if the game was reachable at fixture startup.</summary>
    public bool GameReachable { get; private set; }

    public async Task InitializeAsync()
    {
        try
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var response = await Client.GetAsync("/state", cts.Token);
            GameReachable = response.IsSuccessStatusCode;
        }
        catch
        {
            GameReachable = false;
        }
    }

    public Task DisposeAsync()
    {
        Client.Dispose();
        return Task.CompletedTask;
    }

    /// <summary>GET /state and deserialize.</summary>
    public async Task<GameStateDto> GetStateAsync()
    {
        var response = await Client.GetAsync("/state");
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<GameStateDto>(json, JsonOptions)!;
    }

    /// <summary>
    /// Polls GET /state every 50 ms until <paramref name="condition"/> is true or timeout expires.
    /// Default timeout: 2 seconds.
    /// </summary>
    public async Task<GameStateDto> WaitForState(
        Func<GameStateDto, bool> condition,
        TimeSpan? timeout = null)
    {
        var deadline = DateTime.UtcNow + (timeout ?? TimeSpan.FromSeconds(2));
        while (DateTime.UtcNow < deadline)
        {
            var state = await GetStateAsync();
            if (condition(state)) return state;
            await Task.Delay(50);
        }
        throw new TimeoutException("State condition not met within timeout.");
    }

    /// <summary>POST /command with the given key. Returns the raw response.</summary>
    public async Task<HttpResponseMessage> PostCommandAsync(string key)
    {
        var json = JsonSerializer.Serialize(new { key });
        using var content = new StringContent(json, Encoding.UTF8, "application/json");
        return await Client.PostAsync("/command", content);
    }
}
