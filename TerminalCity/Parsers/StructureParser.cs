namespace TerminalCity.Parsers;

using SadRogue.Primitives;
using TerminalCity.Domain;

/// <summary>
/// Parses structure (outbuilding) definitions from text files
/// </summary>
public static class StructureParser
{
    public static List<StructureDefinition> LoadFromFile(string filePath)
    {
        var structures = new List<StructureDefinition>();

        if (!File.Exists(filePath))
            return structures;

        var lines = File.ReadAllLines(filePath);
        StructureDefinition? current = null;
        string currentSection = "";
        List<string> patternLines = new();
        string currentPatternZoom = "";

        foreach (var line in lines)
        {
            var trimmed = line.Trim();

            // Skip comments and empty lines
            if (string.IsNullOrWhiteSpace(trimmed) || trimmed.StartsWith("#"))
                continue;

            // Section headers
            if (trimmed.StartsWith("[") && trimmed.EndsWith("]"))
            {
                currentSection = trimmed.Trim('[', ']').ToLower();

                if (currentSection == "structure")
                {
                    // Save previous structure if exists
                    if (current != null)
                    {
                        structures.Add(current);
                    }

                    current = new StructureDefinition();
                    patternLines.Clear();
                    currentPatternZoom = "";
                }
                continue;
            }

            if (current == null) continue;

            // Handle multi-line patterns
            if (!string.IsNullOrEmpty(currentPatternZoom))
            {
                // Check if this is a new field (contains ':')
                if (trimmed.Contains(':'))
                {
                    // Finished reading pattern, save it
                    SavePattern(current, currentPatternZoom, patternLines);
                    patternLines.Clear();
                    currentPatternZoom = "";
                    // Fall through to process this line as a field
                }
                else
                {
                    // Continue reading pattern
                    patternLines.Add(line); // Use original line to preserve spacing
                    continue;
                }
            }

            // Parse key: value pairs
            var parts = trimmed.Split(':', 2);
            if (parts.Length != 2) continue;

            var key = parts[0].Trim().ToLower();
            var value = parts[1].Trim();

            switch (key)
            {
                case "id":
                    current.Id = value;
                    break;
                case "name":
                    current.Name = value;
                    break;
                case "type":
                    current.Type = value;
                    break;
                case "variant_of":
                    current.VariantOf = value;
                    break;
                case "size":
                    current.Size = value;
                    break;
                case "description":
                    current.Description = value;
                    break;
                case "color":
                    current.Color = ParseColor(value);
                    break;
                case "background_color":
                    current.BackgroundColor = ParseColor(value);
                    break;
                case "can_attach_to":
                    current.CanAttachTo = value;
                    break;
                case "pattern_25ft":
                    currentPatternZoom = "25ft";
                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        patternLines.Add(value);
                    }
                    break;
                case "pattern_50ft":
                    currentPatternZoom = "50ft";
                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        patternLines.Add(value);
                    }
                    break;
                case "pattern_100ft":
                    currentPatternZoom = "100ft";
                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        patternLines.Add(value);
                    }
                    break;
                case "pattern_200ft":
                    currentPatternZoom = "200ft";
                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        patternLines.Add(value);
                    }
                    break;
                case "pattern_400ft":
                    currentPatternZoom = "400ft";
                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        patternLines.Add(value);
                    }
                    break;
                case "important_50ft":
                    if (current.Pattern50ft != null)
                        current.Pattern50ft.Important = bool.Parse(value);
                    break;
                case "important_100ft":
                    if (current.Pattern100ft != null)
                        current.Pattern100ft.Important = bool.Parse(value);
                    break;
                case "important_200ft":
                    if (current.Pattern200ft != null)
                        current.Pattern200ft.Important = bool.Parse(value);
                    break;
                case "important_400ft":
                    if (current.Pattern400ft != null)
                        current.Pattern400ft.Important = bool.Parse(value);
                    break;
            }
        }

        // Save last structure
        if (current != null)
        {
            if (!string.IsNullOrEmpty(currentPatternZoom))
            {
                SavePattern(current, currentPatternZoom, patternLines);
            }
            structures.Add(current);
        }

        return structures;
    }

    private static string ConvertUnicodeToExtendedAscii(string pattern)
    {
        // Map Unicode box-drawing and special characters to their extended ASCII equivalents
        // This ensures compatibility with fonts that only support codepage 437 (extended ASCII)
        var result = pattern
            .Replace('⌂', (char)127)  // House symbol
            .Replace('▐', (char)222)  // Right half block
            .Replace('▌', (char)221)  // Left half block
            .Replace('█', (char)219)  // Full block
            .Replace('▄', (char)220)  // Lower half block
            .Replace('▀', (char)223)  // Upper half block
            .Replace('╗', (char)187)  // Box drawing double
            .Replace('║', (char)186)  // Box drawing double
            .Replace('╝', (char)188)  // Box drawing double
            .Replace('╚', (char)200)  // Box drawing double
            .Replace('╔', (char)201)  // Box drawing double
            .Replace('═', (char)205)  // Box drawing double
            .Replace('╩', (char)202)  // Box drawing double
            .Replace('╦', (char)203)  // Box drawing double
            .Replace('╠', (char)204)  // Box drawing double
            .Replace('╣', (char)185)  // Box drawing double
            .Replace('╬', (char)206)  // Box drawing double
            .Replace('└', (char)192)  // Box drawing single
            .Replace('┘', (char)217)  // Box drawing single
            .Replace('┌', (char)218)  // Box drawing single
            .Replace('┐', (char)191)  // Box drawing single
            .Replace('├', (char)195)  // Box drawing single
            .Replace('┤', (char)180)  // Box drawing single
            .Replace('┬', (char)194)  // Box drawing single
            .Replace('┴', (char)193)  // Box drawing single
            .Replace('┼', (char)197)  // Box drawing single
            .Replace('─', (char)196)  // Box drawing single
            .Replace('│', (char)179); // Box drawing single

        return result;
    }

    private static void SavePattern(StructureDefinition structure, string zoom, List<string> lines)
    {
        if (lines.Count == 0) return;

        var pattern = string.Join("\n", lines);

        // Convert Unicode characters to extended ASCII equivalents for font compatibility
        pattern = ConvertUnicodeToExtendedAscii(pattern);

        var zoomPattern = new ZoomPattern { Pattern = pattern, Important = false };

        switch (zoom)
        {
            case "25ft":
                structure.Pattern25ft = zoomPattern;
                break;
            case "50ft":
                structure.Pattern50ft = zoomPattern;
                break;
            case "100ft":
                structure.Pattern100ft = zoomPattern;
                break;
            case "200ft":
                structure.Pattern200ft = zoomPattern;
                break;
            case "400ft":
                structure.Pattern400ft = zoomPattern;
                break;
        }
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
            _ => Color.White
        };
    }
}
