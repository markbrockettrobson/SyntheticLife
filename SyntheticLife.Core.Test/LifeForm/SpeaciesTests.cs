using System;
using NetTopologySuite.Geometries;
using NUnit.Framework;

namespace SyntheticLife.Core.LifeForm.Test
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
            var species = new Species(Location, 40.4, 10);
            // Act
            var movementSpeed = species.MovementSpeed;
            // Assert
            Assert.That(movementSpeed, Is.EqualTo(40.4));
        }

        [Test]
        public void HarvestRateSet()
        {
            // Arrange
            var species = new Species(Location, 40.4, 1.2093);
            // Act
            var harvestRate = species.HarvestRate;
            // Assert
            Assert.That(harvestRate, Is.EqualTo(1.2093));
        }

        [Test]
        public void NegativeMovementSpeedSet()
        {
            // Arrange
            // Act
            // Assert
            Assert.Throws<ArgumentException>(() => new Species(Location, -40.4, 10));
        }

        [Test]
        public void NegativeHarvestRateSet()
        {
            // Arrange
            // Act
            // Assert
            Assert.Throws<ArgumentException>(() => new Species(Location, 40.4, -10));
        }
    }
}
