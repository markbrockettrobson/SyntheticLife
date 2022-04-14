using NetTopologySuite.Geometries;
using NUnit.Framework;

namespace SyntheticLife.Core.Map.Test
{
    [TestFixture]
    public class MovementOrderTests
    {
        public Envelope LocationOne = new ();
        public Envelope LocationTwo = new ();

        [SetUp]
        public void SetUp()
        {
            LocationOne = new Envelope(1, 1, 1, 1);
            LocationTwo = new Envelope(2, 2, 2, 2);
        }

        [Test]
        public void OldLocation()
        {
            // Arrange
            var movementOrder = new MovementOrder(LocationOne, LocationTwo);
            // Act
            var location = movementOrder.OldLocation;
            // Assert
            Assert.That(location, Is.EqualTo(LocationOne));
        }

        [Test]
        public void NewLocation()
        {
            // Arrange
            var movementOrder = new MovementOrder(LocationOne, LocationTwo);
            // Act
            var location = movementOrder.OldLocation;
            // Assert
            Assert.That(location, Is.EqualTo(LocationOne));
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

            var movementOrder = new MovementOrder(locationOne, locationTwo);
            // Act
            var energyCost = movementOrder.EnergyCost;
            // Assert
            Assert.That(energyCost, Is.EqualTo(expectedCost));
        }
    }
}
