using SyntheticLife.Core.LifeForm;
using SyntheticLife.Core.Map;

namespace SyntheticLife.Core.Energy
{
    public interface IEnergyConsumptionOrder
    {
        public ICreature ConsumingEntity { get; }
        public IEnergySource ConsumedEntity { get; }
        public double Energy { get; }

        public void ExecuteOrder(IEntityMap map);
    }
}
