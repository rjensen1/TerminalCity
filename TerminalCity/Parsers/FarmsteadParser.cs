namespace TerminalCity.Parsers;

using TerminalCity.Domain;

/// <summary>
/// Parses farmstead definitions from text files
/// </summary>
public static class FarmsteadParser
{
    public static FarmsteadTemplate? LoadFromFile(string filePath)
    {
        if (!File.Exists(filePath))
            return null;

        var lines = File.ReadAllLines(filePath);
        FarmsteadTemplate? template = null;
        string currentSection = "";
        List<string> mapLines = new();
        Dictionary<char, string> legend = new();

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

                if (currentSection == "farmstead")
                {
                    // Save previous template if exists
                    if (template != null)
                    {
                        // Set the map and legend before returning
                        template.MapRows = mapLines;
                        template.Legend = legend;
                        return template; // Return first one for now
                    }

                    template = new FarmsteadTemplate();
                    mapLines.Clear();
                    legend.Clear();
                }
                continue;
            }

            if (template == null) continue;

            // Parse based on current section
            if (currentSection == "farmstead")
            {
                var parts = trimmed.Split(':', 2);
                if (parts.Length != 2) continue;

                var key = parts[0].Trim().ToLower();
                var value = parts[1].Trim();

                switch (key)
                {
                    case "id":
                        template.Id = value;
                        break;
                    case "name":
                        template.Name = value;
                        break;
                    case "size":
                        var sizeParts = value.Split('x');
                        if (sizeParts.Length == 2)
                        {
                            template.Width = int.Parse(sizeParts[0]);
                            template.Height = int.Parse(sizeParts[1]);
                        }
                        break;
                    case "description":
                        template.Description = value;
                        break;
                    case "map":
                        currentSection = "map";
                        break;
                    case "legend":
                        currentSection = "legend";
                        break;
                }
            }
            else if (currentSection == "map")
            {
                // Check if this line starts a new section
                if (trimmed.Contains(':'))
                {
                    currentSection = "farmstead";
                    // Re-process this line
                    var parts = trimmed.Split(':', 2);
                    if (parts.Length == 2 && parts[0].Trim().ToLower() == "legend")
                    {
                        currentSection = "legend";
                    }
                }
                else
                {
                    // Map row
                    mapLines.Add(trimmed);
                }
            }
            else if (currentSection == "legend")
            {
                // Parse legend entry: "H = farmhouse"
                var parts = trimmed.Split('=', 2);
                if (parts.Length == 2)
                {
                    var symbol = parts[0].Trim();
                    var tileType = parts[1].Trim();
                    if (symbol.Length == 1)
                    {
                        legend[symbol[0]] = tileType;
                    }
                }
            }
        }

        // Set the map and legend on the template
        if (template != null)
        {
            template.MapRows = mapLines;
            template.Legend = legend;
        }

        return template;
    }
}
