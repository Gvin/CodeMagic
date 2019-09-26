using CodeMagic.Core.Items;
using CodeMagic.UI.Images;

namespace CodeMagic.Implementations.Items.Materials
{
    public sealed class CopperIngot : IngotBase
    {
        public const string ResourceKey = "resource_ingot_copper";

        public CopperIngot() : base(MetalType.Copper)
        {
        }

        public override string Name => "Copper Ingot";

        public override string Key => ResourceKey;

        public override ItemRareness Rareness => ItemRareness.Common;

        public override int Weight => 9000;
    }
}