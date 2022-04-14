using System;
using Moq;
using NUnit.Framework;
using SyntheticLife.Core.Energy;
using SyntheticLife.Core.LifeForm;

namespace SyntheticLife.Core.Test
{
    [TestFixture]
    public class OffspringCreationOrderTests
    {
        public Mock<ICreature> MockParentEntity = new ();

        [SetUp]
        public void SetUp()
        {
            MockParentEntity = new ();
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
    }
}
