namespace TerminalCity.Domain;

/// <summary>
/// Weather conditions
/// </summary>
public enum WeatherCondition
{
    Clear,
    PartlyCloudy,
    Overcast,
    Fog,
    LightRain,
    HeavyRain,
    Thunderstorm,
    LightSnow,
    HeavySnow,
    Blizzard
}

/// <summary>
/// Wind direction
/// </summary>
public enum WindDirection
{
    Calm,
    N,
    NE,
    E,
    SE,
    S,
    SW,
    W,
    NW
}

/// <summary>
/// Fire danger level
/// </summary>
public enum FireDanger
{
    Low,
    Moderate,
    High,
    VeryHigh,
    Extreme
}

/// <summary>
/// Moon phase
/// </summary>
public enum MoonPhase
{
    NewMoon,
    WaxingCrescent,
    FirstQuarter,
    WaxingGibbous,
    FullMoon,
    WaningGibbous,
    LastQuarter,
    WaningCrescent
}

/// <summary>
/// Current weather state (cosmetic, doesn't affect gameplay yet)
/// </summary>
public class Weather
{
    /// <summary>
    /// Air temperature in Fahrenheit
    /// </summary>
    public int TemperatureF { get; set; } = 68;

    /// <summary>
    /// Current weather condition
    /// </summary>
    public WeatherCondition Condition { get; set; } = WeatherCondition.Clear;

    /// <summary>
    /// Wind speed in mph
    /// </summary>
    public int WindSpeedMph { get; set; } = 0;

    /// <summary>
    /// Wind direction
    /// </summary>
    public WindDirection WindDirection { get; set; } = WindDirection.Calm;

    /// <summary>
    /// Humidity percentage (0-100)
    /// </summary>
    public int HumidityPercent { get; set; } = 50;

    /// <summary>
    /// Barometric pressure in inches of mercury (inHg)
    /// Normal range: 28.5 - 31.0 (29.92 is standard)
    /// </summary>
    public double BarometricPressure { get; set; } = 29.92;

    /// <summary>
    /// Visibility in miles
    /// 0 = dense fog, 10+ = clear day
    /// </summary>
    public double VisibilityMiles { get; set; } = 10.0;

    /// <summary>
    /// Fire danger level
    /// </summary>
    public FireDanger FireDanger { get; set; } = FireDanger.Low;

    /// <summary>
    /// Cycle to next weather condition
    /// </summary>
    public void CycleWeatherCondition()
    {
        Condition = Condition switch
        {
            WeatherCondition.Clear => WeatherCondition.PartlyCloudy,
            WeatherCondition.PartlyCloudy => WeatherCondition.Overcast,
            WeatherCondition.Overcast => WeatherCondition.Fog,
            WeatherCondition.Fog => WeatherCondition.LightRain,
            WeatherCondition.LightRain => WeatherCondition.HeavyRain,
            WeatherCondition.HeavyRain => WeatherCondition.Thunderstorm,
            WeatherCondition.Thunderstorm => WeatherCondition.LightSnow,
            WeatherCondition.LightSnow => WeatherCondition.HeavySnow,
            WeatherCondition.HeavySnow => WeatherCondition.Blizzard,
            WeatherCondition.Blizzard => WeatherCondition.Clear,
            _ => WeatherCondition.Clear
        };
    }

    /// <summary>
    /// Cycle to next wind direction
    /// </summary>
    public void CycleWindDirection()
    {
        WindDirection = WindDirection switch
        {
            WindDirection.Calm => WindDirection.N,
            WindDirection.N => WindDirection.NE,
            WindDirection.NE => WindDirection.E,
            WindDirection.E => WindDirection.SE,
            WindDirection.SE => WindDirection.S,
            WindDirection.S => WindDirection.SW,
            WindDirection.SW => WindDirection.W,
            WindDirection.W => WindDirection.NW,
            WindDirection.NW => WindDirection.Calm,
            _ => WindDirection.Calm
        };
    }

    /// <summary>
    /// Get display name for weather condition
    /// </summary>
    public string GetConditionName()
    {
        return Condition switch
        {
            WeatherCondition.Clear => "Clear",
            WeatherCondition.PartlyCloudy => "Partly Cloudy",
            WeatherCondition.Overcast => "Overcast",
            WeatherCondition.Fog => "Fog",
            WeatherCondition.LightRain => "Light Rain",
            WeatherCondition.HeavyRain => "Heavy Rain",
            WeatherCondition.Thunderstorm => "Thunderstorm",
            WeatherCondition.LightSnow => "Light Snow",
            WeatherCondition.HeavySnow => "Heavy Snow",
            WeatherCondition.Blizzard => "Blizzard",
            _ => "Unknown"
        };
    }

    /// <summary>
    /// Get wind description (direction and speed)
    /// </summary>
    public string GetWindDescription()
    {
        if (WindDirection == WindDirection.Calm || WindSpeedMph == 0)
            return "Calm";

        return $"{WindDirection} {WindSpeedMph} mph";
    }

