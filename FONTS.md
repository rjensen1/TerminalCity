# Font Configuration - TerminalCity

## Current Font Setup

**Active Font**: IBM Extended 8x16 (IBM_ext)
- **Size**: 8x16 pixels per character
- **Character Set**: CP437 (256 standard) + 70+ additional glyphs
- **Source**: SadConsole official font repository

## Available Fonts

### 1. IBM_ext (Default - Active)
- **Files**: `fonts/IBM_ext.font` + `IBM8x16_NoPadding_extended.png`
- **Size**: 8x16 pixels
- **Characters**: 256 CP437 + 70+ extended glyphs
- **Best For**: General use, maximum compatibility, extended UI elements
- **Extended Features**:
  - Advanced box drawing (256-268)
  - Line variations with half-height segments (269-294)
  - Progressive fill patterns (304-319)
  - UI directional arrows (320-322)

### 2. Cheepicus12 (Alternative)
- **Files**: `fonts/Cheepicus12.font` + `Cheepicus_12x12.png`
- **Size**: 12x12 pixels
- **Characters**: Full CP437 support
- **Best For**: Larger, more readable text

## Switching Fonts

### Option 1: Change Default Font (Program.cs, line 31)
```csharp
.ConfigureFonts((fontConfig, gameHost) => {
    // Switch to Cheepicus12
    fontConfig.UseCustomFont("fonts\\Cheepicus12.font");

    // Or back to standard IBM
    fontConfig.UseBuiltinFont();
})
```

### Option 2: Switch at Runtime
```csharp
// Access loaded fonts
var cheepicus = Game.Instance.Fonts["Cheepicus12"];
mainConsole.Font = cheepicus;
```

## Adding New Fonts

1. **Download/Create** font files (PNG + .font JSON)
2. **Place** in `TerminalCity/fonts/` directory
3. **Add** to `ConfigureFonts()` via `AddExtraFonts()`
4. **Build** - files automatically copy to output

## Font Format Requirements

SadConsole uses bitmap fonts consisting of:
- **PNG image**: White glyphs on transparent background
- **.font JSON**: Configuration (size, layout, glyph mapping)

### Creating Custom Fonts
Use these tools to convert TrueType fonts to SadConsole format:
- **BMFont** (Windows): https://www.angelcode.com/products/bmfont/
- **Font2Bitmap** (Web): https://stmn.itch.io/font2bitmap
- **fontbm** (CLI): https://github.com/vladimirgamalyan/fontbm

## Benefits of IBM_ext

✅ **70+ additional characters** beyond standard CP437
✅ **Advanced box drawing** for complex UI elements
✅ **Progressive fill patterns** for better visual effects
✅ **Directional arrows** for UI navigation
✅ **100% backward compatible** with all existing CP437 content
✅ **Same 8x16 size** as default - no layout changes

## Testing Fonts

Use the built-in Font Test mode:
1. Run the game
2. Press **F** from title screen
3. View all characters 33-255
4. Press **T** to toggle between random/organized display
5. Press **Escape** to return

## Resources

- [SadConsole Fonts](https://github.com/Thraka/SadConsole/tree/master/Fonts)
- [SadConsole Font Documentation](https://sadconsole.com/articles/systems/fonts.html)
- [CP437 Reference](https://cp437.github.io/)
- [Ultimate Oldschool PC Font Pack](https://int10h.org/oldschool-pc-fonts/)
