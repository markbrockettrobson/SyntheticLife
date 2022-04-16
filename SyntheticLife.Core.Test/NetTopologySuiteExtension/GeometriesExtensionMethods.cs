using System;
using Moq;
using NetTopologySuite.Geometries;
using NUnit.Framework;

namespace SyntheticLife.Core.Test
{
    [TestFixture]
    public class GeometriesExtensionMethodsTests
    {
        private static readonly double Epsilon = 0.001;

        [TestCase(0, 0, 0, 10, 10, 0)]
        [TestCase(4, 0, 0.5, 20, 4, 20)]
        [TestCase(1, 1, 1, 30, -29, 1)]
        [TestCase(1000, 0.1, 1.5, 30.1, 1000, -30)]
        [TestCase(-7, -11, 3, 30, -37, -11)]
        [TestCase(3, 297, -3, 300, -297, 297)]
        [TestCase(2, 10, 0.25, 1, 2.707, 10.707)]
        [TestCase(-10, 4, 0.75, 1, -10.707, 4.707)]
        [TestCase(7, -10, 1.25, 1, 6.293, -10.707)]
        [TestCase(-0.3, 0, 1.75, 1, 0.407, -0.707)]
        public void TranslateAtAngle(double x1, double y1, double radianAngle, double distance, double x2, double y2)
        {
            // Arrange
            var startingEnvelope = new Envelope(x1, x1 + 1, y1, y1 + 1);
            // Act
            var location = startingEnvelope.TranslateAtAngle(radianAngle * Math.PI, distance);
            // Assert
            Assert.That(location.MinX, Is.InRange(x2 - Epsilon, x2 + Epsilon));
            Assert.That(location.MinY, Is.InRange(y2 - Epsilon, y2 + Epsilon));
            Assert.That(location.MaxX, Is.InRange(x2 - Epsilon + 1, x2 + Epsilon + 1));
            Assert.That(location.MaxY, Is.InRange(y2 - Epsilon + 1, y2 + Epsilon + 1));
        }

        [TestCase(0, 0, 0, 10, 10, 0)]
        [TestCase(4, 0, 0.5, 20, 4, 20)]
        [TestCase(1, 1, 1, 30, -29, 1)]
        [TestCase(1000, 0.1, 1.5, 30.1, 1000, -30)]
        [TestCase(-7, -11, 3, 30, -37, -11)]
        [TestCase(3, 297, -3, 300, -297, 297)]
        [TestCase(2, 10, 0.25, 1, 2.707, 10.707)]
        [TestCase(-10, 4, 0.75, 1, -10.707, 4.707)]
        [TestCase(7, -10, 1.25, 1, 6.293, -10.707)]
        [TestCase(-0.3, 0, 1.75, 1, 0.407, -0.707)]
        public void TranslateRandomDirection(double x1, double y1, double radianAngle, double distance, double x2, double y2)
        {
            // Arrange
            var startingEnvelope = new Envelope(x1, x1 + 1, y1, y1 + 1);
            var mockRandom = new Mock<Random>();
            mockRandom.Setup(random => random.NextDouble()).Returns(radianAngle / 2);
            // Act
            var location = startingEnvelope.TranslateRandomDirection(mockRandom.Object, distance);
            // Assert
            Assert.That(location.MinX, Is.InRange(x2 - Epsilon, x2 + Epsilon));
            Assert.That(location.MinY, Is.InRange(y2 - Epsilon, y2 + Epsilon));
            Assert.That(location.MaxX, Is.InRange(x2 - Epsilon + 1, x2 + Epsilon + 1));
            Assert.That(location.MaxY, Is.InRange(y2 - Epsilon + 1, y2 + Epsilon + 1));
        }

