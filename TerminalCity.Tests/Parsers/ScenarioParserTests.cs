using TerminalCity.Parsers;
using Xunit;

namespace TerminalCity.Tests.Parsers;

public class ScenarioParserTests
{
    [Fact]
    public void LoadFromFile_BedroomCommunity_ParsesCorrectly()
    {
        // Arrange
        var scenarioPath = Path.Combine("definitions", "scenarios", "bedroom_community.txt");

        // Act
        var scenario = ScenarioParser.LoadFromFile(scenarioPath);

        // Assert
        Assert.NotNull(scenario);
        Assert.Equal("bedroom_community_1955", scenario.Id);
        Assert.Equal("Suburban Spillover, 1955", scenario.Name);
        Assert.Equal("traditional", scenario.GameType);
        Assert.Equal(1955, scenario.Era);
        Assert.Equal(1955, scenario.StartYear);
        Assert.Equal(25000, scenario.StartMoney);
        Assert.Equal(0, scenario.StartPopulation);
    }

    [Fact]
    public void LoadFromFile_BedroomCommunity_ParsesDescription()
    {
        // Arrange
        var scenarioPath = Path.Combine("definitions", "scenarios", "bedroom_community.txt");

        // Act
        var scenario = ScenarioParser.LoadFromFile(scenarioPath);

        // Assert
        Assert.NotNull(scenario);
        Assert.Contains("Metropolis", scenario.Description);
        Assert.Contains("suburban", scenario.Description.ToLower());
        Assert.NotEmpty(scenario.GetFormattedDescription());
    }

    [Fact]
    public void LoadFromFile_BedroomCommunity_ParsesParentCity()
    {
        // Arrange
        var scenarioPath = Path.Combine("definitions", "scenarios", "bedroom_community.txt");

        // Act
        var scenario = ScenarioParser.LoadFromFile(scenarioPath);

        // Assert
        Assert.NotNull(scenario);
        Assert.Equal("Metropolis", scenario.ParentCityName);
        Assert.Equal("15 miles", scenario.ParentCityDistance);
        Assert.Equal(500000, scenario.ParentCityPopulation);
    }

    [Fact]
    public void LoadFromFile_BedroomCommunity_ParsesObjectives()
    {
        // Arrange
        var scenarioPath = Path.Combine("definitions", "scenarios", "bedroom_community.txt");

        // Act
        var scenario = ScenarioParser.LoadFromFile(scenarioPath);

        // Assert
        Assert.NotNull(scenario);
        Assert.NotNull(scenario.PrimaryObjective);
        Assert.Contains("1000", scenario.PrimaryObjective);
        Assert.NotNull(scenario.SecondaryObjective);
        Assert.NotNull(scenario.OptionalObjective);
    }

    [Fact]
    public void LoadFromFile_BedroomCommunity_ParsesChallenges()
    {
        // Arrange
        var scenarioPath = Path.Combine("definitions", "scenarios", "bedroom_community.txt");

        // Act
        var scenario = ScenarioParser.LoadFromFile(scenarioPath);

        // Assert
        Assert.NotNull(scenario);
        Assert.NotEmpty(scenario.Challenges);
        Assert.True(scenario.Challenges.Count >= 3);
    }

    [Fact]
    public void LoadFromFile_NonExistentFile_ReturnsNull()
    {
        // Arrange
        var scenarioPath = "nonexistent.txt";

        // Act
        var scenario = ScenarioParser.LoadFromFile(scenarioPath);

        // Assert
        Assert.Null(scenario);
    }

    [Fact]
    public void LoadFromFile_BedroomCommunity_ParsesDemandLevels()
    {
        // Arrange
        var scenarioPath = Path.Combine("definitions", "scenarios", "bedroom_community.txt");

        // Act
        var scenario = ScenarioParser.LoadFromFile(scenarioPath);

        // Assert
        Assert.NotNull(scenario);
        Assert.Equal(85, scenario.ResidentialDemand);
        Assert.Equal(70, scenario.CommercialDemand);
        Assert.Equal(60, scenario.IndustrialDemand);
        Assert.Equal(10, scenario.FarmDemand);
    }

    [Fact]
    public void LoadFromFile_BedroomCommunity_ParsesTerrainGeneration()
    {
        // Arrange
        var scenarioPath = Path.Combine("definitions", "scenarios", "bedroom_community.txt");

        // Act
        var scenario = ScenarioParser.LoadFromFile(scenarioPath);

        // Assert
        Assert.NotNull(scenario);
        Assert.Equal("flat_farmland", scenario.TerrainType);
        Assert.Contains("existing_road_grid", scenario.TerrainFeatures);
        Assert.Contains("active_farms", scenario.TerrainFeatures);
    }

    [Fact]
    public void LoadFromFile_BedroomCommunity_ParsesRoadGrid()
    {
        // Arrange
        var scenarioPath = Path.Combine("definitions", "scenarios", "bedroom_community.txt");

        // Act
        var scenario = ScenarioParser.LoadFromFile(scenarioPath);

        // Assert
        Assert.NotNull(scenario);
        Assert.Equal(10, scenario.RoadGridSpacing);
        Assert.Equal("dirt_road", scenario.RoadGridType);
        Assert.Equal("grid", scenario.RoadGridPattern);
    }

    [Fact]
    public void LoadFromFile_BedroomCommunity_ParsesInitialLandUse()
    {
        // Arrange
        var scenarioPath = Path.Combine("definitions", "scenarios", "bedroom_community.txt");

        // Act
        var scenario = ScenarioParser.LoadFromFile(scenarioPath);

        // Assert
        Assert.NotNull(scenario);
        Assert.Equal(85, scenario.InitialFarmPercent);
        Assert.Equal(10, scenario.InitialRoadPercent);
        Assert.Equal(3, scenario.InitialTreesPercent);
        Assert.Equal(2, scenario.InitialEmptyPercent);
    }
}
