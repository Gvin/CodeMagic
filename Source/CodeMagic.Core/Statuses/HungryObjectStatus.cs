namespace CodeMagic.Core.Statuses
{
    public class HungryObjectStatus : PassiveObjectStatusBase
    {
        public const string StatusType = "hungry";

        public HungryObjectStatus() 
            : base(1)
        {
        }

        public override string Type => StatusType;
    }
}