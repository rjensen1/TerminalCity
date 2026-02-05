# TerminalCity - Development Vision & TODO

## Project Vision

### Two City Builder Games, One Codebase

**Game 1: City Rebuilder (Post-Apocalyptic)**
- Start in an existing, ruined city
- Focus on survival, scavenging, and gradual rebuilding
- Manage scarce resources and population needs
- Repair infrastructure rather than building from scratch
- Narrative-driven scenarios with story elements

**Game 2: Traditional City Builder**
- Start with empty land
- Classic growth-focused city building
- Economic management and expansion
- Zone-based development (residential, commercial, industrial)
- Sandbox/creative mode

### Shared Core Philosophy
Both games will share:
- **Core engine** (tile system, rendering, input handling)
- **UI components** (dialogs, menus, HUD elements)
- **Base mechanics** (building placement, resource management, pathfinding)
- **Content system** (external definition files for buildings, items, scenarios)

Game-specific differences via:
- **Scenario files** defining starting conditions
- **Building/technology availability** per mode
- **Win conditions** and objectives
- **Content definitions** (rebuilder has "ruins", traditional has "zones")

---

## Common City Builder Criticisms (To Avoid)

Based on research from Cities: Skylines 2 and genre analysis:

### 1. Technical & Performance Issues
- ❌ **Poor performance** at scale ([Cities: Skylines 2 struggles with 150K+ citizens](https://www.gamesradar.com/cities-skylines-2-review/))
- ❌ **Unoptimized rendering** (CS2 renders character teeth at max quality always)
- ❌ **Bad pathfinding** (cars doing U-turns on highways)
- ✅ **Our approach**: ASCII rendering = minimal performance overhead, simple pathfinding

### 2. Excessive Micromanagement
- ❌ **Tedious clicking** ([Dawn of Man praised for automation](https://www.pcgamer.com/dawn-of-man-automation-micromanagement/))
- ❌ **"Cursor collecting"** - manually collecting factory output
- ✅ **Our approach**: Automation rules, smart defaults, macro-level control

### 3. Lack of Challenge/Depth
- ❌ **Too easy once established** (CS2: "just build two industries of any type")
- ❌ **Optimal strategies too obvious**
- ❌ **No interesting feedback loops** ([Citalis criticism](https://pcgamesnnews.wordpress.com/2016/11/22/review-citalis/))
- ✅ **Our approach**:
  - Rebuilder mode: constant resource scarcity
  - Traditional mode: escalating challenges, disasters, political pressure

### 4. Shallow Simulation
- ❌ **Cosmetic rather than systemic** ([academic criticism](https://domford.net/conference-talk/ford-the-city-2023/))
- ❌ **Stats without meaning** (happiness bars that don't affect anything)
- ✅ **Our approach**: Emergent gameplay from simple rules, meaningful citizen needs

### 5. Poor Feedback
- ❌ **Unclear why things are failing**
- ❌ **Hidden mechanics** (players don't understand cause/effect)
- ✅ **Our approach**: Clear status messages, visible supply chains, debug info available

### 6. DLC Bloat & Incomplete Launch
- ❌ **$89.99 Ultimate Edition before game is polished**
- ❌ **Features held back for DLC**
- ✅ **Our approach**: Complete game first, free content via modding, no DLC plans

### 7. Boring Late Game
- ❌ **Nothing to do once city is established**
- ❌ **No ongoing challenges**
- ✅ **Our approach**:
  - Rebuilder: narrative scenarios with endings
  - Traditional: sandbox creativity, optional challenges

---

## Architectural Decisions

### Why This Architecture Supports Both Games

#### 1. **Scenario-Based System**
```
scenarios/
├── rebuilder/
│   ├── fallen_metropolis.txt
│   ├── nuclear_winter.txt
│   └── flooded_coast.txt
└── traditional/
    ├── greenfield_easy.txt
    ├── desert_challenge.txt
    └── island_sandbox.txt
```

Each scenario defines:
- Starting conditions (ruins vs empty land)
- Available buildings and technologies
- Win/lose conditions
- Population and resources
- Story events (for rebuilder mode)

#### 2. **Content-Driven Design**
Everything in external text files:
- **Buildings** (`buildings_common.txt`, `buildings_rebuilder.txt`, `buildings_traditional.txt`)
- **Items/Resources** (`items_common.txt`, `items_rebuilder.txt`)
- **Technologies** (`tech_tree_rebuilder.txt`, `tech_tree_traditional.txt`)
- **Events** (`events_rebuilder.txt` - story beats, disasters)
- **Citizens** (`citizen_types.txt` - survivors vs workers)

Benefits:
- ✅ Easy to add content without code changes
- ✅ Modding support built-in
- ✅ Balance tweaking without recompiling
- ✅ Community can contribute content
- ✅ Easy A/B testing of game mechanics

#### 3. **Shared Domain Logic**
```
Domain/
├── Core/              # Shared by both games
│   ├── GameState.cs
│   ├── Tile.cs
│   ├── Building.cs
│   ├── Citizen.cs
│   └── ResourceManager.cs
├── Rebuilder/         # Rebuilder-specific
│   ├── RuinedBuilding.cs
│   ├── ScavengingSystem.cs
│   └── SurvivalNeeds.cs
└── Traditional/       # Traditional-specific
    ├── Zone.cs
    ├── GrowthSystem.cs
    └── EconomicModel.cs
```

#### 4. **Shared UI Components**
```
UI/
├── Dialog.cs          # Modal dialogs (✅ Already implemented!)
├── Menu.cs            # Menu system
├── HUD.cs             # Status displays
├── BuildingMenu.cs    # Building placement UI
└── InfoPanel.cs       # Building/citizen info
```

Same controls, same visual style, different content.

#### 5. **Mode Selection**
```csharp
public enum GameType
{
    Rebuilder,
    Traditional
}

// Selected at game start, loads appropriate content and systems
gameState.Initialize(GameType.Rebuilder, "fallen_metropolis");
```

---

## Development Roadmap

### Phase 0: Shared Foundation (Current)
- [x] Basic SadConsole setup
- [x] Splash screen
- [x] Camera controls
- [x] Tile rendering system
- [x] Dialog system (modal popups)
- [x] GameState architecture
- [ ] **Next**: Building placement system (works for both games)

### Phase 1: Core Mechanics (Shared)
- [ ] Building placement with collision detection
- [ ] Resource/inventory system
- [ ] Time progression (day/night, seasons)
- [ ] Citizen/population system
- [ ] Pathfinding basics
- [ ] Save/load system
- [ ] Content parsing (buildings, items, scenarios from .txt files)

### Phase 2: Rebuilder Mode (MVP)
- [ ] Ruined building types (partially destroyed, repairable)
- [ ] Scavenging mechanics
- [ ] Survival needs (food, water, shelter, safety)
- [ ] Simple scenario: "Survive 30 days"
- [ ] Story event system
- [ ] Win/lose conditions

### Phase 3: Traditional Mode (MVP)
- [ ] Zoning system (R/C/I)
- [ ] Economic model (tax, budget, income/expenses)
- [ ] Building growth/upgrade system
- [ ] Blank slate starting conditions
- [ ] Sandbox mode (no win condition)

### Phase 4: Polish & Balance
- [ ] Tutorial scenarios for both modes
- [ ] Sound effects (optional, ASCII games work well silent)
- [ ] Multiple scenarios per mode
- [ ] Balance testing
- [ ] Performance optimization

### Phase 5: Modding & Community
- [ ] Documentation for content file formats
- [ ] Example mods/scenarios
- [ ] Workshop/sharing system (optional)

---

## Technical Decisions for Code Reuse

### 1. **Strategy Pattern for Game Mode**
```csharp
interface IGameMode
{
    void Initialize(Scenario scenario);
    void Update(TimeSpan deltaTime);
    void HandleInput(Keyboard keyboard);
    List<Building> GetAvailableBuildings();
    bool CheckWinCondition();
    bool CheckLoseCondition();
}

class RebuilderMode : IGameMode { /* ... */ }
class TraditionalMode : IGameMode { /* ... */ }
```

### 2. **Composition Over Inheritance**
Buildings don't need separate classes for each game mode:
```csharp
public class Building
{
    public string Id { get; set; }
    public BuildingState State { get; set; }  // Ruined, Damaged, Intact
    public Dictionary<string, int> RepairCosts { get; set; }  // For rebuilder
    public ZoneType? RequiresZone { get; set; }  // For traditional
    // ... shared properties
}
```

### 3. **Data-Driven Everything**
```
# buildings_rebuilder.txt
[ruined_apartment]
name: Ruined Apartment Building
type: residential
width: 4
height: 6
capacity: 0
state: ruined
repair_costs: concrete=50, steel=30, wood=20
repair_time: 10
repaired_capacity: 12
provides: shelter

# buildings_traditional.txt
[apartment]
name: Apartment Building
type: residential
width: 4
height: 6
capacity: 12
cost: money=5000
requires_zone: residential
provides: housing
```

### 4. **Scenario System**
```
# scenarios/rebuilder/fallen_metropolis.txt
[scenario]
name: The Fallen Metropolis
game_type: rebuilder
description: A once-great city lies in ruins after the collapse...

[starting_conditions]
population: 50
resources: food=100, water=50, materials=20
buildings: ruined_apartment=5, ruined_warehouse=2, ruined_hospital=1
time: day
season: summer

[objectives]
primary: Reach population of 200
secondary: Repair the hospital, Establish water purification
time_limit: 100 days

[events]
day_10: raider_attack
day_25: trade_caravan_arrives
day_50: harsh_winter_begins
```

---

## Content File Benefits

### For Development
- ✅ **Fast iteration** - tweak balance without recompiling
- ✅ **Easy testing** - create test scenarios quickly
- ✅ **Parallel work** - designers create content while programmers code systems
- ✅ **Version control friendly** - text diffs show exactly what changed

### For Players
- ✅ **Transparent mechanics** - players can read the files to understand the game
- ✅ **Easy modding** - no programming knowledge required
- ✅ **Community scenarios** - players share custom challenges
- ✅ **Learning tool** - see how game is structured

### For Both Games
- ✅ **Shared format** - buildings work in both games with different properties
- ✅ **Mix and match** - could even have hybrid scenarios
- ✅ **DRY principle** - common buildings defined once

---

## Key Design Principles

### 1. **ASCII Aesthetic = Performance + Charm**
- No 3D rendering overhead
- Large cities won't slow down
- Clean, readable interface
- Nostalgic appeal
- Easy to mod (just text files)

### 2. **Meaningful Choices**
- No obvious optimal strategies
- Trade-offs in every decision
- Multiple paths to success
- Emergent gameplay from simple rules

### 3. **Respect Player Time**
- Automation where appropriate
- Clear feedback on problems
- Save anywhere, anytime
- Fast-forward time option

### 4. **Depth Through Simplicity**
- Simple mechanics that interact in complex ways
- Dwarf Fortress-style emergent storytelling
- Systems-driven rather than scripted

### 5. **Content Is King**
- Easy to create new buildings, scenarios, challenges
- Community can expand the game
- Both games benefit from shared content library

---

## Questions to Answer

### Design Questions
- [ ] How does citizen happiness work? (simple mood vs detailed needs)
- [ ] Pathfinding: A* or flow fields?
- [ ] Time scale: real-time vs turn-based vs hybrid?
- [ ] Can buildings be rotated? (affects ASCII rendering)
- [ ] How granular are resources? (abstract vs detailed supply chains)

### Technical Questions
- [ ] Max city size? (performance target)
- [ ] Multiplayer ever? (affects architecture)
- [ ] Mobile/console ports? (affects controls)
- [ ] Localization support? (affects text file parsing)

### Scope Questions
- [ ] Release one game mode first, or both simultaneously?
- [ ] How many scenarios for initial release?
- [ ] Audio? (sound effects, music)
- [ ] Graphics options? (different tilesets/fonts)

---

## Current Status

### Completed ✅
- SadConsole framework setup
- Title screen with ASCII art
- Camera movement and viewport rendering
- Modal dialog system with box-drawing borders
- Unit tests for dialog system
- Font test mode (verify extended ASCII support)

### In Progress 🚧
- Building placement system
- Content file format design

### Next Up 📋
1. Define building file format
2. Implement BuildingParser (similar to roguelike)
3. Create BuildingMenu UI
4. Implement ghost building placement
5. Test with sample buildings (ruins for rebuilder, zones for traditional)

---

## Resources & Inspiration

### Games to Study
- **Dwarf Fortress** - Emergent storytelling, ASCII, depth
- **Frostpunk** - Survival city builder with narrative
- **Rimworld** - Storytelling through systems
- **Against the Storm** - Roguelike city builder
- **Dawn of Man** - Praised automation system

### Technical Resources
- [SadConsole Documentation](https://sadconsole.com/)
- [GoRogue - Roguelike utilities](https://github.com/Chris3606/GoRogue)
- ASCII Art generators for UI elements

### Research
- [The Problem with City-Building Games](https://youngryan.com/2014/03/the-problem-with-city-building-games/)
- [The city, according to city builders](https://domford.net/conference-talk/ford-the-city-2023/)
- [Cities: Skylines 2 criticism](https://www.gamesradar.com/cities-skylines-2-review/)

---

## Long-Term Vision

### Year 1
- Release Rebuilder mode with 5 scenarios
- Release Traditional mode with sandbox
- Build active community
- Iterate based on feedback

### Year 2+
- Expand scenario library
- Add advanced mechanics (trade routes, diplomacy, disasters)
- Mobile version?
- Potential commercial release or keep free/open-source

### Dream Features
- Scenario editor (in-game tool to create content files)
- Steam Workshop integration
- Multiplayer co-op city building
- Procedurally generated maps
- Mod support for custom game modes

---

*"The best city builder is one you want to play for hours, not one you feel obligated to micromanage."*

---

## Notes & Ideas

*Use this section for random thoughts and ideas*

- Idea: "Story mode" in rebuilder where you play as a specific character/leader
- Idea: Random events in traditional mode (natural disasters, economic crashes)
- Idea: Photography mode to capture cool city views
- Idea: Time-lapse replay of city growth
- Question: Should buildings have "health" that decays over time?
- Question: How do we handle multi-tile buildings in ASCII? (use different chars for different parts)
