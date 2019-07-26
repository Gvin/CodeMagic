using System;
using System.IO;
using CodeMagic.Configuration.Xml;
using CodeMagic.Core.Configuration;
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

            MapObjectsFactory.Initialize(new MapObjectsCreator());
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

        private class MapObjectsCreator : IMapObjectsCreator
        {
            public ICodeSpell CreateCodeSpell(ICreatureObject caster, string name, string code, int mana)
            {
                return new CodeSpellImpl(caster, name, code, mana);
            }

            public IFireDecorativeObject CreateFire(int temperature)
            {
                return new FireObjectImpl(temperature);
            }

            public IDecorativeObject CreateDecorativeObject(DecorativeObjectConfiguration configuration)
            {
                return new DecorativeObjectImpl(configuration);
            }

            public TIce CreateIce<TIce>(int volume) where TIce : class, IIceObject
            {
                var iceType = typeof(TIce);
                if (iceType == typeof(WaterIceObject))
                    return new WaterIceImpl(volume) as TIce;
                if (iceType == typeof(AcidIceObject))
                    return new AcidIceImpl(volume) as TIce;

                throw new ApplicationException($"Unknown ice type: {iceType.FullName}");
            }

            public TLiquid CreateLiquid<TLiquid>(int volume) where TLiquid : class, ILiquidObject
            {
                var liquidType = typeof(TLiquid);
                if (liquidType == typeof(WaterLiquidObject))
                    return new WaterLiquidImpl(volume) as TLiquid;
                if (liquidType == typeof(AcidLiquidObject))
                    return new AcidLiquidImpl(volume) as TLiquid;
                if (liquidType == typeof(OilLiquidObject))
                    return new OilLiquidImpl(volume) as TLiquid;

                throw new ApplicationException($"Unknown liquid type: {liquidType.FullName}");
            }

            public TSteam CreateSteam<TSteam>(int volume) where TSteam : class, ISteamObject
            {
                var steamType = typeof(TSteam);
                if (steamType == typeof(WaterSteamObject))
                    return new WaterSteamImpl(volume) as TSteam;
                if (steamType == typeof(AcidSteamObject))
                    return new AcidSteamImpl(volume) as TSteam;

                throw new ApplicationException($"Unknown steam type: {steamType.FullName}");
            }

            public IEnergyWall CreateEnergyWall(int lifeTime)
            {
                return new EnergyWallImpl(lifeTime);
            }
        }
    }
}