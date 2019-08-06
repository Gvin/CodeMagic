using CodeMagic.Core.Items;

namespace CodeMagic.Core.Game.Journaling.Messages
{
    public class UsedItemMessage : IJournalMessage
    {
        public UsedItemMessage(IUsableItem item)
        {
            Item = item;
        }

        public IUsableItem Item { get; }
    }
}