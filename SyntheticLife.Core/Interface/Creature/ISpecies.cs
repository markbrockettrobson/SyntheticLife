namespace SyntheticLife.Core.LifeForm
{
    public interface ISpecies
    {
        public double MovementSpeed { get; }
        public double MovementEnergyCoast(double distance);
    }
}
