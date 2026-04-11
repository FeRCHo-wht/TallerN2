using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using TallerN2.Simulation.Physics;
using TallerN2.Simulation.Simulation;

namespace TallerN2.WinForms.Presentation;

/// <summary>
/// Conecta <see cref="SimulationController"/> con etiquetas y gráficas creadas en el diseñador.
/// No crea controles: solo asigna <see cref="Control.Text"/> y puntos de series.
/// </summary>
public sealed class SimulationPresentationBinder
{
    private readonly FlightMetricLabels _labels;
    private readonly FlightChartsSet _charts;
    private readonly int _decimals;

    public SimulationPresentationBinder(
        FlightMetricLabels labels,
        FlightChartsSet charts,
        int decimals = PresentationFormatter.DefaultDecimals)
    {
        _labels = labels;
        _charts = charts;
        _decimals = decimals;
    }

    /// <summary>Actualiza métricas teóricas, estado en vivo y rebotes.</summary>
    public void UpdateLabels(SimulationController sim)
    {
        var m = sim.ParabolicMetrics;
        var fi = sim.FirstImpact;
        var vi = PhysicsModel.ToVelocityInfo(sim.Velocity);

        Set(_labels.lblTiempoVueloTeorico, $"{PresentationFormatter.FormatDouble(m.TotalFlightTimeToGround, _decimals)} s");
        Set(_labels.lblAlcanceMaximo, $"{PresentationFormatter.FormatDouble(m.MaxHorizontalRange, _decimals)} m");
        Set(_labels.lblAlturaMaxima, $"{PresentationFormatter.FormatDouble(m.MaxHeight, _decimals)} m");
        Set(_labels.lblVelocidadEnVertice, PresentationFormatter.FormatVelocityInfo(m.VelocityAtApex, _decimals));
        Set(_labels.lblVelocidadInicial, PresentationFormatter.FormatVelocityInfo(m.InitialVelocity, _decimals));
        Set(_labels.lblVelocidadImpactoSueloTeorico, PresentationFormatter.FormatVelocityInfo(m.ImpactVelocityGroundOnly, _decimals));

        Set(_labels.lblTiempoSimulacion, $"{PresentationFormatter.FormatDouble(sim.SimulationTime, _decimals)} s");
        Set(_labels.lblPosicionX, $"{PresentationFormatter.FormatDouble(sim.Position.X, _decimals)} m");
        Set(_labels.lblPosicionY, $"{PresentationFormatter.FormatDouble(sim.Position.Y, _decimals)} m");
        Set(_labels.lblVelocidadVxActual, $"{PresentationFormatter.FormatDouble(vi.ComponentX, _decimals)} m/s");
        Set(_labels.lblVelocidadVyActual, $"{PresentationFormatter.FormatDouble(vi.ComponentY, _decimals)} m/s");
        Set(_labels.lblVelocidadMagnitudActual, $"{PresentationFormatter.FormatDouble(vi.Magnitude, _decimals)} m/s");
        Set(_labels.lblVelocidadAnguloActual, PresentationFormatter.FormatAngle(vi.AngleDegrees, _decimals));

        Set(_labels.lblPrimerImpactoSuperficie, PresentationFormatter.FormatSurface(fi.Surface));
        Set(_labels.lblPrimerImpactoTiempo, $"{PresentationFormatter.FormatDouble(fi.Time, _decimals)} s");
        Set(_labels.lblVelocidadPrimerImpacto, PresentationFormatter.FormatVelocityInfo(fi.VelocityInfoBeforeImpact, _decimals));

        Set(_labels.lblEstadoSimulacion, sim.IsCompleted ? "Finalizada" : "En curso");

        if (_labels.lblObjetivoInfo is { } lo && sim.Target is { } tgt)
        {
            lo.Text = $"Objetivo: y = {PresentationFormatter.FormatDouble(tgt.Y, _decimals)} m, " +
                      $"x ∈ [{PresentationFormatter.FormatDouble(tgt.MinX, _decimals)} ; {PresentationFormatter.FormatDouble(tgt.MaxX, _decimals)}] m";
        }

        if (_labels.lblRebotesResumen is { } lr)
        {
            if (sim.BounceEvents.Count == 0)
                lr.Text = "Rebotes: —";
            else
            {
                var parts = sim.BounceEvents.Select(b =>
                    $"#{b.Index} {PresentationFormatter.FormatSurface(b.Surface)} @ t={PresentationFormatter.FormatDouble(b.SimulationTime, _decimals)} s, " +
                    $"|v|={PresentationFormatter.FormatDouble(b.VelocityAfterBounce.Magnitude, _decimals)} m/s");
                lr.Text = string.Join(" | ", parts);
            }
        }
    }

