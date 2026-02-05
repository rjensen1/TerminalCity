# TerminalCity

A terminal-based ASCII city builder built with SadConsole and MonoGame.

## About

TerminalCity is a city building simulation game rendered entirely with ASCII/ANSI characters. Build your dream city from the ground up, manage resources, zone land for residential, commercial, and industrial use, and watch your population grow!

## Features

- **ASCII Graphics**: Classic terminal aesthetics with modern SadConsole rendering
- **City Building**: Place buildings, roads, and zones
- **Resource Management**: Balance your budget and manage city resources
- **Camera Controls**: Pan around your growing city
- **Splash Screen**: Beautiful ASCII art title screen

## Controls

- **Arrow Keys / WASD**: Move camera
- **ESC**: Return to title screen / Quit game
- **Enter / Space**: Start game (from title screen)

## Planned Features

- Place buildings and roads
- Zone residential, commercial, and industrial areas
- Dynamic population simulation
- Budget and tax management
- Time progression and seasonal cycles
- Random events and challenges

## Technology

- **SadConsole 10.8.0**: ASCII rendering engine
- **MonoGame**: Graphics framework
- **C# / .NET 8.0**: Core language and platform
- **xUnit**: Unit testing

## Building and Running

```bash
# Clone the repository
git clone https://github.com/yourusername/TerminalCity.git

# Navigate to the project directory
cd TerminalCity

# Build the solution
dotnet build

# Run the game
cd TerminalCity
dotnet run
```

## Testing

```bash
# Run all unit tests
dotnet test
```

## Project Structure

```
TerminalCity/
├── TerminalCity/           # Main game project
│   ├── Domain/             # Game logic and domain models
│   │   ├── GameState.cs    # Core game state
│   │   ├── Building.cs     # Building definitions
│   │   └── Tile.cs         # Map tile system
│   └── Program.cs          # Main entry point and rendering
└── TerminalCity.Tests/     # Unit tests
    └── Domain/             # Domain logic tests
```

## License

MIT License - See LICENSE file for details

## Acknowledgments

- Inspired by classic city builders like SimCity and modern colony sims like Dwarf Fortress
- Built with [SadConsole](https://github.com/Thraka/SadConsole) by Thraka
- ASCII art generated with various online tools
