namespace TallerN2.WinForms.Presentation;

/// <summary>
/// Nombres (<c>Name</c> en el diseñador de Visual Studio) que deben coincidir con los controles
/// que enlazas al construir <see cref="FlightMetricLabels"/> y <see cref="FlightChartsSet"/>.
/// </summary>
public static class VisualComponentIds
{
    public const string lblTiempoVueloTeorico = nameof(lblTiempoVueloTeorico);
    public const string lblAlcanceMaximo = nameof(lblAlcanceMaximo);
    public const string lblAlturaMaxima = nameof(lblAlturaMaxima);
    public const string lblVelocidadEnVertice = nameof(lblVelocidadEnVertice);
    public const string lblVelocidadInicial = nameof(lblVelocidadInicial);
    public const string lblVelocidadImpactoSueloTeorico = nameof(lblVelocidadImpactoSueloTeorico);
    public const string lblTiempoSimulacion = nameof(lblTiempoSimulacion);
    public const string lblPrimerImpactoSuperficie = nameof(lblPrimerImpactoSuperficie);
    public const string lblPrimerImpactoTiempo = nameof(lblPrimerImpactoTiempo);
    public const string lblVelocidadPrimerImpacto = nameof(lblVelocidadPrimerImpacto);
    public const string lblPosicionX = nameof(lblPosicionX);
    public const string lblPosicionY = nameof(lblPosicionY);
    public const string lblVelocidadVxActual = nameof(lblVelocidadVxActual);
    public const string lblVelocidadVyActual = nameof(lblVelocidadVyActual);
    public const string lblVelocidadMagnitudActual = nameof(lblVelocidadMagnitudActual);
    public const string lblVelocidadAnguloActual = nameof(lblVelocidadAnguloActual);
    public const string lblEstadoSimulacion = nameof(lblEstadoSimulacion);
    public const string lblObjetivoInfo = nameof(lblObjetivoInfo);
    public const string lblRebotesResumen = nameof(lblRebotesResumen);

    public const string chartYvsT = nameof(chartYvsT);
    public const string chartXvsT = nameof(chartXvsT);
    public const string chartYvsX = nameof(chartYvsX);
    public const string chartVxvsT = nameof(chartVxvsT);
    public const string chartVyvsT = nameof(chartVyvsT);
    public const string chartVvsT = nameof(chartVvsT);
    public const string chartAnguloVsT = nameof(chartAnguloVsT);

    /// <summary>Opcional: un solo Chart que muestra una gráfica según el índice seleccionado.</summary>
    public const string chartGraficaSeleccionada = nameof(chartGraficaSeleccionada);

    /// <summary>Opcional: ComboBox con ítems 0..6 para elegir qué serie muestra <see cref="chartGraficaSeleccionada"/>.</summary>
    public const string comboGraficaSeleccion = nameof(comboGraficaSeleccion);
}
