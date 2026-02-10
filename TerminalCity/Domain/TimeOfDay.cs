namespace TerminalCity.Domain;

/// <summary>
/// Represents the visual time of day for lighting effects (cosmetic only, not game time)
/// </summary>
public enum TimeOfDay
{
    /// <summary>
    /// Dawn - warm orange/pink glow, moderate brightness (5-7 AM)
    /// </summary>
    Dawn,

    /// <summary>
    /// Morning - clear bright light, full colors (7-11 AM)
    /// </summary>
    Morning,

    /// <summary>
    /// Midday - brightest light, full saturation (11 AM - 3 PM)
    /// </summary>
    Midday,

    /// <summary>
    /// Afternoon - still bright but softening (3-6 PM)
    /// </summary>
    Afternoon,

    /// <summary>
    /// Dusk - warm orange/red glow, dimming (6-8 PM)
    /// </summary>
    Dusk,

    /// <summary>
    /// Evening - deep blue/purple, much darker (8-10 PM)
    /// </summary>
    Evening,

    /// <summary>
    /// Night - very dark, blue tint (10 PM - 5 AM)
    /// </summary>
    Night
}
