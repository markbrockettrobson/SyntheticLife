using Moq;
using NetTopologySuite.Geometries;
using NUnit.Framework;

namespace SyntheticLife.Core.LifeForm.Test
{
    [TestFixture]
    public class CreatureTests
    {
        public Envelope Location = new ();
        public Mock<ISpecies> MockSpecies = new ();

        [SetUp]
        public void SetUp()
        {
            Location = new Envelope(1, 1, 1, 1);
            MockSpecies = new ();
        }

        [Test]
        public void EnergySetOnConstruction()
        {
            // Arrange
            var creature = new Creature(Location, MockSpecies.Object, 100.2);
            // Act
            var energy = creature.Energy;
            // Assert
            Assert.That(energy, Is.EqualTo(100.2));
        }

        [Test]
        public void EnergySet()
        {
            // Arrange
            var creature = new Creature(Location, MockSpecies.Object, 1310.4);
            // Act
            creature.Energy -= 100;
            // Assert
            Assert.That(creature.Energy, Is.EqualTo(1210.4));
        }

        [Test]
        public void Species()
        {
            // Arrange
            var creature = new Creature(Location, MockSpecies.Object, 1310.4);
            // Act
            var species = creature.Species;
            // Assert
            Assert.That(species, Is.EqualTo(MockSpecies.Object));
        }
    }
}
