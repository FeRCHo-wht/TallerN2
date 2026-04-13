using System.Windows.Forms;
using TallerN2.Simulation.Physics;
using TallerN2.Simulation.Simulation;
using TallerN2.WinForms.Presentation;

namespace TallerN2.App;

public partial class MainForm : Form
{
    private SimulationController? _controller;
    private System.Windows.Forms.Timer? _updateTimer;
    private SimulationPresentationBinder? _binder;
    private Random _random = new();

    public MainForm()
    {
        InitializeComponent();
        this.StartPosition = FormStartPosition.CenterScreen;
        this.Size = new System.Drawing.Size(1400, 900);

        InitializeUpdateTimer();
    }

    private void InitializeUpdateTimer()
    {
        _updateTimer = new System.Windows.Forms.Timer();
        _updateTimer.Interval = 16; // ~60 FPS
        _updateTimer.Tick += UpdateTimer_Tick;
    }

    private void BtnIniciar_Click(object? sender, EventArgs e)
    {
        try
        {
            if (!double.TryParse(_txtVelocidad.Text, out var velocity) || velocity <= 0)
            {
                MessageBox.Show("Ingrese una velocidad válida (> 0)", "Error");
                return;
            }

            if (!double.TryParse(_txtAngulo.Text, out var angleDegrees) || angleDegrees < 0 || angleDegrees > 90)
            {
                MessageBox.Show("Ingrese un ángulo válido (0-90°)", "Error");
                return;
            }

            ClearAllCharts();

            var config = new SimulationConfig(
                Gravity: -9.81,
                GroundY: 0,
                TargetXMin: 10,
                TargetXMax: 40,
                TargetYMin: 0.5,
                TargetYMax: 3,
                TargetWidth: 2.0);

            _controller = new SimulationController(config);

            var labels = new FlightMetricLabels(
                lblTiempoVueloTeorico: _lblTiempoVueloTeorico,
                lblAlcanceMaximo: _lblAlcanceMaximo,
                lblAlturaMaxima: _lblAlturaMaxima,
                lblVelocidadEnVertice: _lblVelocidadEnVertice,
                lblVelocidadInicial: _lblVelocidadInicial,
                lblVelocidadImpactoSueloTeorico: _lblVelocidadImpacto,
                lblTiempoSimulacion: _lblTiempoSimulacion,
                lblPosicionX: _lblPosicionX,
                lblPosicionY: _lblPosicionY,
                lblVelocidadVxActual: _lblVelocidadVx,
                lblVelocidadVyActual: _lblVelocidadVy,
                lblVelocidadMagnitudActual: _lblVelocidadMagnitud,
                lblVelocidadAnguloActual: _lblVelocidadAngulo,
                lblRebotesResumen: _lblRebotes);

            var charts = new FlightChartsSet(
                chartYvsT: _chartYvsT,
                chartXvsT: _chartXvsT,
                chartYvsX: _chartYvsX,
                chartVxvsT: _chartVxvsT,
                chartVyvsT: _chartVyvsT,
                chartVvsT: _chartVvsT,
                chartAnguloVsT: _chartAngulovsT);

            _binder = new SimulationPresentationBinder(labels, charts, decimals: 3);

            var angleRadians = angleDegrees * Math.PI / 180;
            var initialVelocity = new Vector2D(
                velocity * Math.Cos(angleRadians),
                velocity * Math.Sin(angleRadians));

            var origin = new Vector2D(0, 0);
            _controller.Reset(origin, initialVelocity, _random);
            _binder.UpdateLabels(_controller);

            _updateTimer?.Start();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error: {ex.Message}", "Error");
        }
    }

    private void BtnDetener_Click(object? sender, EventArgs e)
    {
        _updateTimer?.Stop();
    }

    private void BtnReiniciar_Click(object? sender, EventArgs e)
    {
        _updateTimer?.Stop();
        ClearAllCharts();
        _controller = null;
        _binder = null;
    }

    private void UpdateTimer_Tick(object? sender, EventArgs e)
    {
        try
        {
            if (_controller is null || _binder is null) return;

            _controller.Tick(0.016); // ~60 FPS
            _binder.UpdateLabels(_controller);
            UpdateCharts();

            if (_controller.IsCompleted)
            {
                _updateTimer?.Stop();
            }
        }
        catch (Exception ex)
        {
            _updateTimer?.Stop();
            MessageBox.Show($"Error en simulación: {ex.Message}", "Error");
        }
    }

    private void UpdateCharts()
    {
        if (_controller is null) return;

        var samples = _controller.TrajectorySamples;
        if (samples.Count == 0) return;

        var sample = samples[samples.Count - 1];

        // Y vs T
        _chartYvsT.Series[0].Points.AddXY(sample.TimeSeconds, sample.Y);

        // X vs T
        _chartXvsT.Series[0].Points.AddXY(sample.TimeSeconds, sample.X);

        // Y vs X (Trayectoria)
        _chartYvsX.Series[0].Points.AddXY(sample.X, sample.Y);

        // Vx vs T
        _chartVxvsT.Series[0].Points.AddXY(sample.TimeSeconds, sample.Vx);

        // Vy vs T
        _chartVyvsT.Series[0].Points.AddXY(sample.TimeSeconds, sample.Vy);

        // |V| vs T
        _chartVvsT.Series[0].Points.AddXY(sample.TimeSeconds, sample.Speed);

        // θ vs T
        _chartAngulovsT.Series[0].Points.AddXY(sample.TimeSeconds, sample.AngleDegrees);
    }

    private void ClearAllCharts()
    {
        _chartYvsT.Series[0].Points.Clear();
        _chartXvsT.Series[0].Points.Clear();
        _chartYvsX.Series[0].Points.Clear();
        _chartVxvsT.Series[0].Points.Clear();
        _chartVyvsT.Series[0].Points.Clear();
        _chartVvsT.Series[0].Points.Clear();
        _chartAngulovsT.Series[0].Points.Clear();
    }
}
