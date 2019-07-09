using CodeMagic.Core.Game;
using NUnit.Framework;

namespace CodeMagic.Core.Tests.Game
{
    [TestFixture]
    public class PointTests
    {
        [TestCase(0, 0, 0, 2, 1, 1, ExpectedResult = false)]
        [TestCase(0, 0, 0, 1, 1, 2, ExpectedResult = false)]
        [TestCase(0, 0, 0, 1, 0, 2, ExpectedResult = false)]
        [TestCase(0, 0, 1, 1, 2, 2, ExpectedResult = false)]
        public bool IsOnLineTest(int startX, int startY, int endX, int endY, int pointX, int pointY)
        {
            var startPoint = new Point(startX, startY);
            var endPoint = new Point(endX, endY);
            var checkPoint = new Point(pointX, pointY);

            return Point.IsOnLine(startPoint, endPoint, checkPoint);
        }
    }
}