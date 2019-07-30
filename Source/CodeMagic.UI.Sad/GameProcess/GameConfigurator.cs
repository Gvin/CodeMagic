using System;
using System.Collections.Generic;
using System.IO;
using CodeMagic.Configuration.Xml;
using CodeMagic.Core.Configuration;
using CodeMagic.Core.Game;
using CodeMagic.Core.Injection;
using CodeMagic.Core.Injection.Configuration;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Objects.Creatures;
using CodeMagic.Core.Objects.DecorativeObjects;
using CodeMagic.Core.Objects.IceObjects;
using CodeMagic.Core.Objects.LiquidObjects;
using CodeMagic.Core.Objects.SolidObjects;
using CodeMagic.Core.Objects.SteamObjects;
using CodeMagic.Core.Spells;
using CodeMagic.Objects.Implementation;
using CodeMagic.Objects.Implementation.DecorativeObjects;
using CodeMagic.Objects.Implementation.IceObjects;
using CodeMagic.Objects.Implementation.LiquidObjects;
using CodeMagic.Objects.Implementation.SolidObjects;
using CodeMagic.Objects.Implementation.SteamObjects;
using CodeMagic.UI.Sad.Drawing;

namespace CodeMagic.UI.Sad.GameProcess
{
    public static class GameConfigurator
    {
        public static void Configure()
        {
            var config = LoadConfiguration();
            ConfigurationManager.InitializeConfiguration(config);

            ImagesStorage.Current.Load();

            InitializeInjector();
        }

        private static void InitializeInjector()
        {
            Injector.Initialize(new InjectorConfiguration());
        }

        private static IConfigurationProvider LoadConfiguration()
        {
            using (var spellsConfig = File.OpenRead(@".\Configuration\Spells.xml"))
            using (var physicsConfig = File.OpenRead(@".\Configuration\Physics.xml"))
            using (var liquidsConfig = File.OpenRead(@".\Configuration\Liquids.xml"))
            {
                return ConfigurationProvider.Load(spellsConfig, physicsConfig, liquidsConfig);
            }
        }

        private class InjectorConfiguration : IInjectorConfiguration
        {
            public Dictionary<Type, InjectorMappingType> GetMapping()
            {
                return new Dictionary<Type, InjectorMappingType>
                {
                    // Liquid
                    {
                        typeof(IWaterLiquidObject),
                        new InjectorMappingType {FactoryMethod = args => new WaterLiquidImpl((int) args[0])}
                    },
                    {
                        typeof(IAcidLiquidObject),
                        new InjectorMappingType {FactoryMethod = args => new AcidLiquidImpl((int) args[0])}
                    },
                    {
                        typeof(IOilLiquidObject),
                        new InjectorMappingType {FactoryMethod = args => new OilLiquidImpl((int) args[0])}
                    },
                    // Ice
                    {
                        typeof(IWaterIceObject),
                        new InjectorMappingType {FactoryMethod = args => new WaterIceImpl((int) args[0])}
                    },
                    {
                        typeof(IAcidIceObject),
                        new InjectorMappingType {FactoryMethod = args => new AcidIceImpl((int) args[0])}
                    },
                    // Steam
                    {
                        typeof(IWaterSteamObject),
                        new InjectorMappingType {FactoryMethod = args => new WaterSteamImpl((int) args[0])}
                    },
                    {
                        typeof(IAcidSteamObject),
                        new InjectorMappingType {FactoryMethod = args => new AcidSteamImpl((int) args[0])}
                    },
                    // Objects
                    {
                        typeof(IEnergyWall),
                        new InjectorMappingType {FactoryMethod = args => new EnergyWallImpl((int) args[0])}
                    },
                    {
                        typeof(IDecorativeObject),
                        new InjectorMappingType
                            {FactoryMethod = args => new DecorativeObjectImpl((DecorativeObjectConfiguration) args[0])}
                    },
                    {
                        typeof(IFireDecorativeObject),
                        new InjectorMappingType {FactoryMethod = args => new FireObjectImpl((int) args[0])}
                    },
                    {
                        typeof(ICodeSpell),
                        new InjectorMappingType {FactoryMethod = args => new CodeSpellImpl((ICreatureObject) args[0], (string) args[1], (string) args[2], (int) args[3])}
                    },
                    // Misc
                    {
                        typeof(IDamageRecord),
                        new InjectorMappingType {FactoryMethod = args => new DamageRecord((int) args[0], (Element?) args[1])}
                    }
                };
            }
        }
    }
}