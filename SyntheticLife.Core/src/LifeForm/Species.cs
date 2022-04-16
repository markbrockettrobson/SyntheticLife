using Ardalis.GuardClauses;
using NetTopologySuite.Geometries;

namespace SyntheticLife.Core.LifeForm
{
    public class Species : ISpecies
    {
        public double MovementSpeed { get; }
        public double HarvestRate { get; }
        public double MinimumOffspringCost { get; }

        public Species(double movementSpeed, double harvestRate, double minimumOffspringCost)
        {
            Guard.Against.Negative(movementSpeed);
            Guard.Against.Negative(harvestRate);
            Guard.Against.Negative(minimumOffspringCost);

            MovementSpeed = movementSpeed;
            HarvestRate = harvestRate;
            MinimumOffspringCost = minimumOffspringCost;
        }
    }
}
