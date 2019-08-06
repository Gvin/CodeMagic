using CodeMagic.Core.Objects;

namespace CodeMagic.Core.Game.Journaling.Messages
{
    public class ManaRestoredMessage : IJournalMessage
    {
        public ManaRestoredMessage(IMapObject target, int manaRestoreValue)
        {
            Target = target;
            ManaRestoreValue = manaRestoreValue;
        }

        public IMapObject Target { get; }

        public int ManaRestoreValue { get; }
    }
}