using System;
using System.Collections.Generic;
using Moq;
using NetTopologySuite.Geometries;
using NUnit.Framework;
using SyntheticLife.Core.Energy;
using SyntheticLife.Core.Map;

namespace SyntheticLife.Core.LifeForm.Test
{
    [TestFixture]
    public class MoveToClosestEnergySourceBehaviourEngineTests
    {
        public double Epsilon = 0.001;
        public Envelope LocationOne = new ();
        public Envelope LocationTwo = new ();
        public Envelope LocationThree = new ();

        public Mock<ICreature> MockCreature = new ();
        public Mock<ISpecies> MockSpecies = new ();
        public Mock<IEntityMap> MockMap = new ();
        public Mock<IEnergySource> MockEnergySourceOne = new ();
        public Mock<IEnergySource> MockEnergySourceTwo = new ();
        public Mock<IMapEntity> MockEntityOne = new ();
        public Mock<IMapEntity> MockEntityTwo = new ();
        public Mock<Random> MockRandom = new ();

        [SetUp]
        public void SetUp()
        {
            LocationOne = new Envelope(1, 1, 1, 1);
            LocationTwo = new Envelope(10, 10, 10, 10);
            LocationThree = new Envelope(20, 10, 20, 10);

            MockSpecies = new ();
            MockSpecies.Setup(species => species.MovementSpeed).Returns(1000);
            MockCreature = new ();
            MockCreature.Setup(creature => creature.Species).Returns(MockSpecies.Object);
            MockCreature.Setup(creature => creature.Location).Returns(LocationOne);

            MockEnergySourceOne = new ();
            MockEnergySourceOne.Setup(energySource => energySource.Location).Returns(LocationTwo);
            MockEnergySourceTwo = new ();
            MockEnergySourceTwo.Setup(energySource => energySource.Location).Returns(LocationThree);
            MockEntityOne = new ();
            MockEntityOne.Setup(entity => entity.Location).Returns(LocationTwo);
            MockEntityTwo = new ();
            MockEntityTwo.Setup(entity => entity.Location).Returns(LocationThree);

            MockMap = new ();
            MockMap
                .Setup(map => map.Query(It.IsAny<Envelope>()))
                .Returns(new List<IMapEntity>()
                {
                    MockEnergySourceTwo.Object,
                    MockEntityOne.Object,
                    MockEntityTwo.Object,
                    MockEnergySourceOne.Object
                });
            MockMap.Setup(map => map.Bounds).Returns(new Envelope(0, 30, 0, 30));

            MockRandom = new Mock<Random>();
            MockRandom.Setup(random => random.NextDouble()).Returns(0.5);
        }

        [Test]
        public void SelectMovementNothingInSearchArea()
        {
            // Arrange
            var behaviour = new MoveToClosestEnergySourceBehaviourEngine(MockRandom.Object, 0);
            MockMap.Setup(map => map.Bounds).Returns(new Envelope(-2000, 2000, -2000, 2000));

            MockMap
                .Setup(map => map.Query(It.IsAny<Envelope>()))
                .Returns(new List<IMapEntity>() { });

            // Act
            var movementOrder = behaviour.SelectMovement(
                MockCreature.Object,
                MockMap.Object);

            // Assert
            MockMap.Verify(map => map.Query(LocationOne), Times.Once);

            Assert.That(movementOrder.OldLocation, Is.EqualTo(LocationOne));
            Assert.That(movementOrder.NewLocation.MinX, Is.InRange(-999 - Epsilon, -999 + Epsilon));
            Assert.That(movementOrder.NewLocation.MaxX, Is.InRange(-999 - Epsilon, -999 + Epsilon));
            Assert.That(movementOrder.NewLocation.MinY, Is.InRange(1 - Epsilon, 1 + Epsilon));
            Assert.That(movementOrder.NewLocation.MaxY, Is.InRange(1 - Epsilon, 1 + Epsilon));
        }

        [Test]
        public void SelectMovementNoEnergySourceInSearchArea()
        {
            // Arrange
            var behaviour = new MoveToClosestEnergySourceBehaviourEngine(MockRandom.Object, 30);
            MockMap.Setup(map => map.Bounds).Returns(new Envelope(-2000, 2000, -2000, 2000));
            MockMap
                .Setup(map => map.Query(It.IsAny<Envelope>()))
                .Returns(new List<IMapEntity>() { MockEntityOne.Object, MockEntityTwo.Object });

            // Act
            var movementOrder = behaviour.SelectMovement(
                MockCreature.Object,
                MockMap.Object);

            var searchArea = LocationOne.Copy();
            searchArea.ExpandBy(30);

            // Assert
            MockMap.Verify(map => map.Query(searchArea), Times.Once);
            Assert.That(movementOrder.OldLocation, Is.EqualTo(LocationOne));
            Assert.That(movementOrder.NewLocation.MinX, Is.InRange(-999 - Epsilon, -999 + Epsilon));
            Assert.That(movementOrder.NewLocation.MaxX, Is.InRange(-999 - Epsilon, -999 + Epsilon));
            Assert.That(movementOrder.NewLocation.MinY, Is.InRange(1 - Epsilon, 1 + Epsilon));
            Assert.That(movementOrder.NewLocation.MaxY, Is.InRange(1 - Epsilon, 1 + Epsilon));
        }

