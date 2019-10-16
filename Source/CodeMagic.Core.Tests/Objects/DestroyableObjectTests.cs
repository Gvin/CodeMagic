using CodeMagic.Core.Objects;
using NUnit.Framework;

namespace CodeMagic.Core.Tests.Objects
{
    [TestFixture]
    public class DestroyableObjectTests
    {
        [TestCase(100, 100, ExpectedResult = 100)]
        [TestCase(101, 100, ExpectedResult = 100)]
        [TestCase(99, 100, ExpectedResult = 99)]
        [TestCase(10000, 100, ExpectedResult = 100)]
        [TestCase(0, 100, ExpectedResult = 0)]
        [TestCase(-1, 100, ExpectedResult = 0)]
        [TestCase(-10000, 100, ExpectedResult = 0)]
        public int SetHealthTest(int value, int maxHealth)
        {
            // Arrange
            var player = new TestDestroyableObject(maxHealth);

            // Act
            player.Health = value;

            // Assert
            return player.Health;
        }

        private class TestDestroyableObject : DestroyableObject
        {
            public TestDestroyableObject(int maxHealth) : base(maxHealth)
            {
            }

            public override string Name => "Test";
            public override ZIndex ZIndex => ZIndex.Air;
            public override ObjectSize Size => ObjectSize.Huge;
        }
    }
}