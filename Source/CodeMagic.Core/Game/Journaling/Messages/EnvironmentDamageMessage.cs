using CodeMagic.Core.Objects;

namespace CodeMagic.Core.Game.Journaling.Messages
{
    public class EnvironmentDamageMessage : IJournalMessage
    {
        public EnvironmentDamageMessage(IMapObject @object, int damage, Element? element)
        {
            Object = @object;
            Damage = damage;
            Element = element;
        }

        public IMapObject Object { get; }

        public int Damage { get; }

        public Element? Element { get; }
    }
}