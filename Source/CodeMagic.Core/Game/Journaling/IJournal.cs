using CodeMagic.Core.Objects;

namespace CodeMagic.Core.Game.Journaling
{
    public interface IJournal
    {
        void Write(IJournalMessage message);

        void Write(IJournalMessage message, IMapObject source);
    }
}