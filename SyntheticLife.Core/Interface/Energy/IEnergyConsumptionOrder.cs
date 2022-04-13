using SyntheticLife.Core.Map;

namespace SyntheticLife.Core.Energy
{
    public interface IEnergyConsumptionOrder
    {
        public IMapEntity ConsumingEntity { get; }
        public IMapEntity ConsumedEntity { get; }
        public double Energy { get; }
    }
}
