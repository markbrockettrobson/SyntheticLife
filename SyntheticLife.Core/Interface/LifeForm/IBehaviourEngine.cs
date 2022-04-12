using SyntheticLife.Core.Map;

namespace SyntheticLife.Core.LifeForm
{
    public interface IBehaviourEngine
    {
        public IMovementOrder SelectMovement(ICreature creature, IEntityMap entityMap);
    }
}
