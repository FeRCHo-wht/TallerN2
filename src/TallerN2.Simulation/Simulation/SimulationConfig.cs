namespace TallerN2.Simulation.Simulation;

/// <summary>
/// Parámetros de escenario: gravedad, suelo y límites para el objetivo aleatorio.
/// </summary>
public sealed record SimulationConfig(
    double Gravity,
    double GroundY,
    double TargetXMin,
    double TargetXMax,
    double TargetYMin,
    double TargetYMax,
    double TargetWidth,
    double MinSpeedToContinue = 0.05);
