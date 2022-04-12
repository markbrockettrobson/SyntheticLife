using NetTopologySuite.Geometries;
using NUnit.Framework;
using SyntheticLife.Core.Map;

namespace SyntheticLife.Core.Test
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
            var mapEntityBase = new MovementOrder(LocationOne, LocationTwo);
            // Act
            var location = mapEntityBase.OldLocation;
            // Assert
            Assert.That(location, Is.EqualTo(LocationOne));
        }

        [Test]
        public void NewLocation()
        {
            // Arrange
            var mapEntityBase = new MovementOrder(LocationOne, LocationTwo);
            // Act
            var location = mapEntityBase.OldLocation;
            // Assert
            Assert.That(location, Is.EqualTo(LocationOne));
        }
    }
}
