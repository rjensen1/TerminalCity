using TerminalCity.Domain;
using TerminalCity.Parsers;
using Xunit;
using SadRogue.Primitives;

namespace TerminalCity.Tests.Parsers;

/// <summary>
/// Classical unit tests for BorderParser
/// Tests definition file loading, validation, and error handling
/// </summary>
public class BorderParserTests
{
    [Fact]
    public void LoadFromFile_ValidBordersFile_LoadsAllBorders()
    {
        // Arrange
        var bordersPath = Path.Combine("definitions", "borders", "border_definitions.txt");

        // Act
        var borders = BorderParser.LoadFromFile(bordersPath);

        // Assert
        Assert.NotEmpty(borders);
        Assert.Contains(borders, b => b.Id == "fence");
        Assert.Contains(borders, b => b.Id == "trees");
        Assert.Contains(borders, b => b.Id == "dirt");
        Assert.Contains(borders, b => b.Id == "tall_grass");
        Assert.Contains(borders, b => b.Id == "ditch");
        Assert.Contains(borders, b => b.Id == "stacked_stones");
    }

    [Fact]
    public void LoadFromFile_Fence_HasCorrectProperties()
    {
        // Arrange
        var bordersPath = Path.Combine("definitions", "borders", "border_definitions.txt");

        // Act
        var borders = BorderParser.LoadFromFile(bordersPath);
        var fence = borders.FirstOrDefault(b => b.Id == "fence");

        // Assert
        Assert.NotNull(fence);
        Assert.Equal("fence", fence.Id);
        Assert.Equal("Split-rail Fence", fence.Name);
        Assert.NotEmpty(fence.Description);
        Assert.Equal(Color.SaddleBrown, fence.Color);
        Assert.Equal(Color.Transparent, fence.BackgroundColor);
    }

    [Fact]
    public void LoadFromFile_AllBorders_HaveRequiredFields()
    {
        // Arrange
        var bordersPath = Path.Combine("definitions", "borders", "border_definitions.txt");

        // Act
        var borders = BorderParser.LoadFromFile(bordersPath);

        // Assert - Validate all required fields are present
        foreach (var border in borders)
        {
            Assert.NotNull(border.Id);
            Assert.NotEmpty(border.Id);

            Assert.NotNull(border.Name);
            Assert.NotEmpty(border.Name);

            Assert.NotNull(border.Description);
            Assert.NotEmpty(border.Description);
        }
    }

    [Fact]
    public void LoadFromFile_AllBorders_HaveUniqueIds()
    {
        // Arrange
        var bordersPath = Path.Combine("definitions", "borders", "border_definitions.txt");

        // Act
        var borders = BorderParser.LoadFromFile(bordersPath);
        var ids = borders.Select(b => b.Id).ToList();

        // Assert
        var distinctIds = ids.Distinct().ToList();
        Assert.Equal(ids.Count, distinctIds.Count); // No duplicate IDs
    }

    [Fact]
    public void LoadFromFile_NonExistentFile_ReturnsEmptyList()
    {
        // Arrange
        var fakePath = "nonexistent/path/borders.txt";

        // Act
        var borders = BorderParser.LoadFromFile(fakePath);

        // Assert
        Assert.NotNull(borders);
        Assert.Empty(borders);
    }

    [Fact]
    public void LoadFromFile_Fence_VisibleOnlyAtCloseZoom()
    {
        // Arrange
        var bordersPath = Path.Combine("definitions", "borders", "border_definitions.txt");

        // Act
        var borders = BorderParser.LoadFromFile(bordersPath);
        var fence = borders.FirstOrDefault(b => b.Id == "fence");

        // Assert - Fence should be visible at 25ft/50ft but not at far zoom
        Assert.NotNull(fence);
        Assert.NotNull(fence.Pattern25ft);
        Assert.NotNull(fence.Pattern50ft);

        // Check far zoom returns null (invisible)
        Assert.Null(fence.GetPatternForZoom(-1, BorderSides.North));  // 200ft - invisible
        Assert.Null(fence.GetPatternForZoom(-2, BorderSides.North));  // 400ft - invisible

        // Check directional patterns work at close zoom (converted to CP437)
        Assert.Equal((char)196, fence.GetPatternForZoom(2, BorderSides.North));  // ─ → CP437 196 (horizontal)
        Assert.Equal((char)179, fence.GetPatternForZoom(2, BorderSides.East));   // │ → CP437 179 (vertical)
    }

