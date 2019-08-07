using System;
using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Game;
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
                new[] {new StyledString($"Weight: {Weight}")}
            };
            AddDamageDescription(result);
            result.Add(new[] { new StyledString($"Hit Chance: {HitChance}") });
            result.Add(new StyledString[0]);
            result.AddRange(description.Select(line => new[] { new StyledString(line, ElementTextHelper.DescriptionTextColor) }).ToArray());

            return result.ToArray();
        }

        private void AddDamageDescription(List<StyledString[]> descriptionResult)
        {
            foreach (Element element in Enum.GetValues(typeof(Element)))
            {
                if (!MaxDamageValue.ContainsKey(element))
                    continue;

                descriptionResult.Add(new[]
                {
                    new StyledString($"{ElementTextHelper.GetElementName(element)} Damage: {MinDamageValue[element]} - {MaxDamageValue[element]}")
                });
            }
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