using Ardalis.GuardClauses;
using NetTopologySuite.Geometries;
using SyntheticLife.Core.Interface.LifeForm;

namespace SyntheticLife.Core.LifeForm
{
    public class PopulationGenerator : IPopulationGenerator
    {
        private Random RandomNumberGenerator { get; }
        private double MovementSpeed { get; }
        private double HarvestRate { get; }
        private double MinimumOffspringCost { get; }

        public PopulationGenerator(Random random, double movementSpeed, double harvestRate, double minimumOffspringCost)
        {
            Guard.Against.Negative(movementSpeed);
            Guard.Against.Negative(harvestRate);
            Guard.Against.Negative(minimumOffspringCost);

            RandomNumberGenerator = random;
            MovementSpeed = movementSpeed;
            HarvestRate = harvestRate;
            MinimumOffspringCost = minimumOffspringCost;
        }

        public IEnumerable<ICreature> CreateCreatures(int initialPopulationSize, Envelope mapBounds)
        {
            Guard.Against.Negative(initialPopulationSize);

            var species = new Species(MovementSpeed, HarvestRate, MinimumOffspringCost);
            var creatures = new List<ICreature>();

            for (int i = 0; i < initialPopulationSize; i++)
            {
                var location = mapBounds.RandomEnvelopeInside(RandomNumberGenerator, 1);
                var creature = new Creature(location, species, MinimumOffspringCost);
                creatures.Add(creature);
            }

            return creatures;
        }
    }
}
