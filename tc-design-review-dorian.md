# TerminalCity — Design Review by Dorian (game_dev)
*First impressions review, 2026-02-20. Based on reading: Program.cs, GameState.cs, tc-design.md, DESIGN_NOTES.md, ZOOM_BUILDING_DETAIL.md, scenario files, definition files.*

---

## What I Read

- `Program.cs` — full game loop, rendering pipeline, input handling
- `Domain/GameState.cs` — state model, camera, time, building placement stub
- `tc-design.md` — design vision document
- `DESIGN_NOTES.md` — economics and tech tree philosophy
- `ZOOM_BUILDING_DETAIL.md` — zoom rendering architecture
- `definitions/scenarios/scenarios_test_tiny.txt` — starting scenario
- `definitions/buildings/*.txt`, `definitions/crops/*.txt` — content files

---

## What's Working

### The Zoom System Is the Right Foundation

Five zoom levels (25ft → 400ft per tile) with zoom-aware rendering is genuinely well-conceived. The key insight — storing tiles at 25ft granularity and sampling up — gives you a single source of truth with multiple views. The neighbor-aware road rendering (detecting intersections, horizontal vs vertical segments) already produces something that *reads as roads* at every zoom. That's not trivial.

The content-driven approach (building appearances in .txt files per zoom level) is correct. Balance tweaks and visual iteration without recompile is exactly right for this stage.

### The Design Thinking in DESIGN_NOTES.md

The disruption economics framing is the most interesting design idea in the project. Most city builders fail in the late game because they're purely additive — you optimize, reach steady state, and then there's nothing left to decide. The framing of technology as "winners and losers" rather than "new toys" is a meaningful design intervention against that failure mode.

The 1955 bedroom community starting point is a specific, evocative hook. Players know what a 1950s suburb *feels like*, which creates intuitions the game can work with or against.

The "Cities: Skylines mistakes to avoid" table is exactly the right kind of pre-mortem thinking.

### Scenario Architecture

Loading initial map state from .txt scenario files is correct. It separates content from code. The distinction between "ruins" (Rebuilder) and "empty land" (Traditional) being scenario-driven rather than branching codebases is elegant — as long as the `Building` class abstraction doesn't get stretched.

---

## What Concerns Me

### 1. Weather System: Premature Complexity

**What's there:** Temperature (°F), humidity (%), barometric pressure (inHg), visibility (miles), wind speed/direction, fire danger level, moon phase, sunrise/sunset times.

**What it does:** Nothing. All cosmetic. No weather condition affects population, economy, building damage, crop yield, anything.

**The problem:** This system consumes 9 rows of HUD (rows 0–8 in `RenderUI`), twelve keyboard shortcuts (Q, PgUp/PgDn, Home/End, Ins, Del, E, R, F, G, V), and significant implementation complexity — before a single building can be placed or a single meaningful choice can be made.

That's "feature completeness theater." It *looks* like depth. It isn't yet.

**What I'd do:** Collapse the weather display to one row (`Weather: Sunny, 72°F`) and move the detailed controls to a debug/dev panel. When weather actually drives gameplay choices — "do I build the barn before the storm?" — bring it back as a first-class UI element. Right now it's noise.

### 2. The Key Space Is Nearly Full Before the Game Exists

Current key bindings in Playing mode:
- Arrow keys / WASD: camera
- `[ ]`: zoom
- `+ -`: speed
- `T`: time of day
- `Q`: weather condition
- `PgUp/PgDn`: temperature
- `Home/End`: wind speed
- `Ins`: wind direction
- `Del`: humidity
- `E/R`: barometric pressure
- `F/G`: visibility
- `V`: fire danger
- `X`: screen dump
- `Escape`: exit confirm

That's 20+ bindings, mostly for cosmetic weather toggles. When building placement arrives, where do the controls go? `B` to open build menu? `Enter` to place? What gets displaced?

This needs a plan before the key space gets any more crowded.

### 3. The Camera Crosshair Is a UX Problem Waiting to Happen

`Program.cs:768` — `mainConsole.SetGlyph(centerX, centerY, '+', Color.Yellow)` — permanently draws a yellow + at the viewport center.

Right now this reads as "viewport center marker." When building placement arrives, a player will naturally interpret any cursor as "this is where I'm placing." If the build cursor is the crosshair, that's actually fine — but it needs to be designed intentionally, not inherited from a debug marker.

### 4. Debug Logging in Production Code Path

`GetBorderAppearance()` in Program.cs (lines ~1544–1618) has 8+ `System.Console.WriteLine("BORDER DEBUG: ...")` calls that fire every frame at specific coordinates. This will print thousands of lines per minute during normal gameplay. It needs to come out (or move to a conditional `#if DEBUG` block) before any serious playtesting.

