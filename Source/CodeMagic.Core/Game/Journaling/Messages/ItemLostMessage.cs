using CodeMagic.Core.Items;

namespace CodeMagic.Core.Game.Journaling.Messages
{
    public class ItemLostMessage : IJournalMessage
    {
        public ItemLostMessage(IItem item)
        {
            Item = item;
        }

        public IItem Item { get; }
    }
}