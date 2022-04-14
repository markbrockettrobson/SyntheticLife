namespace SyntheticLife.Core.LifeForm
{
    public interface IOffspringCreationOrder
    {
        public ICreature Parent { get; }
        public double OffspringStartingEnergy { get; }
    }
}
