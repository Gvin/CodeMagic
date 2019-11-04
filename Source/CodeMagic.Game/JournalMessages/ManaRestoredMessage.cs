using CodeMagic.Core.Objects;

namespace CodeMagic.Game.JournalMessages
{
    public class ManaRestoredMessage : SelfDescribingJournalMessage
    {
        private readonly IMapObject target;
        private readonly int manaRestoreValue;

        public ManaRestoredMessage(IMapObject target, int manaRestoreValue)
        {
            this.target = target;
            this.manaRestoreValue = manaRestoreValue;
        }

        public override StyledLine GetDescription()
        {
            return new StyledLine
            {
                $"{GetMapObjectName(target)} restored ",
                new StyledString(manaRestoreValue, ManaColor),
                " mana"
            };
        }
    }
}