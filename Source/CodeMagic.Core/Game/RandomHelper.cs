using System;

namespace CodeMagic.Core.Game
{
    public class RandomHelper
    {
        private static RandomHelper Current { get; } = new RandomHelper();

        private readonly Random random;

        private RandomHelper()
        {
            var seedRandom = new Random();
            random = new Random(seedRandom.Next());
        }

        public static Random GetRandomGenerator()
        {
            return Current.random;
        }

        public static int GetRandomValue(int min, int max)
        {
            return Current.random.Next(min, max + 1);
        }

        public static bool CheckChance(int chancePercent)
        {
            if (chancePercent <= 0)
                return false;
            if (chancePercent >= 100)
                return true;

            var value = GetRandomValue(0, 100);
            return value <= chancePercent;
        }

        public static T GetRandomElement<T>(T[] array)
        {
            if (array.Length == 0)
                throw new ArgumentException("Unable to get random element for empty array.");
            return array[GetRandomValue(0, array.Length - 1)];
        }
    }
}