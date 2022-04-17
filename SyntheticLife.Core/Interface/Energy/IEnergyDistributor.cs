using SyntheticLife.Core.Map;

namespace SyntheticLife.Core.Energy
{
    public interface IEnergyDistributor
    {
        public IEnumerable<IEnergySource> CreateEnergySources(IEntityMap map, double energyAmount);
    }
}
