using System.Drawing;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling.Messages;
using CodeMagic.Core.Items;
using CodeMagic.UI.Images;

namespace CodeMagic.Implementations.Items.Usable
{
    public class ManaRestorationItem : SimpleItemImpl, IUsableItem, IDescriptionProvider, IWorldImageProvider
    {
        private readonly SymbolsImage worldImage;
        private readonly int restoreValue;
        private readonly string description;

        public ManaRestorationItem(ManaRestorationItemConfiguration configuration)
            : base(configuration)
        {
            worldImage = configuration.WorldImage;
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

        public SymbolsImage GetWorldImage(IImagesStorage storage)
        {
            return worldImage;
        }
    }

    public class ManaRestorationItemConfiguration : SimpleItemConfiguration
    {
        public int ManaRestoreValue { get; set; }

        public string Description { get; set; }

        public SymbolsImage WorldImage { get; set; }
    }
}