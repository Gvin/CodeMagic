using CodeMagic.Core.Game;
using CodeMagic.Core.Objects;

namespace CodeMagic.Game.JournalMessages
{
    public class BurningDamageMessage : SelfDescribingJournalMessage
    {
        private readonly IDestroyableObject obj;
        private readonly int damage;

        public BurningDamageMessage(IDestroyableObject obj, int damage)
        {
            this.obj = obj;
            this.damage = damage;
        }

        public override StyledLine GetDescription()
        {
            return new StyledLine
            {
                $"{GetMapObjectName(obj)} got ",
                GetDamageText(damage, Element.Fire),
                " from burning"
            };
        }
    }
}