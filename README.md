# TallerN2 - Simulación de Computadores

## Descripción
Aplicación de simulación de trayectorias parabólicas con análisis de física en tiempo real. Desarrollada como parte del taller de **Simulación de Computadores** en la **Universidad Pedagógica y Tecnológica de Colombia**.

**Estudiante:** Edwar Fernando Ramirez Gallo (202021451)  
**Programa:** Ingeniería de Sistemas y Computación

## Estructura del Proyecto

El proyecto está organizado en tres módulos principales:

### 📁 `src/TallerN2.App`
- **Descripción:** Aplicación principal con interfaz gráfica (WinForms)
- **Framework:** .NET 8 - Windows
- **Características:**
  - Interfaz de usuario para configurar parámetros de simulación
  - Visualización en tiempo real de gráficos
  - Mostrador de métricas teóricas y actuales

### 📁 `src/TallerN2.Simulation`
- **Descripción:** Motor de simulación y cálculos físicos
- **Framework:** .NET 8
- **Componentes:**
  - **Physics/:** Modelos de física (vectores, colisiones, métricas parabólicas)
  - **Simulation/:** Controlador de simulación y configuración

### 📁 `src/TallerN2.WinForms.Presentation`
- **Descripción:** Componentes de presentación y visualización
- **Framework:** .NET 8 - Windows
- **Características:**
  - Enlazadores de datos (binders)
  - Formateadores de presentación
  - Gestión de gráficos y etiquetas

## Requisitos

- **.NET 8** SDK o superior
- **Windows 7** o superior
- **Visual Studio 2022** o superior (opcional, puede usar la línea de comandos)

## Instalación y Configuración

### 1. Clonar el repositorio
```bash
git clone https://github.com/FeRCHo-wht/TallerN2.git
cd TallerN2
```

### 2. Compilar el proyecto
```bash
dotnet build
```

### 3. Ejecutar la aplicación
```bash
dotnet run --project src/TallerN2.App/TallerN2.App.csproj
```

## Arquitectura

```
TallerN2/
├── src/
│   ├── TallerN2.App/                    # Aplicación principal (WinForms)
│   │   ├── Program.cs                   # Punto de entrada
│   │   ├── MainForm.cs                  # Interfaz principal
│   │   └── MainForm.Designer.cs         # Diseño de la interfaz
│   ├── TallerN2.Simulation/             # Motor de simulación
│   │   ├── Physics/
│   │   │   ├── Vector2D.cs              # Vectores 2D
│   │   │   ├── PhysicsModel.cs          # Modelo de física
│   │   │   ├── ParabolicMetrics.cs      # Cálculos parabólicos
│   │   │   └── ...
│   │   └── Simulation/
│   │       ├── SimulationController.cs  # Controlador
│   │       ├── SimulationConfig.cs      # Configuración
│   │       └── ...
│   └── TallerN2.WinForms.Presentation/  # Componentes de UI
│       ├── SimulationPresentationBinder.cs
│       ├── FlightChartsSet.cs
│       └── ...
└── README.md
```

## Características Principales

✅ Simulación de trayectoria parabólica  
✅ Cálculo de métricas teóricas (tiempo de vuelo, alcance, altura máxima)  
✅ Monitoreo en tiempo real de posición y velocidad  
✅ Gráficos interactivos (Y vs T, X vs T, Y vs X, Vx vs T, Vy vs T, V vs T, Ángulo vs T)  
✅ Detección de colisiones y rebotes  
✅ Interfaz amigable y responsiva  

## Cambios Recientes

### v1.1.0 - Correcciones de Configuración
- ✅ Actualizado SDK de `Microsoft.NET.Sdk.WindowsDesktop` a `Microsoft.NET.Sdk`
- ✅ Optimizado para .NET 8 moderna
- ✅ Eliminadas advertencias de compilación

## Desarrollo

### Compilar sin advertencias
```bash
dotnet build --no-warnings
```

### Compilar en Release
```bash
dotnet build --configuration Release
```

### Limpiar artefactos de compilación
```bash
dotnet clean
```

## Contribuciones

Para contribuir al proyecto, por favor:
1. Fork el repositorio
2. Crea una rama con tu feature (`git checkout -b feature/AmazingFeature`)
3. Commit tus cambios (`git commit -m 'Add some AmazingFeature'`)
4. Push a la rama (`git push origin feature/AmazingFeature`)
5. Abre un Pull Request

## Licencia

Este proyecto es de uso educativo.

## Contacto

- **GitHub:** [FeRCHo-wht](https://github.com/FeRCHo-wht)
- **Email:** (Contacto disponible en el perfil de GitHub)

## Estado del Proyecto

✅ Compilación: Exitosa  
✅ Aplicación: Ejecutando correctamente  
✅ Tests: Pendientes