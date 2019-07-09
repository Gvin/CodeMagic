using CodeMagic.Core.Objects;

namespace CodeMagic.Core.Game.Journaling.Messages
{
    public class DeathMessage : IJournalMessage
    {
        public DeathMessage(IMapObject @object)
        {
            Object = @object;
        }

        public IMapObject Object { get; }
    }
}