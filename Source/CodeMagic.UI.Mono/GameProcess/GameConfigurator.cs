using System.IO;
using CodeMagic.Configuration.Xml;
using CodeMagic.Game.Configuration;
using CodeMagic.Game.Items.ItemsGeneration;
using CodeMagic.UI.Mono.Drawing;

namespace CodeMagic.UI.Mono.GameProcess
{
    public static class GameConfigurator
    {
        public static void Configure()
        {
            var config = LoadConfiguration();
            ConfigurationManager.InitializeConfiguration(config);

            ImagesStorage.Current.Load();

            ItemsGeneratorManager.Initialize(new ItemsGenerator(
                config.ItemGenerator,
                ImagesStorage.Current, 
                new AncientSpellsProvider()));

        }

        private static ConfigurationProvider LoadConfiguration()
        {
            using (var spellsConfig = File.OpenRead(@".\Configuration\Spells.xml"))
            using (var physicsConfig = File.OpenRead(@".\Configuration\Physics.xml"))
            using (var liquidsConfig = File.OpenRead(@".\Configuration\Liquids.xml"))
            using (var itemGeneratorConfig = File.OpenRead(@".\Configuration\ItemGenerator.xml"))
            using (var monstersConfig = File.OpenRead(@".\Configuration\Monsters.xml"))
            using (var levelsConfig = File.OpenRead(@".\Configuration\Levels.xml"))
            using (var treasureConfig = File.OpenRead(@".\Configuration\Treasure.xml"))
            {
                return ConfigurationProvider.Load(
                    spellsConfig,
                    physicsConfig,
                    liquidsConfig,
                    itemGeneratorConfig,
                    monstersConfig,
                    levelsConfig,
                    treasureConfig);
            }
        }
    }
}