    /// <summary>
    /// Rellena las series de datos y marca tiempos de rebote con bandas verticales en el eje X (t) o en x (trayectoria).
    /// </summary>
    public void UpdateCharts(SimulationController sim)
    {
        var samples = sim.TrajectorySamples;
        var bounceTimes = sim.BounceEvents.Select(e => e.SimulationTime).ToList();

        if (_charts.chartGraficaSeleccionada is { } chartUnico)
        {
            EnsureComboItems();
            var idx = Math.Clamp(_charts.comboGraficaSeleccion?.SelectedIndex ?? 0, 0, 6);
            FillSingleChart(chartUnico, samples, bounceTimes, idx);
            return;
        }

        BindTimeSeries(_charts.chartYvsT, samples, bounceTimes, s => s.TimeSeconds, s => s.Y, "y(t)", "t (s)", "y (m)", true);
        BindTimeSeries(_charts.chartXvsT, samples, bounceTimes, s => s.TimeSeconds, s => s.X, "x(t)", "t (s)", "x (m)", true);
        BindTrajectory(_charts.chartYvsX, samples, bounceTimes);
        BindTimeSeries(_charts.chartVxvsT, samples, bounceTimes, s => s.TimeSeconds, s => s.Vx, "vx(t)", "t (s)", "vx (m/s)", true);
        BindTimeSeries(_charts.chartVyvsT, samples, bounceTimes, s => s.TimeSeconds, s => s.Vy, "vy(t)", "t (s)", "vy (m/s)", true);
        BindTimeSeries(_charts.chartVvsT, samples, bounceTimes, s => s.TimeSeconds, s => s.Speed, "|v|(t)", "t (s)", "|v| (m/s)", true);
        BindTimeSeries(_charts.chartAnguloVsT, samples, bounceTimes, s => s.TimeSeconds, s => s.AngleDegrees, "θ(t)", "t (s)", "θ (°)", true);
    }

    public void Refresh(SimulationController sim)
    {
        UpdateLabels(sim);
        UpdateCharts(sim);
    }

    /// <summary>
    /// Conecta el cambio de índice del ComboBox del modo “una gráfica” para redibujar sin esperar al Timer.
    /// Llamar una vez tras crear el formulario: <c>binder.SubscribeGraficaSelectionChanged(() => binder.UpdateCharts(sim));</c>
    /// </summary>
    public void SubscribeGraficaSelectionChanged(Action? onSelectionChanged)
    {
        if (onSelectionChanged is null || _charts.comboGraficaSeleccion is not { } c) return;
        c.SelectedIndexChanged += (_, _) => onSelectionChanged();
    }

    private void EnsureComboItems()
    {
        var combo = _charts.comboGraficaSeleccion;
        if (combo is null) return;
        if (combo.Items.Count >= 7) return;
        combo.Items.Clear();
        combo.Items.AddRange(new object[]
        {
            "y vs t", "x vs t", "y vs x", "vx vs t", "vy vs t", "|v| vs t", "θ vs t"
        });
        if (combo.SelectedIndex < 0)
            combo.SelectedIndex = 0;
    }

