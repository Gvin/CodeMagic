using CodeMagic.Core.Saving;
using CodeMagic.Core.Statuses;

namespace CodeMagic.Game.Statuses
{
    public class OverweightObjectStatus : PassiveObjectStatusBase
    {
        public const string StatusType = "overweight";

        public OverweightObjectStatus() 
            : base(1)
        {
        }

        public OverweightObjectStatus(SaveData data) : base(data)
        {
        }

        public override string Type => StatusType;
    }
}