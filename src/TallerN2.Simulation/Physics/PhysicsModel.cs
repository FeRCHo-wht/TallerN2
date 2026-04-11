namespace TallerN2.Simulation.Physics;

/// <summary>
/// Núcleo físico: movimiento parabólico con gravedad constante y rebotes sobre superficies horizontales.
/// Métodos puros (sin estado) para facilitar pruebas y cumplir SRP.
/// </summary>
/// <remarks>
/// Ecuaciones por tramo (MRUA):
/// <list type="bullet">
/// <item><description>x(t) = x₀ + vₓ·t</description></item>
/// <item><description>y(t) = y₀ + vᵧ·t + ½·g·t²</description></item>
/// <item><description>vₓ(t) = vₓ₀</description></item>
/// <item><description>vᵧ(t) = vᵧ₀ + g·t</description></item>
/// </list>
/// Convención: eje Y hacia arriba; g es negativo (p. ej. -9,81 m/s²).
/// </remarks>
public static class PhysicsModel
{
    /// <summary>
    /// Factor de retención de la rapidez tras cada rebote: la rapidez se multiplica por (1 − 0,40) = 0,60.
    /// Equivale a una pérdida del 40% de la magnitud del vector velocidad.
    /// </summary>
    public const double BounceSpeedRetentionFactor = 0.6;

    /// <summary>
    /// Posición en el instante local <paramref name="t"/> desde el inicio del tramo actual.
    /// </summary>
    public static Vector2D PositionAtTime(double t, Vector2D position0, Vector2D velocity0, double g)
    {
        var x = position0.X + velocity0.X * t;
        var y = position0.Y + velocity0.Y * t + 0.5 * g * t * t;
        return new Vector2D(x, y);
    }

    /// <summary>
    /// Velocidad en el instante local <paramref name="t"/> desde el inicio del tramo.
    /// </summary>
    public static Vector2D VelocityAtTime(double t, Vector2D velocity0, double g) =>
        new(velocity0.X, velocity0.Y + g * t);

    /// <summary>
    /// Descompone la velocidad en componentes, magnitud y ángulo (grados, atan2(vᵧ, vₓ)).
    /// </summary>
    public static VelocityInfo ToVelocityInfo(Vector2D v)
    {
        var mag = v.Magnitude;
        var angleDeg = Math.Atan2(v.Y, v.X) * (180.0 / Math.PI);
        return new VelocityInfo(v.X, v.Y, mag, angleDeg);
    }

    /// <summary>
    /// Métricas del arco inicial suponiendo lanzamiento desde <paramref name="origin"/> sin obstáculos
    /// hasta impactar el suelo <paramref name="groundY"/>.
    /// </summary>
    /// <remarks>
    /// Tiempo de vuelo hasta el suelo (y = groundY): resuelve y₀ + vᵧ·t + ½·g·t² = groundY con t &gt; 0.
    /// Altura máxima: yₘₐₓ = y₀ − vᵧ₀²/(2g) cuando g &lt; 0 y vᵧ₀ &gt; 0.
    /// En el vértice, vᵧ = 0 ⇒ v = (vₓ₀, 0).
    /// </remarks>
    public static ParabolicMetrics ComputeParabolicMetrics(
        Vector2D origin,
        Vector2D initialVelocity,
        double groundY,
        double g)
    {
        var vx0 = initialVelocity.X;
        var vy0 = initialVelocity.Y;

        var tFlight = TrySmallestPositiveTimeToHorizontalLine(origin, initialVelocity, g, groundY)
                      ?? 0.0;

        var range = origin.X + vx0 * tFlight;
        var maxHeight = ComputeMaxHeight(origin.Y, vy0, g);

        var tApex = ComputeTimeToVerticalVelocityZero(vy0, g);
        var velApex = VelocityAtTime(tApex, initialVelocity, g);

        var impactVel = VelocityAtTime(tFlight, initialVelocity, g);

        return new ParabolicMetrics(
            TotalFlightTimeToGround: tFlight,
            MaxHorizontalRange: range,
            MaxHeight: maxHeight,
            VelocityAtApex: ToVelocityInfo(velApex),
            InitialVelocity: ToVelocityInfo(initialVelocity),
            ImpactVelocityGroundOnly: ToVelocityInfo(impactVel));
    }

