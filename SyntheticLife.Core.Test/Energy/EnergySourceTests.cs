using NetTopologySuite.Geometries;
using NUnit.Framework;
using SyntheticLife.Core.Energy;

namespace SyntheticLife.Core.Test;

public class EnergySourceTests
{
    public Envelope Location = new ();

    [SetUp]
    public void SetUp()
    {
        Location = new Envelope(1, 1, 1, 1);
    }

    [Test]
    public void EnergySetOnConstruction()
    {
        // Arrange
        var energySource = new EnergySource(Location, 100.2);
        // Act
        var energy = energySource.Energy;
        // Assert
        Assert.That(energy, Is.EqualTo(100.2));
    }

    [Test]
    public void EnergySet()
    {
        // Arrange
        var energySource = new EnergySource(Location, 1310.4);
        // Act
        energySource.Energy -= 100;
        // Assert
        Assert.That(energySource.Energy, Is.EqualTo(1210.4));
    }
}
