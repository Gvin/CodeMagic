using CodeMagic.Core.Items;
using CodeMagic.Game.Items.ItemsGeneration.Configuration.Tool;

namespace CodeMagic.Game.Items.ItemsGeneration.Implementations.Tool
{
    public class LumberjackAxeGenerator : ToolsGenerator
    {
        public LumberjackAxeGenerator(IToolConfiguration configuration, IImagesStorage imagesStorage) 
            : base(configuration, imagesStorage, "Lumberjack Axe", "ItemsOnGround_Weapon_Axe")
        {
        }

        protected override WeaponItemConfiguration GetConfiguration(int toolPower)
        {
            return new LumberjackAxeConfiguration
            {
                LumberjackPower = toolPower
            };
        }

        protected override IItem CreateItem(WeaponItemConfiguration config)
        {
            var realConfig = (LumberjackAxeConfiguration) config;
            return new LumberjackAxe(realConfig);
        }

        protected override string[] GenerateDescription()
        {
            return new[]
            {
                "A lumberjack axe which can be used to chop trees.",
                "It can also be used in combat."
            };
        }
    }
}