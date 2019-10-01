using CodeMagic.Core.Items;

namespace CodeMagic.Implementations.Items.Materials
{
    public class BronzeIngot : IngotBase
    {
        public const string ResourceKey = "resource_ingot_bronze";

        public BronzeIngot() : base(MetalType.Bronze)
        {
        }

        public override string Name => "Bronze Ingot";

        public override string Key => ResourceKey;

        public override ItemRareness Rareness => ItemRareness.Common;

        public override int Weight => 7500;
    }
}