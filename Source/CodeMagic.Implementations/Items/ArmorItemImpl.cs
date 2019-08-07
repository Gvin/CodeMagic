using System;
using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.UI.Images;

namespace CodeMagic.Implementations.Items
{
    public class ArmorItemImpl : ArmorItem, IImageProvider, IDescriptionProvider
    {
        private readonly SymbolsImage image;
        private readonly string[] description;

        public ArmorItemImpl(ArmorItemImplConfiguration configuration) : base(configuration)
        {
            image = configuration.Image;
            description = configuration.Description;
        }

        public SymbolsImage GetImage(IImagesStorage storage)
        {
            return image;
        }

        public StyledString[][] GetDescription()
        {
            var result = new List<StyledString[]>
            {
                new[] {new StyledString($"Weight: {Weight}")}
            };

            AddProtectionDescription(result);

            result.Add(new StyledString[0]);
            result.AddRange(description.Select(line => new[] { new StyledString(line) }).ToArray());

            return result.ToArray();
        }

        private void AddProtectionDescription(List<StyledString[]> descriptionResult)
        {
            foreach (Element element in Enum.GetValues(typeof(Element)))
            {
                var value = GetProtection(element);
                if (value != 0)
                {
                    descriptionResult.Add(new[] { new StyledString($"{ElementNameHelper.GetElementName(element)} Protection: {value}%") });
                }
            }
        }
    }

    public class ArmorItemImplConfiguration : ArmorItemConfiguration
    {
        public SymbolsImage Image { get; set; }

        public string[] Description { get; set; }
    }
}