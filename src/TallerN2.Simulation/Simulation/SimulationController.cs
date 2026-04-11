using TallerN2.Simulation.Physics;

namespace TallerN2.Simulation.Simulation;

/// <summary>
/// Controlador MVC: orquesta el estado de simulación sin conocer controles WinForms.
/// La vista puede suscribirse a <see cref="StateChanged"/> o consultar propiedades tras cada tick.
/// </summary>
public sealed class SimulationController : IGameLoop
{
    private readonly SimulationConfig _config;
    private Vector2D _origin;
    private Vector2D _initialVelocity;
    private Vector2D _position;
    private Vector2D _velocity;
    private double _simulationTime;
    private HorizontalTarget? _target;
    private ParabolicMetrics _parabolicMetrics;
    private FirstImpactSnapshot _firstImpact = default!;
    private int _bounceCount;
    private bool _firstImpactWasTarget;
    private bool _isCompleted;

    public SimulationController(SimulationConfig config)
    {
        _config = config;
        _parabolicMetrics = default!;
    }

    public SimulationConfig Config => _config;

    /// <summary>Objetivo horizontal actual (null hasta el primer <see cref="Reset"/>).</summary>
    public HorizontalTarget? Target => _target;

    public double SimulationTime => _simulationTime;
    public Vector2D Position => _position;
    public Vector2D Velocity => _velocity;
    public IReadOnlyList<BounceEvent> BounceEvents => _bounceEvents;
    public bool IsCompleted => _isCompleted;
    public int BounceCount => _bounceCount;

    /// <summary>Métricas analíticas del arco inicial (tiempo/alcance/altura/velocidades teóricas sin obstáculo).</summary>
    public ParabolicMetrics ParabolicMetrics => _parabolicMetrics;

    /// <summary>Primer impacto real (con objetivo): instante y velocidad justo antes del rebote.</summary>
    public FirstImpactSnapshot FirstImpact => _firstImpact;

    public event Action? StateChanged;

    private readonly List<BounceEvent> _bounceEvents = new();
    private readonly List<TrajectorySample> _trajectorySamples = new();

    /// <summary>Historial muestreado para gráficas (se añade un punto al final de cada <see cref="Tick"/> y al <see cref="Reset"/>).</summary>
    public IReadOnlyList<TrajectorySample> TrajectorySamples => _trajectorySamples;

    /// <summary>
    /// Reinicia la simulación: genera objetivo aleatorio, posiciona el proyectil en el origen y calcula métricas.
    /// </summary>
    public void Reset(Vector2D launchOrigin, Vector2D initialVelocity, Random random)
    {
        _origin = launchOrigin;
        _initialVelocity = initialVelocity;
        _position = launchOrigin;
        _velocity = initialVelocity;
        _simulationTime = 0.0;
        _bounceCount = 0;
        _isCompleted = false;
        _bounceEvents.Clear();
        _trajectorySamples.Clear();

        _target = CreateRandomTarget(random, _config);
        _parabolicMetrics = PhysicsModel.ComputeParabolicMetrics(
            launchOrigin, initialVelocity, _config.GroundY, _config.Gravity);

        _firstImpact = ComputeFirstImpact(launchOrigin, initialVelocity, _target);
        _firstImpactWasTarget = _firstImpact.Surface == CollisionSurface.Target;

        RecordTrajectorySample();
    }

    /// <summary>
    /// Bucle de animación: avanza el tiempo físico y resuelve colisiones subpaso a subpaso.
    /// </summary>
    /// <remarks>
    /// Patrón típico desde el temporizador de la vista (Designer): <c>controller.Tick(e.ElapsedMilliseconds / 1000.0)</c>.
    /// </remarks>
    public void Tick(double deltaTimeSeconds)
    {
        if (_isCompleted || deltaTimeSeconds <= 0) return;

        var remaining = deltaTimeSeconds;
        const double eps = 1e-9;

        while (remaining > eps && !_isCompleted)
        {
            var hit = PhysicsModel.TryFindNextCollision(
                _position, _velocity, _config.Gravity, _config.GroundY, _target, remaining);

            if (hit is null)
            {
                AdvanceFreeFlight(remaining);
                remaining = 0;
            }
            else
            {
                var h = hit.Value;
                AdvanceFreeFlight(h.Time);
                remaining -= h.Time;

                ResolveBounce(h);
            }
        }

        RecordTrajectorySample();
        StateChanged?.Invoke();
    }

