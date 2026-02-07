# Zoom-Level Building Detail System

## Design Overview

Buildings occupy more data tiles as you zoom in, with more visual detail at each level.

### Zoom Levels and Building Sizes

**Example: Farmhouse**
- **400ft zoom**: 1 tile (⌂) - important structures only
- **100ft zoom**: 1 tile (⌂) - standard display
- **50ft zoom**: 2 tiles - TODO: define if 1x2 or 2x1
- **25ft zoom**: 4+ tiles (2x2) - ASCII art pattern

**Example: Large Barn**
- **100ft zoom**: 2 tiles (larger than farmhouse)
- **25ft zoom**: 8+ tiles - more detailed

## File Organization

### Plot File (farmsteads.txt)
Defines **spatial blocking** at maximum detail (25ft zoom):

```
map:
...
HHS
HH.

legend:
. = yard
H = farmhouse  # These 4 tiles are RESERVED/BLOCKED for farmhouse
S = shed
```

The plot file shows the footprint - which tiles belong to which building.

### Building File (agriculture.txt)
Defines **visual appearance** at each zoom level:

```
[building]
id: farmhouse
width: 2
height: 2
display_char: ⌂  # Default for 100ft zoom
color: White
background_color: DarkKhaki

# At 25ft zoom - 2x2 ASCII art pattern
pattern_25ft:
/‾\
\__/
```

## Implementation Plan

### Phase 1: Parser (TODO)
1. Create BuildingParser to load building definitions
2. Parse pattern_25ft (and future pattern_50ft, pattern_100ft)
3. Store patterns in Building class

### Phase 2: Tile Position Tracking (TODO)
Add to Tile class:
```csharp
public Point? BuildingOffset { get; set; }  // Position within parent building (0,0 = top-left)
```

When placing farmstead, set:
- Tile (8,18): CropType="farmhouse", BuildingOffset=(0,0) → renders as '/'
- Tile (9,18): CropType="farmhouse", BuildingOffset=(1,0) → renders as '‾'
- Tile (8,19): CropType="farmhouse", BuildingOffset=(0,1) → renders as '\'
- Tile (9,19): CropType="farmhouse", BuildingOffset=(1,1) → renders as '/'

### Phase 3: Zoom-Aware Rendering (TODO)
Update GetFarmsteadStructureAppearance:
```csharp
(char glyph, Color fg, Color bg) GetFarmsteadStructureAppearance(
    string structureType,
    int zoomLevel,
    Point? buildingOffset)
{
    // At 25ft zoom (zoomLevel == 2)
    if (zoomLevel == 2 && buildingOffset != null)
    {
        // Look up building definition
        var building = GetBuildingDefinition(structureType);

        // Get character from pattern at this offset
        char patternChar = building.Pattern25ft[buildingOffset.Y][buildingOffset.X];
        return (patternChar, building.Color, building.BackgroundColor);
    }

    // Otherwise use standard single-character display
    return GetStandardAppearance(structureType);
}
```

### Phase 4: Multiple Zoom Patterns (Future)
Add more patterns as needed:
```
pattern_50ft:  # 2-tile pattern (decide if 1x2 or 2x1)
⌂⌂

pattern_100ft:  # Standard single tile (can override display_char)
⌂
```

## Current Status

✅ Definition files updated with pattern_25ft examples
✅ TODO comments added throughout code
⏸️ Parser not yet implemented - using hardcoded appearances
⏸️ BuildingOffset not yet tracked in Tile class
⏸️ Zoom-aware rendering not yet implemented

**Next Steps**: User will experiment with patterns in definition files, then we'll implement the parser and rendering logic.
