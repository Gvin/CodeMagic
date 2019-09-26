using CodeMagic.Core.Items;
using CodeMagic.UI.Images;

namespace CodeMagic.Implementations.Items.Materials
{
    public sealed class SteelIngot : IngotBase
    {
        public const string ResourceKey = "resource_ingot_steel";

        public SteelIngot() : base(MetalType.Steel)
        {
        }

        public override string Name => "Steel Ingot";

        public override string Key => ResourceKey;

        public override ItemRareness Rareness => ItemRareness.Common;

        public override int Weight => 8000;
    }
}