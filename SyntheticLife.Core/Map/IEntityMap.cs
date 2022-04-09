using NetTopologySuite.Geometries;

namespace SyntheticLife.Core.Map
{
    public interface IEntityMap
    {
        public IEnumerable<IMapEntity> MapEntities { get; }

        public void AddEntity(IMapEntity entity);
        public void RemoveEntity(IMapEntity entity);
        public IList<IMapEntity> Query(Envelope envelope);
    }
}
