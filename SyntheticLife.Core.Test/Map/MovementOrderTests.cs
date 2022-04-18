using Moq;
using NetTopologySuite.Geometries;
using NUnit.Framework;
using SyntheticLife.Core.LifeForm;

namespace SyntheticLife.Core.Map.Test
{
    [TestFixture]
    public class MovementOrderTests
    {
        public Envelope LocationOne = new ();
        public Envelope LocationTwo = new ();

        public Mock<ICreature> MockCreature = new ();
        public Mock<IEntityMap> MockEntityMap = new ();

        [SetUp]
        public void SetUp()
        {
            LocationOne = new Envelope(1, 1, 1, 1);
            LocationTwo = new Envelope(2, 2, 2, 2);

            MockCreature = new ();
            MockCreature.SetupProperty(creature => creature.Location, LocationOne);
            MockCreature.SetupProperty(creature => creature.Energy, 1000);

            MockEntityMap = new ();
        }

        [Test]
        public void Creature()
        {
            // Arrange
            var movementOrder = new MovementOrder(MockCreature.Object, LocationTwo);
            // Act
            // Assert
            Assert.That(movementOrder.Creature, Is.EqualTo(MockCreature.Object));
        }

        [Test]
        public void NewLocation()
        {
            // Arrange
            var movementOrder = new MovementOrder(MockCreature.Object, LocationTwo);
            // Act
            // Assert
            Assert.That(movementOrder.NewLocation, Is.EqualTo(LocationTwo));
        }

        [TestCase(1, 1, 0)]
        [TestCase(12, 32, 20)]
        [TestCase(-23, 4, 27)]
        [TestCase(-1, -10, 9)]
        public void EnergyCost(double coordinateOne, double coordinateTwo, double expectedCost)
        {
            // Arrange
            var locationOne = new Envelope(coordinateOne, coordinateOne, coordinateOne, coordinateOne);
            var locationTwo = new Envelope(coordinateOne, coordinateOne, coordinateTwo, coordinateTwo);
            MockCreature.SetupProperty(creature => creature.Location, locationOne);

            var movementOrder = new MovementOrder(MockCreature.Object, locationTwo);
            // Act
            var energyCost = movementOrder.EnergyCost;
            // Assert
            Assert.That(energyCost, Is.EqualTo(expectedCost));
        }

        [TestCase(1, 1, 0)]
        [TestCase(12, 32, 20)]
        [TestCase(-23, 4, 27)]
        [TestCase(-1, -10, 9)]
        public void ExecuteOrderEnergyCost(double coordinateOne, double coordinateTwo, double expectedCost)
        {
            // Arrange
            var locationOne = new Envelope(coordinateOne, coordinateOne, coordinateOne, coordinateOne);
            var locationTwo = new Envelope(coordinateOne, coordinateOne, coordinateTwo, coordinateTwo);
            MockCreature.SetupProperty(creature => creature.Location, locationOne);

            var movementOrder = new MovementOrder(MockCreature.Object, locationTwo);
            // Act
            movementOrder.ExecuteOrder(MockEntityMap.Object);
            // Assert
            MockCreature.VerifySet(entity => entity.Energy = 1000 - expectedCost);
            MockEntityMap.Verify(
                map => map.RemoveEntity(MockCreature.Object),
                Times.Never);
        }

        [Test]
        public void LocationSet()
        {
            // Arrange
            var movementOrder = new MovementOrder(MockCreature.Object, LocationTwo);
            // Act
            movementOrder.ExecuteOrder(MockEntityMap.Object);
            // Assert
            MockCreature.VerifySet(entity => entity.Location = LocationTwo);
        }

        [Test]
        public void ExecuteOrderRemoveDeadCreature()
        {
            // Arrange
            var movementOrder = new MovementOrder(MockCreature.Object, LocationTwo);
            MockCreature.SetupProperty(creature => creature.Energy, 1);
            // Act
            movementOrder.ExecuteOrder(MockEntityMap.Object);
            // Assert
            MockEntityMap.Verify(
                map => map.RemoveEntity(MockCreature.Object),
                Times.Once);
        }
    }
}
