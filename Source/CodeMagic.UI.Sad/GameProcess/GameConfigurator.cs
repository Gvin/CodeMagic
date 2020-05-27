using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using CodeMagic.Configuration.Xml;
using CodeMagic.Core.Common;
using CodeMagic.Core.Game;
using CodeMagic.Core.Injection;
using CodeMagic.Core.Injection.Configuration;
using CodeMagic.Core.Logging;
using CodeMagic.Core.Objects.ObjectEffects;
using CodeMagic.Game;
using CodeMagic.Game.Configuration;
using CodeMagic.Game.Items.ItemsGeneration;
using CodeMagic.UI.Sad.Drawing;
using CodeMagic.UI.Sad.Drawing.ObjectEffects;
using CodeMagic.UI.Sad.Logger;

namespace CodeMagic.UI.Sad.GameProcess
{
    public static class GameConfigurator
    {
        public static void Configure()
        {
            LogManager.Initialize(new LogProvider());

            LogManager.GetLog(nameof(GameConfigurator)).Info($"Starting game. Version {Assembly.GetExecutingAssembly().GetName().Version}");

            var config = LoadConfiguration();
            ConfigurationManager.InitializeConfiguration(config);

            ImagesStorage.Current.Load();

            DialogsManager.Initialize(new DialogsProvider());

            ItemsGeneratorManager.Initialize(new ItemsGenerator(
                config.ItemGenerator,
                ImagesStorage.Current, 
                new AncientSpellsProvider()));

            InitializeInjector();

#if DEBUG
            PerformanceMeter.Initialize(@".\Performance.log");
#endif
        }

        private static void InitializeInjector()
        {
            Injector.Initialize(new InjectorConfiguration());
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

        private class InjectorConfiguration : IInjectorConfiguration
        {
            public Dictionary<Type, InjectorMappingType> GetMapping()
            {
                return new Dictionary<Type, InjectorMappingType>
                {
                    // Misc
                    {
                        typeof(IDamageEffect),
                        new InjectorMappingType
                            {FactoryMethod = args => new DamageEffect((int) args[0], (Element) args[1])}
                    },
                    {
                        typeof(ISpellCastEffect),
                        new InjectorMappingType
                            {FactoryMethod = args => new SpellCastEffect()}
                    }
                };
            }
        }
    }
}