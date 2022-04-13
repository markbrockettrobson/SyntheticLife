using Ardalis.GuardClauses;
using SyntheticLife.Core.Map;

namespace SyntheticLife.Core.Energy
{
    public class EnergyConsumptionOrder : IEnergyConsumptionOrder
    {
        public IMapEntity ConsumingEntity { get; }
        public IMapEntity ConsumedEntity { get; }
        public double Energy { get; }

        public EnergyConsumptionOrder(
            IMapEntity consumingEntity,
            IMapEntity consumedEntity,
            double energy)
        {
            Guard.Against.Negative(energy);

            ConsumingEntity = consumingEntity;
            ConsumedEntity = consumedEntity;
            Energy = energy;
        }
    }
}
