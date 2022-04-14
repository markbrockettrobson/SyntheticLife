using SyntheticLife.Core.Map;

namespace SyntheticLife.Core.LifeForm
{
    public interface IOffspringCreationEngine
    {
        public IEnumerable<IOffspringCreationOrder> ProssesCurrectState(ICreature creature, IEntityMap entityMap);
    }
}
