using CodeMagic.Core.Objects;

namespace CodeMagic.Core.Game.Journaling.Messages
{
    public class DamageBlockedMessage : IJournalMessage
    {
        public DamageBlockedMessage(IMapObject target, int blockedValue, Element damageElement)
        {
            Target = target;
            BlockedValue = blockedValue;
            DamageElement = damageElement;
        }

        public IMapObject Target { get; }

        public int BlockedValue { get; }

        public Element DamageElement { get; }
    }
}