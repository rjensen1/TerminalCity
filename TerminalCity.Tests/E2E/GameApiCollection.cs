namespace TerminalCity.Tests.E2E;

/// <summary>
/// Forces all E2E tests into one collection so they run sequentially and share one fixture.
/// Sequential execution avoids test interference (shared game state on the live server).
/// </summary>
[CollectionDefinition("GameApi")]
public class GameApiCollection : ICollectionFixture<GameApiFixture> { }
