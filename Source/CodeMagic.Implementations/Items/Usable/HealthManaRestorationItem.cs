using System.Collections.Generic;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling.Messages;
using CodeMagic.Core.Items;
using CodeMagic.Core.Objects.PlayerData;
using CodeMagic.UI.Images;

namespace CodeMagic.Implementations.Items.Usable
{
    public class HealthManaRestorationItem : SimpleItemImpl, IUsableItem, IDescriptionProvider, IWorldImageProvider
    {
        private readonly int healValue;
        private readonly int manaRestoreValue;
        private readonly SymbolsImage worldImage;
        private readonly string description;

        public HealthManaRestorationItem(HealthManaRestorationItemConfiguration configuration) : base(configuration)
        {
            healValue = configuration.HealValue;
            manaRestoreValue = configuration.ManaRestoreValue;
            worldImage = configuration.WorldImage;
            description = configuration.Description;
        }

        public bool Use(IGameCore game)
        {
            if (healValue > 0)
            {
                game.Player.Health += healValue;
                game.Journal.Write(new HealedMessage(game.Player, healValue));
            }

            if (manaRestoreValue > 0)
            {
                game.Player.Mana += manaRestoreValue;
                game.Journal.Write(new ManaRestoredMessage(game.Player, manaRestoreValue));
            }

            return false;
        }

        public StyledLine[] GetDescription(IPlayer player)
        {
            var result = new List<StyledLine>
            {
                ItemTextHelper.GetWeightLine(Weight),
                StyledLine.Empty
            };

            if (healValue > 0)
            {
                result.Add(new StyledLine
                {
                    "Heals ",
                    new StyledString(healValue.ToString(), ItemTextHelper.HealthColor),
                    " health when used."
                });
            }

            if (manaRestoreValue > 0)
            {
                result.Add(new StyledLine
                {
                    "Restores ",
                    new StyledString(manaRestoreValue.ToString(), ItemTextHelper.ManaColor),
                    " mana when used."
                });
            }

            result.Add(StyledLine.Empty);
            result.Add(new StyledLine { description });

            return result.ToArray();
        }

        public SymbolsImage GetWorldImage(IImagesStorage storage)
        {
            return worldImage;
        }
    }

    public class HealthManaRestorationItemConfiguration : SimpleItemConfiguration
    {
        public int HealValue { get; set; }

        public int ManaRestoreValue { get; set; }

        public string Description { get; set; }

        public SymbolsImage WorldImage { get; set; }
    }
}