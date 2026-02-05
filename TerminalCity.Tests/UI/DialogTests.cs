using SadRogue.Primitives;
using TerminalCity.UI;
using Xunit;

namespace TerminalCity.Tests.UI;

public class DialogTests
{
    [Fact]
    public void Dialog_InitializesWithTitle()
    {
        // Act
        var dialog = new Dialog("Test Title");

        // Assert
        Assert.Equal("Test Title", dialog.Title);
        Assert.Empty(dialog.Lines);
        Assert.Empty(dialog.Options);
        Assert.Equal(50, dialog.Width); // Default width
    }

    [Fact]
    public void AddLine_AddsLineToDialog()
    {
        // Arrange
        var dialog = new Dialog("Test");

        // Act
        dialog.AddLine("First line");
        dialog.AddLine("Second line");

        // Assert
        Assert.Equal(2, dialog.Lines.Count);
        Assert.Equal("First line", dialog.Lines[0]);
        Assert.Equal("Second line", dialog.Lines[1]);
    }

    [Fact]
    public void AddLine_ReturnsDialogForFluentAPI()
    {
        // Arrange
        var dialog = new Dialog("Test");

        // Act
        var result = dialog.AddLine("Test line");

        // Assert
        Assert.Same(dialog, result);
    }

    [Fact]
    public void AddOption_AddsOptionToDialog()
    {
        // Arrange
        var dialog = new Dialog("Test");

        // Act
        dialog.AddOption("Y", "Yes", Color.Green);
        dialog.AddOption("N", "No", Color.Red);

        // Assert
        Assert.Equal(2, dialog.Options.Count);
        Assert.Equal("Y", dialog.Options[0].Key);
        Assert.Equal("Yes", dialog.Options[0].Label);
        Assert.Equal(Color.Green, dialog.Options[0].Color);
        Assert.Equal("N", dialog.Options[1].Key);
        Assert.Equal("No", dialog.Options[1].Label);
        Assert.Equal(Color.Red, dialog.Options[1].Color);
    }

    [Fact]
    public void AddOption_DefaultsToWhiteColor()
    {
        // Arrange
        var dialog = new Dialog("Test");

        // Act
        dialog.AddOption("X", "Option");

        // Assert
        Assert.Equal(Color.White, dialog.Options[0].Color);
    }

    [Fact]
    public void AddOption_ReturnsDialogForFluentAPI()
    {
        // Arrange
        var dialog = new Dialog("Test");

        // Act
        var result = dialog.AddOption("Y", "Yes");

        // Assert
        Assert.Same(dialog, result);
    }

    [Fact]
    public void CalculateHeight_ReturnsCorrectHeightForEmptyDialog()
    {
        // Arrange
        var dialog = new Dialog("Test")
            .AddOption("OK", "OK");

        // Act
        int height = dialog.CalculateHeight();

        // Assert
        // Border (2) + Title (1) + Padding (1) + Lines (0) + Padding (1) + Options (1) + Padding (1) = 7
        Assert.Equal(7, height);
    }

    [Fact]
    public void CalculateHeight_IncludesAllLines()
    {
        // Arrange
        var dialog = new Dialog("Test")
            .AddLine("Line 1")
            .AddLine("Line 2")
            .AddLine("Line 3")
            .AddOption("OK", "OK");

        // Act
        int height = dialog.CalculateHeight();

        // Assert
        // Border (2) + Title (1) + Padding (1) + Lines (3) + Padding (1) + Options (1) + Padding (1) = 10
        Assert.Equal(10, height);
    }

    [Fact]
    public void CreateYesNo_CreatesDialogWithYesNoOptions()
    {
        // Act
        var dialog = Dialog.CreateYesNo("Confirm", "Are you sure?");

        // Assert
        Assert.Equal("Confirm", dialog.Title);
        Assert.Equal(2, dialog.Lines.Count);
        Assert.Equal("Are you sure?", dialog.Lines[0]);
        Assert.Equal("", dialog.Lines[1]); // Empty line for spacing
        Assert.Equal(2, dialog.Options.Count);
        Assert.Equal("Y", dialog.Options[0].Key);
        Assert.Equal("Yes", dialog.Options[0].Label);
        Assert.Equal(Color.Green, dialog.Options[0].Color);
        Assert.Equal("N", dialog.Options[1].Key);
        Assert.Equal("No", dialog.Options[1].Label);
        Assert.Equal(Color.Red, dialog.Options[1].Color);
    }

