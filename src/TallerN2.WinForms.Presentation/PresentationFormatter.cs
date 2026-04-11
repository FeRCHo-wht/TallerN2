using TallerN2.Simulation.Physics;

namespace TallerN2.WinForms.Presentation;

/// <summary>
/// Redondeos y validación para etiquetas (evita NaN/Inf y textos largos innecesarios).
/// </summary>
public static class PresentationFormatter
{
    public const int DefaultDecimals = 3;

    public static string FormatDouble(double value, int decimals = DefaultDecimals)
    {
        if (double.IsNaN(value)) return "—";
        if (double.IsInfinity(value)) return "—";
        return Math.Round(value, decimals).ToString($"F{decimals}");
    }

    public static string FormatAngle(double degrees, int decimals = 2) =>
        FormatDouble(degrees, decimals) + "°";

    public static string FormatVelocityInfo(VelocityInfo v, int decimals = 3) =>
        $"vx={FormatDouble(v.ComponentX, decimals)} m/s, " +
        $"vy={FormatDouble(v.ComponentY, decimals)} m/s, " +
        $"|v|={FormatDouble(v.Magnitude, decimals)} m/s, " +
        $"θ={FormatAngle(v.AngleDegrees, decimals)}";

    public static string FormatSurface(CollisionSurface surface) =>
        surface == CollisionSurface.Target ? "Objetivo" : "Suelo";
}
