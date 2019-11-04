namespace CodeMagic.Game.JournalMessages
{
    public class DungeonLevelMessage : SelfDescribingJournalMessage
    {
        private readonly int level;

        public DungeonLevelMessage(int level)
        {
            this.level = level;
        }

        public override StyledLine GetDescription()
        {
            return new StyledLine {$"{PlayerName} reached {level} level of the dungeon"};
        }
    }
}