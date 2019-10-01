using CodeMagic.Core.Items;

namespace CodeMagic.Implementations.Items.Materials
{
    public sealed class SilverIngot : IngotBase
    {
        public const string ResourceKey = "resource_ingot_silver";

        public SilverIngot() : base(MetalType.Silver)
        {
        }

        public override string Name => "Silver Ingot";

        public override string Key => ResourceKey;

        public override ItemRareness Rareness => ItemRareness.Uncommon;

        public override int Weight => 10500;
    }
}