    private void FillSingleChart(Chart chart, IReadOnlyList<TrajectorySample> samples, IReadOnlyList<double> bounceTimes, int index)
    {
        switch (index)
        {
            case 0:
                BindTimeSeries(chart, samples, bounceTimes, s => s.TimeSeconds, s => s.Y, "y(t)", "t (s)", "y (m)", true);
                break;
            case 1:
                BindTimeSeries(chart, samples, bounceTimes, s => s.TimeSeconds, s => s.X, "x(t)", "t (s)", "x (m)", true);
                break;
            case 2:
                BindTrajectory(chart, samples, bounceTimes);
                break;
            case 3:
                BindTimeSeries(chart, samples, bounceTimes, s => s.TimeSeconds, s => s.Vx, "vx(t)", "t (s)", "vx (m/s)", true);
                break;
            case 4:
                BindTimeSeries(chart, samples, bounceTimes, s => s.TimeSeconds, s => s.Vy, "vy(t)", "t (s)", "vy (m/s)", true);
                break;
            case 5:
                BindTimeSeries(chart, samples, bounceTimes, s => s.TimeSeconds, s => s.Speed, "|v|(t)", "t (s)", "|v| (m/s)", true);
                break;
            default:
                BindTimeSeries(chart, samples, bounceTimes, s => s.TimeSeconds, s => s.AngleDegrees, "θ(t)", "t (s)", "θ (°)", true);
                break;
        }
    }

    private static void Set(Label? label, string text)
    {
        if (label is null) return;
        label.Text = text;
    }

    private static void BindTimeSeries(
        Chart? chart,
        IReadOnlyList<TrajectorySample> samples,
        IReadOnlyList<double> bounceTimes,
        Func<TrajectorySample, double> x,
        Func<TrajectorySample, double> y,
        string seriesName,
        string xTitle,
        string yTitle,
        bool markBouncesOnTimeAxis)
    {
        if (chart is null) return;
        chart.Series.Clear();
        chart.Annotations.Clear();
        var area = EnsureArea(chart);
        area.AxisX.Title = xTitle;
        area.AxisY.Title = yTitle;

        var series = new Series(seriesName) { ChartType = SeriesChartType.Line, BorderWidth = 2 };
        chart.Series.Add(series);
        foreach (var p in samples)
            series.Points.AddXY(x(p), y(p));

        if (markBouncesOnTimeAxis && bounceTimes.Count > 0)
            AddBounceStripLines(chart, area, bounceTimes, isTimeAxis: true);
    }

    private static void BindTrajectory(Chart? chart, IReadOnlyList<TrajectorySample> samples, IReadOnlyList<double> bounceTimes)
    {
        if (chart is null) return;
        chart.Series.Clear();
        chart.Annotations.Clear();
        var area = EnsureArea(chart);
        area.AxisX.Title = "x (m)";
        area.AxisY.Title = "y (m)";

        var series = new Series("y(x)") { ChartType = SeriesChartType.Line, BorderWidth = 2 };
        chart.Series.Add(series);
        foreach (var p in samples)
            series.Points.AddXY(p.X, p.Y);

        if (samples.Count == 0 || bounceTimes.Count == 0) return;

        var bounceXs = new List<double>();
        foreach (var t in bounceTimes)
        {
            var near = samples.OrderBy(s => Math.Abs(s.TimeSeconds - t)).FirstOrDefault();
            if (Math.Abs(near.TimeSeconds - t) < 0.25 || samples.Count < 5)
                bounceXs.Add(near.X);
        }
        if (bounceXs.Count > 0)
            AddBounceStripLines(chart, area, bounceXs, isTimeAxis: false);
    }

    private static ChartArea EnsureArea(Chart chart)
    {
        if (chart.ChartAreas.Count == 0)
            chart.ChartAreas.Add(new ChartArea("AreaPrincipal"));
        return chart.ChartAreas[0];
    }

    /// <summary>
    /// Bandas verticales finas en el instante de cada rebote (visibles en todas las gráficas vs t).
    /// En y vs x, se usan posiciones x aproximadas asociadas al tiempo de rebote.
    /// </summary>
    private static void AddBounceStripLines(Chart chart, ChartArea area, IReadOnlyList<double> positions, bool isTimeAxis)
    {
        area.AxisX.StripLines.Clear();
        foreach (var pos in positions)
        {
            var strip = new StripLine
            {
                Interval = 0,
                IntervalOffset = pos,
                StripWidth = isTimeAxis ? 0.02 : 0.05,
                BackColor = Color.FromArgb(90, 255, 140, 0),
                BorderColor = Color.DarkOrange,
                BorderWidth = 1,
                Text = "rebote",
                ForeColor = Color.DarkRed,
                TextAlignment = StringAlignment.Near
            };
            area.AxisX.StripLines.Add(strip);
        }
    }
}
