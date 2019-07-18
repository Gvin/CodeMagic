using CodeMagic.Core.Area;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Objects;

namespace CodeMagic.Core.Statuses
{
    public class WetObjectStatus : IObjectStatus
    {
        public const string StatusType = "wet";

        public const int SelfExtinguishChanceBonus = 30;
        public const int CatchFireChancePenalty = 10;

        private readonly int maxLifeTime;
        private int lifeTime;

        public WetObjectStatus(int maxLifeTime)
        {
            this.maxLifeTime = maxLifeTime;
            lifeTime = 0;
        }

        public bool Update(IDestroyableObject owner, AreaMapCell cell, Journal journal)
        {
            if (lifeTime >= maxLifeTime)
            {
                return false;
            }

            lifeTime++;
            return true;
        }

        public string Type => StatusType;
    }
}