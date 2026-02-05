using SadConsole;
using SadRogue.Primitives;

namespace TerminalCity.UI;

/// <summary>
/// Reusable modal dialog system for displaying popup boxes
/// </summary>
public class Dialog
{
    public string Title { get; set; }
    public List<string> Lines { get; set; } = new();
    public List<DialogOption> Options { get; set; } = new();
    public int Width { get; set; } = 50;

    public Dialog(string title)
    {
        Title = title;
    }

    /// <summary>
    /// Add a line of text to the dialog body
    /// </summary>
    public Dialog AddLine(string line)
    {
        Lines.Add(line);
        return this;
    }

    /// <summary>
    /// Add a clickable option to the dialog
    /// </summary>
    public Dialog AddOption(string key, string label, Color? color = null)
    {
        Options.Add(new DialogOption
        {
            Key = key,
            Label = label,
            Color = color ?? Color.White
        });
        return this;
    }

    /// <summary>
    /// Calculate the height needed for this dialog
    /// </summary>
    public int CalculateHeight()
    {
        // Border (2) + Title (1) + Padding (1) + Lines + Padding (1) + Options (1) + Padding (1)
        return 2 + 1 + 1 + Lines.Count + 1 + 1 + 1;
    }

    /// <summary>
    /// Render this dialog to the console
    /// </summary>
    public void Render(ScreenSurface console)
    {
        int height = CalculateHeight();
        int dialogX = (console.Width - Width) / 2;
        int dialogY = (console.Height - height) / 2;

        // Draw semi-transparent dark background for entire dialog area
        for (int y = dialogY; y < dialogY + height; y++)
        {
            for (int x = dialogX; x < dialogX + Width; x++)
            {
                console.Print(x, y, " ", Color.Black, Color.Black * 0.9f);
            }
        }

        // Draw border
        DrawBorder(console, dialogX, dialogY, Width, height);

        // Draw title (centered, row 1 inside border)
        int titleX = dialogX + (Width - Title.Length) / 2;
        console.Print(titleX, dialogY + 1, Title, Color.Yellow);

        // Draw lines starting at row 3 (after title + padding)
        int currentY = dialogY + 3;
        foreach (var line in Lines)
        {
            int lineX = dialogX + (Width - line.Length) / 2;
            console.Print(lineX, currentY, line, Color.White);
            currentY++;
        }

        // Draw options at bottom (1 row before border)
        currentY = dialogY + height - 2;
        string optionsText = string.Join("  |  ", Options.Select(o => $"{o.Key}: {o.Label}"));
        int optionsX = dialogX + (Width - optionsText.Length) / 2;

        // Print each option with its color
        int printX = optionsX;
        for (int i = 0; i < Options.Count; i++)
        {
            var option = Options[i];
            var text = $"{option.Key}: {option.Label}";
            console.Print(printX, currentY, text, option.Color);
            printX += text.Length;

            // Add separator if not last option
            if (i < Options.Count - 1)
            {
                console.Print(printX, currentY, "  |  ", Color.Gray);
                printX += 5;
            }
        }
    }

    /// <summary>
    /// Draw a box border with Unicode box-drawing characters
    /// </summary>
    private void DrawBorder(ScreenSurface console, int x, int y, int width, int height)
    {
        // Top and bottom borders
        for (int i = x + 1; i < x + width - 1; i++)
        {
            console.Print(i, y, "─", Color.White);
            console.Print(i, y + height - 1, "─", Color.White);
        }

        // Left and right borders
        for (int i = y + 1; i < y + height - 1; i++)
        {
            console.Print(x, i, "│", Color.White);
            console.Print(x + width - 1, i, "│", Color.White);
        }

        // Corners
        console.Print(x, y, "┌", Color.White);
        console.Print(x + width - 1, y, "┐", Color.White);
        console.Print(x, y + height - 1, "└", Color.White);
        console.Print(x + width - 1, y + height - 1, "┘", Color.White);
    }

    /// <summary>
    /// Create a Yes/No confirmation dialog
    /// </summary>
    public static Dialog CreateYesNo(string title, string message)
    {
        return new Dialog(title)
            .AddLine(message)
            .AddLine("")
            .AddOption("Y", "Yes", Color.Green)
            .AddOption("N", "No", Color.Red);
    }

    /// <summary>
    /// Create an OK dialog (for alerts/messages)
    /// </summary>
    public static Dialog CreateOK(string title, params string[] messages)
    {
        var dialog = new Dialog(title);
        foreach (var msg in messages)
        {
            dialog.AddLine(msg);
        }
        dialog.AddLine("");
        dialog.AddOption("Enter", "OK", Color.Cyan);
        return dialog;
    }

    /// <summary>
    /// Create a multi-choice dialog
    /// </summary>
    public static Dialog CreateChoice(string title, string message, params (string key, string label)[] choices)
    {
        var dialog = new Dialog(title).AddLine(message).AddLine("");
        foreach (var (key, label) in choices)
        {
            dialog.AddOption(key, label);
        }
        return dialog;
    }
}

/// <summary>
/// Represents a clickable option in a dialog
/// </summary>
public class DialogOption
{
    public string Key { get; set; } = "";
    public string Label { get; set; } = "";
    public Color Color { get; set; } = Color.White;
}
