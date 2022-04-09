using NetTopologySuite.Geometries;
using SyntheticLife.Core.Map;

namespace SyntheticLife.Core.LifeForm
{
    public class Species : MapEntityBase, ISpecies
    {
        private static readonly double MovementEnergyCostMultiplier = 0.1;
        public double MovementSpeed { get; private set; }
        public Species(Envelope location, double movementSpeed) : base(location)
        {
            MovementSpeed = movementSpeed;
        }

        public double MovementEnergyCoast(double distance)
        {
            if (distance < 0)
            {
                throw new ArgumentException("Distance can not be negative.");
            }

            return MovementEnergyCostMultiplier * distance * distance;
        }
    }
}
