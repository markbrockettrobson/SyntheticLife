using SyntheticLife.Core.LifeForm;

namespace SyntheticLife.Core.Energy
{
    public interface IEnergyConsumptionOrder
    {
        public ICreature ConsumingEntity { get; }
        public IEnergySource ConsumedEntity { get; }
        public double Energy { get; }
    }
}
