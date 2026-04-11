using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace TallerN2.WinForms.Presentation;

/// <summary>
/// Referencias a controles Chart del diseñador (uno por tipo de gráfica).
/// Modo alternativo: asignar <see cref="chartGraficaSeleccionada"/> + <see cref="comboGraficaSeleccion"/> para una sola gráfica con selector.
/// </summary>
public sealed record FlightChartsSet(
    Chart? chartYvsT = null,
    Chart? chartXvsT = null,
    Chart? chartYvsX = null,
    Chart? chartVxvsT = null,
    Chart? chartVyvsT = null,
    Chart? chartVvsT = null,
    Chart? chartAnguloVsT = null,
    Chart? chartGraficaSeleccionada = null,
    ComboBox? comboGraficaSeleccion = null);
