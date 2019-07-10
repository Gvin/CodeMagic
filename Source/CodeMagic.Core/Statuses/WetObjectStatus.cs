using CodeMagic.Core.Area;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Objects;

namespace CodeMagic.Core.Statuses
{
    public class WetObjectStatus : IObjectStatus
    {
        private const int TimeToWearOut = 3;
        public const string StatusType = "wet";

        public const int SelfExtinguishChanceBonus = 30;
        public const int CatchFireChancePenalty = 10;

        private int lifeTime;

        public WetObjectStatus()
        {
            lifeTime = 0;
        }

        public bool Update(IDestroyableObject owner, AreaMapCell cell, Journal journal)
        {
            if (lifeTime >= TimeToWearOut)
            {
                return false;
            }

            lifeTime++;
            return true;
        }

        public string Type => StatusType;
    }
}