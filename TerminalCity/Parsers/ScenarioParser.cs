using TerminalCity.Domain;

namespace TerminalCity.Parsers;

/// <summary>
/// Parses scenario definition files
/// </summary>
public static class ScenarioParser
{
    /// <summary>
    /// Load scenario from file
    /// </summary>
    public static Scenario? LoadFromFile(string filePath)
    {
        if (!File.Exists(filePath))
            return null;

        var lines = File.ReadAllLines(filePath);
        var scenario = new Scenario();
        string? currentSection = null;
        var descriptionLines = new List<string>();
        var backstoryLines = new List<string>();

        foreach (var rawLine in lines)
        {
            var line = rawLine.Trim();

            // Skip comments and empty lines
            if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
                continue;

            // Section headers
            if (line.StartsWith("[") && line.EndsWith("]"))
            {
                // Save accumulated description/backstory
                if (currentSection == "description" && descriptionLines.Any())
                {
                    scenario.Description = string.Join(" ", descriptionLines);
                    descriptionLines.Clear();
                }
                if (currentSection == "backstory" && backstoryLines.Any())
                {
                    scenario.Backstory = string.Join(" ", backstoryLines);
                    backstoryLines.Clear();
                }

                currentSection = line.Trim('[', ']').ToLower();
                continue;
            }

            // Parse based on current section
            switch (currentSection)
            {
                case "scenario":
                    ParseScenarioProperty(line, scenario);
                    break;

                case "description":
                    descriptionLines.Add(line);
                    break;

                case "backstory":
                    backstoryLines.Add(line);
                    break;

                case "starting_conditions":
                    ParseStartingConditions(line, scenario);
                    break;

                case "parent_city":
                    ParseParentCity(line, scenario);
                    break;

                case "objectives":
                    ParseObjectives(line, scenario);
                    break;

                case "challenges":
                    if (line.StartsWith("-"))
                        scenario.Challenges.Add(line.Substring(1).Trim());
                    break;

                case "initial_demand":
                    ParseInitialDemand(line, scenario);
                    break;

                case "terrain_generation":
                    ParseTerrainGeneration(line, scenario);
                    break;

                case "road_grid":
                    ParseRoadGrid(line, scenario);
                    break;

                case "initial_land_use":
                    ParseInitialLandUse(line, scenario);
                    break;

                case "camera":
                    ParseCamera(line, scenario);
                    break;
            }
        }

        // Save final description/backstory
        if (descriptionLines.Any())
            scenario.Description = string.Join(" ", descriptionLines);
        if (backstoryLines.Any())
            scenario.Backstory = string.Join(" ", backstoryLines);

        return scenario;
    }

    private static void ParseScenarioProperty(string line, Scenario scenario)
    {
        var parts = line.Split(':', 2, StringSplitOptions.TrimEntries);
        if (parts.Length != 2) return;

        var key = parts[0].ToLower();
        var value = parts[1];

        switch (key)
        {
            case "id":
                scenario.Id = value;
                break;
            case "name":
                scenario.Name = value;
                break;
            case "game_type":
                scenario.GameType = value;
                break;
            case "era":
                if (int.TryParse(value, out var era))
                    scenario.Era = era;
                break;
            case "latitude":
                if (int.TryParse(value, out var lat))
                    scenario.Latitude = lat;
                break;
            case "climate_zone":
                scenario.ClimateZone = value;
                break;
        }
    }

    private static void ParseStartingConditions(string line, Scenario scenario)
    {
        var parts = line.Split(':', 2, StringSplitOptions.TrimEntries);
        if (parts.Length != 2) return;

        var key = parts[0].ToLower();
        var value = parts[1];

        switch (key)
        {
            case "year":
                if (int.TryParse(value, out var year))
                    scenario.StartYear = year;
                break;
            case "population":
                if (int.TryParse(value, out var pop))
                    scenario.StartPopulation = pop;
                break;
            case "money":
                if (int.TryParse(value, out var money))
                    scenario.StartMoney = money;
                break;
            case "map_size":
                scenario.MapSize = value;
                break;
        }
    }

