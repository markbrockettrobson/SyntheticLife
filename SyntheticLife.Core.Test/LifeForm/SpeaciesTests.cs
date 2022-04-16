using System;
using NUnit.Framework;

namespace SyntheticLife.Core.LifeForm.Test
{
    [TestFixture]
    public class SpeciesTests
    {
        [Test]
        public void MovementSpeedSet()
        {
            // Arrange
            var species = new Species(40.4, 10, 300);
            // Act
            var movementSpeed = species.MovementSpeed;
            // Assert
            Assert.That(movementSpeed, Is.EqualTo(40.4));
        }

        [Test]
        public void HarvestRateSet()
        {
            // Arrange
            var species = new Species(40.4, 1.2093, 300);
            // Act
            var harvestRate = species.HarvestRate;
            // Assert
            Assert.That(harvestRate, Is.EqualTo(1.2093));
        }

        [Test]
        public void MinimumOffspringCostSet()
        {
            // Arrange
            var species = new Species(40.4, 1.2093, 123.456);
            // Act
            var minimumOffspringCost = species.MinimumOffspringCost;
            // Assert
            Assert.That(minimumOffspringCost, Is.EqualTo(123.456));
        }

        [Test]
        public void NegativeMovementSpeed()
        {
            // Arrange
            // Act
            // Assert
            Assert.Throws<ArgumentException>(() => new Species(-40.4, 10, 300));
        }

        [Test]
        public void NegativeHarvestRate()
        {
            // Arrange
            // Act
            // Assert
            Assert.Throws<ArgumentException>(() => new Species(40.4, -10, 300));
        }

        [Test]
        public void NegativeMinimumOffspringCost()
        {
            // Arrange
            // Act
            // Assert
            Assert.Throws<ArgumentException>(() => new Species(40.4, 10, -300));
        }
    }
}
