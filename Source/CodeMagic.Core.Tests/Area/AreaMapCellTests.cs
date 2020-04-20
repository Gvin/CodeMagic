using CodeMagic.Core.Area;
using CodeMagic.Core.Objects;
using Moq;
using NUnit.Framework;

namespace CodeMagic.Core.Tests.Area
{
    [TestFixture]
    public class AreaMapCellTests
    {
        [TestCase(true, ExpectedResult = true)]
        [TestCase(false, ExpectedResult = false)]
        public bool ContainsBigObjectsTest(bool isBigObject)
        {
            var cell = new AreaMapCell((IEnvironment)null);
            var objectMock = new Mock<IMapObject>();
            objectMock.SetupGet(obj => obj.BlocksMovement).Returns(isBigObject);
            cell.ObjectsCollection.Add(objectMock.Object);

            return cell.BlocksMovement;
        }
    }
}