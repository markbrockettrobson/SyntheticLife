using System;
using Moq;
using NUnit.Framework;
using SyntheticLife.Core.LifeForm;
using SyntheticLife.Core.Map;

namespace SyntheticLife.Core.Energy.Test
{
    [TestFixture]
    public class EnergyConsumptionOrderTests
    {
        public Mock<ICreature> MockConsumingEntity = new ();
        public Mock<IEnergySource> MockConsumedEntity = new ();
        public Mock<IEntityMap> MockMap = new ();

        [SetUp]
        public void SetUp()
        {
            MockConsumingEntity = new ();
            MockConsumingEntity.SetupProperty(entity => entity.Energy, 100);
            MockConsumedEntity = new ();
            MockConsumedEntity.SetupProperty(entity => entity.Energy, 100);
            MockMap = new ();
        }

        [Test]
        public void ConsumingEntity()
        {
            // Arrange
            var energyConsumptionOrder = new EnergyConsumptionOrder(
                MockConsumingEntity.Object,
                MockConsumedEntity.Object,
                100);
            // Act
            var entity = energyConsumptionOrder.ConsumedEntity;
            // Assert
            Assert.That(entity, Is.EqualTo(MockConsumedEntity.Object));
        }

        [Test]
        public void ConsumedEntity()
        {
            // Arrange
            var energyConsumptionOrder = new EnergyConsumptionOrder(
                MockConsumingEntity.Object,
                MockConsumedEntity.Object,
                100);
            // Act
            var entity = energyConsumptionOrder.ConsumedEntity;
            // Assert
            Assert.That(entity, Is.EqualTo(MockConsumedEntity.Object));
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2.322)]
        [TestCase(31)]
        public void Energy(double energy)
        {
            // Arrange
            var energyConsumptionOrder = new EnergyConsumptionOrder(
                MockConsumingEntity.Object,
                MockConsumedEntity.Object,
                energy);
            // Act
            // Assert
            Assert.That(energyConsumptionOrder.Energy, Is.EqualTo(energy));
        }

        [TestCase(-1)]
        [TestCase(-22)]
        [TestCase(-3.1)]
        public void EnergyNegative(double energy)
        {
            // Arrange
            // Act
            // Assert
            Assert.Throws<ArgumentException>(
                () => new EnergyConsumptionOrder(
                    MockConsumingEntity.Object,
                    MockConsumedEntity.Object,
                    energy));
        }

        [Test]
        public void ExecuteOrderInsufficientEnergy()
        {
            // Arrange
            var energyConsumptionOrder = new EnergyConsumptionOrder(
                MockConsumingEntity.Object,
                MockConsumedEntity.Object,
                10000);
            // Act
            // Assert
            Assert.Throws<InvalidOperationException>(
                () => energyConsumptionOrder.ExecuteOrder(MockMap.Object));
        }

        [Test]
        public void ExecuteOrderConsumingEntityEnergyUpdated()
        {
            // Arrange
            var energyConsumptionOrder = new EnergyConsumptionOrder(
                MockConsumingEntity.Object,
                MockConsumedEntity.Object,
                10);
            // Act
            energyConsumptionOrder.ExecuteOrder(MockMap.Object);
            // Assert
            MockConsumingEntity.VerifySet(entity => entity.Energy = 110);
        }

        [Test]
        public void ExecuteOrderConsumedEntityEnergyUpdated()
        {
            // Arrange
            var energyConsumptionOrder = new EnergyConsumptionOrder(
                MockConsumingEntity.Object,
                MockConsumedEntity.Object,
                10);
            // Act
            energyConsumptionOrder.ExecuteOrder(MockMap.Object);
            // Assert
            MockConsumedEntity.VerifySet(entity => entity.Energy = 90);
            MockMap.Verify(
                map => map.RemoveEntity(It.IsAny<IMapEntity>()), Times.Never);
        }

        [Test]
        public void ExecuteOrderConsumedEntityRemoved()
        {
            // Arrange
            var energyConsumptionOrder = new EnergyConsumptionOrder(
                MockConsumingEntity.Object,
                MockConsumedEntity.Object,
                100);
            // Act
            energyConsumptionOrder.ExecuteOrder(MockMap.Object);
            // Assert
            MockMap.Verify(
                map => map.RemoveEntity(MockConsumedEntity.Object), Times.Once);
        }
    }
}
