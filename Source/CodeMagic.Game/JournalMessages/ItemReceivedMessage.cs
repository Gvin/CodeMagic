using CodeMagic.Core.Items;

namespace CodeMagic.Game.JournalMessages
{
    public class ItemReceivedMessage : SelfDescribingJournalMessage
    {
        private readonly IItem item;

        public ItemReceivedMessage(IItem item)
        {
            this.item = item;
        }

        public override StyledLine GetDescription()
        {
            return new StyledLine
            {
                $"{PlayerName} got [",
                GetItemNameText(item),
                "]"
            };
        }
    }
}