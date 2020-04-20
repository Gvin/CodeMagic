using System.Collections.Generic;
using CodeMagic.Core.Game;
using CodeMagic.Core.Saving;
using CodeMagic.Game.JournalMessages;
using CodeMagic.Game.Objects.Creatures;
using CodeMagic.Game.Saving;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Items.Usable
{
    public class HealthManaRestorationItem : SimpleItemImpl, IUsableItem, IDescriptionProvider, IWorldImageProvider
    {
        private const string SaveKeyHealValue = "HealValue";
        private const string SaveKeyManaRestoreValue = "ManaRestore";
        private const string SaveKeyDescription = "Description";
        private const string SaveKeyWorldImage = "WorldImage";

        private readonly int healValue;
        private readonly int manaRestoreValue;
        private readonly SymbolsImage worldImage;
        private readonly string description;

        public HealthManaRestorationItem(SaveData data) : base(data)
        {
            healValue = data.GetIntValue(SaveKeyHealValue);
            manaRestoreValue = data.GetIntValue(SaveKeyManaRestoreValue);
            description = data.GetStringValue(SaveKeyDescription);
            worldImage = data.GetObject<SymbolsImageSaveable>(SaveKeyWorldImage)?.GetImage();
        }

        public HealthManaRestorationItem(HealthManaRestorationItemConfiguration configuration) : base(configuration)
        {
            healValue = configuration.HealValue;
            manaRestoreValue = configuration.ManaRestoreValue;
            worldImage = configuration.WorldImage;
            description = configuration.Description;
        }

        protected override Dictionary<string, object> GetSaveDataContent()
        {
            var data = base.GetSaveDataContent();
            data.Add(SaveKeyHealValue, healValue);
            data.Add(SaveKeyManaRestoreValue, manaRestoreValue);
            data.Add(SaveKeyDescription, description);
            data.Add(SaveKeyWorldImage, worldImage != null ? new SymbolsImageSaveable(worldImage) : null);
            return data;
        }

        public bool Use(CurrentGame.GameCore<Player> game)
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

        public StyledLine[] GetDescription(Player player)
        {
            var result = new List<StyledLine>
            {
                TextHelper.GetWeightLine(Weight),
                StyledLine.Empty
            };

            if (healValue > 0)
            {
                result.Add(new StyledLine
                {
                    "Heals ",
                    new StyledString(healValue.ToString(), TextHelper.HealthColor),
                    " health when used."
                });
            }

            if (manaRestoreValue > 0)
            {
                result.Add(new StyledLine
                {
                    "Restores ",
                    new StyledString(manaRestoreValue.ToString(), TextHelper.ManaColor),
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