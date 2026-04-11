namespace TallerN2.Simulation.Simulation;

/// <summary>
/// Muestra instantánea de la simulación para alimentar gráficas (t, posición, velocidad).
/// </summary>
public readonly record struct TrajectorySample(
    double TimeSeconds,
    double X,
    double Y,
    double Vx,
    double Vy,
    double Speed,
    double AngleDegrees);
