using System.Windows.Forms;

namespace TallerN2.WinForms.Presentation;

/// <summary>
/// Referencias a etiquetas del diseñador. Todas opcionales: si una es null, se omite su actualización.
/// Use los mismos <see cref="Control.Name"/> en el IDE que los nombres de parámetros documentados en <see cref="VisualComponentIds"/>.
/// </summary>
public sealed record FlightMetricLabels(
    /// <summary>Name sugerido: <see cref="VisualComponentIds.lblTiempoVueloTeorico"/></summary>
    Label? lblTiempoVueloTeorico = null,
    /// <summary>Name: lblAlcanceMaximo</summary>
    Label? lblAlcanceMaximo = null,
    /// <summary>Name: lblAlturaMaxima</summary>
    Label? lblAlturaMaxima = null,
    /// <summary>Name: lblVelocidadEnVertice</summary>
    Label? lblVelocidadEnVertice = null,
    /// <summary>Name: lblVelocidadInicial</summary>
    Label? lblVelocidadInicial = null,
    /// <summary>Name: lblVelocidadImpactoSueloTeorico</summary>
    Label? lblVelocidadImpactoSueloTeorico = null,
    /// <summary>Name: lblTiempoSimulacion</summary>
    Label? lblTiempoSimulacion = null,
    /// <summary>Name: lblPrimerImpactoSuperficie</summary>
    Label? lblPrimerImpactoSuperficie = null,
    /// <summary>Name: lblPrimerImpactoTiempo</summary>
    Label? lblPrimerImpactoTiempo = null,
    /// <summary>Name: lblVelocidadPrimerImpacto</summary>
    Label? lblVelocidadPrimerImpacto = null,
    /// <summary>Name: lblPosicionX</summary>
    Label? lblPosicionX = null,
    /// <summary>Name: lblPosicionY</summary>
    Label? lblPosicionY = null,
    /// <summary>Name: lblVelocidadVxActual</summary>
    Label? lblVelocidadVxActual = null,
    /// <summary>Name: lblVelocidadVyActual</summary>
    Label? lblVelocidadVyActual = null,
    /// <summary>Name: lblVelocidadMagnitudActual</summary>
    Label? lblVelocidadMagnitudActual = null,
    /// <summary>Name: lblVelocidadAnguloActual</summary>
    Label? lblVelocidadAnguloActual = null,
    /// <summary>Name: lblEstadoSimulacion</summary>
    Label? lblEstadoSimulacion = null,
    /// <summary>Name: lblObjetivoInfo</summary>
    Label? lblObjetivoInfo = null,
    /// <summary>Name: lblRebotesResumen</summary>
    Label? lblRebotesResumen = null);
