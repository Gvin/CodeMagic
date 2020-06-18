using CodeMagic.Core.Items;

namespace CodeMagic.Game.JournalMessages
{
    public class CantAttackWithItemMessage : SelfDescribingJournalMessage
    {
        private readonly IItem item;

        public CantAttackWithItemMessage(IItem item)
        {
            this.item = item;
        }

        public override StyledLine GetDescription()
        {
            return new StyledLine
            {
                $"{PlayerName} can't attack with [",
                GetItemNameText(item),
                "]"
            };
        }
    }
}