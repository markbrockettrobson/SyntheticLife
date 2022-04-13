using System;
using Moq;
using NUnit.Framework;
using SyntheticLife.Core.Energy;
using SyntheticLife.Core.Map;

namespace SyntheticLife.Core.Test
{
    [TestFixture]
    public class EnergyConsumptionOrderTests
    {
        public Mock<IMapEntity> MockConsumingEntity = new ();
        public Mock<IMapEntity> MockConsumedEntity = new ();

        [SetUp]
        public void SetUp()
        {
            MockConsumingEntity = new ();
            MockConsumedEntity = new ();
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
    }
}
