namespace TallerN2.Simulation.Physics;

/// <summary>
/// Registro de un rebote: posición y velocidad justo después del impacto (post pérdida del 40%).
/// </summary>
public sealed record BounceEvent(
    int Index,
    CollisionSurface Surface,
    Vector2D Position,
    Vector2D VelocityAfterBounce,
    double SimulationTime);
