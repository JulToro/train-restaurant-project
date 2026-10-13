# Railway Control

Solución en **.NET 10** para el control y automatización ferroviaria del restaurante (Railway Restaurant), integrando modelos de dominio, cliente de comunicación con JMRI y pruebas unitarias.

## Estructura del Proyecto

```text
railway-control/
├── RailwayControl.sln
├── src/
│   ├── Railway.Core/              # Librería principal de dominio y lógica ferroviaria
│   ├── Railway.Jmri.Client/       # Cliente de comunicación con JMRI
│   └── Railway.Control.Spike/     # Aplicación de consola / Spike para prototipado
├── tests/
│   └── Railway.Tests/             # Pruebas xUnit de dominio e integración
│       ├── Core/                  # Pruebas unitarias del dominio
│       └── Jmri/                  # Pruebas del cliente con WebSocket local
├── README.md
└── .gitignore
```

### Proyectos

- **`src/Railway.Core`**: Biblioteca de clases (.NET 10) que contiene las entidades de dominio, contratos, interfaces de servicios y lógica de control de trenes y vías.
- **`src/Railway.Jmri.Client`**: Biblioteca de clases (.NET 10) que implementa la integración y cliente para comunicarse con JMRI (Java Model Railroad Interface).
- **`src/Railway.Control.Spike`**: Aplicación de consola (.NET 10) utilizada para prototipos rápidos y pruebas directas de control de hardware/servicios.
- **`tests/Railway.Tests`**: Proyecto de pruebas xUnit (.NET 10). Incluye pruebas unitarias del dominio en `Core/` y pruebas de integración del cliente JMRI en `Jmri/`, usando un servidor WebSocket local sin requerir JMRI instalado.

## Requisitos Previos

- [.NET 10 SDK](https://dotnet.microsoft.com/download)

## Comandos Principales

### Restaurar dependencias
```bash
dotnet restore RailwayControl.sln
```

### Compilar la solución
```bash
dotnet build RailwayControl.sln
```

### Ejecutar las pruebas
```bash
dotnet test RailwayControl.sln
```

### Ejecutar el Spike
```bash
dotnet run --project src/Railway.Control.Spike/Railway.Control.Spike.csproj
```
