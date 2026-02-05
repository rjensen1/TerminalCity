# Dialog System

Reusable modal dialog system for TerminalCity using Unicode box-drawing characters.

## Features

- **Modal overlays** - Blocks background interaction while dialog is open
- **Auto-sizing** - Dialogs automatically size based on content
- **Beautiful borders** - Uses Unicode box-drawing characters (`┌─┐│└─┘`)
- **Colored options** - Each option can have its own color
- **Centered display** - Automatically centers on screen

## Quick Start

### Yes/No Confirmation

```csharp
gameState.CurrentDialog = Dialog.CreateYesNo(
    "Delete Building?",
    "Are you sure you want to delete this building?"
);
```

### Alert/Message

```csharp
gameState.CurrentDialog = Dialog.CreateOK(
    "Building Complete!",
    "Your residential tower is now complete.",
    "Population capacity increased by 500."
);
```

### Multiple Choice

```csharp
gameState.CurrentDialog = Dialog.CreateChoice(
    "Zone Type",
    "Select a zone type:",
    ("R", "Residential"),
    ("C", "Commercial"),
    ("I", "Industrial")
);
```

## Custom Dialogs

```csharp
var dialog = new Dialog("Custom Dialog")
    .AddLine("First line of text")
    .AddLine("Second line of text")
    .AddLine("") // Empty line for spacing
    .AddOption("1", "Option One", Color.Green)
    .AddOption("2", "Option Two", Color.Yellow)
    .AddOption("3", "Option Three", Color.Red);

gameState.CurrentDialog = dialog;
```

## Handling Responses

In your keyboard handler:

```csharp
void HandleDialogResponse(string optionKey)
{
    if (gameState?.CurrentDialog == null) return;

    // Check which dialog is open based on title
    if (gameState.CurrentDialog.Title == "Delete Building?")
    {
        if (optionKey.ToUpper() == "Y")
        {
            // Delete the building
            DeleteSelectedBuilding();
        }
        // Close dialog
        gameState.CurrentDialog = null;
        Render();
    }
}
```

## Dialog Lifecycle

1. **Create** - Instantiate a Dialog object
2. **Show** - Assign to `gameState.CurrentDialog`
3. **Input** - Dialog automatically blocks other input
4. **Response** - Handle option selection in `HandleDialogResponse()`
5. **Close** - Set `gameState.CurrentDialog = null`

## Static Factory Methods

### `Dialog.CreateYesNo(title, message)`
- Two options: Y (Green) and N (Red)
- Perfect for confirmations

### `Dialog.CreateOK(title, ...messages)`
- Single OK button (Cyan)
- Good for alerts and notifications
- Accepts multiple message lines

### `Dialog.CreateChoice(title, message, ...choices)`
- Multiple custom options
- Each choice is a tuple of (key, label)
- All options use default color (White)

## Styling

Box drawing characters used:
- Corners: `┌` `┐` `└` `┘`
- Edges: `─` (horizontal) `│` (vertical)

Colors:
- Border: White
- Title: Yellow
- Body text: White
- Background: Semi-transparent dark (Black * 0.9)
- Options: Customizable per option

## Example: Building Placement

```csharp
// Show building menu
gameState.CurrentDialog = Dialog.CreateChoice(
    "Place Building",
    "Select a building type:",
    ("H", "House"),
    ("T", "Tower"),
    ("M", "Mall"),
    ("F", "Factory")
);

// In HandleDialogResponse:
if (gameState.CurrentDialog.Title == "Place Building")
{
    switch (optionKey.ToUpper())
    {
        case "H":
            StartBuildingPlacement(BuildingType.House);
            break;
        case "T":
            StartBuildingPlacement(BuildingType.Tower);
            break;
        // ... etc
    }
    gameState.CurrentDialog = null;
    Render();
}
```

## Tips

- Keep dialog width reasonable (30-60 characters)
- Use empty lines (`AddLine("")`) for spacing
- Title should be concise (under 40 characters)
- Options should be single letters or short keys
- Always close the dialog after handling response
