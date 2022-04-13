using Ardalis.GuardClauses;
using NetTopologySuite.Geometries;
using SyntheticLife.Core.Map;

namespace SyntheticLife.Core.LifeForm
{
    public class Species : MapEntityBase, ISpecies
    {
        public double MovementSpeed { get; private set; }
        public double HarvestRate { get; }

        public Species(Envelope location, double movementSpeed, double harvestRate) : base(location)
        {
            Guard.Against.Negative(movementSpeed);
            Guard.Against.Negative(harvestRate);

            MovementSpeed = movementSpeed;
            HarvestRate = harvestRate;
        }
    }
}
