using NetTopologySuite.Geometries;
using SyntheticLife.Core.Map;

namespace SyntheticLife.Core.LifeForm
{
    public class Creature : MapEntityBase, ICreature
    {
        public double Energy { get; set; }
        public ISpecies Species { get; }

        public Creature(Envelope location, ISpecies species, double energy) : base(location)
        {
            Species = species;
            Energy = energy;
        }
    }
}
