namespace CodeMagic.Game.JournalMessages
{
    public class LevelUpMessage : SelfDescribingJournalMessage
    {
        private readonly int level;

        public LevelUpMessage(int level)
        {
            this.level = level;
        }

        public override StyledLine GetDescription()
        {
            return new StyledLine
            {
                $"You reached level {level}"
            };
        }
    }
}