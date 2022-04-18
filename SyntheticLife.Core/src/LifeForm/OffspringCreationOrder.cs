using Ardalis.GuardClauses;
using SyntheticLife.Core.Map;

namespace SyntheticLife.Core.LifeForm
{
    public class OffspringCreationOrder : IOffspringCreationOrder
    {
        public ICreature Parent { get; }
        public double OffspringStartingEnergy { get; }

        public OffspringCreationOrder(ICreature parent, double offspringStartingEnergy)
        {
            Guard.Against.Negative(offspringStartingEnergy);

            Parent = parent;
            OffspringStartingEnergy = offspringStartingEnergy;
        }

        public void ExecuteOrder(IEntityMap map)
        {
            if (Parent.Energy <= OffspringStartingEnergy)
            {
                throw new InvalidOperationException("Parent has insufficient energy.");
            }

            var offspring = new Creature(Parent.Location, Parent.Species, OffspringStartingEnergy);
            Parent.Energy -= OffspringStartingEnergy;
            map.AddEntity(offspring);
        }
    }
}
