using Ardalis.GuardClauses;
using SyntheticLife.Core.LifeForm;
using SyntheticLife.Core.Map;

namespace SyntheticLife.Core.Energy
{
    public class EnergyConsumptionOrder : IEnergyConsumptionOrder
    {
        public ICreature ConsumingEntity { get; }
        public IEnergySource ConsumedEntity { get; }
        public double Energy { get; }

        public EnergyConsumptionOrder(
            ICreature consumingEntity,
            IEnergySource consumedEntity,
            double energy)
        {
            Guard.Against.Negative(energy);

            ConsumingEntity = consumingEntity;
            ConsumedEntity = consumedEntity;
            Energy = energy;
        }

        public void ExecuteOrder(IEntityMap map)
        {
            if (ConsumedEntity.Energy < Energy)
            {
                throw new InvalidOperationException("Consumed Entity has insufficient energy.");
            }

            ConsumingEntity.Energy += Energy;
            ConsumedEntity.Energy -= Energy;

            if (ConsumedEntity.Energy <= 0)
            {
                map.RemoveEntity(ConsumedEntity);
            }
        }
    }
}
