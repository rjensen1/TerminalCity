namespace TerminalCity.Tests.E2E;

/// <summary>
/// DTOs for deserializing game state from GET /state (camelCase JSON).
/// </summary>
public record GameStateDto(
    string Mode,
    int Money,
    int Population,
    int GameSpeed,
    int ZoomLevel,
    PositionDto CameraPosition,
    string VisualTimeOfDay,
    WeatherDto Weather
);

public record PositionDto(int X, int Y);

public record WeatherDto(
    string Condition,
    int TemperatureF,
    int WindSpeedMph,
    string WindDirection,
    int HumidityPercent
);
