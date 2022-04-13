using NetTopologySuite.Geometries;

namespace SyntheticLife.Core.Map
{
    public class MovementOrder : IMovementOrder
    {
        public Envelope OldLocation { get; }
        public Envelope NewLocation { get; }
        public double EnergyCost { get; }

        public MovementOrder(Envelope oldLocation, Envelope newLocation)
        {
            OldLocation = oldLocation;
            NewLocation = newLocation;
            EnergyCost = oldLocation.Centre.Distance(newLocation.Centre);
        }
    }
}
