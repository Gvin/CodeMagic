namespace CodeMagic.Core.Game.Journaling.Messages
{
    public class DungeonLevelMessage : IJournalMessage
    {
        public DungeonLevelMessage(int level)
        {
            Level = level;
        }

        public int Level { get; }
    }
}