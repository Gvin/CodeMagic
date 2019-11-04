using CodeMagic.Core.Items;

namespace CodeMagic.Game.JournalMessages
{
    public class ItemLostMessage : SelfDescribingJournalMessage
    {
        private readonly IItem item;

        public ItemLostMessage(IItem item)
        {
            this.item = item;
        }

        public override StyledLine GetDescription()
        {
            return new StyledLine
            {
                $"{PlayerName} lost [",
                GetItemNameText(item),
                "]"
            };
        }
    }
}