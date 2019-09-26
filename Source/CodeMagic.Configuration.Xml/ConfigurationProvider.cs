using System.IO;
using System.Xml.Serialization;
using CodeMagic.Configuration.Xml.Types.Buildings;
using CodeMagic.Configuration.Xml.Types.Items;
using CodeMagic.Configuration.Xml.Types.Liquids;
using CodeMagic.Configuration.Xml.Types.Monsters;
using CodeMagic.Configuration.Xml.Types.Physics;
using CodeMagic.Configuration.Xml.Types.Spells;
using CodeMagic.Core.Configuration;
using CodeMagic.Core.Configuration.Buildings;
using CodeMagic.Core.Configuration.Liquids;
using CodeMagic.Core.Configuration.Monsters;
using CodeMagic.Core.Configuration.Spells;
using CodeMagic.ItemsGeneration.Configuration;

namespace CodeMagic.Configuration.Xml
{
    public class ConfigurationProvider : IConfigurationProvider
    {
        private ConfigurationProvider(
            ISpellsConfiguration spells,
            IPhysicsConfiguration physics,
            ILiquidsConfiguration liquids,
            IItemGeneratorConfiguration itemGenerator,
            IMonstersConfiguration monsters,
            IBuildingsConfiguration buildings)
        {
            Spells = spells;
            Physics = physics;
            Liquids = liquids;
            ItemGenerator = itemGenerator;
            Monsters = monsters;
            Buildings = buildings;
        }

        public ISpellsConfiguration Spells { get; }

        public IPhysicsConfiguration Physics { get; }

        public ILiquidsConfiguration Liquids { get; }

        public IItemGeneratorConfiguration ItemGenerator { get; }

        public IMonstersConfiguration Monsters { get; }
        public IBuildingsConfiguration Buildings { get; }

        public static IConfigurationProvider Load(
            Stream spellsConfigStream,
            Stream physicsConfigStream,
            Stream liquidsConfigStream,
            Stream itemsGeneratorConfigStream,
            Stream monstersConfigStream,
            Stream buildingsConfigStream)
        {
            var spells = LoadConfig<ISpellsConfiguration, XmlSpellsConfigurationType>(spellsConfigStream);
            var physics = LoadConfig<IPhysicsConfiguration, XmlPhysicsConfigurationType>(physicsConfigStream);
            var liquids = LoadConfig<ILiquidsConfiguration, XmlLiquidsConfiguration>(liquidsConfigStream);
            var monsters = LoadConfig<IMonstersConfiguration, XmlMonstersConfiguration>(monstersConfigStream);
            var buildings = LoadConfig<IBuildingsConfiguration, XmlBuildingsConfiguration>(buildingsConfigStream);

            var itemGenerator = LoadConfig<IItemGeneratorConfiguration, XmlItemGeneratorConfiguration>(itemsGeneratorConfigStream);

            return new ConfigurationProvider(spells, physics, liquids, itemGenerator, monsters, buildings);
        }

        private static TConfig LoadConfig<TConfig, TSerializable>(Stream fileStream)
        {
            var serializer = new XmlSerializer(typeof(TSerializable));
            var data = serializer.Deserialize(fileStream);
            if (data is TConfig)
            {
                return (TConfig) data;
            }

            return default(TConfig);
        }
    }
}