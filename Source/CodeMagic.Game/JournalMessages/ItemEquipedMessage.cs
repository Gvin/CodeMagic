using CodeMagic.Core.Items;
using CodeMagic.Game.Items;

namespace CodeMagic.Game.JournalMessages
{
    public class ItemEquipedMessage : SelfDescribingJournalMessage
    {
        private readonly IEquipableItem item;

        public ItemEquipedMessage(IEquipableItem item)
        {
            this.item = item;
        }

        public override StyledLine GetDescription()
        {
            return new StyledLine
            {
                $"{PlayerName} equiped [",
                GetItemNameText(item),
                "]"
            };
        }
    }
}