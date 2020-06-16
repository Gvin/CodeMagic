using System;
using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.Core.Objects;
using Moq;
using NUnit.Framework;

namespace CodeMagic.Core.Tests.Area
{
    [TestFixture]
    [Parallelizable]
    public class AreaMapTests
    {
        [Test]
        public void ConstructorTest()
        {
            // Arrange
            var mapCellsFactory = new Func<IAreaMapCellInternal>(() => new Mock<IAreaMapCellInternal>().Object);

            // Act
            var map = new AreaMap(1, mapCellsFactory, 5, 5);

            // Assert
            var cell = map.GetCell(2, 2);
            Assert.NotNull(cell);
        }

        [TestCase(0, 0, ExpectedResult = true)]
        [TestCase(-1, 0, ExpectedResult = false)]
        [TestCase(0, -1, ExpectedResult = false)]
        [TestCase(-1, -1, ExpectedResult = false)]
        [TestCase(2, 2, ExpectedResult = true)]
        [TestCase(5, 0, ExpectedResult = false)]
        [TestCase(0, 5, ExpectedResult = true)]
        [TestCase(0, 7, ExpectedResult = false)]
        [TestCase(10000, 10000, ExpectedResult = false)]
        public bool TryGetCellTest(int x, int y)
        {
            // Arrange
            const int width = 5;
            const int height = 7;
            var mapCellsFactory = new Func<IAreaMapCellInternal>(() => new Mock<IAreaMapCellInternal>().Object);

            // Act
            var map = new AreaMap(1, mapCellsFactory, width, height);

            // Assert
            var cell = map.TryGetCell(x, y);
            return cell != null;
        }

        [Test]
        public void AddObjectTest()
        {
            // Arrange
            const int posX = 1;
            const int posY = 2;
            var mapObject = new Mock<IMapObject>();
            var mapCellsFactory = new Func<IAreaMapCellInternal>(() => new AreaMapCell(new Mock<IEnvironment>().Object));
            var map = new AreaMap(1, mapCellsFactory, 5, 5);

            // Act
            map.AddObject(posX, posY, mapObject.Object);

            // Assert
            var cell = (AreaMapCell) map.GetCell(posX, posY);
            var objects = cell.ObjectsCollection.ToArray();
            Assert.AreEqual(1, objects.Length);
            Assert.AreSame(mapObject.Object, objects[0]);
        }

        [Test]
        public void RemoveObjectTest()
        {
            // Arrange
            const int posX = 1;
            const int posY = 2;
            var mapObject = new Mock<IMapObject>();
            var mapCellsFactory = new Func<IAreaMapCellInternal>(() => new AreaMapCell(new Mock<IEnvironment>().Object));
            var map = new AreaMap(1, mapCellsFactory, 5, 5);
            var position = new Point(posX, posY);

            // Act
            map.AddObject(posX, posY, mapObject.Object);
            map.RemoveObject(position, mapObject.Object);

            // Assert
            var cell = (AreaMapCell)map.GetCell(position);
            var objects = cell.ObjectsCollection.ToArray();
            Assert.AreEqual(0, objects.Length);
        }

        [Test]
        public void DestroyableObjectsAreAddedToCache()
        {
            // Arrange
            const string destroyableId = "destr_id";
            var destroyable = new Mock<IDestroyableObject>();
            destroyable.SetupGet(d => d.Id).Returns(destroyableId);

            var mapCellsFactory = new Func<IAreaMapCellInternal>(() => new AreaMapCell(new Mock<IEnvironment>().Object));

            var map = new AreaMap(1, mapCellsFactory, 5, 5);

            // Act
            map.AddObject(1, 1, destroyable.Object);
            var cachedDestroyable = map.GetDestroyableObject(destroyableId);

            // Assert
            Assert.AreSame(destroyable.Object, cachedDestroyable);
        }

