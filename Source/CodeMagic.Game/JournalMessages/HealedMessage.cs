using CodeMagic.Core.Objects;

namespace CodeMagic.Game.JournalMessages
{
    public class HealedMessage : SelfDescribingJournalMessage
    {
        private readonly IMapObject target;
        private readonly int healValue;

        public HealedMessage(IMapObject target, int healValue)
        {
            this.target = target;
            this.healValue = healValue;
        }

        public override StyledLine GetDescription()
        {
            return new StyledLine
            {
                $"{GetMapObjectName(target)} restored ",
                new StyledString(healValue, HealValueColor),
                " health"
            };
        }
    }
}