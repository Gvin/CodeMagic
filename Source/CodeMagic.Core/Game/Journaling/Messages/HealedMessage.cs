using CodeMagic.Core.Objects;

namespace CodeMagic.Core.Game.Journaling.Messages
{
    public class HealedMessage : IJournalMessage
    {
        public HealedMessage(IMapObject target, int healValue)
        {
            Target = target;
            HealValue = healValue;
        }

        public IMapObject Target { get; }

        public int HealValue { get; }
    }
}