    /// <summary>
    /// Altura máxima teórica del tramo si el movimiento alcanza el vértice antes de colisionar.
    /// </summary>
    public static double ComputeMaxHeight(double y0, double vy0, double g)
    {
        if (g >= 0 || vy0 <= 0) return y0;
        return y0 - (vy0 * vy0) / (2.0 * g);
    }

    /// <summary>
    /// Tiempo hasta que la componente vertical sea cero: vᵧ(t) = 0 ⇒ t = −vᵧ₀/g.
    /// </summary>
    public static double ComputeTimeToVerticalVelocityZero(double vy0, double g)
    {
        if (Math.Abs(g) < 1e-12) return 0.0;
        return -vy0 / g;
    }

    /// <summary>
    /// Rebote sobre superficie horizontal: se refleja la componente normal (eje Y),
    /// luego se escala el vector al factor de retención de rapidez (pérdida del 40%).
    /// </summary>
    public static Vector2D BounceOnHorizontalSurface(Vector2D velocityBefore, double speedRetentionFactor)
    {
        var reflected = new Vector2D(velocityBefore.X, -velocityBefore.Y);
        var dir = reflected.Normalized();
        var vin = velocityBefore.Magnitude;
        return dir * (speedRetentionFactor * vin);
    }

    /// <summary>
    /// Primer tiempo positivo en que la trayectoria corta la recta y = <paramref name="lineY"/>,
    /// con x(t) dentro del segmento del objetivo horizontal (si aplica).
    /// </summary>
    public static double? TryFirstImpactTimeOnTargetOrGround(
        Vector2D position0,
        Vector2D velocity0,
        double g,
        double groundY,
        HorizontalTarget? target)
    {
        double? tTarget = null;
        if (target is { } tgt)
        {
            tTarget = TrySmallestPositiveTimeToHorizontalLineInXRange(
                position0, velocity0, g, tgt.Y, tgt.MinX, tgt.MaxX);
        }

        var tGround = TrySmallestPositiveTimeToHorizontalLine(position0, velocity0, g, groundY);

        if (tTarget is { } tt && tGround is { } tg)
        {
            if (Math.Abs(tt - tg) <= 1e-9) return tt;
            return Math.Min(tt, tg);
        }

        return tTarget ?? tGround;
    }

    /// <summary>
    /// Determina si el primer impacto es sobre el objetivo (true) o el suelo (false).
    /// Si ambos ocurren al mismo instante (raro), se prioriza el objetivo.
    /// </summary>
    public static bool FirstImpactIsTarget(
        Vector2D position0,
        Vector2D velocity0,
        double g,
        double groundY,
        HorizontalTarget target,
        double firstImpactTime,
        double tolerance = 1e-6)
    {
        var pt = PositionAtTime(firstImpactTime, position0, velocity0, g);
        if (Math.Abs(pt.Y - target.Y) <= tolerance && target.ContainsX(pt.X))
            return true;
        return false;
    }

    /// <summary>
    /// Resuelve ½·g·t² + vᵧ₀·t + (y₀ − yₗᵢₙₑ) = 0 para t &gt; ε, devuelve el menor válido
    /// con llegada al suelo en descenso (vᵧ &lt; 0), ignorando el raíz trivial de despegue.
    /// </summary>
    public static double? TrySmallestPositiveTimeToHorizontalLine(
        Vector2D position0,
        Vector2D velocity0,
        double g,
        double lineY,
        double minTimeEpsilon = 1e-9)
    {
        var roots = SolveQuadraticAllPositive(0.5 * g, velocity0.Y, position0.Y - lineY, minTimeEpsilon);
        double? best = null;
        foreach (var t in roots.OrderBy(t => t))
        {
            var vy = VelocityAtTime(t, velocity0, g).Y;
            if (vy >= -1e-6) continue; // impacto al bajar: vᵧ debe ser negativa
            best = t;
            break;
        }
        return best;
    }

