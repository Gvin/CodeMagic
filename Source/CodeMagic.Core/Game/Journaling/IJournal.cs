using CodeMagic.Core.Saving;

namespace CodeMagic.Core.Game.Journaling
{
    public interface IJournal
    {
        void Write(IJournalMessage message);
    }
}