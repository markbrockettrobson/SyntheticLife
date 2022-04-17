using System;
using System.Linq;
using Moq;
using NetTopologySuite.Geometries;
using NUnit.Framework;
using SyntheticLife.Core.Map;

namespace SyntheticLife.Core.Energy.Test
{
    [TestFixture]
    public class RandomEnergyDistributorTests
    {
        public Envelope Location = new ();
        public Mock<IEntityMap> MockMap = new ();
        public Mock<Random> MockRandom = new ();

        [SetUp]
        public void SetUp()
        {
            Location = new Envelope(0, 100, 0, 100);
            MockMap = new ();
            MockMap.Setup(map => map.Bounds).Returns(new Envelope(0, 30, 0, 30));

            MockRandom = new Mock<Random>();
            MockRandom.Setup(random => random.NextDouble()).Returns(0.5);
        }

        [Test]
        public void MaxSize()
        {
            // Arrange
            var randomEnergyDistributor = new RandomEnergyDistributor(MockRandom.Object, 10);
            // Act
            var energySource = randomEnergyDistributor.CreateEnergySources(MockMap.Object, 50).First();
            // Assert
            Assert.That(energySource.Energy, Is.EqualTo(10));
        }

        [TestCase(-1)]
        [TestCase(-10)]
        [TestCase(-9328.56)]
        public void MaxSizeNegative(double energy)
        {
            // Arrange
            // Act
            // Assert
            Assert.Throws<ArgumentException>(() => new RandomEnergyDistributor(MockRandom.Object, energy));
        }

        [TestCase(-1)]
        [TestCase(-10)]
        [TestCase(-9328.56)]
        public void EnergyNegative(double energy)
        {
            // Arrange
            var randomEnergyDistributor = new RandomEnergyDistributor(MockRandom.Object, 10);
            // Act
            // Assert
            Assert.Throws<ArgumentException>(() => randomEnergyDistributor.CreateEnergySources(MockMap.Object, energy));
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(10)]
        [TestCase(11)]
        [TestCase(9328.56)]
        public void AllEnergyDistributed(double energy)
        {
            // Arrange
            var randomEnergyDistributor = new RandomEnergyDistributor(MockRandom.Object, 10);
            // Act
            var energySources = randomEnergyDistributor.CreateEnergySources(MockMap.Object, energy);
            // Assert
            Assert.That(energySources.Sum(energySource => energySource.Energy), Is.EqualTo(energy));
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(10)]
        [TestCase(11)]
        [TestCase(9328.56)]
        public void AtMostOneUndersized(double energy)
        {
            // Arrange
            var randomEnergyDistributor = new RandomEnergyDistributor(MockRandom.Object, 10);
            // Act
            var energySources = randomEnergyDistributor.CreateEnergySources(MockMap.Object, energy);
            // Assert
            Assert.That(energySources.Count(energySource => energySource.Energy != 10), Is.LessThan(2));
        }

        [Test]
        public void AllWithinBounds()
        {
            // Arrange
            var randomEnergyDistributor = new RandomEnergyDistributor(MockRandom.Object, 10);
            // Act
            var energySources = randomEnergyDistributor.CreateEnergySources(MockMap.Object, 3425.2);
            // Assert
            foreach (var energySource in energySources)
            {
                Assert.That(Location.Contains(energySource.Location), Is.True);
            }
        }
    }
}