        [TestCase(0, 0, 100, 10, 0)]
        [TestCase(4, 0, 21, 4, 20)]
        [TestCase(1, 1, 130, -29, 1)]
        [TestCase(1000, 0.1, 30.2, 1000, -30)]
        [TestCase(-7, -11, 30, -37, -11)]
        [TestCase(3, 297, 300, -297, 297)]
        [TestCase(2, 10, 1, 2.707, 10.707)]
        [TestCase(-10, 4, 1, -10.707, 4.707)]
        [TestCase(7, -10, 1, 6.293, -10.707)]
        [TestCase(-0.3, 0, 1, 0.407, -0.707)]
        public void TranslateToWithinRange(double x1, double y1, double distance, double x2, double y2)
        {
            // Arrange
            var startingEnvelope = new Envelope(x1, x1 + 1, y1, y1 + 1);
            var targetEnvelope = new Envelope(x2, x2 + 1, y2, y2 + 1);

            // Act
            var location = startingEnvelope.TranslateTo(targetEnvelope, distance);
            // Assert
            Assert.That(location.MinX, Is.InRange(x2 - Epsilon, x2 + Epsilon));
            Assert.That(location.MinY, Is.InRange(y2 - Epsilon, y2 + Epsilon));
            Assert.That(location.MaxX, Is.InRange(x2 - Epsilon + 1, x2 + Epsilon + 1));
            Assert.That(location.MaxY, Is.InRange(y2 - Epsilon + 1, y2 + Epsilon + 1));
        }

        [TestCase(0, 0, 5, 10, 0, 5, 0)]
        [TestCase(4, 0, 3, 4, 12, 4, 3)]
        [TestCase(1, 1, 12, -29, 1, -11, 1)]
        [TestCase(1000, 0.1, 3.2, 1000, -30, 1000, -3.1)]
        [TestCase(-7, -11, 0, -37, -11, -7, -11)]
        [TestCase(3, 297, 30, -297, 297, -27, 297)]
        [TestCase(2, 10, 0.5, 2.707, 10.707, 2.3535, 10.3535)]
        [TestCase(-10, 4, 0.1, -10.707, 4.707, -10.0707, 4.0707)]
        public void TranslateToOutsideRange(double x1, double y1, double distance, double x2, double y2, double x3, double y3)
        {
            // Arrange
            var startingEnvelope = new Envelope(x1, x1 + 1, y1, y1 + 1);
            var targetEnvelope = new Envelope(x2, x2 + 1, y2, y2 + 1);

            // Act
            var location = startingEnvelope.TranslateTo(targetEnvelope, distance);
            // Assert
            Assert.That(location.MinX, Is.InRange(x3 - Epsilon, x3 + Epsilon));
            Assert.That(location.MinY, Is.InRange(y3 - Epsilon, y3 + Epsilon));
            Assert.That(location.MaxX, Is.InRange(x3 - Epsilon + 1, x3 + Epsilon + 1));
            Assert.That(location.MaxY, Is.InRange(y3 - Epsilon + 1, y3 + Epsilon + 1));
        }

        [TestCase(0, 0)]
        [TestCase(10, 110)]
        [TestCase(30, -10)]
        [TestCase(31, -29)]
        [TestCase(-23, -21)]
        [TestCase(12, 32)]
        public void TranslateToInsideEnvelope(double x, double y)
        {
            // Arrange
            var startingEnvelope = new Envelope(x, x + 1, y, y + 1);
            var boundsEnvelope = new Envelope(30, 32, -22, -20);

            // Act
            var location = startingEnvelope.TranslateToInsideEnvelope(boundsEnvelope);
            // Assert
            Assert.That(location.MinX, Is.InRange(30, 32));
            Assert.That(location.MaxX, Is.InRange(30, 32));
            Assert.That(location.MinY, Is.InRange(-22, -20));
            Assert.That(location.MaxY, Is.InRange(-22, -20));
        }

        [TestCase(0, 0)]
        [TestCase(10, 110)]
        [TestCase(30, -10)]
        [TestCase(31, -22)]
        [TestCase(-23, -21)]
        [TestCase(12, 32)]
        public void TranslateToAlreadyInsideEnvelope(double x, double y)
        {
            // Arrange
            var startingEnvelope = new Envelope(x, x + 1, y, y + 1);
            var boundsEnvelope = new Envelope(-1000, 1000, -1000, 1000);

            // Act
            var location = startingEnvelope.TranslateToInsideEnvelope(boundsEnvelope);
            // Assert
            Assert.That(location.MinX, Is.InRange(x - Epsilon, x + Epsilon));
            Assert.That(location.MaxX, Is.InRange(x + 1 - Epsilon, x + 1 + Epsilon));
            Assert.That(location.MinY, Is.InRange(y - Epsilon, y + Epsilon));
            Assert.That(location.MaxY, Is.InRange(y + 1 - Epsilon, y + 1 + Epsilon));
        }

