using SadRogue.Primitives;
using TerminalCity.Domain;

namespace TerminalCity.Parsers;

/// <summary>
/// Parses crop definitions from text files
/// </summary>
public static class CropParser
{
    public static List<CropDefinition> LoadFromFile(string filePath)
    {
        var crops = new List<CropDefinition>();

        if (!File.Exists(filePath))
        {
            Console.WriteLine($"WARNING: Crop file not found: {filePath}");
            return crops;
        }

        var lines = File.ReadAllLines(filePath);
        CropDefinition? currentCrop = null;

        foreach (var line in lines)
        {
            var trimmed = line.Trim();

            // Skip comments and empty lines
            if (string.IsNullOrWhiteSpace(trimmed) || trimmed.StartsWith("#"))
                continue;

            // New crop section
            if (trimmed == "[crop]")
            {
                if (currentCrop != null)
                    crops.Add(currentCrop);

                currentCrop = new CropDefinition();
                continue;
            }

            if (currentCrop == null) continue;

            // Parse key-value pairs
            if (trimmed.Contains(':'))
            {
                var parts = trimmed.Split(':', 2);
                var key = parts[0].Trim();
                var value = parts[1].Trim();

                switch (key)
                {
                    case "id":
                        currentCrop.Id = value;
                        break;
                    case "name":
                        currentCrop.Name = value;
                        break;
                    case "description":
                        currentCrop.Description = value;
                        break;
                    case "color":
                        currentCrop.Color = ParseColor(value);
                        break;
                    case "background_color":
                        currentCrop.BackgroundColor = ParseColor(value);
                        break;
                    case "pattern_25ft":
                        currentCrop.Pattern25ft = new ZoomPattern { Pattern = ConvertUnicodeToExtendedAscii(value) };
                        break;
                    case "pattern_50ft":
                        currentCrop.Pattern50ft = new ZoomPattern { Pattern = ConvertUnicodeToExtendedAscii(value) };
                        break;
                    case "pattern_100ft":
                        currentCrop.Pattern100ft = new ZoomPattern { Pattern = ConvertUnicodeToExtendedAscii(value) };
                        break;
                    case "pattern_200ft":
                        currentCrop.Pattern200ft = new ZoomPattern { Pattern = ConvertUnicodeToExtendedAscii(value) };
                        break;
                    case "pattern_400ft":
                        currentCrop.Pattern400ft = new ZoomPattern { Pattern = ConvertUnicodeToExtendedAscii(value) };
                        break;
                }
            }
        }

        // Add last crop
        if (currentCrop != null)
            crops.Add(currentCrop);

        Console.WriteLine($"Loaded {crops.Count} crop definitions from {filePath}");
        return crops;
    }

    private static string ConvertUnicodeToExtendedAscii(string pattern)
    {
        // Map Unicode characters to their CP437 (extended ASCII) equivalents
        // This ensures compatibility with fonts that only support codepage 437
        var result = pattern
            // Crop-specific characters
            .Replace('≡', (char)240)  // Triple bar
            .Replace('≈', (char)247)  // Almost equal (wavy lines)
            .Replace('″', (char)34)   // Double prime (use quote)
            .Replace('·', (char)250)  // Middle dot
            .Replace('┊', (char)179)  // Box drawings light quadruple dash vertical → use │
            .Replace('░', (char)176)  // Light shade
            .Replace('▒', (char)177)  // Medium shade
            .Replace('▓', (char)178)  // Dark shade
            // Common characters from buildings (in case crops use them)
            .Replace('█', (char)219)  // Full block
            .Replace('║', (char)186)  // Box drawing double vertical
            .Replace('│', (char)179); // Box drawing single vertical

        return result;
    }

    private static Color ParseColor(string colorName)
    {
        // Use SadRogue.Primitives color names
        return colorName.ToLower() switch
        {
            "white" => Color.White,
            "black" => Color.Black,
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
            "gold" => Color.Gold,
            "goldenrod" => Color.Goldenrod,
            "greenyellow" => Color.GreenYellow,
            "tan" => Color.Tan,
            "darkgoldenrod" => Color.DarkGoldenrod,
            "yellowgreen" => Color.YellowGreen,
            _ => Color.White
        };
    }
}