    [Fact]
    public void LoadFromFile_Trees_VisibleAtAllZoomLevels()
    {
        // Arrange
        var bordersPath = Path.Combine("definitions", "borders", "border_definitions.txt");

        // Act
        var borders = BorderParser.LoadFromFile(bordersPath);
        var trees = borders.FirstOrDefault(b => b.Id == "trees");

        // Assert - Trees should be visible at all zoom levels
        Assert.NotNull(trees);
        Assert.NotNull(trees.Pattern25ft);
        Assert.NotNull(trees.Pattern50ft);
        Assert.NotNull(trees.Pattern100ft);
        Assert.NotNull(trees.Pattern200ft);
        Assert.NotNull(trees.Pattern400ft);
    }

    [Fact]
    public void BorderDefinition_GetPatternForZoom_ReturnsCorrectPattern()
    {
        // Arrange
        var border = new BorderDefinition
        {
            Pattern25ft = new BorderPatternSet { Default = 'A' },
            Pattern50ft = new BorderPatternSet { Default = 'B' },
            Pattern100ft = new BorderPatternSet { Default = 'C' },
            Pattern200ft = new BorderPatternSet { Default = 'D' },
            Pattern400ft = new BorderPatternSet { Default = 'E' }
        };

        // Act & Assert
        Assert.Equal('A', border.GetPatternForZoom(2, BorderSides.North));   // 25ft
        Assert.Equal('B', border.GetPatternForZoom(1, BorderSides.South));   // 50ft
        Assert.Equal('C', border.GetPatternForZoom(0, BorderSides.East));   // 100ft
        Assert.Equal('D', border.GetPatternForZoom(-1, BorderSides.West));  // 200ft
        Assert.Equal('E', border.GetPatternForZoom(-2, BorderSides.North));  // 400ft
    }

    [Fact]
    public void BorderDefinition_GetPatternForZoom_InvalidZoom_ReturnsNull()
    {
        // Arrange
        var border = new BorderDefinition { Pattern25ft = new BorderPatternSet { Default = 'A' } };

        // Act & Assert
        Assert.Null(border.GetPatternForZoom(99, BorderSides.North));   // Invalid zoom
        Assert.Null(border.GetPatternForZoom(-99, BorderSides.South));  // Invalid zoom
    }

    [Fact]
    public void BorderDefinition_DirectionalPatterns_ReturnsCorrectCharacterPerSide()
    {
        // Arrange - Fence with horizontal/vertical patterns
        var border = new BorderDefinition
        {
            Pattern25ft = new BorderPatternSet
            {
                North = '─',  // Horizontal
                South = '─',  // Horizontal
                East = '│',   // Vertical
                West = '│'    // Vertical
            }
        };

        // Act & Assert
        Assert.Equal('─', border.GetPatternForZoom(2, BorderSides.North));
        Assert.Equal('─', border.GetPatternForZoom(2, BorderSides.South));
        Assert.Equal('│', border.GetPatternForZoom(2, BorderSides.East));
        Assert.Equal('│', border.GetPatternForZoom(2, BorderSides.West));
    }

    [Fact]
    public void LoadFromFile_Ditch_HasWaterColors()
    {
        // Arrange
        var bordersPath = Path.Combine("definitions", "borders", "border_definitions.txt");

        // Act
        var borders = BorderParser.LoadFromFile(bordersPath);
        var ditch = borders.FirstOrDefault(b => b.Id == "ditch");

        // Assert
        Assert.NotNull(ditch);
        Assert.Equal(Color.DarkBlue, ditch.Color);
        Assert.Equal(Color.Blue, ditch.BackgroundColor);
    }
}
