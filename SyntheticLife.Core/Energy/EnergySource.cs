using NetTopologySuite.Geometries;
using SyntheticLife.Core.Map;

namespace SyntheticLife.Core.Energy
{
    public class EnergySource : MapEntityBase, IEnergySource
    {
        public int Energy { get; set; }
        public EnergySource(Envelope location, int energy) : base(location)
        {
            Energy = energy;
        }
    }
}
