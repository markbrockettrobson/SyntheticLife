using NetTopologySuite.Geometries;
using SyntheticLife.Core.Map;

namespace SyntheticLife.Core.Energy
{
    public class EnergySource : MapEntityBase, IEnergySource
    {
        public double Energy { get; set; }
        public EnergySource(Envelope location, double energy) : base(location)
        {
            Energy = energy;
        }
    }
}
