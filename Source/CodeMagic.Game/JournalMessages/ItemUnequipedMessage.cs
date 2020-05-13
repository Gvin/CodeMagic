using CodeMagic.Game.Items;

namespace CodeMagic.Game.JournalMessages
{
    public class ItemUnequipedMessage : SelfDescribingJournalMessage
    {
        private readonly IEquipableItem item;

        public ItemUnequipedMessage(IEquipableItem item)
        {
            this.item = item;
        }

        public override StyledLine GetDescription()
        {
            return new StyledLine
            {
                $"{PlayerName} took off [",
                GetItemNameText(item),
                "]"
            };
        }
    }
}