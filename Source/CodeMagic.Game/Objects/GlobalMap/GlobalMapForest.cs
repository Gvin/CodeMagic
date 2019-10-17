using System;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling.Messages;
using CodeMagic.Core.Objects;
using CodeMagic.Game.Items;
using CodeMagic.Game.Items.Materials;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Objects.GlobalMap
{
    public class GlobalMapForest : IMapObject, IWorldImageProvider, IUsableObject
    {
        private const int MinWoodCount = 10;
        private const int MaxWoodCount = 20;

        private static readonly string[] ImagesForest = {
            "GlobalMap_Forest1",
            "GlobalMap_Forest2",
            "GlobalMap_Forest3",
            "GlobalMap_Forest4",
            "GlobalMap_Forest5",
            "GlobalMap_Forest6",
            "GlobalMap_Forest7",
            "GlobalMap_Forest8",
            "GlobalMap_Forest9",
        };

        private readonly int initialTrees;
        private int remainingTrees;

        public GlobalMapForest()
        {
            initialTrees = RandomHelper.GetRandomValue(20, 50);
            remainingTrees = initialTrees;
        }

        public string Name => "Forest";

        public bool BlocksMovement => false;

        public bool BlocksProjectiles => false;

        public bool IsVisible => true;

        public bool BlocksVisibility => false;

        public bool BlocksAttack => false;

        public bool BlocksEnvironment => false;

        public ZIndex ZIndex => ZIndex.AreaDecoration;

        public bool Equals(IMapObject other)
        {
            return ReferenceEquals(this, other);
        }

        public ObjectSize Size => ObjectSize.Huge;

        public SymbolsImage GetWorldImage(IImagesStorage storage)
        {
            var remainingPercent = remainingTrees / (double) initialTrees;
            var imageTreesCount = (int) Math.Round(9 * remainingPercent);
            imageTreesCount = Math.Max(1, imageTreesCount);
            var imageIndex = imageTreesCount - 1;

            return storage.GetImage(ImagesForest[imageIndex]);
        }

        public void Use(IGameCore game, Point position)
        {
            if (!(game.Player.Equipment.Weapon is LumberjackAxe axe))
            {
                game.Journal.Write(new ToolRequiredMessage());
                return;
            }

            var maxWoodCount = RandomHelper.GetRandomValue(MinWoodCount, MaxWoodCount);
            var woodCount = (int) Math.Round(maxWoodCount * axe.LumberjackPower / 100d);
            for (var counter = 0; counter < woodCount; counter++)
            {
                game.Player.Inventory.AddItem(new Wood());
            }

            remainingTrees--;
            if (remainingTrees == 0)
            {
                game.Map.RemoveObject(position, this);
            }
        }
    }
}