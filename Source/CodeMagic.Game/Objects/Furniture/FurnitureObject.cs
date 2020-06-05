using System.Collections.Generic;
using CodeMagic.Core.Game;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Saving;
using CodeMagic.Game.Items.Materials;
using CodeMagic.Game.Saving;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Objects.Furniture
{
    public class FurnitureObject : DestroyableObject, IWorldImageProvider
    {
        private const string SaveKeyWorldImage = "WorldImage";
        private const string SaveKeyZIndex = "ZIndex";
        private const string SaveKeySize = "Size";
        private const string SaveKeyMaxWoodCount = "MaxWoodCount";
        private const string SaveKeyMinWoodCount = "MinWoodCount";
        private const string SaveKeyBlocksMovement = "BlocksMovement";

        private readonly string worldImage;
        private readonly int maxWoodCount;
        private readonly int minWoodCount;

        public FurnitureObject(SaveData data) 
            : base(data)
        {
            worldImage = data.GetStringValue(SaveKeyWorldImage);
            ZIndex = (ZIndex) data.GetIntValue(SaveKeyZIndex);
            Size = (ObjectSize) data.GetIntValue(SaveKeySize);
            maxWoodCount = data.GetIntValue(SaveKeyMaxWoodCount);
            minWoodCount = data.GetIntValue(SaveKeyMinWoodCount);
            BlocksMovement = data.GetBoolValue(SaveKeyBlocksMovement);
        }

        public FurnitureObject(FurnitureObjectConfiguration configuration) 
            : base(configuration.Name, configuration.MaxHealth)
        {
            worldImage = configuration.WorldImage;
            ZIndex = configuration.ZIndex;
            Size = configuration.Size;
            MaxHealth = configuration.MaxHealth;
            maxWoodCount = configuration.MaxWoodCount;
            minWoodCount = configuration.MinWoodCount;
            BlocksMovement = configuration.BlocksMovement;
        }

        protected override Dictionary<string, object> GetSaveDataContent()
        {
            var data = base.GetSaveDataContent();
            data.Add(SaveKeyWorldImage, worldImage);
            data.Add(SaveKeyZIndex, (int)ZIndex);
            data.Add(SaveKeySize, (int)Size);
            data.Add(SaveKeyMaxWoodCount, maxWoodCount);
            data.Add(SaveKeyMinWoodCount, minWoodCount);
            data.Add(SaveKeyBlocksMovement, BlocksMovement);
            return data;
        }

        public override ZIndex ZIndex { get; }

        public override ObjectSize Size { get; }

        public override int MaxHealth { get; }

        public override bool BlocksMovement { get; }

        public SymbolsImage GetWorldImage(IImagesStorage storage)
        {
            return storage.GetImage(worldImage);
        }

        public override void OnDeath(Point position)
        {
            base.OnDeath(position);

            var woodCount = RandomHelper.GetRandomValue(minWoodCount, maxWoodCount);
            for (var counter = 0; counter < woodCount; counter++)
            {
                CurrentGame.Map.AddObject(position, new Wood());
            }
        }
    }

    public class FurnitureObjectConfiguration
    {
        public FurnitureObjectConfiguration()
        {
            ZIndex = ZIndex.BigDecoration;
            Size = ObjectSize.Big;
            MaxHealth = 1;
            BlocksMovement = false;
        }

        public string WorldImage { get; set; }

        public string Name { get; set; }

        public ZIndex ZIndex { get; set; }

        public ObjectSize Size { get; set; }

        public int MaxHealth { get; set; }

        public int MaxWoodCount { get; set; }

        public int MinWoodCount { get; set; }

        public bool BlocksMovement { get; set; }
    }
}