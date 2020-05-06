using CodeMagic.Core.Saving;
using CodeMagic.Core.Statuses;

namespace CodeMagic.Game.Statuses
{
    public class HungryObjectStatus : PassiveObjectStatusBase
    {
        public const string StatusType = "hungry";

        public HungryObjectStatus(SaveData data) : base(data)
        {
        }

        public HungryObjectStatus() 
            : base(1)
        {
        }

        public override string Type => StatusType;
    }
}