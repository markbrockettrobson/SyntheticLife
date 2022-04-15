using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NetTopologySuite.Geometries;
using NUnit.Framework;

namespace SyntheticLife.Core.Map.Test
{
    [TestFixture]
    public class EntityMapTests
    {
        public List<Mock<IMapEntity>> TestEntities { get; set; } = new ();
        public Action<IMapEntity, Envelope>? LastOnUpdateLocationAction { get; set; } = null;

        public Envelope Bounds = new Envelope(-1000, 1000, -1000, 1000);

        [SetUp]
        public void SetUp()
        {
            TestEntities = new ();
            for (int index = 0; index < 10; index++)
            {
                Mock<IMapEntity> entity = new ();
                entity
                    .Setup(entity => entity.Location)
                    .Returns(new Envelope(index, index + 0.5, index, index + 0.5));
                entity
                    .Setup(entity => entity.AddOnUpdateLocationAction(It.IsAny<Action<IMapEntity, Envelope>>()))
                    .Callback<Action<IMapEntity, Envelope>>(action => LastOnUpdateLocationAction = action);

                TestEntities.Add(entity);
            }
        }

        [Test]
        public void MapEntitiesEmpty()
        {
            // Arrange
            var map = new EntityMap(Bounds);
            // Act
            var mapEntities = map.MapEntities;
            // Assert
            Assert.That(mapEntities, Is.Empty);
        }

        [Test]
        public void BoundsSet()
        {
            // Arrange
            var map = new EntityMap(Bounds);
            // Act
            // Assert
            Assert.That(map.Bounds, Is.EqualTo(Bounds));
        }

        [TestCase(new[] { 0 })]
        [TestCase(new[] { 0, 1 })]
        [TestCase(new[] { 0, 2, 3 })]
        [TestCase(new[] { 0, 2, 4, 6, 8 })]
        public void MapEntities(int[] entityIndexes)
        {
            // Arrange
            var map = new EntityMap(Bounds);
            foreach (var index in entityIndexes)
            {
                map.AddEntity(TestEntities[index].Object);
            }

            // Act
            var mapEntities = map.MapEntities;
            // Assert
            Assert.That(mapEntities, Is.EquivalentTo(entityIndexes.Select(index => TestEntities[index].Object)));
        }

        [Test]
        public void AddExceptionOnRepeat()
        {
            // Arrange
            var map = new EntityMap(Bounds);
            map.AddEntity(TestEntities[0].Object);
            // Act
            // Assert
            Assert.Throws<InvalidOperationException>(() => map.AddEntity(TestEntities[0].Object));
        }

        [Test]
        public void AddExceptionOnOutsideBounds()
        {
            // Arrange
            var map = new EntityMap(Bounds);
            TestEntities[0]
                .Setup(entity => entity.Location)
                .Returns(new Envelope(-1012, 0, 0, 0));
            TestEntities[1]
                .Setup(entity => entity.Location)
                .Returns(new Envelope(0, 1001, 0, 0));
            TestEntities[2]
                .Setup(entity => entity.Location)
                .Returns(new Envelope(0, 0, -1002, 0));
            TestEntities[3]
                .Setup(entity => entity.Location)
                .Returns(new Envelope(0, 0, 0, 1002));
            // Act
            // Assert
            Assert.Throws<InvalidOperationException>(() => map.AddEntity(TestEntities[0].Object));
            Assert.Throws<InvalidOperationException>(() => map.AddEntity(TestEntities[1].Object));
            Assert.Throws<InvalidOperationException>(() => map.AddEntity(TestEntities[2].Object));
            Assert.Throws<InvalidOperationException>(() => map.AddEntity(TestEntities[3].Object));
        }

        [Test]
        public void AddEntityOnEntityLocationUpdateSet()
        {
            // Arrange
            var map = new EntityMap(Bounds);
            map.AddEntity(TestEntities[0].Object);
            // Act
            // Assert
            TestEntities[0].Verify(
                entity => entity.AddOnUpdateLocationAction(It.IsAny<Action<IMapEntity, Envelope>>()),
                Times.Once);
        }

        [Test]
        public void QueryEmpty()
        {
            // Arrange
            var map = new EntityMap(Bounds);
            // Act
            var mapEntities = map.Query(new Envelope(-10, 10, -10, 10));
            // Assert
            Assert.That(mapEntities, Is.Empty);
        }

