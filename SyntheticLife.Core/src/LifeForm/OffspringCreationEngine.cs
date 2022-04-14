using SyntheticLife.Core.Map;

namespace SyntheticLife.Core.LifeForm
{
    public class OffspringCreationEngine : IOffspringCreationEngine
    {
        private static readonly double SafeEnergyMultiplier = 2;

        public IEnumerable<IOffspringCreationOrder> ProssesCurrectState(ICreature creature, IEntityMap entityMap)
        {
            if (creature.Energy >= SafeEnergyMultiplier * creature.Species.MinimumOffspringCost)
            {
                return new List<IOffspringCreationOrder>()
                {
                    new OffspringCreationOrder(creature, creature.Species.MinimumOffspringCost),
                };
            }

            return new List<IOffspringCreationOrder>();
        }
    }
}
