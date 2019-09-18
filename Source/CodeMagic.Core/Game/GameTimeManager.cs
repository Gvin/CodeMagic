using System;

namespace CodeMagic.Core.Game
{
    public class GameTimeManager
    {
        private static readonly TimeSpan TimePerTurn = TimeSpan.FromMinutes(3);

        public GameTimeManager()
        {
            CurrentTime = new DateTime(3581, 6, 25, 12, 00, 00);
        }

        public DateTime CurrentTime { get; private set; }

        public void RegisterTurn(int turnsCount)
        {
            for (int counter = 0; counter < turnsCount; counter++)
            {
                CurrentTime += TimePerTurn;
            }
        }
    }
}