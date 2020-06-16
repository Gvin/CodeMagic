using System;
using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Statuses;
using Moq;
using NUnit.Framework;

namespace CodeMagic.Core.Tests.Area
{
    [TestFixture]
    [Parallelizable]
    public class AreaMapCellTests
    {
        [TestCase(true, ExpectedResult = true)]
        [TestCase(false, ExpectedResult = false)]
        public bool ContainsBigObjectsTest(bool isBigObject)
        {
            // Arrange
            var cell = new AreaMapCell((IEnvironment)null);
            var objectMock = new Mock<IMapObject>();
            objectMock.SetupGet(obj => obj.BlocksMovement).Returns(isBigObject);

            // Act
            cell.ObjectsCollection.Add(objectMock.Object);

            // Assert
            return cell.BlocksMovement;
        }

        [TestCase(UpdateOrder.Early)]
        [TestCase(UpdateOrder.Medium)]
        [TestCase(UpdateOrder.Late)]
        public void UpdatesCorrectObjectsTest(UpdateOrder order)
        {
            // Arrange
            var cell = new AreaMapCell((IEnvironment)null);

            var objects = new List<Mock<IDynamicObject>>();
            foreach (var updateOrder in Enum.GetValues(typeof(UpdateOrder)).Cast<UpdateOrder>())
            {
                var objectMock = new Mock<IDynamicObject>();
                objectMock.SetupGet(obj => obj.UpdateOrder).Returns(updateOrder);
                objects.Add(objectMock);
                cell.ObjectsCollection.Add(objectMock.Object);
            }

            var position = new Point(3, 8);
            
            // Act
            cell.Update(position, order);

            // Assert
            foreach (var objectMock in objects)
            {
                if (objectMock.Object.UpdateOrder == order)
                {
                    objectMock.Verify(obj => obj.Update(position), Times.Once);
                }
                else
                {
                    objectMock.Verify(obj => obj.Update(position), Times.Never);
                }
            }
        }

        [Test]
        public void PostUpdateRemovesDeadDestroyableTest()
        {
            // Arrange
            var cell = new AreaMapCell((IEnvironment)null);

            var statusesMock = new Mock<IObjectStatusesCollection>();
            var objectMock = new Mock<IDestroyableObject>();
            objectMock.SetupGet(obj => obj.Health).Returns(0);
            objectMock.SetupGet(obj => obj.Statuses).Returns(statusesMock.Object);
            cell.ObjectsCollection.Add(objectMock.Object);

            var mapMock = new Mock<IAreaMap>();

            // Act
            var position = new Point(5, 8);
            cell.PostUpdate(mapMock.Object, position);

            // Assert
            statusesMock.Verify(statuses => statuses.Update(position), Times.Once);
            mapMock.Verify(map => map.RemoveObject(position, objectMock.Object), Times.Once);
            objectMock.Verify(obj => obj.OnDeath(position), Times.Once);
        }

        [Test]
        public void SpreadingTest()
        {
            // Arrange
            var cell = new AreaMapCell((IEnvironment)null);
            var localSpreadingObjMock = new Mock<ISpreadingObject>();
            localSpreadingObjMock.SetupGet(obj => obj.MaxVolumeBeforeSpread).Returns(100);
            localSpreadingObjMock.SetupGet(obj => obj.Volume).Returns(120);
            localSpreadingObjMock.SetupGet(obj => obj.MaxSpreadVolume).Returns(5);
            localSpreadingObjMock.SetupGet(obj => obj.Type).Returns("liquid1");
            var localSeparatedObjMock = new Mock<ISpreadingObject>();
            localSpreadingObjMock.Setup(obj => obj.Separate(It.IsAny<int>())).Returns(localSeparatedObjMock.Object);
            cell.ObjectsCollection.Add(localSpreadingObjMock.Object);

            var otherCell = new AreaMapCell((IEnvironment)null);
            var otherSpreadingObjMock = new Mock<ISpreadingObject>();
            otherSpreadingObjMock.SetupGet(obj => obj.MaxVolumeBeforeSpread).Returns(100);
            otherSpreadingObjMock.SetupGet(obj => obj.Volume).Returns(120);
            otherSpreadingObjMock.SetupGet(obj => obj.MaxSpreadVolume).Returns(5);
            otherSpreadingObjMock.SetupGet(obj => obj.Type).Returns("liquid2");
            var otherSeparatedObjMock = new Mock<ISpreadingObject>();
            otherSpreadingObjMock.Setup(obj => obj.Separate(It.IsAny<int>())).Returns(otherSeparatedObjMock.Object);
            otherCell.ObjectsCollection.Add(otherSpreadingObjMock.Object);

            // Act
            cell.CheckSpreading(otherCell);

            // Assert
            localSpreadingObjMock.Verify(obj => obj.Separate(5), Times.Once);
            otherSpreadingObjMock.Verify(obj => obj.Separate(5), Times.Once);

            Assert.AreEqual(2, cell.ObjectsCollection.Count());
            Assert.AreEqual(2, otherCell.ObjectsCollection.Count());

            CollectionAssert.Contains(cell.ObjectsCollection, localSpreadingObjMock.Object);
            CollectionAssert.Contains(cell.ObjectsCollection, otherSeparatedObjMock.Object);

            CollectionAssert.Contains(otherCell.ObjectsCollection, otherSpreadingObjMock.Object);
            CollectionAssert.Contains(otherCell.ObjectsCollection, localSeparatedObjMock.Object);
        }
    }
}