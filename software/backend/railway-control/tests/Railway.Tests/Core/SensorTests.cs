using Railway.Core.Domain;
using Xunit;

namespace Railway.Tests.Core;

public class SensorTests
{
    [Fact]
    public void Constructor_ShouldInitializePropertiesAndSetStateToUnknown()
    {
        // Arrange (Preparar datos de entrada)
        var id = "B1";
        var name = "Bloque Entrada Estación";

        // Act (Ejecutar la acción a probar)
        var sensor = new Sensor(id, name);

        // Assert (Verificar el resultado esperado)
        Assert.Equal(id, sensor.Id);
        Assert.Equal(name, sensor.Name);
        Assert.Equal(SensorState.Unknown, sensor.State);
    }

        [Fact]
    public void SetState_ShouldChangeStateCorrectly()
    {
        // Arrange: creamos el sensor y lo ponemos primero como activo
        var sensor = new Sensor("B1", "Desactivar Sensor");
        sensor.SetState(SensorState.Active);

        // Act
        sensor.SetState(SensorState.Inactive);

        // Assert
        Assert.Equal(SensorState.Inactive, sensor.State);
    }
}
