using CodeMagic.Core.Items;
using CodeMagic.Game.Items.ItemsGeneration.Configuration.Tool;

namespace CodeMagic.Game.Items.ItemsGeneration.Implementations.Tool
{
    public class PickaxeGenerator : ToolsGenerator
    {
        public PickaxeGenerator(IToolConfiguration configuration, IImagesStorage imagesStorage) 
            : base(configuration, imagesStorage, "Pickaxe", "ItemsOnGround_Weapon_Mace")
        {
        }

        protected override WeaponItemImplConfiguration GetConfiguration(int toolPower)
        {
            return new PickaxeConfiguration {PickaxePower = toolPower};
        }

        protected override IItem CreateItem(WeaponItemImplConfiguration config)
        {
            var realConfig = (PickaxeConfiguration) config;
            return new Pickaxe(realConfig);
        }

        protected override string[] GenerateDescription()
        {
            return new[]
            {
                "A pickaxe which can be used to mine stone and ores.",
                "It can also be used in combat."
            };
        }
    }
}