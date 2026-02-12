using TerminalCity.Domain;
using TerminalCity.Parsers;
using Xunit;
using SadRogue.Primitives;

namespace TerminalCity.Tests.Parsers;

/// <summary>
/// Classical unit tests for CropParser
/// Tests definition file loading, validation, and error handling
/// </summary>
public class CropParserTests
{
    [Fact]
    public void LoadFromFile_ValidCropsFile_LoadsAllCrops()
    {
        // Arrange
        var cropsPath = Path.Combine("definitions", "crops", "crops.txt");

        // Act
        var crops = CropParser.LoadFromFile(cropsPath);

        // Assert
        Assert.NotEmpty(crops);
        Assert.Contains(crops, c => c.Id == "fallow_plowed");
        Assert.Contains(crops, c => c.Id == "fallow_unplowed");
        Assert.Contains(crops, c => c.Id == "tall_grass");
        Assert.Contains(crops, c => c.Id == "wheat");
        Assert.Contains(crops, c => c.Id == "corn");
    }

    [Fact]
    public void LoadFromFile_FallowPlowed_HasCorrectProperties()
    {
        // Arrange
        var cropsPath = Path.Combine("definitions", "crops", "crops.txt");

        // Act
        var crops = CropParser.LoadFromFile(cropsPath);
        var fallow = crops.FirstOrDefault(c => c.Id == "fallow_plowed");

        // Assert
        Assert.NotNull(fallow);
        Assert.Equal("fallow_plowed", fallow.Id);
        Assert.Equal("Fallow (Plowed)", fallow.Name);
        Assert.NotEmpty(fallow.Description);
        Assert.Equal(Color.SaddleBrown, fallow.Color);
        Assert.Equal(Color.Peru, fallow.BackgroundColor);
    }

    [Fact]
    public void LoadFromFile_AllCrops_HaveZoomPatterns()
    {
        // Arrange
        var cropsPath = Path.Combine("definitions", "crops", "crops.txt");

        // Act
        var crops = CropParser.LoadFromFile(cropsPath);

        // Assert
        foreach (var crop in crops)
        {
            Assert.NotNull(crop.Pattern25ft);
            Assert.NotNull(crop.Pattern50ft);
            Assert.NotNull(crop.Pattern100ft);
            Assert.NotNull(crop.Pattern200ft);
            Assert.NotNull(crop.Pattern400ft);

            Assert.NotEmpty(crop.Pattern25ft.Pattern);
            Assert.NotEmpty(crop.Pattern50ft.Pattern);
            Assert.NotEmpty(crop.Pattern100ft.Pattern);
            Assert.NotEmpty(crop.Pattern200ft.Pattern);
            Assert.NotEmpty(crop.Pattern400ft.Pattern);
        }
    }

    [Fact]
    public void LoadFromFile_AllCrops_HaveValidColors()
    {
        // Arrange
        var cropsPath = Path.Combine("definitions", "crops", "crops.txt");

        // Act
        var crops = CropParser.LoadFromFile(cropsPath);

        // Assert
        foreach (var crop in crops)
        {
            // Colors should not be default/transparent
            Assert.NotEqual(Color.Transparent, crop.Color);
            Assert.NotEqual(Color.Transparent, crop.BackgroundColor);

            // Should have valid RGB values
            Assert.InRange(crop.Color.R, (byte)0, (byte)255);
            Assert.InRange(crop.Color.G, (byte)0, (byte)255);
            Assert.InRange(crop.Color.B, (byte)0, (byte)255);
        }
    }

    [Fact]
    public void LoadFromFile_AllCrops_HaveRequiredFields()
    {
        // Arrange
        var cropsPath = Path.Combine("definitions", "crops", "crops.txt");

        // Act
        var crops = CropParser.LoadFromFile(cropsPath);

        // Assert - Validate all required fields are present
        foreach (var crop in crops)
        {
            Assert.NotNull(crop.Id);
            Assert.NotEmpty(crop.Id);

            Assert.NotNull(crop.Name);
            Assert.NotEmpty(crop.Name);

            Assert.NotNull(crop.Description);
            Assert.NotEmpty(crop.Description);
        }
    }

    [Fact]
    public void LoadFromFile_AllCrops_HaveUniqueIds()
    {
        // Arrange
        var cropsPath = Path.Combine("definitions", "crops", "crops.txt");

        // Act
        var crops = CropParser.LoadFromFile(cropsPath);
        var ids = crops.Select(c => c.Id).ToList();

        // Assert
        var distinctIds = ids.Distinct().ToList();
        Assert.Equal(ids.Count, distinctIds.Count); // No duplicate IDs
    }

    [Fact]
    public void LoadFromFile_NonExistentFile_ReturnsEmptyList()
    {
        // Arrange
        var fakePath = "nonexistent/path/crops.txt";

        // Act
        var crops = CropParser.LoadFromFile(fakePath);

        // Assert
        Assert.NotNull(crops);
        Assert.Empty(crops);
    }

    [Fact]
    public void LoadFromFile_Wheat_HasGoldenColors()
    {
        // Arrange
        var cropsPath = Path.Combine("definitions", "crops", "crops.txt");

        // Act
        var crops = CropParser.LoadFromFile(cropsPath);
        var wheat = crops.FirstOrDefault(c => c.Id == "wheat");

        // Assert
        Assert.NotNull(wheat);
        Assert.Equal(Color.Gold, wheat.Color);
        Assert.Equal(Color.Goldenrod, wheat.BackgroundColor);
    }

    [Fact]
    public void LoadFromFile_Corn_HasGreenColors()
    {
        // Arrange
        var cropsPath = Path.Combine("definitions", "crops", "crops.txt");

        // Act
        var crops = CropParser.LoadFromFile(cropsPath);
        var corn = crops.FirstOrDefault(c => c.Id == "corn");

        // Assert
        Assert.NotNull(corn);
        Assert.Equal(Color.GreenYellow, corn.Color);
        Assert.Equal(Color.DarkGreen, corn.BackgroundColor);
    }

    [Fact]
    public void ZoomPattern_AllLevels_SingleCharacter()
    {
        // Arrange
        var cropsPath = Path.Combine("definitions", "crops", "crops.txt");

        // Act
        var crops = CropParser.LoadFromFile(cropsPath);

        // Assert - Crop patterns should be single characters (for now)
        foreach (var crop in crops)
        {
            Assert.Equal(1, crop.Pattern25ft!.Pattern.Length);
            Assert.Equal(1, crop.Pattern50ft!.Pattern.Length);
            Assert.Equal(1, crop.Pattern100ft!.Pattern.Length);
            Assert.Equal(1, crop.Pattern200ft!.Pattern.Length);
            Assert.Equal(1, crop.Pattern400ft!.Pattern.Length);
        }
    }
}
