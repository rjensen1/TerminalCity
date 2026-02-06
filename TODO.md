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
- Zoom system (5 levels: -2 to +2, 400ft to 25ft per tile)
- Dirt road rendering with zoom-appropriate characters
- Map generation from scenario files
- Time progression system with 4 speed levels (starts paused)
- Date/time display with auto-pause on interaction (planned)

### In Progress 🚧
- Building placement system
- Content file format design

### Next Up 📋
1. Define building file format
2. Implement BuildingParser (similar to roguelike)
3. Create BuildingMenu UI
4. Implement ghost building placement
5. Test with sample buildings (ruins for rebuilder, zones for traditional)

### Future Enhancements 🔮

#### Era-Based Time Scaling
**Problem:** Historical scenarios (1700s, 1800s) should have slower pace of change than modern/future scenarios (1950s, 2050s).

**Solution:** Adjust time progression based on scenario era:
- **Pre-1800:** Slower speeds (e.g., Speed 1 = 1 day/tick instead of 8 hours/tick)
- **1800-1950:** Moderate speeds (current implementation)
- **1950+:** Faster speeds available (e.g., Speed 4 = 30 days/tick instead of 7)

**Implementation:**
- Add `era_time_scale` modifier to scenario files
- Multiply tick advancement by era modifier
- UI could show "Era-adjusted speed" or just keep it transparent

**Benefit:** Players in 1700s scenarios won't feel stuck in slow-motion for decades of game time, while modern scenarios can fast-forward through decades quickly.

#### Background Tech Tree & Historical Progression

**Concept:** A tech tree that runs in the background (no micromanagement) representing historical/societal changes that affect city building.

**Types of progression:**
- **Technology:** Electricity, automobiles, air conditioning, internet, renewable energy
- **Infrastructure:** Paved roads, sewers, highways, mass transit, fiber optic
- **Policy:** Zoning laws, HOAs, building codes, environmental regulations, tax structures
- **Social:** Suburbanization, car dependency, telecommuting, work-from-home culture
- **Economic:** FHA loans, mortgage systems, tax incentives, developer financing

**Example - HOAs:**
- Not common in 1950s America
- Become widespread in 1970s+
- Changes how residential development works (private subdivisions vs public streets)
- Could unlock new building types or change mechanics

