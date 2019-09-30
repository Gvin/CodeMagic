using CodeMagic.Core.Items;
using CodeMagic.Implementations;
using CodeMagic.Implementations.Items;
using CodeMagic.ItemsGeneration.Configuration.Tool;

namespace CodeMagic.ItemsGeneration.Implementations.Tool
{
    public class LumberjackAxeGenerator : ToolsGenerator
    {
        public LumberjackAxeGenerator(IToolConfiguration configuration, IImagesStorage imagesStorage) 
            : base(configuration, imagesStorage, "Lumberjack Axe", "ItemsOnGround_Weapon_Axe")
        {
        }

        protected override WeaponItemImplConfiguration GetConfiguration(int toolPower)
        {
            return new LumberjackAxeConfiguration
            {
                LumberjackPower = toolPower
            };
        }

        protected override IItem CreateItem(WeaponItemImplConfiguration config)
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