        [Test]
        public void SelectMovementNothingInSearchAreaOutsidebounds()
        {
            // Arrange
            var behaviour = new MoveToClosestEnergySourceBehaviourEngine(MockRandom.Object, 0);

            MockMap
                .Setup(map => map.Query(It.IsAny<Envelope>()))
                .Returns(new List<IMapEntity>() { });

            // Act
            var movementOrder = behaviour.SelectMovement(
                MockCreature.Object,
                MockMap.Object);

            // Assert
            MockMap.Verify(map => map.Query(LocationOne), Times.Once);

            Assert.That(movementOrder.OldLocation, Is.EqualTo(LocationOne));
            Assert.That(movementOrder.NewLocation.MinX, Is.InRange(0 - Epsilon, 0 + Epsilon));
            Assert.That(movementOrder.NewLocation.MaxX, Is.InRange(0 - Epsilon, 0 + Epsilon));
            Assert.That(movementOrder.NewLocation.MinY, Is.InRange(1 - Epsilon, 1 + Epsilon));
            Assert.That(movementOrder.NewLocation.MaxY, Is.InRange(1 - Epsilon, 1 + Epsilon));
        }

        [Test]
        public void SelectMovementNoEnergySourceInSearchAreaOutsidebounds()
        {
            // Arrange
            var behaviour = new MoveToClosestEnergySourceBehaviourEngine(MockRandom.Object, 30);
            MockMap
                .Setup(map => map.Query(It.IsAny<Envelope>()))
                .Returns(new List<IMapEntity>() { MockEntityOne.Object, MockEntityTwo.Object });

            // Act
            var movementOrder = behaviour.SelectMovement(
                MockCreature.Object,
                MockMap.Object);

            var searchArea = LocationOne.Copy();
            searchArea.ExpandBy(30);

            // Assert
            MockMap.Verify(map => map.Query(searchArea), Times.Once);
            Assert.That(movementOrder.OldLocation, Is.EqualTo(LocationOne));
            Assert.That(movementOrder.NewLocation.MinX, Is.InRange(0 - Epsilon, 1 + Epsilon));
            Assert.That(movementOrder.NewLocation.MaxX, Is.InRange(0 - Epsilon, 19 + Epsilon));
            Assert.That(movementOrder.NewLocation.MinY, Is.InRange(1 - Epsilon, 1 + Epsilon));
            Assert.That(movementOrder.NewLocation.MaxY, Is.InRange(1 - Epsilon, 1 + Epsilon));
        }

        [Test]
        public void SelectMovementOneEnergySourceInSearchArea()
        {
            // Arrange
            var behaviour = new MoveToClosestEnergySourceBehaviourEngine(MockRandom.Object, 30);
            MockMap
                .Setup(map => map.Query(It.IsAny<Envelope>()))
                .Returns(new List<IMapEntity>() { MockEntityOne.Object, MockEnergySourceTwo.Object, MockEntityTwo.Object });

            // Act
            var movementOrder = behaviour.SelectMovement(
                MockCreature.Object,
                MockMap.Object);

            // Assert
            Assert.That(movementOrder.OldLocation, Is.EqualTo(LocationOne));
            Assert.That(movementOrder.NewLocation, Is.EqualTo(LocationThree));
        }

        [Test]
        public void SelectMovementInRange()
        {
            // Arrange
            var behaviour = new MoveToClosestEnergySourceBehaviourEngine(MockRandom.Object, 30);

            // Act
            var movementOrder = behaviour.SelectMovement(
                MockCreature.Object,
                MockMap.Object);

            // Assert
            Assert.That(movementOrder.OldLocation, Is.EqualTo(LocationOne));
            Assert.That(movementOrder.NewLocation, Is.EqualTo(LocationTwo));
        }

        [Test]
        public void SelectMovementOutOfRange()
        {
            // Arrange
            var behaviour = new MoveToClosestEnergySourceBehaviourEngine(MockRandom.Object, 30);
            MockSpecies.Setup(species => species.MovementSpeed).Returns(2);

            // Act
            var movementOrder = behaviour.SelectMovement(
                MockCreature.Object,
                MockMap.Object);

            // Assert
            Assert.That(movementOrder.OldLocation, Is.EqualTo(LocationOne));
            Assert.That(movementOrder.NewLocation.MinX, Is.InRange(2.414 - Epsilon, 2.414 + Epsilon));
            Assert.That(movementOrder.NewLocation.MaxX, Is.InRange(2.414 - Epsilon, 2.414 + Epsilon));
            Assert.That(movementOrder.NewLocation.MinY, Is.InRange(2.414 - Epsilon, 2.414 + Epsilon));
            Assert.That(movementOrder.NewLocation.MaxY, Is.InRange(2.414 - Epsilon, 2.414 + Epsilon));
        }
    }
}
