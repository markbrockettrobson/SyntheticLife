using SyntheticLife.Core.Energy;
using SyntheticLife.Core.Map;

namespace SyntheticLife.Core.LifeForm
{
    public class MoveToClosestEnergySourceBehaviourEngine : IBehaviourEngine
    {
        private Random RandomGenirator { get; }
        private double SearchAreaSize { get; set; }

        public MoveToClosestEnergySourceBehaviourEngine(Random random, double searchAreaSize)
        {
            RandomGenirator = random;
            SearchAreaSize = searchAreaSize;
        }

        public IMovementOrder SelectMovement(ICreature creature, IEntityMap entityMap)
        {
            var searchArea = creature.Location.Copy();
            searchArea.ExpandBy(SearchAreaSize);

            var closestEntity = entityMap
                .Query(searchArea)
                .Where(entity => typeof(IEnergySource).IsInstanceOfType(entity))
                .MinBy(entity => entity.Location.Centre.Distance(creature.Location.Centre));

            if (closestEntity == null)
            {
                var randomLocation = creature.Location.TranslateRandomDirection(RandomGenirator, creature.Species.MovementSpeed);
                return new MovementOrder(
                    creature.Location,
                    randomLocation.TranslateToInsideEnvelope(entityMap.Bounds));
            }

            return new MovementOrder(
                creature.Location,
                creature.Location.TranslateTo(closestEntity.Location, creature.Species.MovementSpeed));
        }
    }
}
