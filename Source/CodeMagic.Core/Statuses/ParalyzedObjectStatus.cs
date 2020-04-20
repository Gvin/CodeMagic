using CodeMagic.Core.Saving;

namespace CodeMagic.Core.Statuses
{
    public class ParalyzedObjectStatus : PassiveObjectStatusBase
    {
        public const string StatusType = "paralyzed";
        private const int MaxLifeTime = 4;

        public ParalyzedObjectStatus(SaveData data) 
            : base(data, MaxLifeTime)
        {
        }

        public ParalyzedObjectStatus():
            base(MaxLifeTime)
        {
        }

        public override string Type => StatusType;
    }
}