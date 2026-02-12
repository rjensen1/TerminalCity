using SadRogue.Primitives;

namespace TerminalCity.Domain;

/// <summary>
/// Defines a crop type with zoom-level appearance patterns
/// </summary>
public class CropDefinition
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public Color Color { get; set; }
    public Color BackgroundColor { get; set; }

    // Zoom-level patterns
    public ZoomPattern? Pattern25ft { get; set; }
    public ZoomPattern? Pattern50ft { get; set; }
    public ZoomPattern? Pattern100ft { get; set; }
    public ZoomPattern? Pattern200ft { get; set; }
    public ZoomPattern? Pattern400ft { get; set; }
}