        [TestCase(new[] { 0 })]
        [TestCase(new[] { 0, 1 })]
        [TestCase(new[] { 0, 2, 3 })]
        [TestCase(new[] { 0, 2, 4, 6, 8 })]
        public void QueryAllInside(int[] entityIndexes)
        {
            // Arrange
            var map = new EntityMap(Bounds);
            foreach (var index in entityIndexes)
            {
                map.AddEntity(TestEntities[index].Object);
            }

            // Act
            var mapEntities = map.Query(new Envelope(-10, 10, -10, 10));
            // Assert
            Assert.That(mapEntities, Is.EquivalentTo(entityIndexes.Select(index => TestEntities[index].Object)));
        }

        [TestCase(new[] { 0 })]
        [TestCase(new[] { 0, 1 })]
        [TestCase(new[] { 0, 2, 3 })]
        [TestCase(new[] { 0, 2, 4, 6, 8 })]
        public void QueryAllOutside(int[] entityIndexes)
        {
            // Arrange
            var map = new EntityMap(Bounds);
            foreach (var index in entityIndexes)
            {
                map.AddEntity(TestEntities[index].Object);
            }

            // Act
            var mapEntities = map.Query(new Envelope(-10, -0.1, -10, -0.1));
            // Assert
            Assert.That(mapEntities, Is.Empty);
        }

        [TestCase(new[] { 0, 1, 2, 3, 4, 5, 6, 7 }, new[] { 4, 5, 6, 7 })]
        [TestCase(new[] { 0, 2, 3, 6, 8 }, new[] { 6, 8 })]
        public void QueryMixed(int[] entityIndexes, int[] entityIndexesInside)
        {
            // Arrange
            var map = new EntityMap(Bounds);
            foreach (var index in entityIndexes)
            {
                map.AddEntity(TestEntities[index].Object);
            }

            // Act
            var mapEntities = map.Query(new Envelope(4, 10, 4, 10));
            // Assert
            Assert.That(mapEntities, Is.EquivalentTo(entityIndexesInside.Select(index => TestEntities[index].Object)));
        }

        [TestCase(new[] { 0, 1, 2, 3 }, new[] { 0, 2, 3 })]
        [TestCase(new[] { 0, 2, 3, 6, 8 }, new[] { 6, 2, 3 })]
        public void Delete(int[] entityIndexes, int[] insideEntityIndexes)
        {
            // Arrange
            var map = new EntityMap(Bounds);
            foreach (var index in entityIndexes)
            {
                map.AddEntity(TestEntities[index].Object);
            }

            // Act
            foreach (var index in entityIndexes.Where(index => !insideEntityIndexes.Contains(index)))
            {
                map.RemoveEntity(TestEntities[index].Object);
            }

            // Assert
            var mapEntities = map.Query(new Envelope(0, 10, 0, 10));
            Assert.That(mapEntities, Is.EquivalentTo(insideEntityIndexes.Select(index => TestEntities[index].Object)));
        }

        [Test]
        public void DeleteExceptionOnRepeat()
        {
            // Arrange
            var map = new EntityMap(Bounds);
            // Act
            // Assert
            Assert.Throws<InvalidOperationException>(() => map.RemoveEntity(TestEntities[0].Object));
        }

        [Test]
        public void EntityLocationUpdate()
        {
            // Arrange
            var map = new EntityMap(Bounds);
            map.AddEntity(TestEntities[0].Object);
            // Act
            if (LastOnUpdateLocationAction == null)
            {
                throw new Exception("LastOnUpdateLocationAction should be set.");
            }

            LastOnUpdateLocationAction(TestEntities[0].Object, new Envelope(30, 31, 30, 31));
            // Assert
            var entitiesAtOldLocation = map.Query(new Envelope(0, 10, 0, 10));
            var entitiesAtNewLocation = map.Query(new Envelope(30, 31, 30, 31));

            Assert.That(entitiesAtOldLocation, Has.Count.EqualTo(0));
            Assert.That(entitiesAtNewLocation, Is.EquivalentTo(new List<IMapEntity>() { TestEntities[0].Object }));
        }
    }
}
