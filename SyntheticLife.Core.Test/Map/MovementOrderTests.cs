using System;
using Moq;
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
        public Envelope LocationThree = new ();

        [SetUp]
        public void SetUp()
        {
            LocationOne = new Envelope(1, 1, 1, 1);
            LocationTwo = new Envelope(2, 2, 2, 2);
            LocationThree = new Envelope(10, 10, 0, 1);
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

        public void Distance()
        {
            // Arrange
            var mapEntityBase = new MovementOrder(LocationOne, LocationTwo);
            // Act
            // Assert
            Assert.That(mapEntityBase.Distance, Is.InRange(0.98, 1.02));
        }

        public void DistanceOverlap()
        {
            // Arrange
            var mapEntityBase = new MovementOrder(LocationOne, LocationThree);
            // Act
            // Assert
            Assert.That(mapEntityBase.Distance, Is.EqualTo(0));
        }
    }
}
