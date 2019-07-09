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
            var config = new DestroyableObjectConfiguration
            {
                Health = 0,
                MaxHealth = maxHealth
            };
            var player = new DestroyableObject(config);

            // Act
            player.Health = value;

            // Assert
            return player.Health;
        }
    }
}