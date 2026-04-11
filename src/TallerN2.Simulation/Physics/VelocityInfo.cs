namespace TallerN2.Simulation.Physics;

/// <summary>
/// Descomposición de velocidad para mostrar en la vista (sin lógica física).
/// </summary>
public readonly record struct VelocityInfo(
    double ComponentX,
    double ComponentY,
    double Magnitude,
    /// <summary>Ángulo en grados respecto al eje +X, rango típico (-180, 180].</summary>
    double AngleDegrees);
