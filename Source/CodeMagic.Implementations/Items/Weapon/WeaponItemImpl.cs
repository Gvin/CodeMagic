using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Items;
using CodeMagic.UI.Images;

namespace CodeMagic.Implementations.Items.Weapon
{
    public class WeaponItemImpl : WeaponItem, IDescriptionProvider, IImageProvider
    {
        private readonly string[] description;
        private readonly SymbolsImage image;

        public WeaponItemImpl(WeaponItemImplConfiguration configuration) : base(configuration)
        {
            description = configuration.Description;

            image = configuration.Image;
        }

        public StyledString[][] GetDescription()
        {
            var result = new List<StyledString[]>
            {
                new[] {new StyledString($"Weight: {Weight}")},
                new[] {new StyledString($"Damage: {MinDamage} - {MaxDamage}")},
                new[] {new StyledString($"Hit Chance: {HitChance}")},
                new StyledString[0]
            };
            result.AddRange(description.Select(line => new[] { new StyledString(line) }).ToArray());

            return result.ToArray();
        }

        public SymbolsImage GetImage(IImagesStorage storage)
        {
            return image;
        }
    }

    public class WeaponItemImplConfiguration : WeaponItemConfiguration
    {
        public SymbolsImage Image { get; set; }

        public string[] Description { get; set; }
    }
}