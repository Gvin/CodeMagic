using System;
using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Implementations;
using CodeMagic.Implementations.Items;
using CodeMagic.ItemsGeneration.Configuration.SpellBook;
using CodeMagic.ItemsGeneration.Implementations.Bonuses;
using CodeMagic.UI.Images;

namespace CodeMagic.ItemsGeneration.Implementations
{
    public class SpellBookGenerator
    {
        private const string DefaultName = "Spell Book";

        private readonly ISpellBooksConfiguration configuration;
        private readonly BonusesGenerator bonusesGenerator;
        private readonly IImagesStorage imagesStorage;

        public SpellBookGenerator(ISpellBooksConfiguration configuration, BonusesGenerator bonusesGenerator, IImagesStorage imagesStorage)
        {
            this.configuration = configuration;
            this.bonusesGenerator = bonusesGenerator;
            this.imagesStorage = imagesStorage;
        }

        public SpellBook GenerateSpellBook(ItemRareness rareness)
        {
            var config = GetConfiguration(rareness);
            var spellsCount = RandomHelper.GetRandomValue(config.MinSpells, config.MaxSpells);
            var bonusesCount = RandomHelper.GetRandomValue(config.MinBonuses, config.MaxBonuses);
            var image = GenerateImage();

            var itemConfig = new SpellBookItemImplConfiguration
            {
                Name = DefaultName,
                Key = Guid.NewGuid().ToString(),
                Description = GenerateDescription(config),
                Size = spellsCount,
                Image = image,
                Weight = configuration.Weight,
                Rareness = rareness
            };

            bonusesGenerator.GenerateBonuses(itemConfig, bonusesCount);

            return new SpellBookItemImpl(itemConfig);
        }

        private SymbolsImage GenerateImage()
        {
            var baseImageInit = imagesStorage.GetImage(configuration.Template);
            var symbolImageInit = imagesStorage.GetImage(RandomHelper.GetRandomElement(configuration.SymbolImages));
            var imageInit = SymbolsImage.Combine(baseImageInit, symbolImageInit);
            return ItemRecolorHelper.RecolorSpellBookImage(imageInit);
        }

        private string[] GenerateDescription(ISpellBookRarenessConfiguration config)
        {
            var result = new List<string>
            {
                RandomHelper.GetRandomElement(config.Description),
                RandomHelper.GetRandomElement(config.Description)
            };
            return result.Distinct().ToArray();
        }

        private ISpellBookRarenessConfiguration GetConfiguration(ItemRareness rareness)
        {
            var result = configuration.Configuration.FirstOrDefault(config => config.Rareness == rareness);
            if (result == null)
                throw new ApplicationException($"Rareness configuration not found for spell book rareness: {rareness}");

            return result;
        }
    }
}