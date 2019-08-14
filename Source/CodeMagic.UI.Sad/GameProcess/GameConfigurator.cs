using System;
using System.Collections.Generic;
using System.IO;
using CodeMagic.Configuration.Xml;
using CodeMagic.Core.Configuration;
using CodeMagic.Core.Game;
using CodeMagic.Core.Injection;
using CodeMagic.Core.Injection.Configuration;
using CodeMagic.Core.Items;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Objects.Creatures;
using CodeMagic.Core.Objects.DecorativeObjects;
using CodeMagic.Core.Objects.IceObjects;
using CodeMagic.Core.Objects.LiquidObjects;
using CodeMagic.Core.Objects.ObjectEffects;
using CodeMagic.Core.Objects.SolidObjects;
using CodeMagic.Core.Objects.SteamObjects;
using CodeMagic.Core.Spells;
using CodeMagic.Implementations.Objects;
using CodeMagic.Implementations.Objects.DecorativeObjects;
using CodeMagic.Implementations.Objects.IceObjects;
using CodeMagic.Implementations.Objects.LiquidObjects;
using CodeMagic.Implementations.Objects.SolidObjects;
using CodeMagic.Implementations.Objects.SteamObjects;
using CodeMagic.ItemsGeneration;
using CodeMagic.UI.Sad.Drawing;
using CodeMagic.UI.Sad.Drawing.ObjectEffects;

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
            using (var itemGeneratorConfig = File.OpenRead(@".\Configuration\ItemGenerator.xml"))
            {
                return ConfigurationProvider.Load(spellsConfig, physicsConfig, liquidsConfig, itemGeneratorConfig);
            }
        }

        private class InjectorConfiguration : IInjectorConfiguration
        {
            public Dictionary<Type, InjectorMappingType> GetMapping()
            {
                return new Dictionary<Type, InjectorMappingType>
                {
                    {
                        typeof(IItemsGenerator),
                        new InjectorMappingType
                        {
                            Object = new ItemsGenerator(
                                ((ConfigurationProvider) ConfigurationManager.Current).ItemGenerator,
                                ImagesStorage.Current)
                        }
                    },
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
                        typeof(ICreatureRemainsObject),
                        new InjectorMappingType { FactoryMethod = args => new CreatureRemainsObjectImpl((CreatureRemainsObjectConfiguration) args[0])}
                    },
                    {
                        typeof(ICodeSpell),
                        new InjectorMappingType
                        {
                            FactoryMethod = args => new CodeSpellImpl((ICreatureObject) args[0], (string) args[1],
                                (string) args[2], (int) args[3])
                        }
                    },
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