    private static void ParseParentCity(string line, Scenario scenario)
    {
        var parts = line.Split(':', 2, StringSplitOptions.TrimEntries);
        if (parts.Length != 2) return;

        var key = parts[0].ToLower();
        var value = parts[1];

        switch (key)
        {
            case "name":
                scenario.ParentCityName = value;
                break;
            case "distance":
                scenario.ParentCityDistance = value;
                break;
            case "population":
                if (int.TryParse(value, out var pop))
                    scenario.ParentCityPopulation = pop;
                break;
            case "description":
                scenario.ParentCityDescription = value;
                break;
        }
    }

    private static void ParseObjectives(string line, Scenario scenario)
    {
        var parts = line.Split(':', 2, StringSplitOptions.TrimEntries);
        if (parts.Length != 2) return;

        var key = parts[0].ToLower();
        var value = parts[1];

        switch (key)
        {
            case "primary":
                scenario.PrimaryObjective = value;
                break;
            case "secondary":
                scenario.SecondaryObjective = value;
                break;
            case "optional":
                scenario.OptionalObjective = value;
                break;
        }
    }

    private static void ParseInitialDemand(string line, Scenario scenario)
    {
        var parts = line.Split(':', 2, StringSplitOptions.TrimEntries);
        if (parts.Length != 2) return;

        var key = parts[0].ToLower();
        var valueStr = parts[1].Split('#')[0].Trim(); // Remove comments

        if (!int.TryParse(valueStr, out var value)) return;

        switch (key)
        {
            case "residential":
                scenario.ResidentialDemand = value;
                break;
            case "commercial":
                scenario.CommercialDemand = value;
                break;
            case "industrial":
                scenario.IndustrialDemand = value;
                break;
            case "farm":
                scenario.FarmDemand = value;
                break;
        }
    }

    private static void ParseTerrainGeneration(string line, Scenario scenario)
    {
        // Handle feature list items first (before splitting on colon)
        if (line.Trim().StartsWith("-"))
        {
            var feature = line.Trim().Substring(1).Trim().Split('#')[0].Trim();
            scenario.TerrainFeatures.Add(feature);
            return;
        }

        var parts = line.Split(':', 2, StringSplitOptions.TrimEntries);
        if (parts.Length != 2) return;

        var key = parts[0].ToLower();
        var value = parts[1].Split('#')[0].Trim(); // Remove comments

        switch (key)
        {
            case "type":
                scenario.TerrainType = value;
                break;
            case "features":
                // Skip - features are on separate lines with '-'
                break;
        }
    }

    private static void ParseRoadGrid(string line, Scenario scenario)
    {
        var parts = line.Split(':', 2, StringSplitOptions.TrimEntries);
        if (parts.Length != 2) return;

        var key = parts[0].ToLower();
        var valueStr = parts[1].Split('#')[0].Trim(); // Remove comments

        switch (key)
        {
            case "spacing":
                if (int.TryParse(valueStr, out var spacing))
                    scenario.RoadGridSpacing = spacing;
                break;
            case "type":
                scenario.RoadGridType = valueStr;
                break;
            case "pattern":
                scenario.RoadGridPattern = valueStr;
                break;
            case "main_vertical_road":
                scenario.HasMainVerticalRoad = valueStr.ToLower() == "true";
                break;
            case "main_horizontal_road":
                scenario.HasMainHorizontalRoad = valueStr.ToLower() == "true";
                break;
        }
    }

    private static void ParseCamera(string line, Scenario scenario)
    {
        var parts = line.Split(':', 2, StringSplitOptions.TrimEntries);
        if (parts.Length != 2) return;

        var key = parts[0].ToLower();
        var valueStr = parts[1].Split('#')[0].Trim(); // Remove comments

        switch (key)
        {
            case "start_position":
                scenario.CameraStartPosition = valueStr;
                break;
        }
    }

    private static void ParseInitialLandUse(string line, Scenario scenario)
    {
        var parts = line.Split(':', 2, StringSplitOptions.TrimEntries);
        if (parts.Length != 2) return;

        var key = parts[0].ToLower();
        var valueStr = parts[1].Split('#')[0].Trim().TrimEnd('%'); // Remove comments and %

        if (!int.TryParse(valueStr, out var value)) return;

        switch (key)
        {
            case "farm":
                scenario.InitialFarmPercent = value;
                break;
            case "road":
                scenario.InitialRoadPercent = value;
                break;
            case "trees":
                scenario.InitialTreesPercent = value;
                break;
            case "empty":
                scenario.InitialEmptyPercent = value;
                break;
        }
    }
}
