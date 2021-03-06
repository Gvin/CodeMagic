﻿using System.IO;
using System.Reflection;
using CodeMagic.Configuration.Xml;
using CodeMagic.Core.Common;
using CodeMagic.Core.Logging;
using CodeMagic.Game;
using CodeMagic.Game.Configuration;
using CodeMagic.Game.GameProcess;
using CodeMagic.Game.Items.ItemsGeneration;
using CodeMagic.Game.MapGeneration.Dungeon;
using CodeMagic.UI.Presenters;
using CodeMagic.UI.Sad.Drawing;
using CodeMagic.UI.Sad.Logger;
using CodeMagic.UI.Sad.Saving;
using CodeMagic.UI.Sad.Views;

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

            IoC.Configure();

            DialogsManager.Initialize(new DialogsProvider(IoC.Container.Resolve<IApplicationController>()));

            ItemsGeneratorManager.Initialize(new ItemsGenerator(
                config.ItemGenerator,
                ImagesStorage.Current, 
                new AncientSpellsProvider()));

            DungeonMapGenerator.Initialize(ImagesStorage.Current, Settings.Current.DebugWriteMapToFile);

#if DEBUG
            PerformanceMeter.Initialize(@".\Performance.log");
#endif
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