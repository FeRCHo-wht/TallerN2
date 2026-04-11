namespace TallerN2.Simulation.Physics;

/// <summary>
/// Vector 2D en el plano de simulación (x horizontal, y vertical hacia arriba).
/// </summary>
public readonly record struct Vector2D(double X, double Y)
{
    public double Magnitude => Math.Sqrt(X * X + Y * Y);

    public static Vector2D operator +(Vector2D a, Vector2D b) => new(a.X + b.X, a.Y + b.Y);
    public static Vector2D operator -(Vector2D a, Vector2D b) => new(a.X - b.X, a.Y - b.Y);
    public static Vector2D operator *(Vector2D v, double s) => new(v.X * s, v.Y * s);
    public static Vector2D operator *(double s, Vector2D v) => v * s;

    public Vector2D Normalized()
    {
        var m = Magnitude;
        return m < 1e-12 ? new Vector2D(0, 0) : new Vector2D(X / m, Y / m);
    }
}