        [Test]
        public void RemovedDestroyableObjectsAreRemovedFromCache()
        {
            // Arrange
            const int posX = 1;
            const int posY = 1;
            const string destroyableId = "destr_id";
            var destroyable = new Mock<IDestroyableObject>();
            destroyable.SetupGet(d => d.Id).Returns(destroyableId);

            var mapCellsFactory = new Func<IAreaMapCellInternal>(() => new AreaMapCell(new Mock<IEnvironment>().Object));

            var map = new AreaMap(1, mapCellsFactory, 5, 5);
            var position = new Point(posX, posY);

            // Act
            map.AddObject(position, destroyable.Object);
            map.RemoveObject(position, destroyable.Object);
            var cachedDestroyable = map.GetDestroyableObject(destroyableId);

            // Assert
            Assert.IsNull(cachedDestroyable);
        }

        [Test]
        public void RemovedObjectsAreRemovedFromPositionCache()
        {
            // Arrange
            const int posX = 1;
            const int posY = 2;
            var destroyable = new Mock<IDestroyableObject>();
            destroyable.SetupGet(d => d.Id).Returns("destr_id");

            var mapCellsFactory = new Func<IAreaMapCellInternal>(() => new AreaMapCell(new Mock<IEnvironment>().Object));

            var map = new AreaMap(1, mapCellsFactory, 5, 5);
            var position = new Point(posX, posY);

            // Act
            map.AddObject(position, destroyable.Object);
            map.RemoveObject(position, destroyable.Object);
            var searchResult = map.GetObjectPosition<IDestroyableObject>();

            // Assert
            Assert.IsNull(searchResult);
        }

        [Test]
        public void ObjectPositionIsCached()
        {
            // Arrange
            const int posX = 1;
            const int posY = 2;
            var destroyable = new Mock<IDestroyableObject>();
            destroyable.SetupGet(d => d.Id).Returns("destr_id");

            var mapCellsFactory = new Func<IAreaMapCellInternal>(() => new AreaMapCell(new Mock<IEnvironment>().Object));

            var map = new AreaMap(1, mapCellsFactory, 5, 5);

            // Act
            map.AddObject(posX, posY, destroyable.Object);
            var position = map.GetObjectPosition<IDestroyableObject>();

            // Assert
            Assert.AreEqual(posX, position.X);
            Assert.AreEqual(posY, position.Y);
        }

        [TestCase(3, 3, 0, 0)]
        [TestCase(3, 3, 2, 0)]
        [TestCase(3, 3, 0, 2)]
        [TestCase(3, 3, 2, 2)]
        [TestCase(2, 5, 1, 4)]
        [TestCase(5, 2, 4, 1)]
        public void GetCellValidTest(int width, int height, int checkX, int checkY)
        {
            // Arrange
            var mapCellsFactory = new Func<IAreaMapCellInternal>(() => new Mock<IAreaMapCellInternal>().Object);

            // Arrange
            var map = new AreaMap(1, mapCellsFactory, width, height);

            // Act
            var cell = map.GetCell(checkX, checkY);

            // Assert
            Assert.NotNull(cell);
        }

