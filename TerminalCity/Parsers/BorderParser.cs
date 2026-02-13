using SadRogue.Primitives;
using TerminalCity.Domain;

namespace TerminalCity.Parsers;

/// <summary>
/// Parses border definitions from text files
/// </summary>
public static class BorderParser
{
    public static List<BorderDefinition> LoadFromFile(string filePath)
    {
        var borders = new List<BorderDefinition>();

        if (!File.Exists(filePath))
        {
            Console.WriteLine($"WARNING: Border file not found: {filePath}");
            return borders;
        }

        var lines = File.ReadAllLines(filePath);
        BorderDefinition? currentBorder = null;

        foreach (var line in lines)
        {
            var trimmed = line.Trim();

            // Skip comments and empty lines
            if (string.IsNullOrWhiteSpace(trimmed) || trimmed.StartsWith("#"))
                continue;

            // New border section
            if (trimmed == "[border]")
            {
                if (currentBorder != null)
                    borders.Add(currentBorder);

                currentBorder = new BorderDefinition();
                continue;
            }

            if (currentBorder == null) continue;

            // Parse key-value pairs
            if (trimmed.Contains(':'))
            {
                var parts = trimmed.Split(':', 2);
                var key = parts[0].Trim();
                var value = parts[1].Trim();

                switch (key)
                {
                    case "id":
                        currentBorder.Id = value;
                        break;
                    case "name":
                        currentBorder.Name = value;
                        break;
                    case "description":
                        currentBorder.Description = value;
                        break;
                    case "color":
                        currentBorder.Color = ParseColor(value);
                        break;
                    case "background_color":
                        currentBorder.BackgroundColor = ParseColor(value);
                        break;
                    // Pattern parsing with directional support
                    case "pattern_25ft":
                        currentBorder.Pattern25ft ??= new BorderPatternSet();
                        currentBorder.Pattern25ft.Default = ParsePattern(value);
                        break;
                    case "pattern_25ft_north":
                        currentBorder.Pattern25ft ??= new BorderPatternSet();
                        currentBorder.Pattern25ft.North = ParsePattern(value);
                        break;
                    case "pattern_25ft_south":
                        currentBorder.Pattern25ft ??= new BorderPatternSet();
                        currentBorder.Pattern25ft.South = ParsePattern(value);
                        break;
                    case "pattern_25ft_east":
                        currentBorder.Pattern25ft ??= new BorderPatternSet();
                        currentBorder.Pattern25ft.East = ParsePattern(value);
                        break;
                    case "pattern_25ft_west":
                        currentBorder.Pattern25ft ??= new BorderPatternSet();
                        currentBorder.Pattern25ft.West = ParsePattern(value);
                        break;

                    case "pattern_50ft":
                        currentBorder.Pattern50ft ??= new BorderPatternSet();
                        currentBorder.Pattern50ft.Default = ParsePattern(value);
                        break;
                    case "pattern_50ft_north":
                        currentBorder.Pattern50ft ??= new BorderPatternSet();
                        currentBorder.Pattern50ft.North = ParsePattern(value);
                        break;
                    case "pattern_50ft_south":
                        currentBorder.Pattern50ft ??= new BorderPatternSet();
                        currentBorder.Pattern50ft.South = ParsePattern(value);
                        break;
                    case "pattern_50ft_east":
                        currentBorder.Pattern50ft ??= new BorderPatternSet();
                        currentBorder.Pattern50ft.East = ParsePattern(value);
                        break;
                    case "pattern_50ft_west":
                        currentBorder.Pattern50ft ??= new BorderPatternSet();
                        currentBorder.Pattern50ft.West = ParsePattern(value);
                        break;

                    case "pattern_100ft":
                        currentBorder.Pattern100ft ??= new BorderPatternSet();
                        currentBorder.Pattern100ft.Default = ParsePattern(value);
                        break;
                    case "pattern_100ft_north":
                        currentBorder.Pattern100ft ??= new BorderPatternSet();
                        currentBorder.Pattern100ft.North = ParsePattern(value);
                        break;
                    case "pattern_100ft_south":
                        currentBorder.Pattern100ft ??= new BorderPatternSet();
                        currentBorder.Pattern100ft.South = ParsePattern(value);
                        break;
                    case "pattern_100ft_east":
                        currentBorder.Pattern100ft ??= new BorderPatternSet();
                        currentBorder.Pattern100ft.East = ParsePattern(value);
                        break;
                    case "pattern_100ft_west":
                        currentBorder.Pattern100ft ??= new BorderPatternSet();
                        currentBorder.Pattern100ft.West = ParsePattern(value);
                        break;

                    case "pattern_200ft":
                        currentBorder.Pattern200ft ??= new BorderPatternSet();
                        currentBorder.Pattern200ft.Default = ParsePattern(value);
                        break;
                    case "pattern_200ft_north":
                        currentBorder.Pattern200ft ??= new BorderPatternSet();
                        currentBorder.Pattern200ft.North = ParsePattern(value);
                        break;
                    case "pattern_200ft_south":
                        currentBorder.Pattern200ft ??= new BorderPatternSet();
                        currentBorder.Pattern200ft.South = ParsePattern(value);
                        break;
                    case "pattern_200ft_east":
                        currentBorder.Pattern200ft ??= new BorderPatternSet();
                        currentBorder.Pattern200ft.East = ParsePattern(value);
                        break;
                    case "pattern_200ft_west":
                        currentBorder.Pattern200ft ??= new BorderPatternSet();
                        currentBorder.Pattern200ft.West = ParsePattern(value);
                        break;

                    case "pattern_400ft":
                        currentBorder.Pattern400ft ??= new BorderPatternSet();
                        currentBorder.Pattern400ft.Default = ParsePattern(value);
                        break;
                    case "pattern_400ft_north":
                        currentBorder.Pattern400ft ??= new BorderPatternSet();
                        currentBorder.Pattern400ft.North = ParsePattern(value);
                        break;
                    case "pattern_400ft_south":
                        currentBorder.Pattern400ft ??= new BorderPatternSet();
                        currentBorder.Pattern400ft.South = ParsePattern(value);
                        break;
                    case "pattern_400ft_east":
                        currentBorder.Pattern400ft ??= new BorderPatternSet();
                        currentBorder.Pattern400ft.East = ParsePattern(value);
                        break;
                    case "pattern_400ft_west":
                        currentBorder.Pattern400ft ??= new BorderPatternSet();
                        currentBorder.Pattern400ft.West = ParsePattern(value);
                        break;
                }
            }
        }

        // Add last border
        if (currentBorder != null)
            borders.Add(currentBorder);

        Console.WriteLine($"Loaded {borders.Count} border definitions from {filePath}");
        return borders;
    }

