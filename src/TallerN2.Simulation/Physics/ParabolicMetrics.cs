namespace TallerN2.Simulation.Physics;

/// <summary>
/// Métricas analíticas del arco parabólico inicial (sin rebotes), útiles para la vista en tiempo real.
/// </summary>
public sealed record ParabolicMetrics(
    double TotalFlightTimeToGround,
    double MaxHorizontalRange,
    double MaxHeight,
    VelocityInfo VelocityAtApex,
    VelocityInfo InitialVelocity,
    /// <summary>Velocidad en el primer impacto con el suelo si no hubiera objetivo.</summary>
    VelocityInfo ImpactVelocityGroundOnly);
