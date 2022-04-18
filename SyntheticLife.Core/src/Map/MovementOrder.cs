using NetTopologySuite.Geometries;
using SyntheticLife.Core.LifeForm;

namespace SyntheticLife.Core.Map
{
    public class MovementOrder : IMovementOrder
    {
        public ICreature Creature { get; }
        public Envelope NewLocation { get; }
        public double EnergyCost { get; }

        public MovementOrder(ICreature creature, Envelope newLocation)
        {
            Creature = creature;
            NewLocation = newLocation;
            EnergyCost = creature.Location.Centre.Distance(newLocation.Centre);
        }

        public void ExecuteOrder(IEntityMap map)
        {
            Creature.Location = NewLocation;
            Creature.Energy -= EnergyCost;

            if (Creature.Energy <= 0)
            {
                map.RemoveEntity(Creature);
            }
        }
    }
}
