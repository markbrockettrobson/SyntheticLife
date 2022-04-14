using System.Linq;
using Moq;
using NUnit.Framework;
using SyntheticLife.Core.Map;

namespace SyntheticLife.Core.LifeForm.Test
{
    [TestFixture]
    public class OffspringCreationEngineTests
    {
        public Mock<ICreature> MockCreature = new ();
        public Mock<ISpecies> MockSpecies = new ();
        public Mock<IEntityMap> MockMap = new ();

        [SetUp]
        public void SetUp()
        {
            MockSpecies = new ();
            MockSpecies.Setup(species => species.MinimumOffspringCost).Returns(300);
            MockCreature = new ();
            MockCreature.Setup(creature => creature.Species).Returns(MockSpecies.Object);

            MockMap = new ();
        }

        [Test]
        public void UnderMinimumEnergy()
        {
            // Arrange
            var offspringCreationEngine = new OffspringCreationEngine();
            MockCreature.Setup(creature => creature.Energy).Returns(300);

            // Act
            var offspringCreationOrders = offspringCreationEngine.ProssesCurrectState(
                MockCreature.Object,
                MockMap.Object);

            // Assert
            Assert.That(offspringCreationOrders, Has.Count.EqualTo(0));
        }

        [Test]
        public void MinimumEnergy()
        {
            // Arrange
            var offspringCreationEngine = new OffspringCreationEngine();
            MockCreature.Setup(creature => creature.Energy).Returns(600);

            // Act
            var offspringCreationOrders = offspringCreationEngine.ProssesCurrectState(
                MockCreature.Object,
                MockMap.Object);
            // Assert
            Assert.That(offspringCreationOrders, Has.Count.EqualTo(1));

            var offspringCreationOrder = offspringCreationOrders.First();
            Assert.That(offspringCreationOrder.Parent, Is.EqualTo(MockCreature.Object));
            Assert.That(offspringCreationOrder.OffspringStartingEnergy, Is.EqualTo(300));
        }

        [Test]
        public void OverMinimumEnergy()
        {
            // Arrange
            var offspringCreationEngine = new OffspringCreationEngine();
            MockCreature.Setup(creature => creature.Energy).Returns(12000);

            // Act
            var offspringCreationOrders = offspringCreationEngine.ProssesCurrectState(
                MockCreature.Object,
                MockMap.Object);
            // Assert
            Assert.That(offspringCreationOrders, Has.Count.EqualTo(1));

            var offspringCreationOrder = offspringCreationOrders.First();
            Assert.That(offspringCreationOrder.Parent, Is.EqualTo(MockCreature.Object));
            Assert.That(offspringCreationOrder.OffspringStartingEnergy, Is.EqualTo(300));
        }
    }
}
