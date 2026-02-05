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

## Notes & Ideas

*Use this section for random thoughts and ideas*

- Idea: "Story mode" in rebuilder where you play as a specific character/leader
- Idea: Random events in traditional mode (natural disasters, economic crashes)
- Idea: Photography mode to capture cool city views
- Idea: Time-lapse replay of city growth
- Question: Should buildings have "health" that decays over time?
- Question: How do we handle multi-tile buildings in ASCII? (use different chars for different parts)
