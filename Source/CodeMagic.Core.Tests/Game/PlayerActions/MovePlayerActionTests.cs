using CodeMagic.Core.Area;
using CodeMagic.Core.Common;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.PlayerActions;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Objects.PlayerData;
using Moq;
using NUnit.Framework;

namespace CodeMagic.Core.Tests.Game.PlayerActions
{
    [TestFixture]
    public class MovePlayerActionTests
    {
        [TestCase(0, 0, Direction.Down, 0, 1)]
        [TestCase(0, 0, Direction.Up, 0, -1)]
        [TestCase(0, 0, Direction.Left, -1, 0)]
        [TestCase(0, 0, Direction.Right, 1, 0)]
        public void PerformValidMovementTest(int startX, int startY, Direction direction, int endX, int endY)
        {
            var playerMock = new Mock<IPlayer>();
            var playerPosition = new Point(startX, startY);
            var mapMock = new Mock<IAreaMap>();
            var gameMock = new Mock<IGameCore>();
            gameMock.SetupGet(game => game.Map).Returns(mapMock.Object);
            gameMock.SetupGet(game => game.Player).Returns(playerMock.Object);
            gameMock.SetupGet(game => game.PlayerPosition).Returns(playerPosition);

            var cell1 = new AreaMapCell();
            cell1.Objects.Add(playerMock.Object);
            var cell2 = new AreaMapCell();

            var newPosition = new Point(endX, endY);

            mapMock.Setup(map => map.GetCell(
                    It.Is<Point>(point => point.Equals(playerPosition))))
                .Returns(cell1);
            mapMock.Setup(map => map.GetCell(
                    It.Is<Point>(point => point.Equals(newPosition))))
                .Returns(cell2);

            mapMock.Setup(map => map.ContainsCell(
                    It.Is<Point>(point => point.Equals(newPosition))))
                .Returns(true);

            var action = new MovePlayerAction(direction);

            action.Perform(gameMock.Object, out var endPosition);

            CollectionAssert.IsEmpty(cell1.Objects);
            CollectionAssert.Contains(cell2.Objects, playerMock.Object);
        }

        [Test]
        public void PerformOutOfMapTest()
        {
            var playerMock = new Mock<IPlayer>();
            var playerPosition = new Point(0, 0);
            var mapMock = new Mock<IAreaMap>();
            var gameMock = new Mock<IGameCore>();
            gameMock.SetupGet(game => game.Map).Returns(mapMock.Object);
            gameMock.SetupGet(game => game.Player).Returns(playerMock.Object);
            gameMock.SetupGet(game => game.PlayerPosition).Returns(playerPosition);

            mapMock.Setup(map => map.ContainsCell(
                    It.IsAny<Point>()))
                .Returns(false);

            var action = new MovePlayerAction(Direction.Down);

            action.Perform(gameMock.Object, out var endPosition);

            mapMock.Verify(map => map.GetCell(It.IsAny<Point>()), Times.Never);
        }

        [Test]
        public void PerformBlockedCellTest()
        {
            var playerMock = new Mock<IPlayer>();
            var playerPosition = new Point(0, 0);
            var mapMock = new Mock<IAreaMap>();
            var gameMock = new Mock<IGameCore>();
            gameMock.SetupGet(game => game.Map).Returns(mapMock.Object);
            gameMock.SetupGet(game => game.Player).Returns(playerMock.Object);
            gameMock.SetupGet(game => game.PlayerPosition).Returns(playerPosition);

            var cell1 = new AreaMapCell();
            cell1.Objects.Add(playerMock.Object);

            var blockingObjectMock = new Mock<IMapObject>();
            blockingObjectMock.SetupGet(obj => obj.BlocksMovement).Returns(true);
            var cell2 = new AreaMapCell();
            cell2.Objects.Add(blockingObjectMock.Object);

            var newPosition = new Point(0, 1);

            mapMock.Setup(map => map.GetCell(
                    It.Is<Point>(point => point.Equals(playerPosition))))
                .Returns(cell1);
            mapMock.Setup(map => map.GetCell(
                    It.Is<Point>(point => point.Equals(newPosition))))
                .Returns(cell2);

            mapMock.Setup(map => map.ContainsCell(
                    It.Is<Point>(point => point.Equals(newPosition))))
                .Returns(true);

            var action = new MovePlayerAction(Direction.Down);

            action.Perform(gameMock.Object, out var endPosition);

            CollectionAssert.DoesNotContain(cell2.Objects, playerMock.Object);
            CollectionAssert.Contains(cell1.Objects, playerMock.Object);
        }
    }
}