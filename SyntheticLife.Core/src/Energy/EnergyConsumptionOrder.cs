using Ardalis.GuardClauses;
using SyntheticLife.Core.LifeForm;

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
    }
}
