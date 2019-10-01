using CodeMagic.Core.Items;

namespace CodeMagic.Implementations.Items.Materials
{
    public sealed class IronIngot : IngotBase
    {
        public const string ResourceKey = "resource_ingot_iron";

        public IronIngot() : base(MetalType.Iron)
        {
        }

        public override string Name => "Iron Ingot";

        public override string Key => ResourceKey;

        public override ItemRareness Rareness => ItemRareness.Common;

        public override int Weight => 8000;
    }
}