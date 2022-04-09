using NetTopologySuite.Geometries;

namespace SyntheticLife.Core.Map
{
    public interface IMapEntity
    {
        public Envelope Location { get; set; }
        public void AddOnUpdateLocationAction(Action<IMapEntity, Envelope> updateAction);
    }
}
