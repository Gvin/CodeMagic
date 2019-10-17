using System;
using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Items;
using CodeMagic.Game.Items.Usable.Food;
using CodeMagic.Game.Items.Usable.Seeds;
using CodeMagic.Game.JournalMessages;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Objects.Buildings.Plants
{
    public class WheatPlant : PlantBase
    {
        private const int GrowingPeriod = 2000;
        private const int HalfReadyPeriod = 15000;
        private const int ReadyPeriod = 20000;
        private const int OvergrownPeriod = 25000;
        private const int MaxGrowthPeriod = 30000;

        public WheatPlant() 
            : base(20)
        {
            BaseProtection.Add(Element.Fire, -100);
            BaseProtection.Add(Element.Frost, -50);
            BaseProtection.Add(Element.Electricity, 100);
            BaseProtection.Add(Element.Magic, 100);
        }

        public override string Name => "Wheat";
        protected override double HumidityConsumption => 0.03d;
        protected override double MaxHumidity => 30;
        protected override double MinHumidity => 5;
        protected override double MinTemperature => 20;
        protected override double MaxTemperature => 60;
        protected override double CatchFireChanceMultiplier => 10;
        protected override double SelfExtinguishChance => 5;

        public override SymbolsImage GetWorldImage(IImagesStorage storage)
        {
            var imageName = GetImageName();
            return storage.GetImage(imageName);
        }

        public override void Update(IAreaMap map, IJournal journal, Point position)
        {
            base.Update(map, journal, position);

            if (GrowthPeriod >= MaxGrowthPeriod)
            {
                map.RemoveObject(position, this);
            }
        }

        public override void Use(IGameCore game, Point position)
        {
            if (GrowthPeriod >= OvergrownPeriod)
            {
                var foodCount = RandomHelper.GetRandomValue(1, 3);
                GiveFood(game, foodCount);
                game.Map.RemoveObject(position, this);
                return;
            }

            if (GrowthPeriod >= ReadyPeriod)
            {
                var foodCount = RandomHelper.GetRandomValue(3, 6);
                GiveFood(game, foodCount);
                var seedsCount = RandomHelper.GetRandomValue(0, 3);
                GiveItems(game.Player.Inventory, seedsCount, () => new WheatSeeds());
                game.Map.RemoveObject(position, this);
                return;
            }

            if (GrowthPeriod >= HalfReadyPeriod)
            {
                var foodCount = RandomHelper.GetRandomValue(1, 3);
                GiveFood(game, foodCount);
                game.Map.RemoveObject(position, this);
                return;
            }

            game.Journal.Write(new NotReadyForHarvestMessage(Name));
        }

        public override void OnDeath(IAreaMap map, IJournal journal, Point position)
        {
            base.OnDeath(map, journal, position);

            if (GrowthPeriod >= HalfReadyPeriod)
            {
                var foodCount = RandomHelper.GetRandomValue(1, 3);
                for (var counter = 0; counter < foodCount; counter++)
                {
                    map.AddObject(position, new Wheat());
                }
            }
        }

        private void GiveFood(IGameCore game, int foodCount)
        {
            GiveItems(game.Player.Inventory, foodCount, () => new Wheat());
        }

        private void GiveItems(Inventory inventory, int count, Func<IItem> itemFactory)
        {
            for (var counter = 0; counter < count; counter++)
            {
                inventory.AddItem(itemFactory());
            }
        }

        private string GetImageName()
        {
            if (GrowthPeriod >= OvergrownPeriod)
                return "Plant_Wheat_Overgrown";
            if (GrowthPeriod >= ReadyPeriod)
                return "Plant_Wheat_Ready";
            if (GrowthPeriod >= HalfReadyPeriod)
                return "Plant_Wheat_HalfReady";
            if (GrowthPeriod >= GrowingPeriod)
                return "Plant_Wheat_Growing";

            return "Plant_Wheat_Seedling";
        }
    }
}