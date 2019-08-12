using System.Drawing;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling.Messages;
using CodeMagic.Core.Items;
using CodeMagic.UI.Images;

namespace CodeMagic.Implementations.Items.Usable
{
    public class HealthRestorationItem : SimpleItemImpl, IUsableItem, IDescriptionProvider, IWorldImageProvider
    {
        private readonly SymbolsImage worldImage;
        private readonly int healValue;
        private readonly string description;

        public HealthRestorationItem(HealthPotionItemConfiguration configuration) 
            : base(configuration)
        {
            worldImage = configuration.WorldImage;
            healValue = configuration.HealValue;
            description = configuration.Description;
        }

        public bool Use(IGameCore game)
        {
            game.Player.Health += healValue;
            game.Journal.Write(new HealedMessage(game.Player, healValue));
            return false;
        }

        public override bool Stackable => true;

        public StyledString[][] GetDescription()
        {
            return new[]
            {
                new[]
                {
                    new StyledString("Heals "),
                    new StyledString(healValue.ToString(), Color.Green),
                    new StyledString(" health when used."),
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

    public class HealthPotionItemConfiguration : SimpleItemConfiguration
    {
        public int HealValue { get; set; }

        public string Description { get; set; }

        public SymbolsImage WorldImage { get; set; }
    }
}