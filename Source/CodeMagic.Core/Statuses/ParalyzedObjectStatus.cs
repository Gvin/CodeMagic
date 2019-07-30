using CodeMagic.Core.Area;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Objects;

namespace CodeMagic.Core.Statuses
{
    public class ParalyzedObjectStatus : IObjectStatus
    {
        public const string StatusType = "paralyzed";
        private const int MaxLifeTime = 4;

        private int lifeTime;

        public ParalyzedObjectStatus()
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