using System.Collections.Generic;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling.Messages;
using CodeMagic.Core.Items;
using CodeMagic.Core.Objects.PlayerData;
using CodeMagic.UI.Images;

namespace CodeMagic.Implementations.Items.Usable.Food
{
    public abstract class FoodItemBase : ItemBase, IUsableItem, IWorldImageProvider, IInventoryImageProvider, IDescriptionProvider
    {
        private const int MinHungerToEat = 10;

        private readonly int hungerDecrease;

        protected FoodItemBase(int hungerDecrease)
        {
            this.hungerDecrease = hungerDecrease;
        }

        public bool Use(IGameCore game)
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

        public StyledLine[] GetDescription(IPlayer player)
        {
            var result = new List<StyledLine>
            {
                ItemTextHelper.GetWeightLine(Weight),
                StyledLine.Empty,
                new StyledLine {"Hunger Decrease: ", ItemTextHelper.GetValueString(hungerDecrease, "%", false)},
                StyledLine.Empty
            };
            result.AddRange(GetDescriptionText());
            return result.ToArray();
        }

        protected abstract StyledLine[] GetDescriptionText();
    }
}