    private static char? ParsePattern(string value)
    {
        // Empty value means no pattern (invisible at this zoom)
        if (string.IsNullOrWhiteSpace(value))
            return null;

        // Convert Unicode to CP437 and return first character
        var converted = ConvertUnicodeToExtendedAscii(value);
        return converted.Length > 0 ? converted[0] : null;
    }

    private static string ConvertUnicodeToExtendedAscii(string pattern)
    {
        // Map Unicode characters to their CP437 (extended ASCII) equivalents
        var result = pattern
            // Border-specific characters
            .Replace('♠', (char)6)    // Spade (tree symbol)
            .Replace('▲', (char)30)   // Up triangle
            .Replace('≈', (char)247)  // Almost equal (wavy lines)
            .Replace('~', (char)126)  // Tilde (wave)
            .Replace('″', (char)34)   // Double prime (grass)
            .Replace('▓', (char)178)  // Dark shade
            .Replace('#', (char)35)   // Hash
            .Replace('·', (char)250)  // Middle dot
            .Replace('-', (char)45)   // Dash/minus
            .Replace('|', (char)179)  // ASCII vertical bar → box drawing
            .Replace('│', (char)179)  // Unicode vertical line → box drawing
            .Replace('─', (char)196); // Horizontal line

        return result;
    }

    private static Color ParseColor(string colorName)
    {
        // Use SadRogue.Primitives color names
        return colorName.ToLower() switch
        {
            "white" => Color.White,
            "black" => Color.Black,
            "transparent" => Color.Transparent,
            "red" => Color.Red,
            "darkred" => Color.DarkRed,
            "green" => Color.Green,
            "darkgreen" => Color.DarkGreen,
            "blue" => Color.Blue,
            "darkblue" => Color.DarkBlue,
            "yellow" => Color.Yellow,
            "brown" => Color.Brown,
            "gray" => Color.Gray,
            "darkgray" => Color.DarkGray,
            "silver" => Color.Silver,
            "saddlebrown" => Color.SaddleBrown,
            "darkkhaki" => Color.DarkKhaki,
            "peru" => Color.Peru,
            "yellowgreen" => Color.YellowGreen,
            _ => Color.White
        };
    }
}
