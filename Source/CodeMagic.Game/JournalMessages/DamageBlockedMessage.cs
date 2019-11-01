using CodeMagic.Core.Game;
using CodeMagic.Core.Objects;

namespace CodeMagic.Game.JournalMessages
{
    public class DamageBlockedMessage : SelfDescribingJournalMessage
    {
        private readonly IMapObject target;
        private readonly int blockedValue;
        private readonly Element damageElement;

        public DamageBlockedMessage(IMapObject target, int blockedValue, Element damageElement)
        {
            this.target = target;
            this.blockedValue = blockedValue;
            this.damageElement = damageElement;
        }

        public override StyledLine GetDescription()
        {
            return new StyledLine
            {
                $"{GetMapObjectName(target)} blocked ",
                GetDamageText(blockedValue, damageElement)
            };
        }
    }
}