namespace TallerN2.Simulation.Physics;

/// <summary>
/// Objetivo horizontal: segmento en y = <see cref="Y"/> entre <see cref="MinX"/> y <see cref="MaxX"/>.
/// </summary>
public readonly record struct HorizontalTarget(double MinX, double MaxX, double Y)
{
    public bool ContainsX(double x) => x >= MinX && x <= MaxX;
}