    /// <summary>
    /// Cycle fire danger level
    /// </summary>
    public void CycleFireDanger()
    {
        FireDanger = FireDanger switch
        {
            FireDanger.Low => FireDanger.Moderate,
            FireDanger.Moderate => FireDanger.High,
            FireDanger.High => FireDanger.VeryHigh,
            FireDanger.VeryHigh => FireDanger.Extreme,
            FireDanger.Extreme => FireDanger.Low,
            _ => FireDanger.Low
        };
    }

    /// <summary>
    /// Get fire danger display name
    /// </summary>
    public string GetFireDangerName()
    {
        return FireDanger switch
        {
            FireDanger.Low => "Low",
            FireDanger.Moderate => "Moderate",
            FireDanger.High => "High",
            FireDanger.VeryHigh => "Very High",
            FireDanger.Extreme => "Extreme",
            _ => "Unknown"
        };
    }

    /// <summary>
    /// Get fire danger color for display
    /// </summary>
    public SadRogue.Primitives.Color GetFireDangerColor()
    {
        return FireDanger switch
        {
            FireDanger.Low => SadRogue.Primitives.Color.Green,
            FireDanger.Moderate => SadRogue.Primitives.Color.Yellow,
            FireDanger.High => SadRogue.Primitives.Color.Orange,
            FireDanger.VeryHigh => SadRogue.Primitives.Color.OrangeRed,
            FireDanger.Extreme => SadRogue.Primitives.Color.Red,
            _ => SadRogue.Primitives.Color.White
        };
    }

    /// <summary>
    /// Calculate sunrise time for given date and latitude (simplified)
    /// </summary>
    public static string GetSunriseTime(DateTime date, double latitude = 42.0)
    {
        // Simplified calculation - approximate for mid-latitudes
        int dayOfYear = date.DayOfYear;

        // Base sunrise is 6:00 AM, varies by season
        // Summer (day 172, June 21): ~5:30 AM
        // Winter (day 355, Dec 21): ~7:30 AM
        double seasonalOffset = Math.Sin((dayOfYear - 80) * Math.PI / 182.5) * 1.0;

        int hour = 6 - (int)seasonalOffset;
        int minute = (int)((seasonalOffset - (int)seasonalOffset) * 60);
        if (minute < 0) { hour--; minute += 60; }

        return $"{hour}:{Math.Abs(minute):D2} AM";
    }

    /// <summary>
    /// Calculate sunset time for given date and latitude (simplified)
    /// </summary>
    public static string GetSunsetTime(DateTime date, double latitude = 42.0)
    {
        // Simplified calculation - approximate for mid-latitudes
        int dayOfYear = date.DayOfYear;

        // Base sunset is 6:00 PM, varies by season
        // Summer (day 172, June 21): ~8:30 PM
        // Winter (day 355, Dec 21): ~4:30 PM
        double seasonalOffset = Math.Sin((dayOfYear - 80) * Math.PI / 182.5) * 2.5;

        int hour = 6 + (int)seasonalOffset;
        int minute = (int)((seasonalOffset - (int)seasonalOffset) * 60);

        // Convert to 12-hour format
        string period = "PM";
        if (hour >= 12)
        {
            if (hour > 12) hour -= 12;
            period = "PM";
        }
        else
        {
            period = "AM";
        }

        return $"{hour}:{Math.Abs(minute):D2} {period}";
    }

    /// <summary>
    /// Calculate moon phase from date
    /// Moon cycle is approximately 29.53 days
    /// </summary>
    public static MoonPhase GetMoonPhase(DateTime date)
    {
        // Known new moon: January 1, 2000
        DateTime knownNewMoon = new DateTime(2000, 1, 6);
        double daysSinceNewMoon = (date - knownNewMoon).TotalDays;
        double phase = (daysSinceNewMoon % 29.53) / 29.53;

        return phase switch
        {
            < 0.0625 => MoonPhase.NewMoon,
            < 0.1875 => MoonPhase.WaxingCrescent,
            < 0.3125 => MoonPhase.FirstQuarter,
            < 0.4375 => MoonPhase.WaxingGibbous,
            < 0.5625 => MoonPhase.FullMoon,
            < 0.6875 => MoonPhase.WaningGibbous,
            < 0.8125 => MoonPhase.LastQuarter,
            < 0.9375 => MoonPhase.WaningCrescent,
            _ => MoonPhase.NewMoon
        };
    }

    /// <summary>
    /// Get moon phase display name with symbol
    /// </summary>
    public static string GetMoonPhaseName(MoonPhase phase)
    {
        return phase switch
        {
            MoonPhase.NewMoon => "New Moon ●",
            MoonPhase.WaxingCrescent => "Waxing Crescent ☽",
            MoonPhase.FirstQuarter => "First Quarter ◐",
            MoonPhase.WaxingGibbous => "Waxing Gibbous ◑",
            MoonPhase.FullMoon => "Full Moon ○",
            MoonPhase.WaningGibbous => "Waning Gibbous ◑",
            MoonPhase.LastQuarter => "Last Quarter ◐",
            MoonPhase.WaningCrescent => "Waning Crescent ☾",
            _ => "Unknown"
        };
    }
}
