using SadRogue.Primitives;
using TerminalCity.Domain;

namespace TerminalCity.Rendering;

/// <summary>
/// Applies time-of-day lighting effects to colors
/// </summary>
public static class LightingEffects
{
    /// <summary>
    /// Apply time-of-day lighting tint to a color
    /// </summary>
    public static Color ApplyTimeOfDayLighting(Color original, TimeOfDay timeOfDay)
    {
        return timeOfDay switch
        {
            TimeOfDay.Dawn => ApplyDawnLighting(original),
            TimeOfDay.Morning => ApplyMorningLighting(original),
            TimeOfDay.Midday => original, // No tint at midday (full bright)
            TimeOfDay.Afternoon => ApplyAfternoonLighting(original),
            TimeOfDay.Dusk => ApplyDuskLighting(original),
            TimeOfDay.Evening => ApplyEveningLighting(original),
            TimeOfDay.Night => ApplyNightLighting(original),
            _ => original
        };
    }

    /// <summary>
    /// Dawn - warm pink/orange glow, moderate brightness
    /// </summary>
    private static Color ApplyDawnLighting(Color color)
    {
        float r = color.R / 255f;
        float g = color.G / 255f;
        float b = color.B / 255f;

        // Add warm orange/pink tint
        r = Math.Min(1.0f, r * 1.1f + 0.15f);
        g = Math.Min(1.0f, g * 0.95f + 0.08f);
        b = Math.Min(1.0f, b * 0.85f + 0.05f);

        // Slightly dim overall
        float brightness = 0.85f;
        r *= brightness;
        g *= brightness;
        b *= brightness;

        return new Color((int)(r * 255), (int)(g * 255), (int)(b * 255));
    }

    /// <summary>
    /// Morning - clear bright light, slightly warm
    /// </summary>
    private static Color ApplyMorningLighting(Color color)
    {
        float r = color.R / 255f;
        float g = color.G / 255f;
        float b = color.B / 255f;

        // Subtle warm tint
        r = Math.Min(1.0f, r * 1.05f);
        g = Math.Min(1.0f, g * 1.02f);
        b = Math.Min(1.0f, b * 0.98f);

        return new Color((int)(r * 255), (int)(g * 255), (int)(b * 255));
    }

    /// <summary>
    /// Afternoon - still bright but slightly softer
    /// </summary>
    private static Color ApplyAfternoonLighting(Color color)
    {
        float r = color.R / 255f;
        float g = color.G / 255f;
        float b = color.B / 255f;

        // Very subtle warm shift
        r = Math.Min(1.0f, r * 1.02f);
        g = Math.Min(1.0f, g * 1.0f);
        b = Math.Min(1.0f, b * 0.98f);

        // Slight dimming
        float brightness = 0.95f;
        r *= brightness;
        g *= brightness;
        b *= brightness;

        return new Color((int)(r * 255), (int)(g * 255), (int)(b * 255));
    }

    /// <summary>
    /// Dusk - warm orange/red glow, getting darker
    /// </summary>
    private static Color ApplyDuskLighting(Color color)
    {
        float r = color.R / 255f;
        float g = color.G / 255f;
        float b = color.B / 255f;

        // Strong warm orange/red tint
        r = Math.Min(1.0f, r * 1.2f + 0.2f);
        g = Math.Min(1.0f, g * 0.9f + 0.1f);
        b = Math.Min(1.0f, b * 0.7f);

        // More dimming
        float brightness = 0.75f;
        r *= brightness;
        g *= brightness;
        b *= brightness;

        return new Color((int)(r * 255), (int)(g * 255), (int)(b * 255));
    }

    /// <summary>
    /// Evening - deep blue/purple, much darker
    /// </summary>
    private static Color ApplyEveningLighting(Color color)
    {
        float r = color.R / 255f;
        float g = color.G / 255f;
        float b = color.B / 255f;

        // Blue/purple tint
        r = Math.Min(1.0f, r * 0.6f + 0.05f);
        g = Math.Min(1.0f, g * 0.65f + 0.05f);
        b = Math.Min(1.0f, b * 0.85f + 0.15f);

        // Significantly darker
        float brightness = 0.5f;
        r *= brightness;
        g *= brightness;
        b *= brightness;

        return new Color((int)(r * 255), (int)(g * 255), (int)(b * 255));
    }

    /// <summary>
    /// Night - very dark with blue tint
    /// </summary>
    private static Color ApplyNightLighting(Color color)
    {
        float r = color.R / 255f;
        float g = color.G / 255f;
        float b = color.B / 255f;

        // Strong blue tint
        r = Math.Min(1.0f, r * 0.4f + 0.02f);
        g = Math.Min(1.0f, g * 0.45f + 0.03f);
        b = Math.Min(1.0f, b * 0.7f + 0.1f);

        // Very dark
        float brightness = 0.3f;
        r *= brightness;
        g *= brightness;
        b *= brightness;

        return new Color((int)(r * 255), (int)(g * 255), (int)(b * 255));
    }
}