    /// <summary>Alias semántico para el game loop (ISP: quien solo anima puede depender de esta firma).</summary>
    public void Update(double deltaTimeSeconds) => Tick(deltaTimeSeconds);

    private void AdvanceFreeFlight(double dt)
    {
        _position = PhysicsModel.PositionAtTime(dt, _position, _velocity, _config.Gravity);
        _velocity = PhysicsModel.VelocityAtTime(dt, _velocity, _config.Gravity);
        _simulationTime += dt;
    }

    private void ResolveBounce(CollisionHit hit)
    {
        var surfaceY = hit.Surface == CollisionSurface.Target && _target is { } t
            ? t.Y
            : _config.GroundY;

        _position = new Vector2D(hit.Position.X, surfaceY);
        var vBefore = hit.VelocityBeforeBounce;
        var vAfter = PhysicsModel.BounceOnHorizontalSurface(vBefore, PhysicsModel.BounceSpeedRetentionFactor);

        if (vAfter.Magnitude < _config.MinSpeedToContinue)
        {
            _velocity = new Vector2D(0, 0);
            _isCompleted = true;
            return;
        }

        _velocity = vAfter;
        _bounceCount++;

        var record = new BounceEvent(
            Index: _bounceCount,
            Surface: hit.Surface,
            Position: _position,
            VelocityAfterBounce: vAfter,
            SimulationTime: _simulationTime);

        _bounceEvents.Add(record);

        if (ShouldEndAfterBounce(hit.Surface))
            _isCompleted = true;
    }

    private bool ShouldEndAfterBounce(CollisionSurface surface)
    {
        // Escenario pedagógico: si el 1.er impacto fue el objetivo, se esperan 2 rebotes (objetivo + suelo).
        if (_firstImpactWasTarget)
            return _bounceCount >= 2;

        // Si el 1.er impacto fue suelo (objetivo esquivado), basta un rebote.
        return _bounceCount >= 1;
    }

    private static HorizontalTarget CreateRandomTarget(Random random, SimulationConfig cfg)
    {
        var cx = random.NextDouble() * (cfg.TargetXMax - cfg.TargetXMin) + cfg.TargetXMin;
        var half = cfg.TargetWidth * 0.5;
        var y = random.NextDouble() * (cfg.TargetYMax - cfg.TargetYMin) + cfg.TargetYMin;
        return new HorizontalTarget(cx - half, cx + half, y);
    }

    private FirstImpactSnapshot ComputeFirstImpact(
        Vector2D origin,
        Vector2D v0,
        HorizontalTarget? target)
    {
        var t = PhysicsModel.TryFirstImpactTimeOnTargetOrGround(
            origin, v0, _config.Gravity, _config.GroundY, target);

        if (t is not { } tf)
        {
            return new FirstImpactSnapshot(
                Time: double.NaN,
                Surface: CollisionSurface.Ground,
                Position: origin,
                VelocityBeforeImpact: v0,
                VelocityInfoBeforeImpact: PhysicsModel.ToVelocityInfo(v0));
        }

        var pos = PhysicsModel.PositionAtTime(tf, origin, v0, _config.Gravity);
        var vel = PhysicsModel.VelocityAtTime(tf, v0, _config.Gravity);

        var isTarget = target is { } tgt &&
                       PhysicsModel.FirstImpactIsTarget(origin, v0, _config.Gravity, _config.GroundY, tgt, tf);

        var surface = isTarget ? CollisionSurface.Target : CollisionSurface.Ground;

        return new FirstImpactSnapshot(
            Time: tf,
            Surface: surface,
            Position: pos,
            VelocityBeforeImpact: vel,
            VelocityInfoBeforeImpact: PhysicsModel.ToVelocityInfo(vel));
    }

    private void RecordTrajectorySample()
    {
        var vi = PhysicsModel.ToVelocityInfo(_velocity);
        _trajectorySamples.Add(new TrajectorySample(
            _simulationTime,
            _position.X,
            _position.Y,
            vi.ComponentX,
            vi.ComponentY,
            vi.Magnitude,
            vi.AngleDegrees));
    }
}

/// <summary>
/// Datos del primer contacto con suelo u objetivo según la trayectoria inicial.
/// </summary>
public readonly record struct FirstImpactSnapshot(
    double Time,
    CollisionSurface Surface,
    Vector2D Position,
    Vector2D VelocityBeforeImpact,
    VelocityInfo VelocityInfoBeforeImpact);
