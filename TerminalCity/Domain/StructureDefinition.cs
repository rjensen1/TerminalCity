using SadRogue.Primitives;

namespace TerminalCity.Domain;

/// <summary>
/// Defines a structure (outbuilding) type with zoom-level appearance patterns
/// </summary>
public class StructureDefinition
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public string Type { get; set; } = "";
    public string VariantOf { get; set; } = "";  // e.g., "shed" - used by plots to reference any variant
    public string Size { get; set; } = "";  // e.g., "1x1", "1x2"
    public string Description { get; set; } = "";
    public Color Color { get; set; }
    public Color BackgroundColor { get; set; }
    public string CanAttachTo { get; set; } = "";  // e.g., "house, barn"

    // Zoom-level patterns
    public ZoomPattern? Pattern25ft { get; set; }
    public ZoomPattern? Pattern50ft { get; set; }
    public ZoomPattern? Pattern100ft { get; set; }
    public ZoomPattern? Pattern200ft { get; set; }
    public ZoomPattern? Pattern400ft { get; set; }
}
