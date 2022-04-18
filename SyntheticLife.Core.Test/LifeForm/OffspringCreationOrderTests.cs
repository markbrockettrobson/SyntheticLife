using System;
using Moq;
using NetTopologySuite.Geometries;
using NUnit.Framework;
using SyntheticLife.Core.Map;

namespace SyntheticLife.Core.LifeForm.Test
{
    [TestFixture]
    public class OffspringCreationOrderTests
    {
        public Envelope Location = new ();
        public Mock<ICreature> MockParentEntity = new ();
        public Mock<ISpecies> MockSpecies = new ();
        public Mock<IEntityMap> MockMap = new ();

        [SetUp]
        public void SetUp()
        {
            Location = new Envelope(0, 1, 2, 3);
            MockSpecies = new ();
            MockParentEntity = new ();
            MockParentEntity.SetupProperty(entity => entity.Energy, 1000);
            MockParentEntity.SetupProperty(entity => entity.Location, Location);
            MockParentEntity.Setup(entity => entity.Species).Returns(MockSpecies.Object);
            MockMap = new ();
        }

        [Test]
        public void Parent()
        {
            // Arrange
            var offspringCreationOrder = new OffspringCreationOrder(
                MockParentEntity.Object,
                100);
            // Act
            var entity = offspringCreationOrder.Parent;
            // Assert
            Assert.That(entity, Is.EqualTo(MockParentEntity.Object));
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2.322)]
        [TestCase(31)]
        public void OffspringStartingEnergy(double energy)
        {
            // Arrange
            var offspringCreationOrder = new OffspringCreationOrder(
                MockParentEntity.Object,
                energy);
            // Act
            // Assert
            Assert.That(offspringCreationOrder.OffspringStartingEnergy, Is.EqualTo(energy));
        }

        [TestCase(-1)]
        [TestCase(-22)]
        [TestCase(-3.1)]
        public void OffspringStartingEnergyNegative(double energy)
        {
            // Arrange
            // Act
            // Assert
            Assert.Throws<ArgumentException>(
                () => new OffspringCreationOrder(MockParentEntity.Object, energy));
        }

        [Test]
        public void ExecuteOrderParentInsufficientEnergy()
        {
            // Arrange
            var offspringCreationOrder = new OffspringCreationOrder(
                MockParentEntity.Object,
                2000);
            // Act
            // Assert
            Assert.Throws<InvalidOperationException>(
                () => offspringCreationOrder.ExecuteOrder(MockMap.Object));
        }

        [Test]
        public void ExecuteOrderParentLostEnergy()
        {
            // Arrange
            var offspringCreationOrder = new OffspringCreationOrder(
                MockParentEntity.Object,
                100);
            // Act
            offspringCreationOrder.ExecuteOrder(MockMap.Object);
            // Assert
            MockParentEntity.VerifySet(entity => entity.Energy = 900);
        }

        [Test]
        public void ExecuteOrderOffspringAddedToMap()
        {
            // Arrange
            var offspringCreationOrder = new OffspringCreationOrder(
                MockParentEntity.Object,
                100);
            // Act
            offspringCreationOrder.ExecuteOrder(MockMap.Object);
            // Assert
            MockMap.Verify(map => map.AddEntity(It.IsAny<Creature>()));
        }

        [Test]
        public void ExecuteOrderOffspringStartingEnergySet()
        {
            // Arrange
            var offspringCreationOrder = new OffspringCreationOrder(
                MockParentEntity.Object,
                100);
            // Act
            offspringCreationOrder.ExecuteOrder(MockMap.Object);
            // Assert
            MockMap.Verify(map => map.AddEntity(It.Is<Creature>(creature => creature.Energy == 100)));
        }

        [Test]
        public void ExecuteOrderOffspringLocationSet()
        {
            // Arrange
            var offspringCreationOrder = new OffspringCreationOrder(
                MockParentEntity.Object,
                100);
            // Act
            offspringCreationOrder.ExecuteOrder(MockMap.Object);
            // Assert
            MockMap.Verify(map => map.AddEntity(It.Is<Creature>(creature => creature.Location == Location)));
        }

        [Test]
        public void ExecuteOrderOffspringSpeciesSet()
        {
            // Arrange
            var offspringCreationOrder = new OffspringCreationOrder(
                MockParentEntity.Object,
                100);
            // Act
            offspringCreationOrder.ExecuteOrder(MockMap.Object);
            // Assert
            MockMap.Verify(map => map.AddEntity(It.Is<Creature>(creature => creature.Species == MockSpecies.Object)));
        }
    }
}
