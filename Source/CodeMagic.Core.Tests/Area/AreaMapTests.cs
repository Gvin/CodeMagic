using System;
using CodeMagic.Core.Area;
using Moq;
using NUnit.Framework;

namespace CodeMagic.Core.Tests.Area
{
    [TestFixture]
    public class AreaMapTests
    {
        [Test]
        public void ConstructorTest()
        {
            // Arrange
            var environmentMock = new Mock<IEnvironment>();
            var lightManagerMock = new Mock<IEnvironmentLightManager>();

            // Act
            var map = new AreaMap(() => environmentMock.Object, 5, 5, lightManagerMock.Object);

            // Assert
            var cell = map.GetCell(2, 2);
            Assert.NotNull(cell);
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
            var environmentMock = new Mock<IEnvironment>();
            var lightManagerMock = new Mock<IEnvironmentLightManager>();

            // Arrange
            var map = new AreaMap(() => environmentMock.Object, width, height, lightManagerMock.Object);

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
            var environmentMock = new Mock<IEnvironment>();
            var lightManagerMock = new Mock<IEnvironmentLightManager>();

            var map = new AreaMap(() => environmentMock.Object, width, height, lightManagerMock.Object);

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
            var environmentMock = new Mock<IEnvironment>();
            var lightManagerMock = new Mock<IEnvironmentLightManager>();

            var map = new AreaMap(() => environmentMock.Object, width, height, lightManagerMock.Object);

            // Act
            return map.ContainsCell(checkX, checkY);
        }
    }
}