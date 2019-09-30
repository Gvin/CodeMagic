using CodeMagic.Core.Items;

namespace CodeMagic.Core.Game.Journaling.Messages
{
    public class ItemReceivedMessage : IJournalMessage
    {
        public ItemReceivedMessage(IItem item)
        {
            Item = item;
        }

        public IItem Item { get; }
    }
}