namespace TallerN2.App;

partial class MainForm
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        
        // Panel de entrada de parámetros
        var panelInput = new System.Windows.Forms.Panel();
        var labelVelocidad = new System.Windows.Forms.Label();
        var txtVelocidad = new System.Windows.Forms.TextBox();
        var labelAngulo = new System.Windows.Forms.Label();
        var txtAngulo = new System.Windows.Forms.TextBox();
        var btnIniciar = new System.Windows.Forms.Button();
        var btnDetener = new System.Windows.Forms.Button();
        var btnReiniciar = new System.Windows.Forms.Button();
        
        // Etiquetas para métricas teóricas
        var labelTeorico = new System.Windows.Forms.Label();
        var lblTiempoVueloTeorico = new System.Windows.Forms.Label();
        var lblAlcanceMaximo = new System.Windows.Forms.Label();
        var lblAlturaMaxima = new System.Windows.Forms.Label();
        var lblVelocidadEnVertice = new System.Windows.Forms.Label();
        var lblVelocidadInicial = new System.Windows.Forms.Label();
        var lblVelocidadImpacto = new System.Windows.Forms.Label();
        
        // Etiquetas para métricas en vivo
        var labelVivo = new System.Windows.Forms.Label();
        var lblTiempoSimulacion = new System.Windows.Forms.Label();
        var lblPosicionX = new System.Windows.Forms.Label();
        var lblPosicionY = new System.Windows.Forms.Label();
        var lblVelocidadVx = new System.Windows.Forms.Label();
        var lblVelocidadVy = new System.Windows.Forms.Label();
        var lblVelocidadMagnitud = new System.Windows.Forms.Label();
        var lblVelocidadAngulo = new System.Windows.Forms.Label();
        
        // Etiquetas para rebotes
        var labelRebotes = new System.Windows.Forms.Label();
        var lblRebotes = new System.Windows.Forms.Label();
        var lblPosicionRebote = new System.Windows.Forms.Label();
        var lblVelocidadRebote = new System.Windows.Forms.Label();
        
        // Panel de gráficas
        var tabControl = new System.Windows.Forms.TabControl();
        var tabYvsT = new System.Windows.Forms.TabPage();
        var tabXvsT = new System.Windows.Forms.TabPage();
        var tabYvsX = new System.Windows.Forms.TabPage();
        var tabVxvsT = new System.Windows.Forms.TabPage();
        var tabVyvsT = new System.Windows.Forms.TabPage();
        var tabVvsT = new System.Windows.Forms.TabPage();
        var tabAngulovsT = new System.Windows.Forms.TabPage();
        
        var chartYvsT = new System.Windows.Forms.DataVisualization.Charting.Chart();
        var chartXvsT = new System.Windows.Forms.DataVisualization.Charting.Chart();
        var chartYvsX = new System.Windows.Forms.DataVisualization.Charting.Chart();
        var chartVxvsT = new System.Windows.Forms.DataVisualization.Charting.Chart();
        var chartVyvsT = new System.Windows.Forms.DataVisualization.Charting.Chart();
        var chartVvsT = new System.Windows.Forms.DataVisualization.Charting.Chart();
        var chartAngulovsT = new System.Windows.Forms.DataVisualization.Charting.Chart();

        // MainForm
        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(1400, 800);
        this.Text = "Simulación de Movimiento Parabólico con Rebotes";
        
        // Panel Input
        panelInput.Dock = System.Windows.Forms.DockStyle.Left;
        panelInput.Width = 250;
        panelInput.BackColor = System.Drawing.SystemColors.Control;
        panelInput.Padding = new System.Windows.Forms.Padding(10);
        
        labelVelocidad.Text = "Velocidad (m/s):";
        labelVelocidad.Location = new System.Drawing.Point(10, 10);
        labelVelocidad.AutoSize = true;
        panelInput.Controls.Add(labelVelocidad);
        
        txtVelocidad.Text = "20";
        txtVelocidad.Location = new System.Drawing.Point(10, 30);
        txtVelocidad.Width = 220;
        panelInput.Controls.Add(txtVelocidad);
        
        labelAngulo.Text = "Ángulo (grados):";
        labelAngulo.Location = new System.Drawing.Point(10, 60);
        labelAngulo.AutoSize = true;
        panelInput.Controls.Add(labelAngulo);
        
        txtAngulo.Text = "45";
        txtAngulo.Location = new System.Drawing.Point(10, 80);
        txtAngulo.Width = 220;
        panelInput.Controls.Add(txtAngulo);
        
        btnIniciar.Text = "Iniciar";
        btnIniciar.Location = new System.Drawing.Point(10, 110);
        btnIniciar.Width = 220;
        btnIniciar.Click += BtnIniciar_Click;
        panelInput.Controls.Add(btnIniciar);
        
        btnDetener.Text = "Detener";
        btnDetener.Location = new System.Drawing.Point(10, 140);
        btnDetener.Width = 220;
        btnDetener.Click += BtnDetener_Click;
        panelInput.Controls.Add(btnDetener);
        
        btnReiniciar.Text = "Reiniciar";
        btnReiniciar.Location = new System.Drawing.Point(10, 170);
        btnReiniciar.Width = 220;
        btnReiniciar.Click += BtnReiniciar_Click;
        panelInput.Controls.Add(btnReiniciar);
        
        // Métricas teóricas
        labelTeorico.Text = "=== MÉTRICAS TEÓRICAS ===";
        labelTeorico.Location = new System.Drawing.Point(10, 210);
        labelTeorico.Font = new System.Drawing.Font(labelTeorico.Font, System.Drawing.FontStyle.Bold);
        labelTeorico.AutoSize = true;
        panelInput.Controls.Add(labelTeorico);
        
        lblTiempoVueloTeorico.Text = "T. Vuelo: -";
        lblTiempoVueloTeorico.Location = new System.Drawing.Point(10, 235);
        lblTiempoVueloTeorico.AutoSize = true;
        panelInput.Controls.Add(lblTiempoVueloTeorico);
        
        lblAlcanceMaximo.Text = "Alcance: -";
        lblAlcanceMaximo.Location = new System.Drawing.Point(10, 255);
        lblAlcanceMaximo.AutoSize = true;
        panelInput.Controls.Add(lblAlcanceMaximo);
        
        lblAlturaMaxima.Text = "Alt. Máx: -";
        lblAlturaMaxima.Location = new System.Drawing.Point(10, 275);
        lblAlturaMaxima.AutoSize = true;
        panelInput.Controls.Add(lblAlturaMaxima);
        
        lblVelocidadEnVertice.Text = "V. Vértice: -";
        lblVelocidadEnVertice.Location = new System.Drawing.Point(10, 295);
        lblVelocidadEnVertice.AutoSize = true;
        panelInput.Controls.Add(lblVelocidadEnVertice);
        
        lblVelocidadInicial.Text = "V. Inicial: -";
        lblVelocidadInicial.Location = new System.Drawing.Point(10, 315);
        lblVelocidadInicial.AutoSize = true;
        panelInput.Controls.Add(lblVelocidadInicial);
        
        lblVelocidadImpacto.Text = "V. Impacto: -";
        lblVelocidadImpacto.Location = new System.Drawing.Point(10, 335);
        lblVelocidadImpacto.AutoSize = true;
        panelInput.Controls.Add(lblVelocidadImpacto);
        
        // Métricas en vivo
        labelVivo.Text = "=== MÉTRICAS VIVAS ===";
        labelVivo.Location = new System.Drawing.Point(10, 365);
        labelVivo.Font = new System.Drawing.Font(labelVivo.Font, System.Drawing.FontStyle.Bold);
        labelVivo.AutoSize = true;
        panelInput.Controls.Add(labelVivo);
        
        lblTiempoSimulacion.Text = "T. Sim: -";
        lblTiempoSimulacion.Location = new System.Drawing.Point(10, 390);
        lblTiempoSimulacion.AutoSize = true;
        panelInput.Controls.Add(lblTiempoSimulacion);
        
        lblPosicionX.Text = "X: -";
        lblPosicionX.Location = new System.Drawing.Point(10, 410);
        lblPosicionX.AutoSize = true;
        panelInput.Controls.Add(lblPosicionX);
        
        lblPosicionY.Text = "Y: -";
        lblPosicionY.Location = new System.Drawing.Point(10, 430);
        lblPosicionY.AutoSize = true;
        panelInput.Controls.Add(lblPosicionY);
        
        lblVelocidadVx.Text = "Vx: -";
        lblVelocidadVx.Location = new System.Drawing.Point(10, 450);
        lblVelocidadVx.AutoSize = true;
        panelInput.Controls.Add(lblVelocidadVx);
        
        lblVelocidadVy.Text = "Vy: -";
        lblVelocidadVy.Location = new System.Drawing.Point(10, 470);
        lblVelocidadVy.AutoSize = true;
        panelInput.Controls.Add(lblVelocidadVy);
        
        lblVelocidadMagnitud.Text = "|V|: -";
        lblVelocidadMagnitud.Location = new System.Drawing.Point(10, 490);
        lblVelocidadMagnitud.AutoSize = true;
        panelInput.Controls.Add(lblVelocidadMagnitud);
        
        lblVelocidadAngulo.Text = "θ: -";
        lblVelocidadAngulo.Location = new System.Drawing.Point(10, 510);
        lblVelocidadAngulo.AutoSize = true;
        panelInput.Controls.Add(lblVelocidadAngulo);
        
        // Rebotes
        labelRebotes.Text = "=== REBOTES ===";
        labelRebotes.Location = new System.Drawing.Point(10, 540);
        labelRebotes.Font = new System.Drawing.Font(labelRebotes.Font, System.Drawing.FontStyle.Bold);
        labelRebotes.AutoSize = true;
        panelInput.Controls.Add(labelRebotes);
        
        lblRebotes.Text = "Rebotes: 0";
        lblRebotes.Location = new System.Drawing.Point(10, 565);
        lblRebotes.AutoSize = true;
        panelInput.Controls.Add(lblRebotes);
        
        lblPosicionRebote.Text = "Pos: -";
        lblPosicionRebote.Location = new System.Drawing.Point(10, 585);
        lblPosicionRebote.AutoSize = true;
        panelInput.Controls.Add(lblPosicionRebote);
        
        lblVelocidadRebote.Text = "Vel: -";
        lblVelocidadRebote.Location = new System.Drawing.Point(10, 605);
        lblVelocidadRebote.AutoSize = true;
        panelInput.Controls.Add(lblVelocidadRebote);
        
        this.Controls.Add(panelInput);
        
        // Tab Control
        tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
        
        // Tab Y vs T
        tabYvsT.Text = "Y vs T";
        chartYvsT.Dock = System.Windows.Forms.DockStyle.Fill;
        this.ConfigureChart(chartYvsT, "Tiempo (s)", "Posición Y (m)", "Y vs Tiempo");
        tabYvsT.Controls.Add(chartYvsT);
        tabControl.TabPages.Add(tabYvsT);
        this._chartYvsT = chartYvsT;
        
        // Tab X vs T
        tabXvsT.Text = "X vs T";
        chartXvsT.Dock = System.Windows.Forms.DockStyle.Fill;
        this.ConfigureChart(chartXvsT, "Tiempo (s)", "Posición X (m)", "X vs Tiempo");
        tabXvsT.Controls.Add(chartXvsT);
        tabControl.TabPages.Add(tabXvsT);
        this._chartXvsT = chartXvsT;
        
        // Tab Y vs X
        tabYvsX.Text = "Y vs X";
        chartYvsX.Dock = System.Windows.Forms.DockStyle.Fill;
        this.ConfigureChart(chartYvsX, "Posición X (m)", "Posición Y (m)", "Trayectoria (Y vs X)");
        tabYvsX.Controls.Add(chartYvsX);
        tabControl.TabPages.Add(tabYvsX);
        this._chartYvsX = chartYvsX;
        
        // Tab Vx vs T
        tabVxvsT.Text = "Vx vs T";
        chartVxvsT.Dock = System.Windows.Forms.DockStyle.Fill;
        this.ConfigureChart(chartVxvsT, "Tiempo (s)", "Velocidad X (m/s)", "Vx vs Tiempo");
        tabVxvsT.Controls.Add(chartVxvsT);
        tabControl.TabPages.Add(tabVxvsT);
        this._chartVxvsT = chartVxvsT;
        
        // Tab Vy vs T
        tabVyvsT.Text = "Vy vs T";
        chartVyvsT.Dock = System.Windows.Forms.DockStyle.Fill;
        this.ConfigureChart(chartVyvsT, "Tiempo (s)", "Velocidad Y (m/s)", "Vy vs Tiempo");
        tabVyvsT.Controls.Add(chartVyvsT);
        tabControl.TabPages.Add(tabVyvsT);
        this._chartVyvsT = chartVyvsT;
        
        // Tab V vs T
        tabVvsT.Text = "|V| vs T";
        chartVvsT.Dock = System.Windows.Forms.DockStyle.Fill;
        this.ConfigureChart(chartVvsT, "Tiempo (s)", "Magnitud de Velocidad (m/s)", "|V| vs Tiempo");
        tabVvsT.Controls.Add(chartVvsT);
        tabControl.TabPages.Add(tabVvsT);
        this._chartVvsT = chartVvsT;
        
        // Tab Ángulo vs T
        tabAngulovsT.Text = "θ vs T";
        chartAngulovsT.Dock = System.Windows.Forms.DockStyle.Fill;
        this.ConfigureChart(chartAngulovsT, "Tiempo (s)", "Ángulo (°)", "θ vs Tiempo");
        tabAngulovsT.Controls.Add(chartAngulovsT);
        tabControl.TabPages.Add(tabAngulovsT);
        this._chartAngulovsT = chartAngulovsT;
        
        this.Controls.Add(tabControl);
        
        // Store references for later
        this._lblTiempoVueloTeorico = lblTiempoVueloTeorico;
        this._lblAlcanceMaximo = lblAlcanceMaximo;
        this._lblAlturaMaxima = lblAlturaMaxima;
        this._lblVelocidadEnVertice = lblVelocidadEnVertice;
        this._lblVelocidadInicial = lblVelocidadInicial;
        this._lblVelocidadImpacto = lblVelocidadImpacto;
        this._lblTiempoSimulacion = lblTiempoSimulacion;
        this._lblPosicionX = lblPosicionX;
        this._lblPosicionY = lblPosicionY;
        this._lblVelocidadVx = lblVelocidadVx;
        this._lblVelocidadVy = lblVelocidadVy;
        this._lblVelocidadMagnitud = lblVelocidadMagnitud;
        this._lblVelocidadAngulo = lblVelocidadAngulo;
        this._lblRebotes = lblRebotes;
        this._lblPosicionRebote = lblPosicionRebote;
        this._lblVelocidadRebote = lblVelocidadRebote;
        this._txtVelocidad = txtVelocidad;
        this._txtAngulo = txtAngulo;
    }
    
    private void ConfigureChart(System.Windows.Forms.DataVisualization.Charting.Chart chart, string xAxisTitle, string yAxisTitle, string title)
    {
        chart.Titles.Add(title);
        chart.Titles[0].Font = new System.Drawing.Font("Arial", 12, System.Drawing.FontStyle.Bold);
        
        var chartArea = new System.Windows.Forms.DataVisualization.Charting.ChartArea("MainArea");
        chartArea.AxisX.Title = xAxisTitle;
        chartArea.AxisY.Title = yAxisTitle;
        chart.ChartAreas.Add(chartArea);
        
        var series = new System.Windows.Forms.DataVisualization.Charting.Series("Data");
        series.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
        chart.Series.Add(series);
    }
    
    private System.Windows.Forms.DataVisualization.Charting.Chart _chartYvsT;
    private System.Windows.Forms.DataVisualization.Charting.Chart _chartXvsT;
    private System.Windows.Forms.DataVisualization.Charting.Chart _chartYvsX;
    private System.Windows.Forms.DataVisualization.Charting.Chart _chartVxvsT;
    private System.Windows.Forms.DataVisualization.Charting.Chart _chartVyvsT;
    private System.Windows.Forms.DataVisualization.Charting.Chart _chartVvsT;
    private System.Windows.Forms.DataVisualization.Charting.Chart _chartAngulovsT;
    
    private System.Windows.Forms.Label _lblTiempoVueloTeorico;
    private System.Windows.Forms.Label _lblAlcanceMaximo;
    private System.Windows.Forms.Label _lblAlturaMaxima;
    private System.Windows.Forms.Label _lblVelocidadEnVertice;
    private System.Windows.Forms.Label _lblVelocidadInicial;
    private System.Windows.Forms.Label _lblVelocidadImpacto;
    private System.Windows.Forms.Label _lblTiempoSimulacion;
    private System.Windows.Forms.Label _lblPosicionX;
    private System.Windows.Forms.Label _lblPosicionY;
    private System.Windows.Forms.Label _lblVelocidadVx;
    private System.Windows.Forms.Label _lblVelocidadVy;
    private System.Windows.Forms.Label _lblVelocidadMagnitud;
    private System.Windows.Forms.Label _lblVelocidadAngulo;
    private System.Windows.Forms.Label _lblRebotes;
    private System.Windows.Forms.Label _lblPosicionRebote;
    private System.Windows.Forms.Label _lblVelocidadRebote;
    private System.Windows.Forms.TextBox _txtVelocidad;
    private System.Windows.Forms.TextBox _txtAngulo;
}
