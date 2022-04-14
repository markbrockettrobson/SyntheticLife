using SyntheticLife.Core.Energy;
using SyntheticLife.Core.Map;

namespace SyntheticLife.Core.LifeForm
{
    public interface IConsumptionEngine
    {
        public IEnumerable<IEnergyConsumptionOrder> ProssesCurrectPosition(ICreature creature, IEntityMap entityMap);
    }
}
