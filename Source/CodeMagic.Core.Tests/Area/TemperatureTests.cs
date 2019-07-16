using CodeMagic.Core.Area.EnvironmentData;
using CodeMagic.Core.Game;
using NUnit.Framework;

namespace CodeMagic.Core.Tests.Area
{
    [TestFixture]
    public class TemperatureTests
    {
        [TestCase(0, 0, null)]
        [TestCase(-70, 1, Element.Frost)]
        [TestCase(-100, 5, Element.Frost)]
        [TestCase(-200, 20, Element.Frost)]
        [TestCase(100, 1, Element.Fire)]
        [TestCase(600, 5, Element.Fire)]
        [TestCase(1200, 15, Element.Fire)]
        [TestCase(1500, 30, Element.Fire)]
        public void GetTemperatureDamageTest(int temperature, int damage, Element? damageElement)
        {
            var temperatureObject = new Temperature(temperature);
            var realDamage = temperatureObject.GetTemperatureDamage(out var readDamageElement);
            Assert.AreEqual(damage, realDamage);
            Assert.AreEqual(damageElement, readDamageElement);
        }
    }
}