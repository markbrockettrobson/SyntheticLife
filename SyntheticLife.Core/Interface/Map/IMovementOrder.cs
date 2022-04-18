using NetTopologySuite.Geometries;
using SyntheticLife.Core.LifeForm;

namespace SyntheticLife.Core.Map
{
    public interface IMovementOrder
    {
        public ICreature Creature { get; }
        public Envelope NewLocation { get; }
        public double EnergyCost { get; }
        public void ExecuteOrder(IEntityMap map);
    }
}