        [TestCase(3, 3, -1, 0, "X")]
        [TestCase(3, 3, 0, -1, "Y")]
        [TestCase(3, 3, 3, 0, "X")]
        [TestCase(3, 3, 0, 3, "Y")]
        public void GetCellInvalidTest(int width, int height, int checkX, int checkY, string errorArgument)
        {
            // Arrange
            var mapCellsFactory = new Func<IAreaMapCellInternal>(() => new Mock<IAreaMapCellInternal>().Object);

            var map = new AreaMap(1, mapCellsFactory, width, height);

            //Act
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                map.GetCell(checkX, checkY);
            });
            StringAssert.StartsWith($"Coordinate {errorArgument} value", exception.Message);
        }

        [TestCase(3, 3, 0, 0, ExpectedResult = true)]
        [TestCase(3, 3, 2, 0, ExpectedResult = true)]
        [TestCase(3, 3, 0, 2, ExpectedResult = true)]
        [TestCase(3, 3, 2, 2, ExpectedResult = true)]
        [TestCase(2, 5, 1, 4, ExpectedResult = true)]
        [TestCase(5, 2, 4, 1, ExpectedResult = true)]
        [TestCase(3, 3, -1, 0, ExpectedResult = false)]
        [TestCase(3, 3, 0, -1, ExpectedResult = false)]
        [TestCase(3, 3, 3, 0, ExpectedResult = false)]
        [TestCase(3, 3, 0, 3, ExpectedResult = false)]
        public bool ContainsCellTest(int width, int height, int checkX, int checkY)
        {
            // Arrange
            var mapCellsFactory = new Func<IAreaMapCellInternal>(() => new Mock<IAreaMapCellInternal>().Object);

            var map = new AreaMap(1, mapCellsFactory, width, height);

            // Act
            return map.ContainsCell(checkX, checkY);
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(3)]
        [TestCase(6)]
        public void GetMapPartTest(int size)
        {
            // Arrange
            var mapCellsFactory = new Func<IAreaMapCellInternal>(() => new Mock<IAreaMapCellInternal>().Object);

            var map = new AreaMap(1, mapCellsFactory, 30, 30);

            // Act
            var area = map.GetMapPart(new Point(1, 1), size);

            // Assert
            var expectedAreaDiameter = size * 2 + 1;
            Assert.AreEqual(expectedAreaDiameter, area.Length);
            Assert.IsTrue(area.All(row => row.Length == expectedAreaDiameter));
        }

        [Test]
        public void RefreshUpdatesLightLevelTest()
        {
            // Arrange
            var mapCellsFactory = new Func<IAreaMapCellInternal>(() => new Mock<IAreaMapCellInternal>().Object);
            var mapLightLevelProcessorMock = new Mock<IMapLightLevelProcessor>();

            var map = new AreaMap(1, mapCellsFactory, 30, 30, mapLightLevelProcessorMock.Object);

            // Act
            map.Refresh();

            // Assert
            Assert.AreEqual(
                mapLightLevelProcessorMock.Invocations.Select(invocation => invocation.Method.Name),
                new[]
                {
                    nameof (IMapLightLevelProcessor.ResetLightLevel),
                    nameof (IMapLightLevelProcessor.UpdateLightLevel)
                }); // Checks call order
            mapLightLevelProcessorMock.Verify(processor => processor.ResetLightLevel(map), Times.Once);
            mapLightLevelProcessorMock.Verify(processor => processor.UpdateLightLevel(map), Times.Once);
            mapLightLevelProcessorMock.VerifyNoOtherCalls();
        }

        [Test]
        public void UpdateCallsCellUpdateTest()
        {
            // Arrange
            var cells = new List<Mock<IAreaMapCellInternal>>();
            var mapCellsFactory = new Func<IAreaMapCellInternal>(() =>
            {
                var environment = new Mock<IEnvironment>();
                var cell = new Mock<IAreaMapCellInternal>();
                cell.SetupGet(c => c.Environment).Returns(environment.Object);
                cells.Add(cell);
                return cell.Object;
            });
            var mapLightLevelProcessorMock = new Mock<IMapLightLevelProcessor>();
            var turnProvider = new Mock<ITurnProvider>();

            var map = new AreaMap(1, mapCellsFactory, 30, 20, mapLightLevelProcessorMock.Object);

            // Act
            map.Update(turnProvider.Object);

            // Assert
            foreach (var cellMock in cells)
            {
                cellMock.Verify(cell => cell.Update(It.IsAny<Point>(), UpdateOrder.Early), Times.Once);
                cellMock.Verify(cell => cell.Update(It.IsAny<Point>(), UpdateOrder.Medium), Times.Once);
                cellMock.Verify(cell => cell.Update(It.IsAny<Point>(), UpdateOrder.Late), Times.Once);
                cellMock.Verify(cell => cell.PostUpdate(map, It.IsAny<Point>()), Times.Once);
                cellMock.Verify(cell => cell.ResetDynamicObjectsState(), Times.Once);
            }

            mapLightLevelProcessorMock.Verify(processor => processor.ResetLightLevel(map), Times.Once);
            mapLightLevelProcessorMock.Verify(processor => processor.UpdateLightLevel(map), Times.Once);
            mapLightLevelProcessorMock.VerifyNoOtherCalls();
        }
    }
}