using NetTopologySuite.Geometries;
using SyntheticLife.Core.LifeForm;

namespace SyntheticLife.Core.Interface.LifeForm
{
    internal interface IPopulationGenerator
    {
        public IEnumerable<ICreature> CreateCreatures(int initialPopulationSize, Envelope mapBounds);
    }
}
