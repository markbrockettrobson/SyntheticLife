using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NetTopologySuite.Geometries;
using NUnit.Framework;
using SyntheticLife.Core.Energy;
using SyntheticLife.Core.Map;

namespace SyntheticLife.Core.LifeForm.Test
{
    [TestFixture]
    public class ConsumeLargestConsumptionEngineTests
    {
        public Envelope Location = new ();

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
            Location = new Envelope(1, 1, 1, 1);

            MockSpecies = new ();
            MockSpecies.Setup(species => species.HarvestRate).Returns(100);
            MockCreature = new ();
            MockCreature.Setup(creature => creature.Species).Returns(MockSpecies.Object);
            MockCreature.Setup(creature => creature.Location).Returns(Location);

            MockEnergySourceOne = new ();
            MockEnergySourceOne.Setup(energySource => energySource.Energy).Returns(100);
            MockEnergySourceTwo = new ();
            MockEnergySourceTwo.Setup(energySource => energySource.Energy).Returns(30);
            MockEntityOne = new ();
            MockEntityTwo = new ();

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

            MockRandom = new Mock<Random>();
            MockRandom.Setup(random => random.NextDouble()).Returns(0.5);
        }

        [Test]
        public void NoEntity()
        {
            // Arrange
            var consumptionEngine = new ConsumeLargestConsumptionEngine();

            MockMap
                .Setup(map => map.Query(It.IsAny<Envelope>()))
                .Returns(new List<IMapEntity>() { });

            // Act
            var energyConsumptionOrders = consumptionEngine.ProssesCurrectPosition(
                MockCreature.Object,
                MockMap.Object);

            // Assert
            MockMap.Verify(map => map.Query(Location), Times.Once);
            Assert.That(energyConsumptionOrders, Has.Count.EqualTo(0));
        }

        [Test]
        public void NoEnergySourceEntity()
        {
            // Arrange
            var consumptionEngine = new ConsumeLargestConsumptionEngine();

            MockMap
                .Setup(map => map.Query(It.IsAny<Envelope>()))
                .Returns(new List<IMapEntity>()
                {
                    MockEntityOne.Object,
                    MockEntityTwo.Object,
                });

            // Act
            var energyConsumptionOrders = consumptionEngine.ProssesCurrectPosition(
                MockCreature.Object,
                MockMap.Object);

            // Assert
            MockMap.Verify(map => map.Query(Location), Times.Once);
            Assert.That(energyConsumptionOrders, Has.Count.EqualTo(0));
        }

        [Test]
        public void SingleEnergySourceEntity()
        {
            // Arrange
            var consumptionEngine = new ConsumeLargestConsumptionEngine();

            MockMap
                .Setup(map => map.Query(It.IsAny<Envelope>()))
                .Returns(new List<IMapEntity>()
                {
                    MockEntityOne.Object,
                    MockEntityTwo.Object,
                    MockEnergySourceOne.Object,
                });

            // Act
            var energyConsumptionOrders = consumptionEngine.ProssesCurrectPosition(
                MockCreature.Object,
                MockMap.Object);

            var energyConsumptionOrder = energyConsumptionOrders.First();
            // Assert
            MockMap.Verify(map => map.Query(Location), Times.Once);
            Assert.That(energyConsumptionOrders, Has.Count.EqualTo(1));
            Assert.That(energyConsumptionOrder.ConsumingEntity, Is.EqualTo(MockCreature.Object));
            Assert.That(energyConsumptionOrder.ConsumedEntity, Is.EqualTo(MockEnergySourceOne.Object));
            Assert.That(energyConsumptionOrder.Energy, Is.EqualTo(100));
        }

        [Test]
        public void SingleLargeEnergySourceEntity()
        {
            // Arrange
            var consumptionEngine = new ConsumeLargestConsumptionEngine();
            MockEnergySourceOne.Setup(energySource => energySource.Energy).Returns(1000);

            MockMap
                .Setup(map => map.Query(It.IsAny<Envelope>()))
                .Returns(new List<IMapEntity>()
                {
                    MockEntityOne.Object,
                    MockEntityTwo.Object,
                    MockEnergySourceOne.Object,
                });

            // Act
            var energyConsumptionOrders = consumptionEngine.ProssesCurrectPosition(
                MockCreature.Object,
                MockMap.Object);

            var energyConsumptionOrder = energyConsumptionOrders.First();
            // Assert
            MockMap.Verify(map => map.Query(Location), Times.Once);
            Assert.That(energyConsumptionOrders, Has.Count.EqualTo(1));
            Assert.That(energyConsumptionOrder.ConsumingEntity, Is.EqualTo(MockCreature.Object));
            Assert.That(energyConsumptionOrder.ConsumedEntity, Is.EqualTo(MockEnergySourceOne.Object));
            Assert.That(energyConsumptionOrder.Energy, Is.EqualTo(100));
        }

        [Test]
        public void SingleSmallEnergySourceEntity()
        {
            // Arrange
            var consumptionEngine = new ConsumeLargestConsumptionEngine();

            MockMap
                .Setup(map => map.Query(It.IsAny<Envelope>()))
                .Returns(new List<IMapEntity>()
                {
                    MockEntityOne.Object,
                    MockEntityTwo.Object,
                    MockEnergySourceTwo.Object,
                });

            // Act
            var energyConsumptionOrders = consumptionEngine.ProssesCurrectPosition(
                MockCreature.Object,
                MockMap.Object);

            var energyConsumptionOrder = energyConsumptionOrders.First();
            // Assert
            MockMap.Verify(map => map.Query(Location), Times.Once);
            Assert.That(energyConsumptionOrders, Has.Count.EqualTo(1));
            Assert.That(energyConsumptionOrder.ConsumingEntity, Is.EqualTo(MockCreature.Object));
            Assert.That(energyConsumptionOrder.ConsumedEntity, Is.EqualTo(MockEnergySourceTwo.Object));
            Assert.That(energyConsumptionOrder.Energy, Is.EqualTo(30));
        }

        [Test]
        public void MoreThenOneEnergySource()
        {
            // Arrange
            var consumptionEngine = new ConsumeLargestConsumptionEngine();
            MockSpecies.Setup(species => species.HarvestRate).Returns(50);

            MockMap
                .Setup(map => map.Query(It.IsAny<Envelope>()))
                .Returns(new List<IMapEntity>()
                {
                    MockEntityOne.Object,
                    MockEntityTwo.Object,
                    MockEnergySourceOne.Object,
                    MockEnergySourceTwo.Object,
                });

            // Act
            var energyConsumptionOrders = consumptionEngine.ProssesCurrectPosition(
                MockCreature.Object,
                MockMap.Object);

            var energyConsumptionOrder = energyConsumptionOrders.First();
            // Assert
            MockMap.Verify(map => map.Query(Location), Times.Once);
            Assert.That(energyConsumptionOrders, Has.Count.EqualTo(1));
            Assert.That(energyConsumptionOrder.ConsumingEntity, Is.EqualTo(MockCreature.Object));
            Assert.That(energyConsumptionOrder.ConsumedEntity, Is.EqualTo(MockEnergySourceOne.Object));
            Assert.That(energyConsumptionOrder.Energy, Is.EqualTo(50));
        }
    }
}
