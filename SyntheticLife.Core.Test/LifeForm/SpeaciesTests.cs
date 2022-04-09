using System;
using NetTopologySuite.Geometries;
using NUnit.Framework;
using SyntheticLife.Core.LifeForm;

namespace SyntheticLife.Core.Test
{
    [TestFixture]
    public class SpeciesTests
    {
        public Envelope Location = new ();

        [SetUp]
        public void SetUp()
        {
            Location = new Envelope(1, 1, 1, 1);
        }

        [Test]
        public void MovementSpeedSet()
        {
            // Arrange
            var species = new Species(Location, 40.4);
            // Act
            var movementSpeed = species.MovementSpeed;
            // Assert
            Assert.That(movementSpeed, Is.EqualTo(40.4));
        }

        [TestCase(-1)]
        [TestCase(-90213.23)]
        [TestCase(-0.1)]
        public void MovementEnergyCoastNegative(double distance)
        {
            // Arrange
            var species = new Species(Location, 100);
            // Act
            // Assert
            Assert.Throws<ArgumentException>(() => species.MovementEnergyCoast(distance));
        }

        [TestCase(-0, 0)]
        [TestCase(1, 0.1)]
        [TestCase(2, 0.4)]
        [TestCase(10, 10)]
        [TestCase(20, 40)]
        [TestCase(20.1, 40.401)]
        public void MovementEnergyCoastNegative(double distance, double expectedCost)
        {
            // Arrange
            var species = new Species(Location, 100);
            // Act
            // Assert
            Assert.That(species.MovementEnergyCoast(distance), Is.InRange(expectedCost - 0.1, expectedCost + 0.1));
        }
    }
}
