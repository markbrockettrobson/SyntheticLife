namespace SyntheticLife.Core.LifeForm
{
    public interface ISpecies
    {
        public double MovementSpeed { get; }
        public double HarvestRate { get; }
        public double MinimumOffspringCost { get; }
    }
}
