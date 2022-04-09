using SyntheticLife.Core.Map;

namespace SyntheticLife.Core.Energy
{
    public interface IEnergySource : IMapEntity
    {
        public int Energy { get; set; }
    }
}
