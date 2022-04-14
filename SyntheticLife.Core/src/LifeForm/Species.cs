using Ardalis.GuardClauses;
using NetTopologySuite.Geometries;
using SyntheticLife.Core.Map;

namespace SyntheticLife.Core.LifeForm
{
    public class Species : MapEntityBase, ISpecies
    {
        public double MovementSpeed { get; }
        public double HarvestRate { get; }
        public double MinimumOffspringCost { get; }

        public Species(Envelope location, double movementSpeed, double harvestRate, double minimumOffspringCost) : base(location)
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
