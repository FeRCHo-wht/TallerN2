namespace TallerN2.Simulation.Simulation;

/// <summary>
/// Contrato mínimo para el bucle de animación (vista WinForms con Timer del diseñador).
/// Facilita pruebas con un doble que avanza el tiempo sin UI.
/// </summary>
public interface IGameLoop
{
    void Tick(double deltaTimeSeconds);
}
