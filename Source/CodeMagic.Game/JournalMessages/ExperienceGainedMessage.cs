using CodeMagic.Game.Items;

namespace CodeMagic.Game.JournalMessages
{
    public class ExperienceGainedMessage : SelfDescribingJournalMessage
    {
        private readonly int experience;

        public ExperienceGainedMessage(int experience)
        {
            this.experience = experience;
        }

        public override StyledLine GetDescription()
        {
            return new StyledLine
            {
                "You got ", new StyledString(experience, TextHelper.XpColor), " XP"
            };
        }
    }
}