    /// <summary>
    /// Como <see cref="TrySmallestPositiveTimeToHorizontalLine"/> pero exige x(t) ∈ [xMin, xMax]
    /// y contacto “desde arriba” (vᵧ ≤ 0 en el impacto), típico para apoyar sobre una plataforma.
    /// </summary>
    public static double? TrySmallestPositiveTimeToHorizontalLineInXRange(
        Vector2D position0,
        Vector2D velocity0,
        double g,
        double lineY,
        double xMin,
        double xMax,
        double minTimeEpsilon = 1e-9)
    {
        var roots = SolveQuadraticAllPositive(0.5 * g, velocity0.Y, position0.Y - lineY, minTimeEpsilon);
        foreach (var t in roots.OrderBy(t => t))
        {
            var x = position0.X + velocity0.X * t;
            if (x < xMin || x > xMax) continue;
            var vy = VelocityAtTime(t, velocity0, g).Y;
            if (vy > 1e-6) continue; // aún subiendo: no es aterrizaje sobre la cara superior
            return t;
        }
        return null;
    }

    /// <summary>
    /// Encuentra el próximo impacto con suelo u objetivo dentro del horizonte temporal (0, maxDelta].
    /// </summary>
    public static CollisionHit? TryFindNextCollision(
        Vector2D position0,
        Vector2D velocity0,
        double g,
        double groundY,
        HorizontalTarget? target,
        double maxDelta,
        double minTimeEpsilon = 1e-9)
    {
        if (maxDelta <= minTimeEpsilon) return null;

        double? bestT = null;
        CollisionSurface? bestSurface = null;

        void Consider(double? t, CollisionSurface surface)
        {
            if (t is not { } tt) return;
            if (tt <= minTimeEpsilon || tt > maxDelta + 1e-12) return;
            if (bestT is null || tt < bestT.Value - 1e-12)
            {
                bestT = tt;
                bestSurface = surface;
            }
            else if (bestSurface is not null && Math.Abs(tt - bestT.Value) <= 1e-9)
            {
                // Misma t: priorizar objetivo sobre suelo (consistente con FirstImpactIsTarget)
                if (surface == CollisionSurface.Target)
                    bestSurface = CollisionSurface.Target;
            }
        }

        Consider(TrySmallestPositiveTimeToHorizontalLine(position0, velocity0, g, groundY, minTimeEpsilon),
            CollisionSurface.Ground);

        if (target is { } tgt)
        {
            Consider(
                TrySmallestPositiveTimeToHorizontalLineInXRange(
                    position0, velocity0, g, tgt.Y, tgt.MinX, tgt.MaxX, minTimeEpsilon),
                CollisionSurface.Target);
        }

        if (bestT is null || bestSurface is null) return null;

        var tHit = bestT.Value;
        var pos = PositionAtTime(tHit, position0, velocity0, g);
        var vel = VelocityAtTime(tHit, velocity0, g);
        return new CollisionHit(tHit, bestSurface.Value, pos, vel);
    }

    private static double? SolveQuadraticSmallestPositive(double a, double b, double c, double eps)
    {
        var roots = SolveQuadraticAllPositive(a, b, c, eps);
        return roots.Count == 0 ? null : roots.Min();
    }

    private static List<double> SolveQuadraticAllPositive(double a, double b, double c, double eps)
    {
        var list = new List<double>();
        if (Math.Abs(a) < 1e-15 && Math.Abs(b) < 1e-15) return list;
        if (Math.Abs(a) < 1e-15)
        {
            // b·t + c = 0
            var t = -c / b;
            if (t > eps) list.Add(t);
            return list;
        }

        var disc = b * b - 4.0 * a * c;
        if (disc < 0) return list;
        var sqrtD = Math.Sqrt(disc);
        var t1 = (-b - sqrtD) / (2.0 * a);
        var t2 = (-b + sqrtD) / (2.0 * a);
        if (t1 > eps) list.Add(t1);
        if (t2 > eps && Math.Abs(t2 - t1) > 1e-9) list.Add(t2);
        return list;
    }
}

public enum CollisionSurface
{
    Ground,
    Target
}

/// <summary>
/// Resultado de colisión en un tramo parabólico: instante local y estado en el impacto (justo antes del rebote).
/// </summary>
public readonly record struct CollisionHit(
    double Time,
    CollisionSurface Surface,
    Vector2D Position,
    Vector2D VelocityBeforeBounce);
