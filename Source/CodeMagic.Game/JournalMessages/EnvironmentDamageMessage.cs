using CodeMagic.Core.Game;
using CodeMagic.Core.Objects;

namespace CodeMagic.Game.JournalMessages
{
    public class EnvironmentDamageMessage : SelfDescribingJournalMessage
    {
        private readonly IMapObject obj;
        private readonly int damage;
        private readonly Element element;

        public EnvironmentDamageMessage(IMapObject @object, int damage, Element element)
        {
            obj = @object;
            this.damage = damage;
            this.element = element;
        }

        public override StyledLine GetDescription()
        {
            return new StyledLine
            {
                $"{GetMapObjectName(obj)} got ",
                GetDamageText(damage, element),
                " from environment"
            };
        }
    }
}