using System;
using Moq;
using NetTopologySuite.Geometries;
using NUnit.Framework;
using SyntheticLife.Core.Map;

namespace SyntheticLife.Core.Test
{
    [TestFixture]
    public class MapEntityBaseTests
    {
        public Envelope LocationOne = new ();
        public Envelope LocationTwo = new ();
        public Mock<Action<IMapEntity, Envelope>> MockOnUpdateAction = new ();
        public Envelope OldLocationAtLastUpdateAction = new ();

        [SetUp]
        public void SetUp()
        {
            LocationOne = new Envelope(1, 1, 1, 1);
            LocationTwo = new Envelope(2, 2, 2, 2);
            MockOnUpdateAction = new Mock<Action<IMapEntity, Envelope>>();
            MockOnUpdateAction
                .Setup(action => action.Invoke(It.IsAny<IMapEntity>(), It.IsAny<Envelope>()))
                .Callback<IMapEntity, Envelope>((mapEntity, newLocation) => OldLocationAtLastUpdateAction = mapEntity.Location);
        }

        [Test]
        public void Location()
        {
            // Arrange
            var mapEntityBase = new MapEntityBase(LocationOne);
            // Act
            var location = mapEntityBase.Location;
            // Assert
            Assert.That(location, Is.EqualTo(LocationOne));
        }

        [Test]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0017:Simplify object initialization", Justification = "Makes test case clear.")]
        public void LocationSet()
        {
            // Arrange
            var mapEntityBase = new MapEntityBase(LocationOne);
            // Act
            mapEntityBase.Location = LocationTwo;
            // Assert
            var location = mapEntityBase.Location;
            Assert.That(location, Is.EqualTo(LocationTwo));
        }

        [Test]
        public void AddOnUpdateLocationAction()
        {
            // Arrange
            var mapEntityBase = new MapEntityBase(LocationOne);
            // Act
            mapEntityBase.AddOnUpdateLocationAction(MockOnUpdateAction.Object);
            mapEntityBase.Location = LocationTwo;
            // Assert
            MockOnUpdateAction.Verify(
                action => action.Invoke(
                    It.Is<IMapEntity>(mapEntity => mapEntity == mapEntityBase),
                    It.Is<Envelope>(envelope => envelope == LocationTwo)),
                Times.Once);
            Assert.That(OldLocationAtLastUpdateAction, Is.EqualTo(LocationOne));
        }
    }
}
