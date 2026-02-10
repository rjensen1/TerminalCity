using TerminalCity.Domain;
using TerminalCity.Parsers;
using Xunit;

namespace TerminalCity.Tests.Parsers;

public class BuildingParserTests
{
    [Fact]
    public void LoadFromFile_TinyFarmhouse_ParsesBasicFields()
    {
        // Arrange
        var buildingsPath = Path.Combine("definitions", "buildings", "buildings_residential.txt");

        // Act
        var buildings = BuildingParser.LoadFromFile(buildingsPath);
        var tinyFarmhouse = buildings.FirstOrDefault(b => b.Id == "tiny_farmhouse");

        // Assert
        Assert.NotNull(tinyFarmhouse);
        Assert.Equal("tiny_farmhouse", tinyFarmhouse.Id);
        Assert.Equal("Tiny Farmhouse", tinyFarmhouse.Name);
        Assert.Equal("Residential", tinyFarmhouse.Type);
        Assert.Equal(2, tinyFarmhouse.Width);
        Assert.Equal(2, tinyFarmhouse.Height);
    }

    [Fact]
    public void LoadFromFile_TinyFarmhouse_ParsesPattern25ft()
    {
        // Arrange
        var buildingsPath = Path.Combine("definitions", "buildings", "buildings_residential.txt");

        // Act
        var buildings = BuildingParser.LoadFromFile(buildingsPath);
        var tinyFarmhouse = buildings.FirstOrDefault(b => b.Id == "tiny_farmhouse");

        // Assert
        Assert.NotNull(tinyFarmhouse);
        Assert.NotNull(tinyFarmhouse.Pattern25ft);
        Assert.Contains("⌂", tinyFarmhouse.Pattern25ft.Pattern);
        Assert.Contains("\n", tinyFarmhouse.Pattern25ft.Pattern); // Multi-line pattern
    }

    [Fact]
    public void LoadFromFile_TinyFarmhouse_ParsesPattern50ft()
    {
        // Arrange
        var buildingsPath = Path.Combine("definitions", "buildings", "buildings_residential.txt");

        // Act
        var buildings = BuildingParser.LoadFromFile(buildingsPath);
        var tinyFarmhouse = buildings.FirstOrDefault(b => b.Id == "tiny_farmhouse");

        // Assert
        Assert.NotNull(tinyFarmhouse);
        Assert.NotNull(tinyFarmhouse.Pattern50ft);
        Assert.Equal("⌂", tinyFarmhouse.Pattern50ft.Pattern);
        Assert.True(tinyFarmhouse.Pattern50ft.Important);
    }

    [Fact]
    public void LoadFromFile_TinyFarmhouse_ParsesAllZoomPatterns()
    {
        // Arrange
        var buildingsPath = Path.Combine("definitions", "buildings", "buildings_residential.txt");

        // Act
        var buildings = BuildingParser.LoadFromFile(buildingsPath);
        var tinyFarmhouse = buildings.FirstOrDefault(b => b.Id == "tiny_farmhouse");

        // Assert
        Assert.NotNull(tinyFarmhouse);
        Assert.NotNull(tinyFarmhouse.Pattern25ft);
        Assert.NotNull(tinyFarmhouse.Pattern50ft);
        Assert.NotNull(tinyFarmhouse.Pattern100ft);
        Assert.NotNull(tinyFarmhouse.Pattern200ft);
        Assert.NotNull(tinyFarmhouse.Pattern400ft);
    }

    [Fact]
    public void LoadFromFile_TinyFarmhouse_ParsesImportanceFlags()
    {
        // Arrange
        var buildingsPath = Path.Combine("definitions", "buildings", "buildings_residential.txt");

        // Act
        var buildings = BuildingParser.LoadFromFile(buildingsPath);
        var tinyFarmhouse = buildings.FirstOrDefault(b => b.Id == "tiny_farmhouse");

        // Assert
        Assert.NotNull(tinyFarmhouse);
        Assert.True(tinyFarmhouse.Pattern50ft?.Important);
        Assert.True(tinyFarmhouse.Pattern100ft?.Important);
        Assert.True(tinyFarmhouse.Pattern200ft?.Important);
        Assert.False(tinyFarmhouse.Pattern400ft?.Important);
    }

    [Fact]
    public void LoadFromFile_NonExistentFile_ReturnsEmptyList()
    {
        // Arrange
        var buildingsPath = "nonexistent.txt";

        // Act
        var buildings = BuildingParser.LoadFromFile(buildingsPath);

        // Assert
        Assert.Empty(buildings);
    }

    [Fact]
    public void ZoomPattern_GetCharAt_ReturnsCorrectCharacter()
    {
        // Arrange
        var pattern = new ZoomPattern
        {
            Pattern = "⌂╗\n└╝",
            Important = true
        };

        // Act & Assert
        Assert.Equal('⌂', pattern.GetCharAt(0, 0));
        Assert.Equal('╗', pattern.GetCharAt(1, 0));
        Assert.Equal('└', pattern.GetCharAt(0, 1));
        Assert.Equal('╝', pattern.GetCharAt(1, 1));
    }

    [Fact]
    public void ZoomPattern_GetWidth_ReturnsCorrectWidth()
    {
        // Arrange
        var pattern = new ZoomPattern
        {
            Pattern = "⌂╗\n└╝",
            Important = true
        };

        // Act
        var width = pattern.GetWidth();

        // Assert
        Assert.Equal(2, width);
    }

    [Fact]
    public void ZoomPattern_GetHeight_ReturnsCorrectHeight()
    {
        // Arrange
        var pattern = new ZoomPattern
        {
            Pattern = "⌂╗\n└╝",
            Important = true
        };

        // Act
        var height = pattern.GetHeight();

        // Assert
        Assert.Equal(2, height);
    }
}
