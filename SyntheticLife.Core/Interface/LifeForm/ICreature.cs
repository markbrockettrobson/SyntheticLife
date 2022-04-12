using SyntheticLife.Core.Map;

namespace SyntheticLife.Core.LifeForm
{
    public interface ICreature : IMapEntity
    {
        public double Energy { get; set; }
        public ISpecies Species { get; }
    }
}
