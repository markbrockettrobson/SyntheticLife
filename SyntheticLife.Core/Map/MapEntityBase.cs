using NetTopologySuite.Geometries;

namespace SyntheticLife.Core.Map
{
    public class MapEntityBase : IMapEntity
    {
        private Envelope location = new Envelope();
        public Envelope Location
        {
            get => location;
            set
            {
                foreach (var updateAction in UpdateActions)
                {
                    updateAction(this, value);
                }

                location = value;
            }
        }

        private IList<Action<IMapEntity, Envelope>> UpdateActions { get; }

        public MapEntityBase(Envelope location)
        {
            UpdateActions = new List<Action<IMapEntity, Envelope>>();
            Location = location;
        }

        public void AddOnUpdateLocationAction(Action<IMapEntity, Envelope> updateAction)
        {
            UpdateActions.Add(updateAction);
        }
    }
}
