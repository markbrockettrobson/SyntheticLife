using SyntheticLife.Core.Map;

namespace SyntheticLife.Core.Energy
{
    public interface IEnergySource : IMapEntity
    {
        public double Energy { get; set; }
    }
}