**Key principles:**
- Automatic progression (player doesn't "research")
- Viewable (can see what's unlocked/coming)
- Era-appropriate (techs unlock when historically accurate)
- Some conditional (e.g., requires population threshold or nearby parent city)
- Affects available buildings, mechanics, and gameplay systems

**Benefits:**
- Historical accuracy without micromanagement
- Gradual unlocking keeps gameplay fresh across decades
- Educational (shows how cities actually evolved)
- Scenario variety (1700s plays different than 2050s)

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

## Fundamental Design Questions

### Why Does This City Exist?

**Core Principle**: Cities don't appear randomly. They form around resources, geography, or strategic locations.

#### Starting Conditions (Traditional Mode)

**Option A: Resource-Based Origins**
Start with a PRIMARY REASON for settlement:
- **Mining Town** (Gold Rush, Coal, Copper, Silver)
  - Initial: Mine entrance, 3 shacks, general store, saloon
  - Economy: Dependent on ore production
  - Risk: Mine depletion forces transition

- **Port Town** (Trade Hub, Fishing)
  - Initial: Dock, warehouse, 2 houses, fish market
  - Economy: Trade routes and fishing
  - Risk: Trade route changes, overfishing

- **Agricultural Center** (Fertile Land)
  - Initial: 3 farms, granary, general store
  - Economy: Food production and export
  - Risk: Drought, soil depletion

- **Railroad Junction** (Transportation)
  - Initial: Station, hotel, 2 houses, telegraph office
  - Economy: Passengers and freight transfer
  - Risk: Railway company builds bypass

- **Logging Town** (Timber)
  - Initial: Sawmill, 3 cabins, company store
  - Economy: Lumber production
  - Risk: Forest depletion

- **Bedroom Community** (Suburban Spillover) ⭐ NEW
  - Initial: 10 houses, elementary school, gas station, small store
  - Economy: Commuters (work in nearby city)
  - Reason: "Neighboring cities were full" (housing demand overflow)
  - Context: Proximity to major city (15-30 minutes)
  - Risk: Dependence on parent city (if it declines, you decline)
  - Challenge: Develop own identity and economy (or remain bedroom community)
  - Era: Typically 1950s+ (post-WWII suburban boom)

**Option B: Historical Progression (Like SimCity)**
- **Era 1: 1850s-1880s** (Old West, Early Industrial)
  - Buildings: Wood construction, simple infrastructure
  - Tech: Horse-drawn, gas lamps, telegraph
  - Challenges: Lawlessness, disease, fire risk

- **Era 2: 1880s-1920s** (Industrial Boom)
  - Buildings: Brick construction, factories
  - Tech: Railroads, electricity, telephones
  - Challenges: Pollution, labor unrest, immigration

- **Era 3: 1920s-1950s** (Modern Era)
  - Buildings: Steel and concrete, skyscrapers
  - Tech: Cars, radio, early computers
  - Challenges: Traffic, suburbanization

- **Era 4: 1950s-Present** (Contemporary)
  - Buildings: Modern architecture, high-rises
  - Tech: Internet, mass transit, green energy
  - Challenges: Sustainability, urban sprawl

**RECOMMENDATION**: Combine both approaches
- Start scenario specifies initial resource/reason (gold mine, port, etc.)
- Progress through eras as city grows
- Player chooses starting era (1850s vs 1950s gives different feel)

#### Starting Conditions (Rebuilder Mode)

Cities already existed for a reason - that reason might be gone or changed:
- **Fallen Mining Town**: Mine was depleted before the collapse, people stayed anyway
- **Coastal Ruins**: Port city, but trade routes are destroyed
- **Industrial Wasteland**: Factory city, but factories are now toxic ruins
- **Farm Community**: Agricultural center, but fields are overgrown

---

### Resource Management Philosophy

**Goal**: Resources matter, but aren't micromanaged

#### Good Examples to Follow
- **Dwarf Fortress**: Abstract resource tracking (count of items, not individual tracking)
- **Banished**: Resources visible in stockpiles, automatic distribution
- **Frostpunk**: Coal matters deeply but collection is automated

#### Bad Examples to Avoid
- **Cities: Skylines 2**: "Cursor collecting" - manually gathering factory output
- **Overly detailed**: Tracking individual ore nuggets or wheat stalks

#### Our Approach: "Macro Resource System"

**High-Level View**:
```
Gold Mine (Active)
├─ Production: 100 ore/month
├─ Workers: 15/20
├─ Status: Healthy
└─ Lifespan: ~15 years remaining (displayed as gauge)

When mine depletes:
├─ Production drops: 100 → 50 → 20 → 0 over 2 years
├─ Workers laid off automatically
├─ City economy affected (tax revenue drops)
└─ Player notified: "Gold mine production declining"
```

**Resource Types**:
1. **Primary Resources** (Why city exists)
   - Gold ore, coal, timber, fish, grain
   - Extracted/produced by buildings
   - Can deplete (mines run dry, forests logged out)
   - Displayed as stockpile counts

2. **Secondary Resources** (Economic)
   - Money (taxes, trade)
   - Building materials (wood, stone, brick)
   - Goods (food, clothing, tools)
   - Abstract rather than physical

3. **Intangible Resources** (Quality of Life)
   - Happiness/satisfaction
   - Safety
   - Health
   - Education
   - Entertainment

**Resource Depletion Events**:
```
Year 15: "Gold mine output declining"
Year 17: "Prospectors find no new veins"
Year 18: "Mine production at 10%"
Year 20: "Gold mine exhausted - closed"

City must adapt:
- Transition to tourism (historical mining town)
- Become bedroom community (if near other city)
- Attract new industry (manufacturing, services)
- Decline and eventual ghost town (failure condition)
```

**Player Interaction**:
- ✅ See resource stockpiles (not individual items)
- ✅ See production rates (ore/month)
- ✅ Set priorities (food before luxury goods)
- ✅ Get warnings about depletion
- ❌ Don't manually move resources
- ❌ Don't micromanage workers

---

### Who Is The Player?

**This is a CRITICAL design decision that affects:**
- What actions are available
- How the game is framed narratively
- What constraints exist
- What feedback makes sense

#### Option 1: The Mayor
**Powers**:
- Approve/deny building permits
- Set tax rates and budgets
- Pass ordinances (noise limits, pollution rules)
- Can be voted out if unpopular

**Constraints**:
- Must balance budget (can't print money)
- Must win elections (need voter approval)
- Subject to city charter/laws
- Can't directly control citizens

**Narrative Frame**: "You are the newly elected mayor of [City Name]..."

**Pros**:
- ✅ Clear role with defined powers
- ✅ Political dimension (elections, approval rating)
- ✅ Realistic constraints
- ✅ Relatable (everyone knows what mayors do)

**Cons**:
- ❌ More restrictive (can't just demolish anything)
- ❌ May be too "realistic" (less creative freedom)

#### Option 2: The Planning Commissioner
**Powers**:
- Zone land (residential, commercial, industrial)
- Approve building designs
- Plan infrastructure (roads, utilities)
- Long-term city planning

**Constraints**:
- Budget set by council (not your decision)
- Must follow zoning laws
- Can't tax or spend directly

**Narrative Frame**: "As Planning Commissioner, you shape [City Name]'s future..."

**Pros**:
- ✅ Focused on building/design (core gameplay)
- ✅ Less political drama
- ✅ More creative freedom than Mayor

**Cons**:
- ❌ Less well-known role
- ❌ Why do you control everything?

#### Option 3: "The Spirit of the City" (Abstract)
**Powers**:
- Omniscient view
- Can influence anything
- Not bound by realism

**Constraints**:
- Resources (money) still matter
- Buildings take time to construct
- Citizens have free will (can leave)

**Narrative Frame**: "Guide [City Name] to prosperity..." (no explicit role)

**Pros**:
- ✅ Maximum creative freedom
- ✅ No need to justify god powers
- ✅ Classic city builder approach (SimCity)
- ✅ Works for both traditional and rebuilder

**Cons**:
- ❌ Less personal investment
- ❌ Less narrative coherence
- ❌ No character to identify with

#### Option 4: Context-Dependent Role

**Traditional Mode**: "The Spirit of the City" (SimCity-style)
- You're an abstract planner/god
- Focus on creative building
- Sandbox feel

**Rebuilder Mode**: "The Leader" (Frostpunk-style)
- You're the elected/appointed leader of survivors
- Story-driven scenarios
- Character you can identify with
- More personal stakes ("your people" are counting on you)

**Pros**:
- ✅ Best of both worlds
- ✅ Mode-appropriate framing
- ✅ Rebuilder has more narrative weight
- ✅ Traditional has more creative freedom

**Cons**:
- ❌ Inconsistent between modes

---

### RECOMMENDATIONS

#### 1. Starting Conditions
**Traditional Mode**:
- Player chooses starting scenario:
  - "Gold Rush, 1860" - Mine + 3 shacks + general store
  - "Port Town, 1875" - Dock + warehouse + fish market
  - "Farm Settlement, 1850" - 3 farms + granary + church
  - "Railroad Junction, 1880" - Station + hotel + telegraph
- Include 5-8 houses, 2-3 businesses, 1 resource building
- Include reason for city's existence (displayed in scenario description)

**Rebuilder Mode**:
- Start with ruins of a city that once had a purpose:
  - "Fallen Gold Town" - Depleted mine, collapsed buildings
  - "Coastal Ruins" - Destroyed port, storm damage
  - "Factory Wasteland" - Abandoned industrial city
- Show what the city WAS (historical info panel)
- Resource that existed may still be partially viable

#### 2. Historical Progression
**Yes, implement era progression**:
- Start in chosen era (1850s, 1900s, 1950s, 2000s)
- Unlock buildings and tech as city grows
- Visual style changes (ASCII chars/colors for different eras)
  - 1850s: Mostly lowercase and simple chars (. for dirt)
  - 1900s: Mixed case, more varied (├┤ for structures)
  - 2000s: Full unicode box-drawing (┌┐└┘)
- Events tied to eras (gold rush, railroad boom, tech boom)

#### 3. Resources
**Abstracted but meaningful**:
- Show stockpile totals (not individual items)
- Show production rates (100 ore/month)
- Automatic distribution (no cursor-clicking)
- Depletion is real and has consequences
- Warning system ("mine has 5 years left")
- Force adaptation when resources run out

**Resource Depletion Gameplay**:
```
Phase 1 (Boom): Mine is productive, city grows
Phase 2 (Warning): "Output declining" - 2 years warning
Phase 3 (Crisis): Mine closes, unemployment rises
Phase 4 (Adaptation):
  Option A: Attract new industry → Success
  Option B: Become ghost town → Failure
  Option C: Tourism/heritage → Different success
```

#### 4. Player Role
**Recommended**: **Context-Dependent**

**Traditional Mode**: "The Planner" (abstract, SimCity-style)
- You represent the collective will of the citizens
- No explicit character, pure gameplay
- Creative sandbox with constraints (money, time)
- Status messages use passive voice: "Gold mine constructed"

**Rebuilder Mode**: "The Leader" (Frostpunk-style)
- Named character elected by survivors
- Story scenarios with personal stakes
- Active voice: "You order construction of shelter"
- Dialog from citizens: "Leader, we need food!"
- Can make moral choices (authoritarian vs democratic)

**Why this works**:
- Traditional mode = pure gameplay, creative freedom
- Rebuilder mode = story-driven, emotional investment
- Each mode gets appropriate framing
- Shared mechanics work for both

---

### Implementation Example

**Scenario File Structure**:
```
# scenarios/traditional/gold_rush_1860.txt
[scenario]
name: Gold Rush, 1860
game_type: traditional
era: 1860
player_role: abstract_planner
reason_for_city: gold_mine

[description]
Gold has been discovered in the nearby hills! Prospectors are
arriving daily, and a settlement is forming. As the town planner,
you must build a city to support the mining operation and the
fortune-seekers who flock here.

[starting_conditions]
year: 1860
population: 50
buildings: gold_mine=1, shack=5, general_store=1, saloon=1
resources: gold=0, money=5000, food=100, wood=50

[primary_resource]
type: gold
location: gold_mine
production: 100/month
depletion_rate: 5%/year
estimated_lifespan: 20 years

[objectives]
population_target: 1000
economic_goal: diversify_before_mine_depletes
optional: build_railway_connection

[historical_events]
year_2: gold_strike_bonus  # Extra production
year_5: railway_proposal     # Connect to national rail
year_10: mine_decline_warning
year_15: seek_new_industry
year_20: mine_depleted
```

**Bedroom Community Scenario**:
```
# scenarios/traditional/bedroom_community_1955.txt
[scenario]
name: Suburban Spillover, 1955
game_type: traditional
era: 1955
player_role: abstract_planner
reason_for_city: bedroom_community

[description]
The neighboring city of Metropolis is bursting at the seams.
Veterans returning from the war need affordable housing, and
this former farmland is the perfect location. Just 20 minutes
from downtown Metropolis by car, you'll create a new suburban
community for families seeking the American Dream.

[starting_conditions]
year: 1955
population: 150
buildings: house=10, elementary_school=1, gas_station=1, grocery_store=1
resources: money=15000, building_materials=200
infrastructure: 2_lane_road_to_metropolis

[parent_city]
name: Metropolis
distance: 15 miles
population: 500000
jobs_available: unlimited  # For now, commuters always find work
economic_health: 90%       # Affects your tax revenue

[economic_model]
primary_income: commuter_taxes  # 70% of residents work in Metropolis
local_jobs: 15  # Gas station + school + store
dependence_on_parent: HIGH  # If Metropolis declines, you're in trouble

[objectives]
population_target: 5000
economic_goal: develop_local_economy  # Reduce dependence on parent city
optional:
  - build_shopping_center    # Creates local jobs
  - attract_light_industry   # Diversify economy
  - establish_identity       # Cultural center, parks, unique features

[challenges]
year_5: traffic_congestion     # Rush hour to Metropolis
year_10: metropolis_recession  # Parent city struggles, affects you
year_15: shopping_center_competition  # Big mall in Metropolis hurts local stores
year_20: decision_point        # Remain bedroom community or develop independence?

[success_paths]
path_a: bedroom_community      # Accept role, optimize for commuters
  - Excellent schools
  - Low crime
  - Good roads to parent city
  - Limited local economy

path_b: independent_city       # Develop own economy
  - Attract businesses
  - Create job centers
  - Reduce commuter percentage
  - Higher risk, higher reward

path_c: balanced_suburb        # Mix of both
  - Some local jobs
  - Still tied to parent city
  - Moderate growth
```

**Key Gameplay Differences for Bedroom Community**:

1. **No Primary Resource** (like mine/port)
   - "Resource" is proximity to parent city
   - Economic engine is commuter taxes
   - Dependent on parent city's health

2. **Different Challenge Arc**:
   - Not about resource depletion
   - About identity and independence
   - Traffic management is critical
   - School quality matters (attracts families)

3. **Success Metrics**:
   - Option A: Be the BEST bedroom community (great schools, low crime, nice parks)
   - Option B: Achieve economic independence (local jobs, businesses)
   - Option C: Balanced approach

4. **Realistic Constraints**:
   - Parent city's economy affects yours
   - Competition from other suburbs
   - Need good transportation links
   - "Why live here vs other suburbs?"

5. **Modern/Relatable**:
   - Most players live in/near bedroom communities
   - Familiar challenges (traffic, sprawl, identity)
   - Not as "romantic" as gold rush, but very real

**This scenario type works perfectly for**:
- 1950s+ starting eras
- Players who want modern/familiar setting
- Less dramatic than resource towns
- Still has meaningful choices and consequences
- "This is literally how my town started"

---

## Terrain & Geography Systems

### Core Principle: Geography Constrains City Development

**Real cities are shaped by terrain** - You can't just plop buildings anywhere! Manhattan's famous "two towers" skyline exists because bedrock is close to the surface in Lower Manhattan and Midtown, but deep in between.

### Terrain Properties (Per Tile)

Each tile in the map has multiple properties that affect what can be built:

#### 1. **Bedrock Depth**
How far down until you hit solid rock:
- **Shallow** (0-10 feet) - Ideal for heavy buildings
- **Medium** (10-50 feet) - Good for most construction
- **Deep** (50-100 feet) - Difficult/expensive for large buildings
- **Very Deep** (100+ feet) - Skyscrapers need expensive deep foundations

**Building Constraints**:
```
Skyscraper (10+ stories):
- Shallow bedrock: Standard cost
- Medium bedrock: +25% cost
- Deep bedrock: +100% cost (deep pilings required)
- Very Deep: +300% cost or impossible without tech unlock

House (1-2 stories):
- Any bedrock depth: Standard cost (light building)
```

**Visual Representation**:
```
Map overlay when placing skyscraper:
. . . # # . .     . = Shallow bedrock (green) - Ideal!
. # # # # # .     # = Deep bedrock (red) - Expensive!
# # # # # # #
. # # . . . .     Lighter tiles = better for tall buildings
```

**Real-World Example**: Manhattan
- Downtown (Wall Street): Shallow bedrock → Tall buildings
- Midtown (42nd St): Shallow bedrock → Tall buildings
- Between (14th-33rd St): Deep bedrock → Historically shorter buildings
- Modern tech allows building anywhere, but at great cost

#### 2. **Soil Quality**
Affects agriculture and foundation stability:
- **Excellent** (5/5) - Rich, deep topsoil, perfect drainage
- **Good** (4/5) - Fertile soil, good for farming
- **Fair** (3/5) - Adequate soil, decent for farming
- **Poor** (2/5) - Sandy/clay, marginal for crops
- **Rocky** (1/5) - Thin soil, mostly rock, poor drainage

**Building Constraints**:
```
Farm:
- Excellent soil: 100% productivity
- Good soil: 80% productivity
- Fair soil: 60% productivity
- Poor soil: 30% productivity
- Rocky soil: Cannot build farm

Large Building (Factory, Mall):
- Excellent/Good: Standard cost (stable foundation)
- Fair: +10% cost (some foundation work)
- Poor: +30% cost (significant foundation work)
- Rocky: +50% cost (blasting/grading required)
```

**Visual Representation**:
```
Soil quality map:
█ █ ░ ░ ░ ▒ ▒     █ = Excellent (dark green) - Prime farmland
█ ░ ░ ░ ▒ ▒ ░     ░ = Good (green) - Good farmland
░ ░ ░ ▒ ▒ ░ ░     ▒ = Fair/Poor (yellow/brown) - Marginal
░ ░ ▒ ░ ░ ░ ░     · = Rocky (gray) - No farming
```

#### 3. **Water Table Depth**
Affects wells, basements, and drainage:
- **High** - Water <10 feet down (good for wells, bad for basements)
- **Medium** - Water 10-50 feet (balanced)
- **Low** - Water 50-100 feet (need deep wells)
- **Very Low** - Water 100+ feet (expensive wells or impossible)

**Building Constraints**:
```
Well:
- High water table: Cheap, easy
- Medium: Standard cost
- Low: +50% cost (deeper drilling)
- Very Low: +200% cost or impossible

Building with Basement:
- High water table: +50% cost (waterproofing, pumps)
- Medium: Standard cost
- Low/Very Low: -10% cost (naturally dry)
```

#### 4. **Elevation & Slope**
Affects building placement and roads:
- **Flat** (0-5% grade) - Ideal for everything
- **Gentle** (5-15% grade) - Fine for buildings, roads more expensive
- **Steep** (15-30% grade) - Difficult, terracing required
- **Cliff** (30%+ grade) - Impossible without major earthworks

**Building Constraints**:
```
House:
- Flat: Standard cost
- Gentle slope: +15% cost (foundation work)
- Steep slope: +50% cost (terracing, retaining walls)
- Cliff: Impossible (or +300% with "hillside construction" tech)

Farm:
- Flat/Gentle: Standard
- Steep/Cliff: Impossible (soil erosion, can't use equipment)

Road:
- Flat: $100/tile
- Gentle: $150/tile
- Steep: $300/tile (switchbacks, grading)
- Cliff: $1000/tile (major cutting/filling)
```

**Visual Representation**:
```
Elevation map (ASCII topographic):
≈ ≈ ≈ ≈ ≈ ≈ ≈     ≈ = Water (blue)
░ ░ . . ░ ░ ≈     ░ = Flat land (green)
. . · · . . ░     . = Gentle slope (light green)
· · ▒ ▒ · · .     · = Steep slope (brown)
▒ ▒ █ █ ▒ ▒ ·     ▒ = Very steep (dark brown)
                  █ = Cliff/mountain (gray)
```

#### 5. **Mineral Deposits** (Underground Resources)

**Core Principle**: Resources exist in the ground, but you don't know where until you survey/prospect.

**Deposit Types**:
- **Gold** - Precious metal, high value, forms veins
- **Silver** - Precious metal, moderate value
- **Copper** - Industrial metal, electrical applications (1900s+)
- **Iron** - Essential for tools, construction, railroads
- **Coal** - Fuel for power plants, industry
- **Oil** - Later eras (1900s+), requires drilling technology
- **Stone/Granite** - Building material, quarries
- **Clay** - Bricks, pottery
- **Limestone** - Cement production

**Discovery System**:
```
Initial State: All deposits HIDDEN
- Map shows no mineral information
- Player must survey to find deposits

Survey Methods:
1. Early Era (1850s-1880s):
   - Hire prospector (costs $500)
   - Surveys 20x20 area over 6 months
   - Success rate: 60% (might miss deposits)
   - Reveals: "Gold vein (small)" or "No ore found"

2. Industrial Era (1880s-1950s):
   - Geological survey team (costs $2000)
   - Surveys 50x50 area over 3 months
   - Success rate: 85%
   - Reveals: "Gold vein (medium, est. 5000 oz)"

3. Modern Era (1950s+):
   - Seismic/satellite survey (costs $5000)
   - Surveys 100x100 area instantly
   - Success rate: 95%
   - Reveals: "Gold vein (large, est. 12000 oz, depth 45ft)"
```

**Visual Representation**:
```
Before survey:            After survey:
. . . . . . .             . . . . . . .
. ? ? ? ? . .             . . ◊ ◊ . . .  ◊ = Gold deposit (found!)
. ? ? ? ? . .             . . ◊ ◊ . . .
. . . . . . .             . . . . . . .

Note: ? = Unsurveyed area (you don't see this)
      Surveyed areas show deposits or remain empty
```

**Deposit Properties**:
```
[gold_deposit]
type: gold
size: medium
estimated_yield: 5000 oz
depth: 35 feet
quality: high       # High quality = easier/cheaper to extract

[coal_deposit]
type: coal
size: large
estimated_yield: 50000 tons
depth: 120 feet
quality: medium     # Seam thickness, purity
```

**Depletion Tracking**:
- Initial estimate (may be wrong by ±30%)
- Production tracked: "3500 oz extracted / ~5000 oz estimated"
- Warning when 80% depleted: "Gold mine nearing exhaustion"
- Possibility of discovery: "New vein found! +2000 oz estimate"

#### 6. **Water Bodies** (Surface Water)

**Core Principle**: Water is essential for cities. Location relative to water determines city viability.

**River Systems**:
```
Properties:
- Width: 1-5 tiles
- Flow direction: North, South, East, West
- Flow rate: Slow, Moderate, Fast
- Navigability: Small boat, Barge, Ship
- Flooding: None, Seasonal, Severe

Gameplay Effects:
- Water source (infinite, no wells needed nearby)
- Transportation (if navigable: trade bonus)
- Fishing (food production)
- Power (water mill, later hydroelectric dam)
- Flooding risk (expensive to build near, needs levees)
- Natural boundary (limits expansion)

Visual:
≈≈≈≈≈≈≈   Small river (1-2 tiles wide)
≈≈≈≈≈≈≈   Flows north/south
≈≈≈≈≈≈≈

███████   Large river (5+ tiles wide)
███████   Dark blue = deep water
███████   Requires bridges to cross
```

**Lakes**:
```
Properties:
- Size: Small (10-50 tiles), Medium (50-200), Large (200+)
- Depth: Shallow, Medium, Deep
- Water quality: Fresh, Brackish, Salt

Gameplay Effects:
- Water source (if fresh)
- Fishing (depends on size/depth)
- Recreation (parks, beaches increase land value nearby)
- Transportation (ferries if large enough)
- Ice in winter (if climate allows)

Visual:
    ≈≈≈≈≈≈≈
  ≈≈≈≈≈≈≈≈≈
 ≈≈≈≈≈≈≈≈≈≈≈
 ≈≈≈≈≈≈≈≈≈≈≈   Lake (irregular shape)
  ≈≈≈≈≈≈≈≈≈
    ≈≈≈≈≈≈
```

**Ocean/Coast**:
```
Properties:
- Always at map edge (north, south, east, or west)
- Coast type: Sandy beach, Rocky cliff, Marsh
- Harbor quality: Excellent, Good, Poor (depth, protection)
- Tide: Low, Medium, High

Gameplay Effects:
- Port cities only (enables maritime trade)
- Fishing (major industry)
- Tourism (beaches, boardwalks)
- Naval facilities (1800s+)
- Hurricane/storm risk (climate dependent)
- Salt water (not drinkable, need wells/aqueducts)

Visual:
~~~~~~~~~~~~~~~~~    ~ = Ocean (extends to map edge)
~~~~~~~~~~~~~~~~~    Infinite water
~~~~~~~~~~~~~~~~~
############         # = Coast/beach
. . . . . . .        . = Land
```

**Streams**:
```
Properties:
- Very small (1 tile wide)
- Seasonal (may dry up in summer)
- Not navigable

Gameplay Effects:
- Minor water source
- Can be diverted for irrigation
- Easy to bridge/cross
- Can dry up (seasonal gameplay)

Visual:
. . ~ . . . .    ~ = Stream (single tile)
. . . ~ . . .    Meanders across land
. . . . ~ . .
```

**Swamps/Wetlands**:
```
Properties:
- High water table + poor drainage
- Difficult terrain
- Unhealthy (disease risk in early eras)

Gameplay Effects:
- Cannot build without draining (costs $$)
- Disease risk increases nearby
- Natural resources (peat, wild rice)
- Wildlife habitat (can be preserved as park)

Visual:
≋ ≋ ≋ ≋ ≋    ≋ = Swamp (wavy water symbol)
≋ ≋ ≋ ≋ ≋    Green-brown color
≋ ≋ ≋ ≋ ≋    Must be drained to build

After draining:
. . . . .    Becomes regular land
. . . . .    Expensive, takes time
. . . . .    Unlocked by drainage tech (1800s+)
```

---

### Climate & Weather Systems

**Core Principle**: Climate is STATIC (doesn't change turn-by-turn), but determines seasonal patterns and building viability.

#### Climate Zones (Based on Latitude)

**Latitude determines base climate**:
```
Map Generation:
- Player or scenario sets latitude: 0° (Equator) to 60° (Far North/South)
- Latitude determines climate zone
- Climate affects temperature range, seasons, precipitation
```

**Climate Zone Chart**:
```
LATITUDE  | CLIMATE ZONE      | CHARACTERISTICS
----------|-------------------|----------------------------------
0° - 15°  | Tropical          | Hot year-round, wet/dry seasons
15° - 30° | Subtropical       | Hot summers, mild winters
30° - 45° | Temperate         | Four distinct seasons
45° - 55° | Cold Temperate    | Cold winters, short summers
55° - 60° | Subarctic         | Very cold winters, brief summers
60°+      | Arctic/Tundra     | Extreme cold, permafrost
```

#### 1. **Tropical Climate** (0° - 15° Latitude)

**Temperature**:
- Average: 75-85°F year-round
- Variation: ±5°F (very stable)
- Never freezes

**Seasons**:
- **Wet Season** (6 months): Heavy rain, flooding risk
- **Dry Season** (6 months): Less rain, drought possible
- NO winter/snow

**Precipitation**: 80-200 inches/year

**Gameplay Effects**:
- No heating buildings needed (ever)
- Cooling/ventilation important
- Agriculture: Year-round growing season
- Disease: Higher risk (mosquitos, etc.)
- Construction: No winter delays
- Food: Tropical crops (bananas, coffee, sugar)

**Visual Seasons**:
```
Wet Season:           Dry Season:
≈≈≈≈≈≈≈ (flooding)    . . . . . (normal)
Trees: ████ (lush)    Trees: ░▒▓█ (still green)
```

#### 2. **Subtropical Climate** (15° - 30° Latitude)

**Temperature**:
- Summer: 80-95°F
- Winter: 50-65°F
- Rare freezing

**Seasons**:
- **Summer** (4 months): Hot, humid
- **Fall** (2 months): Pleasant
- **Winter** (4 months): Mild, some rain
- **Spring** (2 months): Pleasant
- Snow: Rare, 0-2 days/year

**Precipitation**: 30-60 inches/year

**Gameplay Effects**:
- Light heating in winter
- Cooling in summer
- Long growing season (9-10 months)
- Hurricanes possible (if coastal)
- Mild construction season impact

**Visual Seasons**:
```
Summer:      Fall:        Winter:      Spring:
████ (lush)  ▒▒▒▒ (brown) ░░░░ (bare)  ▒▓██ (buds)
Hot          Cool         Mild         Warm
```

#### 3. **Temperate Climate** (30° - 45° Latitude) - **MOST COMMON**

**Temperature**:
- Summer: 70-85°F
- Winter: 20-40°F
- Four distinct seasons

**Seasons**:
- **Spring** (3 months): 40-65°F, rainy
- **Summer** (3 months): 70-85°F, warm
- **Fall** (3 months): 45-65°F, colorful
- **Winter** (3 months): 20-40°F, snow
- Snow: 20-40 days/year

**Precipitation**: 30-50 inches/year

**Gameplay Effects**:
- Heating in winter (significant cost)
- Cooling in summer (moderate cost)
- Growing season: 6-7 months (April-October)
- Snow removal costs in winter
- Construction delays in winter
- Beautiful fall colors (tourism bonus)
- Classic four-season gameplay

**Visual Seasons**:
```
Spring:              Summer:
░▒▓█ (budding)      ████████ (lush green)
Flowers bloom       Hot, sunny

Fall:                Winter:
▓▒░░ (orange/red)   ░░░░ (bare branches)
Harvest time        ≈≈≈≈ (snow on ground)
```

**Season-Specific Gameplay** (Temperate):
```
Spring:
- Planting season begins
- Floods possible (snowmelt)
- Construction season starts
- Citizens happier (winter ended)

Summer:
- Peak tourism season
- Agriculture grows
- High power usage (AC)
- Drought possible

Fall:
- Harvest season
- Prepare for winter (stockpile coal/wood)
- Tourism (leaf peeping)
- Back to school

Winter:
- Heating costs HIGH
- Snow removal costs
- Construction halts (or +50% cost)
- Ice fishing, winter sports
- Higher death rate (elderly)
- Shorter days (more lighting needed)
```

#### 4. **Cold Temperate Climate** (45° - 55° Latitude)

**Temperature**:
- Summer: 55-70°F (short)
- Winter: 0-30°F (long, harsh)
- Long, brutal winters

**Seasons**:
- **Spring** (2 months): 35-55°F, late
- **Summer** (3 months): 55-70°F, brief
- **Fall** (2 months): 35-55°F, early
- **Winter** (5 months): 0-30°F, severe
- Snow: 60-100 days/year

**Precipitation**: 20-40 inches/year (much as snow)

**Gameplay Effects**:
- Very high heating costs
- Short growing season (4-5 months only)
- Long winter = food stockpiling critical
- Winter construction impossible without tech
- Insulation important for all buildings
- Higher building costs (must be insulated)

**Visual Seasons**:
```
Brief Spring:        Short Summer:
░░▒▓ (slow thaw)    ▓███ (green, cool)

Long Winter:
≈≈≈≈≈≈≈≈ (deep snow)
░░░░░░░░ (ice)
```

#### 5. **Subarctic/Arctic Climate** (55°+ Latitude)

**Temperature**:
- Summer: 40-60°F (very brief)
- Winter: -20 to 20°F (extreme)
- Permafrost year-round

**Seasons**:
- **Summer** (2 months): 40-60°F, midnight sun
- **Winter** (10 months): -20 to 20°F, polar night
- Snow: Year-round possible

**Precipitation**: 5-15 inches/year (mostly snow)

**Gameplay Effects**:
- Extreme heating costs
- Growing season: 2-3 months only (greenhouse required)
- All buildings need extreme insulation
- Construction season: 2 months/year
- Permafrost makes building difficult/expensive
- Food must be imported or preserved
- Extreme challenge mode

**Not recommended for beginner scenarios** - Expert mode only!

---

### Weather Patterns (Static Per Map)

**Core Principle**: Each map has a climate profile that doesn't change, but creates predictable seasonal patterns.

#### Climate Properties (Set at Map Creation)

```
[scenario_climate]
latitude: 40°                    # Determines base climate
climate_zone: temperate

# Temperature (seasonal ranges)
temperature_summer_avg: 78°F
temperature_summer_range: 65-90°F
temperature_winter_avg: 32°F
temperature_winter_range: 15-50°F

# Precipitation
precipitation_annual: 42 inches
precipitation_pattern: even      # even, summer_dry, winter_dry, monsoon
snow_days_per_year: 25

# Wind
prevailing_wind: west            # Affects pollution spread
wind_speed_avg: 8 mph
wind_speed_range: 2-25 mph
hurricane_risk: none             # none, low, medium, high (coastal only)

# Special weather
tornado_risk: low                # Great Plains = high
drought_risk: medium
flood_risk: low                  # Near rivers = higher
```

#### Seasonal Transitions (Automatic)

**Game tracks current season**:
```csharp
public enum Season
{
    Spring,
    Summer,
    Fall,
    Winter
}

public class GameState
{
    public DateTime CurrentDate { get; set; }
    public Season CurrentSeason => CalculateSeason();
    public ClimateZone ClimateZone { get; set; }

    private Season CalculateSeason()
    {
        int month = CurrentDate.Month;

        // Temperate climate
        if (ClimateZone == ClimateZone.Temperate)
        {
            return month switch
            {
                3 or 4 or 5 => Season.Spring,
                6 or 7 or 8 => Season.Summer,
                9 or 10 or 11 => Season.Fall,
                12 or 1 or 2 => Season.Winter,
                _ => Season.Summer
            };
        }

        // Tropical climate (wet/dry instead of 4 seasons)
        if (ClimateZone == ClimateZone.Tropical)
        {
            return (month >= 5 && month <= 10)
                ? Season.WetSeason
                : Season.DrySeason;
        }

        // ... other climate zones
    }
}
```

**Visual Seasonal Changes**:
```csharp
// Trees change color based on season
Color GetTreeColor(Season season, ClimateZone climate)
{
    if (climate == ClimateZone.Tropical)
        return Color.Green;  // Always green

    return season switch
    {
        Season.Spring => Color.LightGreen,
        Season.Summer => Color.DarkGreen,
        Season.Fall => Color.Orange,
        Season.Winter => Color.Gray,  // Bare branches
        _ => Color.Green
    };
}

// Ground changes based on season
char GetGroundGlyph(Season season, ClimateZone climate)
{
    if (climate == ClimateZone.Tropical)
        return '.';  // Always same

    if (season == Season.Winter && climate != ClimateZone.Subtropical)
        return '≈';  // Snow on ground

    return '.';  // Normal ground
}
```

---

### Seasonal Gameplay Effects

**Climate-Appropriate Seasonal Challenges**:

#### Temperate Climate Example

**Spring**:
- Agriculture: Planting season
- Weather: Rain (flooding possible)
- Economy: Construction resumes
- Challenge: Spring floods if near river

**Summer**:
- Agriculture: Growth phase, irrigation needed
- Weather: Hot (power for AC)
- Economy: Tourism peak
- Challenge: Drought possible, heat waves

**Fall**:
- Agriculture: Harvest season (food production peak)
- Weather: Pleasant
- Economy: Prepare for winter
- Challenge: Early cold snap can hurt crops

**Winter**:
- Agriculture: Dormant (no food production)
- Weather: Cold, snow
- Economy: Heating costs high, construction stops
- Challenge: Blizzards, ice storms, heating fuel shortages

#### Tropical Climate Example

**Wet Season**:
- Agriculture: Growth (but flooding risk)
- Weather: Heavy rain, storms
- Economy: Tourism lower
- Challenge: Flooding, disease spread

**Dry Season**:
- Agriculture: Irrigation critical
- Weather: Hot, little rain
- Economy: Tourism high
- Challenge: Drought, water shortages, fire risk

**NO winter** - Different challenges!

---

### Building Placement System

#### How It Works

**When Player Selects a Building**:
1. Enter ghost mode (see through building)
2. Hover over map
3. Each tile shows color-coded feedback:
   - **Green**: Ideal terrain, standard cost
   - **Yellow**: Possible but more expensive
   - **Red**: Impossible or prohibitively expensive

**Terrain Info Panel**:
```
╔════════════════════════════════════╗
║ BUILDING: Skyscraper (20 stories) ║
║ LOCATION: Tile (45, 67)           ║
╟────────────────────────────────────╢
║ Bedrock Depth: Deep (~75 feet)    ║
║   Cost Modifier: +100%            ║
║                                    ║
║ Soil Quality: Fair                ║
║   Cost Modifier: +10%             ║
║                                    ║
║ Water Table: Medium               ║
║   Cost Modifier: +0%              ║
║                                    ║
║ Slope: Flat                       ║
║   Cost Modifier: +0%              ║
╟────────────────────────────────────╢
║ BASE COST:    $500,000            ║
║ MODIFIERS:    +$550,000 (+110%)   ║
║ TOTAL COST:   $1,050,000          ║
╚════════════════════════════════════╝

Press Enter to confirm, ESC to cancel
```

**Multi-Tile Buildings**:
- Check ALL tiles the building occupies
- Use worst modifier from any tile
- Or require ALL tiles meet minimum standard
- Warn if building spans problematic terrain

#### Building Definitions with Terrain Requirements

**buildings_traditional.txt**:
```
[skyscraper_office]
name: Office Tower (20 stories)
type: commercial
width: 3
height: 4
cost: 500000
build_time: 24 months

# Terrain requirements
bedrock_depth:
  shallow: 1.0      # Standard cost multiplier
  medium: 1.25      # +25% cost
  deep: 2.0         # +100% cost
  very_deep: 4.0    # +300% cost or impossible

soil_quality:
  excellent: 1.0
  good: 1.0
  fair: 1.1
  poor: 1.3
  rocky: 1.5

slope:
  flat: 1.0
  gentle: 1.15
  steep: 1.5
  cliff: IMPOSSIBLE

min_requirements:
  bedrock_depth: deep_or_better  # Won't build on very_deep without tech
  slope: steep_or_flatter        # Can't build on cliffs

[farm_wheat]
name: Wheat Farm
type: agricultural
width: 6
height: 6
cost: 5000
production: wheat

# Terrain requirements
soil_quality:
  excellent: 1.0    # 100% productivity
  good: 0.8         # 80% productivity
  fair: 0.6         # 60% productivity
  poor: 0.3         # 30% productivity
  rocky: IMPOSSIBLE # Can't farm rocks

slope:
  flat: 1.0
  gentle: 0.9       # Slight erosion issues
  steep: IMPOSSIBLE # Can't farm steep hillsides
  cliff: IMPOSSIBLE

min_requirements:
  soil_quality: fair_or_better
  slope: gentle_or_flatter

[house_small]
name: Small House
type: residential
width: 2
height: 2
cost: 50000

# Terrain requirements (less strict)
bedrock_depth:
  any: 1.0          # Light building, doesn't care

soil_quality:
  excellent: 1.0
  good: 1.0
  fair: 1.05
  poor: 1.1
  rocky: 1.2        # Harder foundation, but doable

slope:
  flat: 1.0
  gentle: 1.1
  steep: 1.4
  cliff: IMPOSSIBLE

water_table:
  high: 1.2         # Basement flooding risk, waterproofing
  medium: 1.0
  low: 1.0
```

---

### Procedural Terrain Generation

**Starting Scenarios Can Specify Terrain**:

**Gold Rush Town**:
```
[terrain]
base_type: mountainous
bedrock_depth: varied (shallow in valleys, N/A on mountains)
soil_quality: poor (rocky mountain soil)
slope: varied (flat valleys, steep hills)
special_features: gold_vein_at(25,30), river_at(10-15,0-50)

# This creates interesting constraints:
# - Gold mine MUST be at (25,30) - not player's choice
# - Flat land is limited - must build in valleys
# - Farming is difficult - poor soil
# - City layout constrained by geography
```

**Bedroom Community (Flat)**:
```
[terrain]
base_type: plains
bedrock_depth: medium (consistent)
soil_quality: excellent (former farmland)
slope: flat (easy development)
special_features: highway_connection_at(50,0)

# This creates different gameplay:
# - Can build anywhere - no terrain constraints
# - But need to create interesting city layout
# - Former farms - guilt about paving over good farmland?
# - Challenge is design, not terrain
```

**Port Town**:
```
[terrain]
base_type: coastal
bedrock_depth: shallow (coastal bedrock)
soil_quality: sandy (poor for farming)
slope: flat_near_water, hills_inland
special_features: deep_water_harbor_at(0,25), cliff_coast_at(0,0-20)
water: ocean_at(0, 0-50)

# Constraints:
# - Port must be at deep water (0,25)
# - Cliffs prevent building near shore (0-20)
# - Hills inland require terracing
# - Sandy soil = poor farming, must import food
```

---

### Strategic Depth This Creates

#### 1. **City Layout Constrained by Reality**
- Can't make perfect grid (like real cities!)
- Natural areas remain (parks, green space)
- Neighborhoods develop character based on terrain
- Historical districts = old flat areas
- New districts = developed hillsides (more expensive)

#### 2. **Economic Trade-offs**
- Prime flat land with good soil: Farmland or housing?
- Shallow bedrock areas: Commercial district premium
- Hillsides: Expensive to develop, but good views
- Swampland: Cheap but requires draining

#### 3. **Technology Unlocks**
```
Year 1860: Basic construction
- Can't build on steep slopes
- Can't build on very deep bedrock
- Must avoid swamps

Year 1900: Industrial era
- Dynamite allows hillside terracing
- Steam drills allow deeper foundations
- Drainage systems allow swamp development

Year 1950: Modern construction
- Heavy equipment makes any slope buildable
- Deep pilings allow very deep bedrock
- Still expensive, but possible

Year 2000: Advanced engineering
- Can build almost anywhere
- Cost modifiers reduced
- Skyscrapers on former "impossible" terrain
```

#### 4. **Realism & Education**
- Players learn why cities look the way they do
- Understand geography's role in urban development
- Appreciate engineering challenges
- "Oh, THAT'S why downtown is here!"

---

### Visual Feedback Systems

#### Terrain Overlay Modes

**Press 'T' to cycle through overlays**:

1. **Normal View** - Buildings and terrain
2. **Bedrock Depth** - Colors show depth (dark green = shallow, red = deep)
3. **Soil Quality** - Show farming potential
4. **Elevation** - Topographic map view
5. **Slope** - Show buildability (green = flat, red = steep)
6. **Water Table** - Show well locations and basement risk

**During Building Placement**:
- Relevant overlay automatically shown
- Placing farm? Show soil quality
- Placing skyscraper? Show bedrock depth
- Placing well? Show water table

#### ASCII Terrain Visualization

**Bedrock Depth Overlay**:
```
When placing skyscraper:

Normal view:        Bedrock overlay:
. . . # # . .       █ █ █ ░ ░ █ █    █ = Shallow (ideal)
. # # # # # .  -->  █ ░ ░ ░ ░ ░ █    ░ = Medium (ok)
# # # # # # #       ░ ░ ░ ░ ░ ░ ░    ▒ = Deep (expensive)
. # # . . . .       █ ░ ░ █ █ █ █    · = Very deep (avoid)

Cursor at (3,1): Deep bedrock - Cost +100%
```

**Soil Quality (Farm Placement)**:
```
Normal view:        Soil overlay:
. . . ~ ~ . .       █ █ █ · · █ █    █ = Excellent
. . ~ ~ ~ . .  -->  █ █ ░ ░ ░ █ █    ░ = Good
~ ~ ~ ~ ~ . .       ░ ░ ░ ░ ░ ░ ░    ▒ = Fair
. . . . . . .       █ █ █ ░ ░ ░ ░    · = Poor/Rocky

Cursor at (4,1): Good soil - 80% productivity
```

---

### Implementation Priority

**Phase 1 (Core Mechanics)**:
- [ ] Tile terrain properties (bedrock, soil, slope, water table)
- [ ] Building requirements in definition files
- [ ] Placement validation (can build here?)
- [ ] Cost modifiers based on terrain
- [ ] Water bodies (rivers, lakes) as impassable terrain
- [ ] Basic climate zone selection (temperate default)

**Phase 2 (Seasonal System)**:
- [ ] Date/time tracking system
- [ ] Season calculation based on climate zone
- [ ] Visual seasonal changes (tree colors, snow on ground)
- [ ] Seasonal gameplay effects (heating costs, construction delays)
- [ ] Climate-appropriate seasons (tropical = wet/dry, temperate = 4 seasons)

**Phase 3 (Visual Feedback)**:
- [ ] Terrain overlays (press T to cycle)
- [ ] Color-coded placement preview (green/yellow/red)
- [ ] Terrain info panel during placement
- [ ] Climate info panel (current season, temperature)
- [ ] Season indicator in UI

**Phase 4 (Mineral Deposits)**:
- [ ] Hidden mineral deposits (gold, coal, iron, etc.)
- [ ] Survey/prospecting system (early/industrial/modern methods)
- [ ] Mineral deposit properties (size, depth, quality)
- [ ] Depletion tracking
- [ ] Discovery events

**Phase 5 (Procedural Generation)**:
- [ ] Generate terrain for scenarios (based on climate zone)
- [ ] Rivers, mountains, coastlines
- [ ] Procedural mineral deposits for mining towns
- [ ] Climate-appropriate vegetation
- [ ] Water table generation

**Phase 6 (Advanced Weather)**:
- [ ] Weather events (storms, droughts, blizzards)
- [ ] Seasonal disasters (hurricanes in subtropical, tornadoes in plains)
- [ ] Weather affects construction speed
- [ ] Weather affects citizen happiness

**Phase 7 (Advanced Terrain)**:
- [ ] Technology unlocks reduce terrain penalties
- [ ] Terraforming (level hills, drain swamps)
- [ ] Levees/flood control
- [ ] Irrigation systems
- [ ] Climate adaptation (greenhouses in cold climates)

---

## Natural Resources & Economic Systems

### Core Principle: Resources Gain Value Over Time

**Key Insight**: Many resources are worthless until technology or demand creates value. A deposit of oil in 1850 is just "smelly ground." By 1900, it's liquid gold.

**Demand Creation**: Building infrastructure creates demand for resources. Build a railroad? Now you need steel, coal, and timber. Build houses? Now you need lumber, brick, and copper.

---

### Primary Resources (Extracted from Nature)

#### Minerals & Metals

**Precious Metals**:
```
[gold]
era_discovered: Ancient (always known)
primary_use_1850s: Currency, jewelry
primary_use_1900s: Currency, jewelry, dentistry
primary_use_1950s: Electronics, aerospace (limited)
primary_use_2000s: Electronics, computers, jewelry
extraction: Placer mining → Hard rock mining → Cyanide leaching
value_trend: High → Stable → Very High (industrial demand)
depletion: Fast (typically 10-30 years)

[silver]
era_discovered: Ancient
primary_use_1850s: Currency, jewelry, mirrors
primary_use_1900s: Photography, medicine
primary_use_1950s: Electronics, batteries
primary_use_2000s: Solar panels, electronics
extraction: Similar to gold, often co-located
value_trend: Moderate → Increasing (industrial)
depletion: Moderate (20-50 years)
```

**Industrial Metals**:
```
[iron]
era_discovered: Ancient (known, but limited use)
primary_use_1850s: Tools, wagon parts, limited construction
primary_use_1900s: RAILROADS, steel production, construction
primary_use_1950s: Everything (vehicles, buildings, appliances)
primary_use_2000s: Still essential, some replacement by aluminum
extraction: Open pit mining → Shaft mining
technology_unlock: Bessemer process (1860s) makes steel cheap
demand_spike: 1880s railroad boom = MASSIVE demand increase
value_trend: Low → HUGE SPIKE (1880s) → Stable High
note: "Useless ore" → "Most valuable resource" in 30 years!

[copper]
era_discovered: Ancient
primary_use_1850s: Pots, coins, limited
primary_use_1900s: ELECTRICAL WIRING (huge demand spike!)
primary_use_1950s: Wiring, plumbing, electronics
primary_use_2000s: Electronics, renewable energy
extraction: Open pit, smelting required
technology_unlock: Electricity (1880s) creates demand
demand_spike: Electrification of cities (1890s-1920s)
value_trend: Low → SPIKE (electricity) → High
note: Copper worthless until electricity arrives!

[aluminum]
era_discovered: 1825 (isolated), but expensive
primary_use_1850s: NONE (more expensive than gold!)
primary_use_1900s: Limited (still expensive until 1886)
primary_use_1950s: Aircraft, vehicles, construction
primary_use_2000s: Cans, vehicles, construction, electronics
extraction: Bauxite mining + electrolysis (needs electricity!)
technology_unlock: Hall-Héroult process (1886) makes cheap
value_trend: Extremely High → Crash (cheap) → Stable
note: From luxury metal to commodity in decades

[lead]
era_discovered: Ancient
primary_use_1850s: Bullets, pipes, paint
primary_use_1900s: Batteries, pipes, paint, gasoline additive
primary_use_1950s: Batteries, radiation shielding
primary_use_2000s: Batteries (declining due to toxicity)
value_trend: Moderate → Declining (health concerns)
note: Value DECREASES as health risks discovered

[zinc]
era_discovered: Ancient (as alloy)
primary_use_1850s: Brass, galvanizing (rust prevention)
primary_use_1900s: Galvanizing, batteries
primary_use_1950s: Galvanizing, die-casting
primary_use_2000s: Galvanizing, batteries
value_trend: Low → Moderate (industrial)

[tin]
era_discovered: Ancient
primary_use_1850s: Tinplate, pewter, bronze
primary_use_1900s: Tin cans (food preservation!)
primary_use_1950s: Tin cans, solder
primary_use_2000s: Solder, specialty alloys
demand_spike: 1890s+ (canned food revolution)
value_trend: Moderate → High (canning) → Moderate
```

**Rare/Specialty Metals** (Later Eras):
```
[uranium]
era_discovered: 1789 (isolated), 1938 (fission discovered)
primary_use_1850s: NONE (unknown use)
primary_use_1900s: Glass coloring (minimal)
primary_use_1950s: NUCLEAR WEAPONS, nuclear power
primary_use_2000s: Nuclear power, medical
technology_unlock: Nuclear fission (1938)
value_trend: Worthless → EXTREMELY HIGH (1940s+)
note: Most dramatic value change - from nothing to strategic resource

[silicon]
era_discovered: Ancient (as silica/sand), 1824 (pure)
primary_use_1850s: Glass, sand
primary_use_1900s: Glass, steel alloy
primary_use_1950s: Transistors (beginning of revolution)
primary_use_2000s: COMPUTERS, solar panels, electronics
technology_unlock: Transistor (1947), integrated circuit (1958)
value_trend: Worthless (sand!) → EXTREMELY HIGH (tech boom)
note: Sand → Most valuable material by weight (computer chips)

[rare_earth_elements]
era_discovered: 1800s-1900s (various)
primary_use_1850s: NONE
primary_use_1900s: Limited scientific
primary_use_1950s: Limited (magnets, glass)
primary_use_2000s: ESSENTIAL (phones, computers, wind turbines, EVs)
technology_unlock: Modern electronics (1990s+)
value_trend: Worthless → Strategic (21st century)
```

#### Energy Resources

```
[coal]
era_discovered: Ancient (known, limited use)
primary_use_1850s: Home heating, early steam engines
primary_use_1900s: RAILROADS, steam power, steel production
primary_use_1950s: Electricity generation, steel
primary_use_2000s: Electricity (declining), steel
demand_spike: 1860s-1920s (railroad + industrial boom)
value_trend: Low → VERY HIGH → High → Declining (climate concerns)
depletion: Slow (100+ years typically)
varieties:
  - Anthracite (hard coal): Best for heating, rare
  - Bituminous (soft coal): Common, good for power
  - Lignite (brown coal): Low quality, cheap

[oil/petroleum]
era_discovered: Ancient (known as seeps), 1859 (first well)
primary_use_1850s: Lamp fuel (kerosene), grease
primary_use_1900s: KEROSENE, gasoline (automobile boom!)
primary_use_1950s: EVERYTHING (gas, diesel, plastics, chemicals)
primary_use_2000s: Transportation, plastics, chemicals
technology_unlock: Internal combustion engine (1880s-1900s)
demand_spike: 1910s-1920s (automobile adoption)
value_trend: Worthless → Moderate → EXTREMELY HIGH
note: Ground that "smelled bad" became most valuable land!
varieties:
  - Light crude: Easy to refine, high value
  - Heavy crude: Harder to refine, lower value
  - Tar sands: Very difficult, only viable when prices high

[natural_gas]
era_discovered: Ancient (burning seeps)
primary_use_1850s: Limited (early gas lights in cities)
primary_use_1900s: Lighting, heating (if available)
primary_use_1950s: Heating, cooking, some power
primary_use_2000s: Heating, power generation, petrochemicals
technology_unlock: Pipeline technology (1890s+)
value_trend: Worthless (often vented/burned at oil wells) → Moderate → High
note: Considered waste product of oil drilling until pipelines!

[wood/timber]
era_discovered: Always (universal)
primary_use_1850s: Construction, fuel, everything
primary_use_1900s: Construction, paper, fuel (declining)
primary_use_1950s: Construction, paper, furniture
primary_use_2000s: Construction, paper, furniture, biofuel
technology_unlock: Sawmills (efficiency), paper mills
value_trend: High → Declining (coal/oil replace fuel) → Stable
depletion: Fast (forests logged out in 20-40 years)
renewal: Possible (tree farms, sustainable forestry)
varieties:
  - Hardwood (oak, maple): Furniture, flooring, high value
  - Softwood (pine, spruce): Construction, paper, common
  - Old growth: Most valuable, rarely renewable

[peat]
era_discovered: Ancient
primary_use_1850s: Fuel (Ireland, Netherlands)
primary_use_1900s: Fuel, soil amendment
primary_use_1950s: Horticulture, limited fuel
primary_use_2000s: Horticulture (declining, environmental concerns)
value_trend: Moderate → Low (better fuels available)
note: Only valuable where coal/wood scarce
```

#### Agricultural Resources

```
[fertile_soil]
era_discovered: Always (obvious)
primary_use_1850s: FARMLAND (food production)
primary_use_1900s: Farmland, but mechanization increases yield
primary_use_1950s: Intensive farming, chemical fertilizers
primary_use_2000s: Farming (with concerns about depletion)
technology_unlock: Crop rotation, fertilizer, tractors
value_trend: High → Very High (population growth)
depletion: Slow (soil erosion over decades/centuries)
note: Never becomes less valuable - always need food!

[wheat]
climate: Temperate (prefers cold winters, warm summers)
growing_season: Spring or winter planting
yield_factors: Soil quality, rainfall, temperature
use: Bread, pasta, staple food
era_value: Always high (staple crop)

[corn/maize]
climate: Warm temperate to subtropical
growing_season: Late spring to fall
yield_factors: Water, heat, soil
use: Food, animal feed, ethanol (2000s+)
era_value: Moderate → High (industrial uses in modern era)

[cotton]
climate: Hot, humid (subtropical to temperate)
growing_season: Long, hot season
yield_factors: Water, heat, soil
use: Textiles, clothing
era_value: VERY HIGH (1850s-1950s) → High (synthetic competition)
note: "King Cotton" drove much of 1800s Southern economy

[tobacco]
climate: Warm temperate
growing_season: Spring to fall
use: Smoking, chewing
era_value: Very High (1850s-1950s) → Declining (health concerns)
note: Like lead, value drops as health risks known

[sugar_cane] (tropical) / [sugar_beets] (temperate)
climate: Tropical (cane) or temperate (beets)
use: Sugar, ethanol (modern)
era_value: High → Very High (always valuable)
note: Sugar cultivation drives tropical settlement

[coffee] (tropical)
climate: Tropical highlands
use: Beverage
era_value: High → Very High (global commodity)

[rubber] (natural)
climate: Tropical rainforest
use: Tires, waterproofing, seals
era_value: Low → VERY HIGH (1890s-1950s) → Moderate (synthetic)
technology_unlock: Vulcanization (1839), automobile (1900s)
demand_spike: Automobile boom creates rubber rush!
note: Amazon rubber boom (1879-1912) then crash (synthetic rubber)
```

#### Water Resources

```
[fresh_water]
era_discovered: Always (essential)
primary_use_1850s: Drinking, irrigation, power (mills)
primary_use_1900s: Drinking, irrigation, industrial processes
primary_use_1950s: All above + cooling (power plants)
primary_use_2000s: All above + high tech manufacturing
value_trend: High → VERY HIGH (population + industry)
depletion: Possible (aquifer depletion, pollution)
note: Never becomes less valuable, scarcity increases value

[fish/fishing_grounds]
location: Rivers, lakes, oceans
varieties: Freshwater, saltwater, shellfish
value_trend: High → Declining (overfishing)
depletion: Fast (20-50 years of heavy fishing)
renewal: Possible (fishing limits, aquaculture)
```

---

### Secondary Resources (Processed Materials)

```
[steel]
made_from: Iron + coal (coke) + limestone
era_available: 1860s+ (Bessemer process)
primary_use: Construction, railroads, machinery, vehicles
demand_creation:
  - Railroad construction → Need steel for rails
  - Skyscraper construction → Need steel frame
  - Ship building → Need steel plates
  - Auto industry → Need steel for cars
note: Steel mill is "demand multiplier" - creates jobs AND demand

[brick]
made_from: Clay + fire (kilns)
era_available: Ancient, but industrial production 1800s+
primary_use: Construction (buildings, chimneys)
demand_creation: Building houses → Need bricks
note: Local resource (heavy, expensive to transport)

[cement/concrete]
made_from: Limestone + clay + water
era_available: Ancient (Roman), modern Portland cement (1824)
primary_use: Construction, roads, dams
demand_creation:
  - Road building → Need concrete
  - Skyscraper foundations → Need concrete
value_trend: Low → High (modern construction essential)

[glass]
made_from: Sand (silica) + soda ash + limestone
era_available: Ancient, but cheap flat glass (1900s)
primary_use: Windows, bottles, containers
demand_creation: Building with windows → Need glass
technology_unlock: Float glass process (1952) = cheap flat glass

[paper]
made_from: Wood pulp (or rags in early era)
era_available: Ancient (rag paper), 1800s+ (wood pulp)
primary_use: Books, newspapers, packaging
demand_creation:
  - Printing press → Need paper
  - Packaging → Need paper
  - Office buildings → Need paper
value_trend: High (expensive) → Low (cheap) → Moderate

[plastics]
made_from: Oil/petroleum (petrochemicals)
era_available: 1900s (Bakelite), 1950s+ (modern plastics)
primary_use: EVERYTHING (packaging, construction, products)
demand_creation: Plastic packaging industry creates oil demand
note: Oil value increases dramatically with plastics industry

[electricity]
made_from: Coal/oil/gas/hydro/nuclear/solar/wind
era_available: 1880s+ (commercial power)
primary_use: Lighting, motors, appliances, everything
demand_creation:
  - Electric lights → Need copper wire + power plant
  - Appliances → Need power plant expansion
  - Computers → Need massive power
technology_unlock: Dynamo (1870s), power distribution (1880s)
note: "Resource" that's produced, not extracted
```

---

### Demand Creation Chains (Building → Resource Demand)

**Example 1: Railroad Construction** (1860s-1920s)
```
Build Railroad
├─> Need STEEL for rails (high demand)
│   └─> Need IRON ore (mine/import)
│   └─> Need COAL for steel production
│   └─> Need LIMESTONE for smelting
├─> Need TIMBER for railroad ties (high demand)
│   └─> Need LOGGING operations
├─> Need LABOR (construction workers)
└─> RESULT: Railroad towns boom, resource towns boom

Railroad Operation (ongoing)
├─> Need COAL for steam engines (1850s-1950s)
│   └─> Coal towns along route prosper
├─> Need WATER for boilers
└─> Creates trade routes (now goods can move cheap)
```

**Example 2: Residential Housing** (any era)
```
Build House (1850s)
├─> Need TIMBER (framing, floors)
├─> Need STONE or BRICK (foundation, chimney)
├─> Need GLASS (windows, minimal)
├─> Need NAILS (iron)
└─> RESULT: Lumber mill, quarry, blacksmith all get business

Build House (1920s)
├─> Need LUMBER (framing)
├─> Need BRICK (exterior)
├─> Need GLASS (more windows)
├─> Need COPPER (electrical wiring!) - NEW DEMAND
├─> Need IRON (pipes, nails)
├─> Need CONCRETE (foundation)
└─> RESULT: Electrician trade emerges, copper mining booms

Build House (2000s)
├─> Need LUMBER (framing)
├─> Need DRYWALL (gypsum)
├─> Need COPPER (wiring)
├─> Need PLASTIC (pipes, siding, insulation)
├─> Need GLASS (efficient windows)
├─> Need CONCRETE (foundation)
└─> RESULT: Modern construction is complex supply chain
```

**Example 3: Power Plant**
```
Build Coal Power Plant (1900s)
├─> Need STEEL (boilers, turbines)
├─> Need CONCRETE (building)
└─> Need BRICK (smokestack)

Operating Coal Power Plant (ongoing)
├─> Need COAL constantly (creates mining jobs)
│   └─> Railroad to transport coal
│   └─> Mining town prosperity tied to power plant
├─> Need WATER for cooling
└─> RESULT: Coal mine has guaranteed customer

Build Nuclear Power Plant (1970s)
├─> Need massive CONCRETE (containment)
├─> Need STEEL (reactor vessel)
└─> Need URANIUM fuel (new resource demand!)

Operating Nuclear Power Plant (ongoing)
├─> Need URANIUM (small amount, but rare)
│   └─> Creates uranium mining industry
├─> Need WATER for cooling
└─> RESULT: Uranium mining becomes strategic
```

**Example 4: City Electrification** (1880s-1920s)
```
Decide to Electrify City
├─> Need POWER PLANT (coal/hydro)
│   └─> See power plant demand above
├─> Need COPPER WIRE (MASSIVE new demand!)
│   └─> Copper mines boom
│   └─> Copper smelters needed
│   └─> Copper wire factories
├─> Need POLES (timber)
│   └─> Logging industry benefits
├─> Need INSULATORS (glass/ceramic)
└─> RESULT: Entire copper industry transforms overnight

BEFORE electrification: Copper = pots and coins (low value)
AFTER electrification: Copper = essential (high value)

This is THE example of technology creating resource value!
```

**Example 5: Automobile Industry** (1910s-1950s)
```
Build Auto Factory
├─> Need STEEL (body, frame)
├─> Need RUBBER (tires) - Creates rubber demand
├─> Need GLASS (windshield)
├─> Need COPPER (wiring)
├─> Need LEATHER (seats, early era)
├─> Need ALUMINUM (engine, modern era)
└─> RESULT: Complex supply chain

Automobile Adoption (society-wide effect)
├─> Need OIL/GASOLINE (massive new demand!)
│   └─> Oil industry transforms
│   └─> Middle East becomes strategic
│   └─> Gas stations everywhere
├─> Need ROADS (concrete/asphalt demand)
├─> Need RUBBER (tire factories)
└─> RESULT: Entire economy restructures around cars

Oil value SKYROCKETS due to automobile adoption!
```

---

### Resource Value Transformation Examples

**The Copper Story** (Most dramatic transformation):
```
1850: Copper ore = "That reddish rock" - Value: Low
      Uses: Pots, coins, some roofing
      Price: $0.20/pound

1880: Electricity invented, cities start electrifying
      Copper needed for WIRING - Value: SKYROCKETING
      Demand: Every city, every building
      Price: $0.50/pound (2.5x increase)

1920: Every city is electrified, appliances spreading
      Copper = Essential resource - Value: Very High
      Price: $0.15/pound (stabilized, but volume is huge)

2020: Electronics, computers, renewable energy
      Copper still essential - Value: High
      Price: $4.00/pound (adjusted for inflation, very valuable)

Result: Worthless ore → Strategic resource in 50 years
```

**The Silicon Story** (Even more dramatic):
```
1850: Silicon (as sand/quartz) = Worthless
      Uses: Glass making (common sand)
      Value: Free (literally sand on beach)

1900: Silicon = Still basically worthless
      Uses: Glass, minor steel alloy
      Value: ~$0

1950: Transistor invented (1947)
      Silicon can be made into semiconductors
      Value: Starting to increase
      Uses: Early electronics (limited)

1970: Integrated circuit revolution
      Silicon wafers = Computer chips
      Value: EXTREMELY HIGH
      Uses: Computers, calculators, everything electronic

2020: Silicon = Most valuable material by weight (chips)
      Uses: Phones, computers, AI, everything
      Value: Purified silicon wafer = $100s/pound
      Computer chip = $1000s/pound equivalent

Result: Beach sand → More valuable than gold (by application)
        This is THE most dramatic value transformation!
```

**The Oil Story**:
```
1850: Oil seeps = "Land that smells bad"
      Uses: Limited (lamp oil, medicine grease)
      Value: Negative (ruins farmland!)
      Note: People avoid oil seeps

1880: Kerosene for lamps = moderate demand
      First oil wells drilled
      Value: Moderate
      Uses: Lighting, lubrication

1910: Automobile invented and spreading
      Gasoline (former waste product) = valuable!
      Value: Rising fast
      Demand spike!

1950: Post-WWII boom, car culture
      Oil = Most valuable commodity
      Uses: Transportation, plastics, chemicals, heating
      Value: EXTREMELY HIGH
      Strategic resource, wars fought over it

Result: "Cursed land" → "Black gold" in 60 years
```

**The Uranium Story** (Fastest transformation):
```
1900: Uranium = Obscure element
      Uses: Glass coloring (makes yellow glass)
      Value: Very low
      Mining: Tiny amounts for novelty

1938: Nuclear fission discovered
      Theoretical: Could be weapon or power source
      Value: Still low (not yet practical)

1945: Atomic bomb demonstrated
      Uranium = STRATEGIC WEAPON MATERIAL
      Value: EXTREMELY HIGH overnight
      Mining: Uranium rush in American Southwest

1950s: Nuclear power plants start
      Uranium = Dual use (weapons + power)
      Value: Very high, but controlled
      Mining: Government-controlled

Result: Worthless → Strategic resource in 5 years
        Fastest value transformation in history!
```

---

### Gameplay Implications

**Resource Discovery**:
- Start scenario: Some resources known (obvious gold, timber, farmland)
- Some resources hidden: Survey required (ore deposits)
- Some resources unknown value: Oil seeps present but "worthless" in 1850

**Technology Unlocks Value**:
- Oil exists in 1850 but worthless → Automobile (1910s) makes valuable
- Copper exists but low value → Electricity (1880s) makes essential
- Silicon exists (sand!) → Transistor (1947) makes valuable
- Uranium exists but obscure → Fission (1938) makes strategic

**Building Creates Demand**:
- Build railroad → Need steel, timber, coal
- Build houses → Need lumber, brick, copper (era-dependent)
- Build power plant → Need coal or uranium
- Electrify city → Need massive copper

**Economic Strategy**:
- Should you develop copper mine in 1850? (Low value NOW, but...)
- Should you secure oil seeps in 1870? (Worthless NOW, but...)
- Should you stockpile iron in 1860? (Railroad boom coming...)
- Speculation and timing matter!

**Depletion Forces Adaptation**:
- Gold mine depletes → Must find new economic base
- Forest logged out → Must import timber or switch materials
- Soil depleted → Must use fertilizer or switch crops
- Coal mine exhausted → Must find alternative energy

**Trade Networks**:
- No local iron? Import from mining town
- No local timber? Import from logging town
- No local oil? Import from oil town
- Resource distribution creates interdependence

---

## Zoom & Level of Detail (LOD) System

### Core Principle: Multi-Scale City Viewing

**Real city builders need multiple zoom levels** - You should be able to see the entire city at once (strategic view) or zoom in to see individual buildings and details (tactical view). Buildings need different representations at different scales.

### Zoom Levels

**Four distinct zoom levels**:

#### Zoom 0: City View (Strategic)
- **Viewport**: 200x80 tiles (entire city or large section)
- **Camera movement**: Fast scrolling (10 tiles per keypress)
- **Buildings shown**:
  - Major landmarks only (city hall, stadium, power plant)
  - Neighborhoods shown as zones (colored blocks)
  - Individual houses NOT visible
  - Roads shown as lines
- **Use case**: City-wide planning, seeing overall layout
- **ASCII representation**: Simple, abstract
```
Example at Zoom 0:
████████████     Downtown (commercial district)
████████████     Each █ = multiple buildings
░░░░░░░░░░░░     Residential zone
░░░░░░░░░░░░     Each ░ = neighborhood
▒▒▒▒▒▒▒▒▒▒▒▒     Industrial zone
≈≈≈≈≈≈≈≈≈≈≈≈     River
```

#### Zoom 1: District View (Normal) - **Default**
- **Viewport**: 120x40 tiles (neighborhood scale)
- **Camera movement**: Normal scrolling (1 tile per keypress)
- **Buildings shown**:
  - Large buildings: 2x2 to 4x4 tiles (office towers, malls)
  - Medium buildings: 1x2 tiles (shops, apartments)
  - Small buildings: 1x1 tile (houses, small shops)
  - Roads between buildings
- **Use case**: Normal gameplay, building placement
- **ASCII representation**: Moderate detail
```
Example at Zoom 1:
┌─┐  ┌──┐  ┌─┐     Buildings shown with borders
│H│  │##│  │H│     H = House (1x1)
└─┘  │##│  └─┘     ## = Apartment (2x2)
     └──┘
─────────────     Road
```

#### Zoom 2: Neighborhood View (Detailed)
- **Viewport**: 80x30 tiles (city block scale)
- **Camera movement**: Slow scrolling (1 tile per keypress)
- **Buildings shown**:
  - Large buildings: 4x6 to 8x8 tiles (detailed interiors visible)
  - Medium buildings: 2x3 tiles (room layout visible)
  - Small buildings: 2x2 tiles (interior details)
  - Sidewalks, trees, parking lots
- **Use case**: Detailed building inspection, micromanagement
- **ASCII representation**: High detail, interior layouts
```
Example at Zoom 2:
┌────────┐         Large office building (6x4)
│ ┌──┐   │         Interior divisions visible
│ │  │ □ │         □ = Desk
│ └──┘   │         Doors, rooms shown
│   ∩∩   │         ∩∩ = People
└────────┘
```

#### Zoom 3: Building Detail View (Maximum)
- **Viewport**: 60x20 tiles (single building focus)
- **Camera movement**: Pixel-precise
- **Buildings shown**:
  - Single large building fills screen
  - Individual rooms and furniture
  - Citizens/workers visible as characters
  - Activity/animation details
- **Use case**: Building management UI, inspecting specific building
- **ASCII representation**: Maximum detail
```
Example at Zoom 3 (House interior):
┌──────────────────┐
│ ┌─────┐   ┌────┐ │   Living room + Kitchen
│ │ ☺   │   │ ▄▄ │ │   ☺ = Citizen
│ │  ╔══╗  │ ║  ║ │   ╔══╗ = Couch
│ │  ║  ║  │ ╚══╝ │   ▄▄ = Stove
│ │  ╚══╝  └────┘ │
│ └─────┘          │
│ ┌────────────┐   │   Bedroom
│ │  ═══       │   │   ═══ = Bed
│ │            │   │
│ └────────────┘   │
└──────────────────┘
```

---

### Building Definitions with Multiple LOD Representations

Each building defines multiple visual representations for different zoom levels:

**buildings_traditional.txt**:
```
[house_small]
name: Small House
type: residential
capacity: 4
cost: 50000

# LOD 0: City view (not visible individually)
zoom_0:
  visible: false           # Don't show individual houses at city view
  aggregate_as: residential_zone

# LOD 1: District view (default gameplay)
zoom_1:
  width: 1
  height: 1
  glyph: H
  color: lightgreen
  background: darkgreen

# LOD 2: Neighborhood view (more detail)
zoom_2:
  width: 2
  height: 2
  floor_plan:
    ┌─┐
    │H│
    └─┘
  colors:
    border: white
    interior: brown

# LOD 3: Building detail (interior view)
zoom_3:
  width: 8
  height: 6
  floor_plan:
    ┌──────┐
    │ LR   │   # LR = Living room
    │  ══  │   # ══ = Couch
    │──┐ ┌─│
    │BR│K│   # BR = Bedroom, K = Kitchen
    └──┴─┘
  furniture: couch, bed, table, stove

[office_tower]
name: Office Tower (20 stories)
type: commercial
cost: 500000

# LOD 0: City view (major landmark, visible)
zoom_0:
  visible: true
  width: 1
  height: 1
  glyph: ▓              # Solid block for skyscraper
  color: cyan

# LOD 1: District view
zoom_1:
  width: 3
  height: 4
  floor_plan:
    ┌─┐
    │▓│
    │▓│
    └─┘
  color: cyan

# LOD 2: Neighborhood view (detailed)
zoom_2:
  width: 6
  height: 8
  floor_plan:
    ┌────┐
    │┌──┐│
    ││  ││   # Multiple floors
    ││  ││
    ││  ││
    │└──┘│
    └────┘
  entrance_at: (3, 7)

# LOD 3: Building detail (focus on single floor)
zoom_3:
  width: 16
  height: 12
  show_floor_selector: true    # UI to pick which floor to view
  floor_plan:
    ┌──────────────┐
    │ □ □ □ □ □ □ │   # □ = Desk
    │ □ □ □ □ □ □ │
    │              │
    │ ┌────┐       │
    │ │CONF│       │   # Conference room
    │ └────┘       │
    └──────────────┘
```

---

### Visibility Thresholds

Buildings have different visibility at different zoom levels:

#### Zoom 0 (City View)
**Visible**:
- Major landmarks (city hall, stadium, power plant, university)
- Industrial complexes
- Large commercial districts
- Infrastructure (power lines, highways)

**Hidden**:
- Individual houses
- Small shops
- Parks (unless large)
- Small roads

**Aggregation**: Small buildings aggregate into zones:
```
Instead of:  H H H H H H H H H H
             H H H H H H H H H H
             H H H H H H H H H H

Show:        ░░░░░░░░░░░░░░░░░
             ░░░░░░░░░░░░░░░░░  (Residential zone)
             ░░░░░░░░░░░░░░░░░
```

#### Zoom 1 (District View - Default)
**Visible**:
- All buildings (at simplified representation)
- Roads and streets
- Parks and open spaces
- Zoning visible

**Detail level**: Building footprints, basic shapes

#### Zoom 2 (Neighborhood View)
**Visible**:
- All buildings with interior details
- Sidewalks, trees, decorations
- Building entrances/exits
- Parking areas
- Citizens (if not too crowded)

**Detail level**: Room layouts, basic furniture

#### Zoom 3 (Building Detail)
**Visible**:
- Single building in full detail
- All furniture and fixtures
- Individual citizens with names
- Activity indicators
- Building stats panel

**Detail level**: Maximum

---

### Camera & Viewport Behavior

#### Zoom Controls
```csharp
Keyboard input:
- '=' or '+': Zoom in (increase zoom level)
- '-' or '_': Zoom out (decrease zoom level)
- '0': Reset to default zoom (Zoom 1)

Mouse input (optional):
- Scroll wheel: Zoom in/out
- Double-click: Zoom to clicked building
```

#### Viewport Size Changes
```csharp
public class GameState
{
    public int ZoomLevel { get; set; } = 1;  // 0-3

    public (int width, int height) GetViewportSize()
    {
        return ZoomLevel switch
        {
            0 => (200, 80),  // City view - large viewport
            1 => (120, 40),  // District view - default
            2 => (80, 30),   // Neighborhood view - medium
            3 => (60, 20),   // Building detail - small
            _ => (120, 40)
        };
    }

    public int GetCameraSpeed()
    {
        return ZoomLevel switch
        {
            0 => 10,  // Fast scrolling for city view
            1 => 1,   // Normal
            2 => 1,   // Normal
            3 => 1,   // Precise
            _ => 1
        };
    }
}
```

#### Smooth Zoom Transitions
```csharp
// When zooming, keep camera centered on current focus
public void ChangeZoom(int newZoomLevel)
{
    var oldCenter = CameraPosition;
    ZoomLevel = newZoomLevel;
    CameraPosition = oldCenter;  // Stay centered on same world position

    // Clamp to new viewport bounds
    ClampCameraToMapBounds();
}
```

---

### Building Placement at Different Zooms

#### Zoom 0 (City View)
- **Can't place buildings** - too zoomed out
- Zoom in prompt: "Zoom in to place buildings (press +)"
- Can only view and plan

#### Zoom 1 (District View) - Primary Building Mode
- **Normal building placement**
- Ghost building preview
- Terrain info panel
- Color-coded placement feedback

#### Zoom 2 (Neighborhood View)
- **Detailed placement**
- See exactly where building fits
- Preview interior layout
- Better for tight spaces

#### Zoom 3 (Building Detail)
- **Can't place buildings** - too zoomed in
- Only for inspection and management
- Zoom out prompt: "Zoom out to place buildings (press -)"

---

### Rendering Strategy

#### Building Render Priority
```csharp
void RenderBuildings(int zoomLevel)
{
    foreach (var building in GetVisibleBuildings())
    {
        // Check if building should render at this zoom level
        if (!building.IsVisibleAtZoom(zoomLevel))
            continue;

        // Get appropriate LOD representation
        var lodData = building.GetLODData(zoomLevel);

        // Render using LOD-specific data
        RenderBuildingLOD(building, lodData, zoomLevel);
    }

    // At Zoom 0, render aggregated zones
    if (zoomLevel == 0)
    {
        RenderAggregatedZones();
    }
}
```

#### Aggregated Zone Rendering (Zoom 0)
```csharp
// Group nearby similar buildings into zones
void RenderAggregatedZones()
{
    // Find clusters of residential buildings
    var residentialClusters = ClusterBuildingsByType(BuildingType.Residential);

    foreach (var cluster in residentialClusters)
    {
        // Fill cluster area with zone pattern
        FillArea(cluster.Bounds, '░', Color.LightGreen, Color.DarkGreen);
    }

    // Do same for commercial (▒) and industrial (▓)
}
```

---

### LOD Performance Benefits

#### Memory Efficiency
- Only load LOD data for current zoom level
- Buildings far from camera don't need high detail
- Huge cities remain performant

#### Rendering Efficiency
```
Zoom 0 (City View):
- 10,000 buildings → 50 major landmarks + 20 zones = 70 draw calls

Zoom 1 (District View):
- Visible area: 120x40 = 4,800 tiles
- Maybe 100-200 buildings in view
- Each building = 1-4 tiles

Zoom 2 (Neighborhood View):
- Visible area: 80x30 = 2,400 tiles
- Maybe 20-50 buildings in view
- Each building = detailed but fewer visible

Zoom 3 (Building Detail):
- Single building = entire viewport
- Maximum detail but only 1 building
```

#### Strategic Depth
- City planning requires Zoom 0 (strategic overview)
- Building placement uses Zoom 1 (tactical)
- Building inspection uses Zoom 2-3 (detailed)
- Mimics real city planning (maps → blueprints → walk-through)

---

### Visual Examples of Zoom Levels

#### Example: Residential Neighborhood

**Zoom 0 (City View)** - Abstract, zone-based:
```
████████████████     Downtown (commercial)
════════════════     Highway
░░░░░░░░░░░░░░░░     Residential (your house is one of these ░)
░░░░░░░░░░░░░░░░     Each ░ represents ~10-20 houses
▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒     Industrial zone
```

**Zoom 1 (District View)** - Individual buildings:
```
H H H H H H H H      H = House (1x1)
 S   H H   S         S = Shop (1x1)
H H ┌──┐ H H         ┌──┐ = Apartment (2x2)
H H │AP│ H H         │AP│
    └──┘
─────────────────    Road
```

**Zoom 2 (Neighborhood View)** - Building details:
```
┌─┐ ┌─┐ ┌─┐ ┌─┐     Individual houses with roofs
│H│ │H│ │H│ │H│     Fences, yards visible
└─┘ └─┘ └─┘ └─┘
 ∩   ∩   ∩   ∩       Trees between houses

┌────────┐           Larger apartment building
│ ┌──┐ □ │           Interior rooms visible
│ │  │   │           □ = Parking
│ └──┘   │
└────────┘
```

**Zoom 3 (Building Detail)** - Single house interior:
```
┌──────────────────┐
│ Living Room      │
│  ╔══╗   ☺        │  ╔══╗ = Couch
│  ║  ║            │  ☺ = Resident (John Smith)
│  ╚══╝   ┌─TV─┐   │
│         └────┘   │
├──────┬───────────┤
│ Kit  │  Bedroom  │
│ ▄▄▄  │  ═══      │  ▄▄▄ = Counter/Stove
│      │           │  ═══ = Bed
└──────┴───────────┘

[Resident: John Smith, Age 34, Happiness: 75%]
[Employment: Office Worker at Downtown Tower]
```

---

### Implementation Priority

**Phase 1 (MVP - Single Zoom)**:
- [x] Current rendering (implicitly Zoom 1)
- [x] Single representation per building
- [ ] Make current system "Zoom 1" explicitly

**Phase 2 (Basic Zoom)**:
- [ ] Add ZoomLevel property to GameState
- [ ] Implement zoom controls (+/- keys)
- [ ] Add Zoom 0 (city view) with zone aggregation
- [ ] Add Zoom 2 (neighborhood view) with more detail
- [ ] Adjust viewport size based on zoom
- [ ] Adjust camera speed based on zoom

**Phase 3 (Multi-LOD Buildings)**:
- [ ] Add LOD data to Building class
- [ ] Parse zoom_0, zoom_1, zoom_2 from definition files
- [ ] Render appropriate LOD based on current zoom
- [ ] Visibility thresholds (hide small buildings at Zoom 0)

**Phase 4 (Advanced)**:
- [ ] Zoom 3 (building detail) with interior management
- [ ] Smooth zoom animations
- [ ] Mouse wheel zoom support
- [ ] Double-click to zoom to building
- [ ] Mini-map showing zoom level indicator

---

### Design Considerations

#### Building Definition Complexity
**Question**: Should every building define all 4 zoom levels?

**Answer**: No, use smart defaults:
- If zoom_0 not defined: Hide at city view (aggregate into zone)
- If zoom_1 not defined: Use simplified version of zoom_2
- If zoom_2 not defined: Scale up zoom_1 representation
- If zoom_3 not defined: Use zoom_2 with stats panel

**Only major/unique buildings need all 4 LODs**.

#### Zone Aggregation Algorithm
At Zoom 0, cluster nearby buildings:
```csharp
ClusterBuildings()
{
    // Find groups of 5+ similar buildings within 10 tiles
    // Replace with colored zone fill
    // Label zone with count: "Residential (47 houses)"
}
```

#### Performance Target
- 10,000 buildings in city
- Zoom 0: Renders in <16ms (60 FPS) - only ~100 elements
- Zoom 1: Renders in <16ms - only visible buildings (~200)
- Zoom 2: Renders in <16ms - fewer visible buildings (~50)
- Zoom 3: Renders in <16ms - single building

ASCII rendering is fast - shouldn't be a problem!

---

## Notes & Ideas

*Use this section for random thoughts and ideas*

- Idea: "Story mode" in rebuilder where you play as a specific character/leader
- Idea: Random events in traditional mode (natural disasters, economic crashes)
- Idea: Photography mode to capture cool city views
- Idea: Time-lapse replay of city growth
- Question: Should buildings have "health" that decays over time?
- Question: How do we handle multi-tile buildings in ASCII? (use different chars for different parts)
- Idea: At Zoom 3, show building management UI (assign workers, set policies)
- Idea: Transition animations between zoom levels (smooth scaling effect)
- Idea: Zoom level affects time scale (Zoom 0 = fast time, Zoom 3 = pause/slow)
- Idea: "Tourist mode" - zoom in and watch citizens go about their day
