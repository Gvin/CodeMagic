using System;

namespace CodeMagic.Core.Game
{
    public class RandomHelper
    {
        private static RandomHelper Current { get; } = new RandomHelper();

        private readonly Random random;

        private RandomHelper()
        {
            random = new Random();
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
    }
}