        [TestCase(0, 0)]
        [TestCase(10, 110)]
        [TestCase(30, -10)]
        [TestCase(31, -22)]
        [TestCase(-23, -21)]
        [TestCase(12, 32)]
        public void TranslateToInsideEnvelopeImpossiblee(double x, double y)
        {
            // Arrange
            var startingEnvelope = new Envelope(x, x + 1, y, y + 1);
            var boundsEnvelopeOne = new Envelope(30, 30.5, -21, -22);
            var boundsEnvelopeTwo = new Envelope(30, 31, -21.5, -22);

            // Act
            // Assert
            Assert.Throws<InvalidOperationException>(
                () => startingEnvelope.TranslateToInsideEnvelope(boundsEnvelopeOne));
            Assert.Throws<InvalidOperationException>(
                () => startingEnvelope.TranslateToInsideEnvelope(boundsEnvelopeTwo));
        }

        [TestCase(2, 1, 1)]
        [TestCase(1, 1, 0)]
        [TestCase(300, 200, 500)]
        [TestCase(200, 500, 199)]
        [TestCase(3, 20, 2)]
        public void RandomEnvelopeInsideImpossiblee(int size, int envelopeX, int envelopeY)
        {
            // Arrange
            var boundsEnvelope = new Envelope(0, envelopeX, 0, envelopeY);

            // Act
            // Assert
            Assert.Throws<InvalidOperationException>(
                () => boundsEnvelope.RandomEnvelopeInside(new Random(), size));
        }

        [TestCase(0, 0, 2)]
        [TestCase(0, 1, 2)]
        [TestCase(1, 1, 1)]
        [TestCase(4, 5, 4)]
        [TestCase(300, 500, 500)]
        [TestCase(2, 500, 300)]
        [TestCase(3, 20, 5)]
        public void RandomEnvelopeInside(int size, int envelopeX, int envelopeY)
        {
            // Arrange
            var boundsEnvelope = new Envelope(0, envelopeX, 0, envelopeY);

            // Act
            // Assert
            var envelope = boundsEnvelope.RandomEnvelopeInside(new Random(), size);

            Assert.IsTrue(boundsEnvelope.Contains(envelope));
        }

        [Test]
        public void RandomEnvelopeInsideSizeNegative()
        {
            // Arrange
            var boundsEnvelope = new Envelope(0, 100, 0, 100);

            // Act
            // Assert
            Assert.Throws<ArgumentException>(
                () => boundsEnvelope.RandomEnvelopeInside(new Random(), -32));
        }

        [Test]
        public void TranslateToNegativeDistance()
        {
            // Arrange
            var startingEnvelope = new Envelope(1, 1, 2, 2);
            var targetEnvelope = new Envelope(3, 3, 4, 4);

            // Act
            // Assert
            Assert.Throws<ArgumentException>(() => startingEnvelope.TranslateTo(targetEnvelope, -1));
        }

        [TestCase(0, 0, 0, 10, 0)]
        [TestCase(0, 0, 0, 0, 0)]
        [TestCase(4, 0, 0.5, 4, 20)]
        [TestCase(1, 1, 1, -29, 1)]
        [TestCase(1000, 0.1, -0.5, 1000, -30)]
        [TestCase(-7, -11, 1, -37, -11)]
        [TestCase(3, 297, 1, -297, 297)]
        [TestCase(2, 10, 0.25, 2.707, 10.707)]
        [TestCase(-10, 4, 0.75, -10.707, 4.707)]
        [TestCase(7, -10, -0.75, 6.293, -10.707)]
        [TestCase(-0.3, 0, -0.25, 0.407, -0.707)]
        public void RadianAngleTo(double x1, double y1, double radianAngle, double x2, double y2)
        {
            // Arrange
            var startingCoordinate = new Coordinate(x1, y1);
            var targetCoordinate = new Coordinate(x2, y2);
            // Act
            var angle = startingCoordinate.RadianAngleTo(targetCoordinate);
            // Assert
            Assert.That(
                angle / Math.PI,
                Is.InRange(radianAngle - 0.00001, radianAngle + 0.00001));
        }
    }
}