### 5. No Player Agency Loop Yet

The current new-player experience:
1. Title screen → press Enter
2. Scenario dialog → press Enter
3. Map: farmland with roads, camera pans
4. Can cycle through cosmetic weather states
5. ...that's it

There's no "what do I do?" signal. No action the player can take that *matters*. The first moment the game feels like a game will be when a player places a building and sees it appear on the map at different zoom levels.

This isn't a criticism — it's a prototype. But it means **building placement needs to be the next thing**, not more cosmetic systems.

---

## Open Design Questions I'm Flagging

These come from reading the design docs. Some are already listed as open questions in `tc-design.md` — I'm adding my game designer take.

### Cursor vs. Camera (Needs Decision Before Building Placement)

**Option A:** One cursor. Camera follows it. Placing a building means you navigate to a location and press a key.

**Option B:** Camera and cursor separate. Camera pans freely. Cursor is a separate movable reticle.

For a city builder at this scale, Option B is standard (like Cities: Skylines). Option A is how Dwarf Fortress works in some modes. The choice affects every future control decision.

**My recommendation:** Option B. At zoom level 0 (100ft), the map is 30x30 tiles at most in the current scenario. Players will want to pan to a location and then precisely click/position. Dual mode (pan camera, then fine-position cursor) is the familiar pattern.

### The "Two Games" Risk

The shared engine / dual game modes plan is architecturally appealing. The risk: Rebuilder (scavenging, survival, narrative) and Traditional (zoning, economy, sandbox) may want the engine to do fundamentally different things that eventually force forking.

**My recommendation:** Don't try to build both at once. Pick one and ship a playable loop. Rebuilder is more interesting from a design standpoint (pressure, narrative, meaningful choices) but Traditional is closer to the current scenario (empty-ish land, expansion focus). I'd do Traditional first because it's already implicit in the "bedroom community 1955" scenario, then see whether the engine genuinely supports Rebuilder without major surgery.

### Citizen Happiness: Mood Bar or Detailed Needs?

Listed as open in tc-design.md. My take: **start with a mood bar** (one aggregated value). Detailed needs (food, shelter, safety separately) is the right long-term design, but you need a working economy loop before individual needs are meaningful. A mood bar lets you validate the economy loop first, then add complexity.

### Time Scale: Real-Time vs Turn-Based vs Hybrid

Listed as open. My take: **hybrid is correct for this genre**. Paused by default, runs when unpaused, 4 speed levels. The current implementation already does this. Lock it in. Turn-based would feel weird for a city builder. Pure real-time at any speed would make the game uncontrollable at scale.

---

## What to Build Next (My Recommendation)

**Single user story:** "As a player, I can place a farmhouse on the map and see it at all five zoom levels."

That's it. No economy attached. No cost. Just: open build menu, pick farmhouse, press a key, it appears.

Why this first:
- Creates the first real gameplay moment
- Forces decisions about cursor/camera UX
- Validates the zoom rendering system with player-placed content (not just generated content)
- Gives something to playtest — "does this feel right to place?"
- Everything else (costs, zoning, roads connecting buildings) can layer on top

**Time estimate:** Not giving one. But the rendering pipeline already handles buildings at all zoom levels. MapGenerator already places buildings. The input handling pattern is in place. The main new pieces are: build menu UI, cursor mode, and wiring placement to player input.

---

## Documentation Gaps

1. **tc-design.md** is well-written but needs a "decisions locked" section distinct from open questions. Some things (hybrid time system, zoom levels, content-driven definitions) should be off the table for debate. Document them as decided.

2. **No first-five-minutes spec.** What should a new player be doing and understanding in their first 5 minutes? This should be written down before building placement is implemented. It constrains how the UI works.

3. **decisions-log.md** doesn't exist yet. Start it. Every significant design decision (even "we're keeping weather cosmetic for now") should have a record: what was decided, why, and what changed it.

---

## Summary

| Area | Status | Priority |
|------|--------|----------|
| Zoom system | ✅ Solid foundation | Maintain |
| Content-driven definitions | ✅ Correct approach | Maintain |
| Building placement | ❌ Not yet | **Next** |
| Weather system | ⚠️ Premature complexity | Reduce UI footprint |
| Debug logging | ⚠️ Noisy in production | Clean up |
| Camera/cursor model | ❓ Unresolved | Decide before build placement |
| Player agency loop | ❌ Missing | Comes with building placement |
| Design docs | 🟡 Good start | Add decisions-log.md |

---

*Dorian — game_dev*
*"If it isn't fun yet, no amount of polish fixes it. Find out if it's fun first."*
