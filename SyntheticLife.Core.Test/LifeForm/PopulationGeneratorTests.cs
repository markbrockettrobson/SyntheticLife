using System;
using System.Linq;
using Moq;
using NetTopologySuite.Geometries;
using NUnit.Framework;
using SyntheticLife.Core.Map;

namespace SyntheticLife.Core.LifeForm.Test
{
    [TestFixture]
    public class PopulationGeneratorTests
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
        public void MovementSpeed()
        {
            // Arrange
            var populationGenerator = new PopulationGenerator(MockRandom.Object, 10, 20, 30);
            // Act
            var creature = populationGenerator.CreateCreatures(1, Location).First();
            // Assert
            Assert.That(creature.Species.MovementSpeed, Is.EqualTo(10));
        }

        [Test]
        public void MovementSpeedNegative()
        {
            // Arrange
            // Act
            // Assert
            Assert.Throws<ArgumentException>(() => new PopulationGenerator(MockRandom.Object, -10, 20, 30));
        }

        [Test]
        public void HarvestRate()
        {
            // Arrange
            var populationGenerator = new PopulationGenerator(MockRandom.Object, 10, 20, 30);
            // Act
            var creature = populationGenerator.CreateCreatures(1, Location).First();
            // Assert
            Assert.That(creature.Species.HarvestRate, Is.EqualTo(20));
        }

        [Test]
        public void HarvestRateNegative()
        {
            // Arrange
            // Act
            // Assert
            Assert.Throws<ArgumentException>(() => new PopulationGenerator(MockRandom.Object, 10, -20, 30));
        }

        [Test]
        public void MinimumOffspringCost()
        {
            // Arrange
            var populationGenerator = new PopulationGenerator(MockRandom.Object, 10, 20, 30);
            // Act
            var creature = populationGenerator.CreateCreatures(1, Location).First();
            // Assert
            Assert.That(creature.Species.MinimumOffspringCost, Is.EqualTo(30));
        }

        [Test]
        public void MinimumOffspringCostNegative()
        {
            // Arrange
            // Act
            // Assert
            Assert.Throws<ArgumentException>(() => new PopulationGenerator(MockRandom.Object, 10, 20, -30));
        }

        [Test]
        public void StartingEnergy()
        {
            // Arrange
            var populationGenerator = new PopulationGenerator(MockRandom.Object, 10, 20, 30);
            // Act
            var creatures = populationGenerator.CreateCreatures(30, Location);
            // Assert
            foreach (var creature in creatures)
            {
                Assert.That(creature.Energy, Is.EqualTo(30));
            }
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(10)]
        [TestCase(20)]
        public void InitialPopulationSize(int number)
        {
            // Arrange
            var populationGenerator = new PopulationGenerator(MockRandom.Object, 10, 20, 30);
            // Act
            var creatures = populationGenerator.CreateCreatures(number, Location);
            // Assert
            Assert.That(creatures.Count, Is.EqualTo(number));
        }

        [TestCase(-2)]
        [TestCase(-20)]
        public void InitialPopulationSizetNegative(int number)
        {
            // Arrange
            var populationGenerator = new PopulationGenerator(MockRandom.Object, 10, 20, 30);
            // Act
            // Assert
            Assert.Throws<ArgumentException>(() => populationGenerator.CreateCreatures(number, Location));
        }

        [Test]
        public void UsesRandom()
        {
            // Arrange
            var populationGenerator = new PopulationGenerator(MockRandom.Object, 10, 20, 30);
            // Act
            populationGenerator.CreateCreatures(1, Location);
            // Assert
            MockRandom.Verify(random => random.NextDouble(), Times.AtLeastOnce());
        }

        [Test]
        public void WithInBounds()
        {
            // Arrange
            var populationGenerator = new PopulationGenerator(new Random(), 10, 20, 30);
            // Act
            var creatures = populationGenerator.CreateCreatures(30, Location);
            // Assert
            foreach (var creature in creatures)
            {
                Assert.That(Location.Contains(creature.Location), Is.True);
            }
        }
    }
}
