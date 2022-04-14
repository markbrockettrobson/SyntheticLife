using Ardalis.GuardClauses;

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
    }
}
