using Railway.Core.Domain;
using Xunit;

namespace Railway.Tests.Core;

public class BlockTests
{
    [Fact]
    public void Constructor_ShouldInitializePropertiesAndSetOccupancyToUnknown()
    {
        // Arrange (Preparar datos de entrada)
        var id = "B1";
        var name = "Bloque Entrada Estación";

        // Act (Ejecutar la acción a probar)
        var block = new Block(id, name);

        // Assert (Verificar el resultado esperado)
        Assert.Equal(id, block.Id);
        Assert.Equal(name, block.Name);
        Assert.Equal(BlockOccupancy.Unknown, block.Occupancy);
    }

    [Fact]
    public void MarkOccupied_ShouldChangeOccupancyToOccupied()
    {
        // Arrange
        var block = new Block("B1", "Bloque Entrada Estación");

        // Act
        block.MarkOccupied();

        // Assert
        Assert.Equal(BlockOccupancy.Occupied, block.Occupancy);
    }

    [Fact]
    public void MarkFree_ShouldChangeOccupancyToFree()
    {
        // Arrange: creamos el bloque y lo ponemos primero como ocupado
        var block = new Block("B1", "Bloque Entrada Estación");
        block.MarkOccupied();

        // Act
        block.MarkFree();

        // Assert
        Assert.Equal(BlockOccupancy.Free, block.Occupancy);
    }
}