    [Fact]
    public void CreateOK_CreatesDialogWithOKOption()
    {
        // Act
        var dialog = Dialog.CreateOK("Alert", "Message 1", "Message 2");

        // Assert
        Assert.Equal("Alert", dialog.Title);
        Assert.Equal(3, dialog.Lines.Count); // 2 messages + empty line
        Assert.Equal("Message 1", dialog.Lines[0]);
        Assert.Equal("Message 2", dialog.Lines[1]);
        Assert.Equal("", dialog.Lines[2]);
        Assert.Single(dialog.Options);
        Assert.Equal("Enter", dialog.Options[0].Key);
        Assert.Equal("OK", dialog.Options[0].Label);
        Assert.Equal(Color.Cyan, dialog.Options[0].Color);
    }

    [Fact]
    public void CreateOK_HandlesMultipleMessages()
    {
        // Act
        var dialog = Dialog.CreateOK("Info", "Line 1", "Line 2", "Line 3", "Line 4");

        // Assert
        Assert.Equal(5, dialog.Lines.Count); // 4 messages + empty line
        Assert.Equal("Line 1", dialog.Lines[0]);
        Assert.Equal("Line 2", dialog.Lines[1]);
        Assert.Equal("Line 3", dialog.Lines[2]);
        Assert.Equal("Line 4", dialog.Lines[3]);
        Assert.Equal("", dialog.Lines[4]);
    }

    [Fact]
    public void CreateChoice_CreatesDialogWithMultipleOptions()
    {
        // Act
        var dialog = Dialog.CreateChoice(
            "Select Zone",
            "Choose a zone type:",
            ("R", "Residential"),
            ("C", "Commercial"),
            ("I", "Industrial")
        );

        // Assert
        Assert.Equal("Select Zone", dialog.Title);
        Assert.Equal(2, dialog.Lines.Count); // Message + empty line
        Assert.Equal("Choose a zone type:", dialog.Lines[0]);
        Assert.Equal("", dialog.Lines[1]);
        Assert.Equal(3, dialog.Options.Count);
        Assert.Equal("R", dialog.Options[0].Key);
        Assert.Equal("Residential", dialog.Options[0].Label);
        Assert.Equal("C", dialog.Options[1].Key);
        Assert.Equal("Commercial", dialog.Options[1].Label);
        Assert.Equal("I", dialog.Options[2].Key);
        Assert.Equal("Industrial", dialog.Options[2].Label);
    }

    [Fact]
    public void FluentAPI_CanChainMultipleCalls()
    {
        // Act
        var dialog = new Dialog("Chained")
            .AddLine("Line 1")
            .AddLine("Line 2")
            .AddOption("A", "Option A")
            .AddOption("B", "Option B");

        // Assert
        Assert.Equal("Chained", dialog.Title);
        Assert.Equal(2, dialog.Lines.Count);
        Assert.Equal(2, dialog.Options.Count);
    }

    [Fact]
    public void DialogOption_InitializesWithProperties()
    {
        // Act
        var option = new DialogOption
        {
            Key = "X",
            Label = "Exit",
            Color = Color.Yellow
        };

        // Assert
        Assert.Equal("X", option.Key);
        Assert.Equal("Exit", option.Label);
        Assert.Equal(Color.Yellow, option.Color);
    }

    [Fact]
    public void Dialog_CanHaveCustomWidth()
    {
        // Arrange
        var dialog = new Dialog("Test")
        {
            Width = 70
        };

        // Assert
        Assert.Equal(70, dialog.Width);
    }

    [Fact]
    public void CreateYesNo_WithLongMessage_CreatesProperlyFormattedDialog()
    {
        // Arrange
        string longMessage = "This is a much longer confirmation message that spans quite a few characters";

        // Act
        var dialog = Dialog.CreateYesNo("Long Confirm", longMessage);

        // Assert
        Assert.Equal("Long Confirm", dialog.Title);
        Assert.Contains(longMessage, dialog.Lines);
        Assert.Equal(2, dialog.Options.Count);
    }

    [Fact]
    public void Dialog_EmptyLinesAllowed()
    {
        // Act
        var dialog = new Dialog("Test")
            .AddLine("First")
            .AddLine("")
            .AddLine("Third");

        // Assert
        Assert.Equal(3, dialog.Lines.Count);
        Assert.Equal("", dialog.Lines[1]);
    }
}
