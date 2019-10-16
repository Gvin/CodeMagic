using CodeMagic.Core.Objects.PlayerData;
using NUnit.Framework;

namespace CodeMagic.Core.Tests.Objects.PlayerData
{
    [TestFixture]
    public class PlayerTests
    {
        [TestCase(100, 100, ExpectedResult = 100)]
        [TestCase(101, 100, ExpectedResult = 100)]
        [TestCase(99, 100, ExpectedResult = 99)]
        [TestCase(10000, 100, ExpectedResult = 100)]
        [TestCase(0, 100, ExpectedResult = 0)]
        [TestCase(-1, 100, ExpectedResult = 0)]
        [TestCase(-10000, 100, ExpectedResult = 0)]
        public int SetManaTest(int value, int maxMana)
        {
            // Arrange
            var player = new Player();
            player.MaxMana = maxMana;

            // Act
            player.Mana = value;

            // Assert
            return player.Mana;
        }
    }
}
