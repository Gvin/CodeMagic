using CodeMagic.Core.Area;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Objects;

namespace CodeMagic.Core.Statuses
{
    public class BlindObjectStatus : IObjectStatus
    {
        public const string StatusType = "blind";
        private const int MaxLifeTime = 4;
        public const double HitChanceMultiplier = 0.1d;

        private int lifeTime;

        public BlindObjectStatus()
        {
            lifeTime = 0;
        }

        public bool Update(IDestroyableObject owner, AreaMapCell cell, Journal journal)
        {
            if (lifeTime >= MaxLifeTime)
                return false;

            lifeTime++;
            return true;
        }

        public string Type => StatusType;
    }
}