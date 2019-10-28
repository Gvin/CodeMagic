using CodeMagic.Core.Items;

namespace CodeMagic.Game.JournalMessages
{
    public class UsedItemMessage : SelfDescribingJournalMessage
    {
        public UsedItemMessage(IUsableItem item)
        {
            Item = item;
        }

        public IUsableItem Item { get; }

        public override StyledLine GetDescription()
        {
            return new StyledLine
            {
                $"{PlayerName} used [",
                GetItemNameText(Item),
                "]"
            };
        }
    }
}