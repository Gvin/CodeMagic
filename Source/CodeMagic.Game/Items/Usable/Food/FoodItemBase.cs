using System.Collections.Generic;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Core.Saving;
using CodeMagic.Game.JournalMessages;
using CodeMagic.Game.Objects.Creatures;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Items.Usable.Food
{
    public abstract class FoodItemBase : Item, IUsableItem, IWorldImageProvider, IInventoryImageProvider, IDescriptionProvider
    {
        private const string SaveKeyHungerDecrease = "HungerDecrease";

        private const int MinHungerToEat = 10;

        private readonly int hungerDecrease;

        protected FoodItemBase(SaveData data)
            : base(data)
        {
            hungerDecrease = data.GetIntValue(SaveKeyHungerDecrease);
        }

        protected FoodItemBase(int hungerDecrease, ItemConfiguration configuration)
            : base(configuration)
        {
            this.hungerDecrease = hungerDecrease;
        }

        protected override Dictionary<string, object> GetSaveDataContent()
        {
            var data = base.GetSaveDataContent();
            data.Add(SaveKeyHungerDecrease, hungerDecrease);
            return data;
        }

        public bool Use(CurrentGame.GameCore<Player> game)
        {
            if (game.Player.HungerPercent < MinHungerToEat)
            {
                game.Journal.Write(new NotHungryMessage());
                return true;
            }

            EatFood(game);
            return false;
        }

        protected virtual void EatFood(IGameCore game)
        {
            game.Player.HungerPercent -= hungerDecrease;
            game.Journal.Write(new HungerDecreasedMessage(hungerDecrease));
        }

        public override bool Stackable => true;

        public abstract SymbolsImage GetWorldImage(IImagesStorage storage);

        public abstract SymbolsImage GetInventoryImage(IImagesStorage storage);

        public StyledLine[] GetDescription(Player player)
        {
            var result = new List<StyledLine>
            {
                TextHelper.GetWeightLine(Weight),
                StyledLine.Empty,
                new StyledLine {"Hunger Decrease: ", TextHelper.GetValueString(hungerDecrease, "%", false)},
                StyledLine.Empty
            };
            result.AddRange(GetDescriptionText());
            return result.ToArray();
        }

        protected abstract StyledLine[] GetDescriptionText();
    }
}