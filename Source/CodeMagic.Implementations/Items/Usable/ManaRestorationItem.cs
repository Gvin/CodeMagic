using System.Drawing;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling.Messages;
using CodeMagic.Core.Items;

namespace CodeMagic.Implementations.Items.Usable
{
    public class ManaRestorationItem : SimpleItemImpl, IUsableItem, IDescriptionProvider
    {
        private readonly int restoreValue;
        private readonly string description;

        public ManaRestorationItem(ManaRestorationItemConfiguration configuration)
            : base(configuration)
        {
            restoreValue = configuration.ManaRestoreValue;
            description = configuration.Description;
        }

        public bool Use(IGameCore game)
        {
            game.Player.Mana += restoreValue;
            game.Journal.Write(new ManaRestoredMessage(game.Player, restoreValue));
            return false;
        }

        public override bool Stackable => true;

        public StyledString[][] GetDescription()
        {
            return new[]
            {
                new[]
                {
                    new StyledString("Restores "),
                    new StyledString(restoreValue.ToString(), Color.Blue),
                    new StyledString(" mana when used."),
                },
                new[]
                {
                    new StyledString(description)
                }
            };
        }
    }

    public class ManaRestorationItemConfiguration : SimpleItemConfiguration
    {
        public int ManaRestoreValue { get; set; }

        public string Description { get; set; }
    }
}