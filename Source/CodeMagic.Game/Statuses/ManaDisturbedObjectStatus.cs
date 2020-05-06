using CodeMagic.Core.Saving;
using CodeMagic.Core.Statuses;

namespace CodeMagic.Game.Statuses
{
    public class ManaDisturbedObjectStatus : PassiveObjectStatusBase
    {
        public const string StatusType = "mana_disturbed";
        private const int TimeToLive = 4;

        public ManaDisturbedObjectStatus(SaveData data) : base(data)
        {
        }

        public ManaDisturbedObjectStatus() 
            : base(TimeToLive)
        {
        }

        public override string Type => StatusType;
    }
}