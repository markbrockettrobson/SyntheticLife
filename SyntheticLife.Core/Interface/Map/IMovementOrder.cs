using NetTopologySuite.Geometries;

namespace SyntheticLife.Core.Map
{
    public interface IMovementOrder
    {
        public Envelope OldLocation { get; }
        public Envelope NewLocation { get; }
        public double EnergyCost { get; }
    }
}
