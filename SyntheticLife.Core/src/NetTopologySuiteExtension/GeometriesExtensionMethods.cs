using Ardalis.GuardClauses;
using NetTopologySuite.Geometries;

namespace SyntheticLife.Core
{
    public static class GeometriesExtensionMethods
    {
        public static Envelope TranslateAtAngle(this Envelope envelope, double radianAngle, double distance)
        {
            (double yTranslateRatio, double xTranslateRatio) = Math.SinCos(radianAngle);
            var envelopeCopy = envelope.Copy();
            envelopeCopy.Translate(xTranslateRatio * distance, yTranslateRatio * distance);
            return envelopeCopy;
        }

        public static Envelope TranslateRandomDirection(this Envelope envelope, Random random, double distance)
        {
            return envelope.TranslateAtAngle(2.0 * Math.PI * random.NextDouble(), distance);
        }

        public static Envelope TranslateTo(this Envelope envelope, Envelope target, double maxDistance)
        {
            Guard.Against.Negative(maxDistance);

            if (envelope.Centre.Distance(target.Centre) < maxDistance)
            {
                return target.Copy();
            }

            double angleToTarget = envelope.Centre.RadianAngleTo(target.Centre);
            return envelope.TranslateAtAngle(angleToTarget, maxDistance);
        }

        public static Envelope TranslateToInsideEnvelope(this Envelope envelope, Envelope bounds)
        {
            var locationInbounds = envelope.Copy();
            if (bounds.Contains(envelope))
            {
                return locationInbounds;
            }

            if (bounds.MinX > locationInbounds.MinX)
            {
                locationInbounds.Translate(bounds.MinX - envelope.MinX, 0);
            }

            if (bounds.MaxX < locationInbounds.MaxX)
            {
                locationInbounds.Translate(bounds.MaxX - envelope.MaxX, 0);
            }

            if (bounds.MinY > locationInbounds.MinY)
            {
                locationInbounds.Translate(0, bounds.MinY - envelope.MinY);
            }

            if (bounds.MaxY < locationInbounds.MaxY)
            {
                locationInbounds.Translate(0, bounds.MaxY - envelope.MaxY);
            }

            if (!bounds.Contains(locationInbounds))
            {
                throw new InvalidOperationException("Unable to translate envelope into bounds.");
            }

            return locationInbounds;
        }

        public static Envelope RandomEnvelopeInside(this Envelope envelope, Random random, double size)
        {
            Guard.Against.Negative(size);

            if (envelope.MinX + size > envelope.MaxX || envelope.MinY + size > envelope.MaxY)
            {
                throw new InvalidOperationException("Size larger then Envelope.");
            }

            var randomX = random.NextDouble() * ((envelope.MaxX - size) - envelope.MinX) + envelope.MinX;
            var randomY = random.NextDouble() * ((envelope.MaxY - size) - envelope.MinY) + envelope.MinY;

            return new Envelope(randomX, randomX + size, randomY, randomY + size);
        }

        public static double RadianAngleTo(this Coordinate coordinate, Coordinate target)
        {
            return new LineSegment(coordinate, target).Angle;
        }
    }
}
