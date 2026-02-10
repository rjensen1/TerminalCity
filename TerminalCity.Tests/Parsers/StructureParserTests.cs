using TerminalCity.Parsers;
using Xunit;

namespace TerminalCity.Tests.Parsers;

public class StructureParserTests
{
    [Fact]
    public void LoadFromFile_ToolShed_ParsesBasicFields()
    {
        // Arrange
        var structuresPath = Path.Combine("definitions", "outbuildings", "outbuildings_structures.txt");

        // Act
        var structures = StructureParser.LoadFromFile(structuresPath);
        var toolShed = structures.FirstOrDefault(s => s.Id == "tool_shed");

        // Assert
        Assert.NotNull(toolShed);
        Assert.Equal("tool_shed", toolShed.Id);
        Assert.Equal("Tool Shed", toolShed.Name);
        Assert.Equal("storage", toolShed.Type);
        Assert.Equal("shed", toolShed.VariantOf);
        Assert.Equal("1x1", toolShed.Size);
    }

    [Fact]
    public void LoadFromFile_ToolShed_ParsesPattern25ft()
    {
        // Arrange
        var structuresPath = Path.Combine("definitions", "outbuildings", "outbuildings_structures.txt");

        // Act
        var structures = StructureParser.LoadFromFile(structuresPath);
        var toolShed = structures.FirstOrDefault(s => s.Id == "tool_shed");

        // Assert
        Assert.NotNull(toolShed);
        Assert.NotNull(toolShed.Pattern25ft);
        Assert.Equal("▐", toolShed.Pattern25ft.Pattern);
    }

    [Fact]
    public void LoadFromFile_ToolShed_ParsesPattern50ft()
    {
        // Arrange
        var structuresPath = Path.Combine("definitions", "outbuildings", "outbuildings_structures.txt");

        // Act
        var structures = StructureParser.LoadFromFile(structuresPath);
        var toolShed = structures.FirstOrDefault(s => s.Id == "tool_shed");

        // Assert
        Assert.NotNull(toolShed);
        Assert.NotNull(toolShed.Pattern50ft);
        Assert.Equal("║", toolShed.Pattern50ft.Pattern);
        Assert.False(toolShed.Pattern50ft.Important);
    }

    [Fact]
    public void LoadFromFile_ToolShed_NoPattern100ft()
    {
        // Arrange
        var structuresPath = Path.Combine("definitions", "outbuildings", "outbuildings_structures.txt");

        // Act
        var structures = StructureParser.LoadFromFile(structuresPath);
        var toolShed = structures.FirstOrDefault(s => s.Id == "tool_shed");

        // Assert
        Assert.NotNull(toolShed);
        Assert.Null(toolShed.Pattern100ft);
        Assert.Null(toolShed.Pattern200ft);
        Assert.Null(toolShed.Pattern400ft);
    }

    [Fact]
    public void LoadFromFile_NonExistentFile_ReturnsEmptyList()
    {
        // Arrange
        var structuresPath = "nonexistent.txt";

        // Act
        var structures = StructureParser.LoadFromFile(structuresPath);

        // Assert
        Assert.Empty(structures);
    }
}
