using NetTopologySuite.Geometries;

namespace SyntheticLife.Core.Map
{
    public class MovementOrder : IMovementOrder
    {
        public Envelope OldLocation { get; }
        public Envelope NewLocation { get; }

        public MovementOrder(Envelope oldLocation, Envelope newLocation)
        {
            OldLocation = oldLocation;
            NewLocation = newLocation;
        }
    }
}
