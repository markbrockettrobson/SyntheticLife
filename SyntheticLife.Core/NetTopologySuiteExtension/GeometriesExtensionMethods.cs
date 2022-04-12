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

        public static double RadianAngleTo(this Coordinate coordinate, Coordinate target)
        {
            return new LineSegment(coordinate, target).Angle;
        }
    }
}
