using NetTopologySuite.Geometries;
using NetTopologySuite.Index.Quadtree;

namespace SyntheticLife.Core.Map
{
    public class EntityMap : IEntityMap
    {
        public IEnumerable<IMapEntity> MapEntities => Tree.QueryAll();
        private Quadtree<IMapEntity> Tree { get; set; }

        public EntityMap()
        {
            Tree = new Quadtree<IMapEntity>();
        }

        public void AddEntity(IMapEntity entity)
        {
            AddEntity(entity, entity.Location);
            entity.AddOnUpdateLocationAction(OnEntityLocationUpdate);
        }

        private void AddEntity(IMapEntity entity, Envelope location)
        {
            if (MapEntities.Contains(entity))
            {
                throw new InvalidOperationException(string.Format("Entiry {0} already in Map.", entity));
            }

            Tree.Insert(location, entity);
        }

        public void RemoveEntity(IMapEntity entity)
        {
            if (!Tree.Remove(entity.Location, entity))
            {
                throw new InvalidOperationException(string.Format("Entiry {0} was not located in Map.", entity));
            }
        }

        public IList<IMapEntity> Query(Envelope envelope)
        {
            return Tree.Query(envelope);
        }

        private void OnEntityLocationUpdate(IMapEntity entity, Envelope newLocation)
        {
            RemoveEntity(entity);
            AddEntity(entity, newLocation);
        }
    }
}
