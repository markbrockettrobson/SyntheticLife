using System;
using SyntheticLife.Core.Energy;
using SyntheticLife.Core.Map;

namespace SyntheticLife.Core.LifeForm
{
    public class ConsumeLargestConsumptionEngine : IConsumptionEngine
    {
        public IEnumerable<IEnergyConsumptionOrder> ProssesCurrectPosition(ICreature creature, IEntityMap entityMap)
        {
            var largestOverlapingEnergySource = entityMap
                .Query(creature.Location)
                .Where(entity => typeof(IEnergySource).IsInstanceOfType(entity))
                .Cast<IEnergySource>()
                .MaxBy(entity => entity.Energy);

            if (largestOverlapingEnergySource == null)
            {
                return new List<IEnergyConsumptionOrder>();
            }

            var amount = Math.Min(
                creature.Species.HarvestRate,
                largestOverlapingEnergySource.Energy);

            return new List<IEnergyConsumptionOrder>()
            {
                new EnergyConsumptionOrder(creature, largestOverlapingEnergySource, amount)
            };
        }
    }
}
