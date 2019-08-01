namespace CodeMagic.Core.Statuses
{
    public class FrozenObjectStatus : PassiveObjectStatusBase
    {
        public const string StatusType = "frozen";
        private const int MaxLifeTime = 4;
        public const float SpeedMultiplier = 2f;

        public FrozenObjectStatus() 
            : base(MaxLifeTime)
        {
        }

        public override string Type => StatusType;
    }
}