namespace TerminalCity.Observability;

/// <summary>
/// A command injected via the REST API to drive game input.
/// Key names match .NET's <c>Keys</c> enum (same values <c>OnKeyPressed</c> uses).
/// </summary>
public record GameCommand(string Key);
