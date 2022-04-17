using NetTopologySuite.Geometries;

namespace SyntheticLife.Core.LifeForm
{
    public interface IPopulationGenerator
    {
        public IEnumerable<ICreature> CreateCreatures(int initialPopulationSize, Envelope mapBounds);
    }
}
